using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using data;

namespace Production_planning
{
    public class EmployeeManagementService
    {
        private readonly string _connectionString;

        public EmployeeManagementService(string connectionString)
        {
            _connectionString = connectionString;
        }

        // =========================================================================
        // РАЗДЕЛ 1: ПОЛУЧЕНИЕ СПИСКОВ И КАРТОЧЕК С ОТСЕЧКОЙ НА ТЕКУЩУЮ ДАТУ
        // =========================================================================

        /// <summary>
        /// Возвращает краткий список сотрудников со срезом данных на текущую дату
        /// </summary>
        /// 
        
        public async Task<List<EmployeeShortRow>> GetEmployeeShortListAsync()
        {
            string sql = @"
        SELECT 
            e.id AS Id,
            e.full_name AS FullName,
            e.last_name AS LastName, 
            e.first_name AS FirstName, 
            e.patronymic AS Patronymic,
            
            -- Возвращаем 1 (работает) или 0 (уволен) без использования кириллицы
            CASE WHEN active_periods.employee_id IS NOT NULL THEN 1 ELSE 0 END AS IsActive,
            
            p.name AS CurrentPosition,
            st.name AS CurrentSchedule,
            eq.name AS CurrentEquipment,
            wa.name AS CurrentWorkArea,
            ec.contact_value AS PrimaryPhone
        FROM employees e
        
        LEFT JOIN (
            SELECT employee_id 
            FROM employment_periods 
            WHERE hire_date <= CURDATE() AND (fire_date IS NULL OR fire_date >= CURDATE())
        ) active_periods ON active_periods.employee_id = e.id
        
        LEFT JOIN employee_position_assignments epa ON epa.id = (
            SELECT id FROM employee_position_assignments 
            WHERE employee_id = e.id AND valid_from <= CURDATE() 
            ORDER BY valid_from DESC, id DESC LIMIT 1
        )
        LEFT JOIN positions p ON epa.position_id = p.id
        
        LEFT JOIN employee_schedule_assignments esa ON esa.id = (
            SELECT id FROM employee_schedule_assignments 
            WHERE employee_id = e.id AND valid_from <= CURDATE() 
            ORDER BY valid_from DESC, id DESC LIMIT 1
        )
        LEFT JOIN schedule_templates st ON esa.template_id = st.id
        
        LEFT JOIN employee_equipment_assignments eea ON eea.id = (
            SELECT id FROM employee_equipment_assignments 
            WHERE employee_id = e.id AND valid_from <= CURDATE() 
            ORDER BY valid_from DESC, id DESC LIMIT 1
        )
        LEFT JOIN equipment eq ON eea.equipment_id = eq.id
        LEFT JOIN work_areas wa ON eq.work_area_id = wa.id
        
        LEFT JOIN employee_contacts ec ON ec.id = (
            SELECT ec_inner.id FROM employee_contacts ec_inner
            JOIN contact_types ct ON ec_inner.contact_type_id = ct.id
            WHERE ec_inner.employee_id = e.id AND ct.code = 'phone'
            LIMIT 1
        )
        
        ORDER BY IsActive DESC, e.full_name ASC;";

            using (var conn = new MySqlConnection(_connectionString))
            {
                var res = await conn.QueryAsync<EmployeeShortRow>(sql);
                return res.ToList();
            }
        }


        /// <summary>
        /// Собирает максимально подробную карточку сотрудника на текущий момент
        /// </summary>
        public async Task<EmployeeFullCard> GetEmployeeFullCardAsync(ulong employeeId)
        {
            const string mainSql = @"
                SELECT 
                    e.id AS Id, e.last_name AS LastName, e.first_name AS FirstName, e.patronymic AS Patronymic, e.full_name AS FullName,
                    emp.id AS CurrentPeriodId, emp.hire_date AS HireDate, emp.fire_date AS FireDate,
                    epa.id AS CurrentPositionAssignmentId, epa.position_id AS PositionId, pos.name AS PositionName, pos.system_role AS SystemRole, epa.valid_from AS PositionValidFrom,
                    esa.id AS CurrentScheduleAssignmentId, esa.template_id AS ScheduleTemplateId, st.name AS ScheduleName, esa.valid_from AS ScheduleValidFrom,
                    eea.id AS CurrentEquipmentAssignmentId, eea.equipment_id AS EquipmentId, eq.name AS EquipmentName, eea.valid_from AS EquipmentValidFrom
                FROM employees e
                LEFT JOIN employment_periods emp ON emp.id = (
                    SELECT id FROM employment_periods WHERE employee_id = e.id ORDER BY hire_date DESC LIMIT 1
                )
                LEFT JOIN employee_position_assignments epa ON epa.id = (
                    SELECT id FROM employee_position_assignments WHERE employee_id = e.id AND valid_from <= CURDATE() ORDER BY valid_from DESC, id DESC LIMIT 1
                )
                LEFT JOIN positions pos ON epa.position_id = pos.id
                LEFT JOIN employee_schedule_assignments esa ON esa.id = (
                    SELECT id FROM employee_schedule_assignments WHERE employee_id = e.id AND valid_from <= CURDATE() ORDER BY valid_from DESC, id DESC LIMIT 1
                )
                LEFT JOIN schedule_templates st ON esa.template_id = st.id
                LEFT JOIN employee_equipment_assignments eea ON eea.id = (
                    SELECT id FROM employee_equipment_assignments WHERE employee_id = e.id AND valid_from <= CURDATE() ORDER BY valid_from DESC, id DESC LIMIT 1
                )
                LEFT JOIN equipment eq ON eea.equipment_id = eq.id
                WHERE e.id = @EmployeeId;";

            const string contactsSql = @"
                SELECT ec.id, ec.contact_type_id AS ContactTypeId, ct.code AS ContactTypeCode, ct.name AS ContactTypeName, ec.contact_value AS ContactValue
                FROM employee_contacts ec
                JOIN contact_types ct ON ec.contact_type_id = ct.id
                WHERE ec.employee_id = @EmployeeId;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                var card = await connection.QueryFirstOrDefaultAsync<EmployeeFullCard>(mainSql, new { EmployeeId = employeeId });
                if (card != null)
                {
                    var contacts = await connection.QueryAsync<EmployeeContactDto>(contactsSql, new { EmployeeId = employeeId });
                    card.Contacts = contacts.ToList();
                }
                return card;
            }
        }

        // =========================================================================
        // РАЗДЕЛ 2: ДОБАВЛЕНИЕ, РЕДАКТИРОВАНИЕ, ПЕРИОДЫ НАЙМА/УВОЛЬНЕНИЯ
        // =========================================================================

        // 3. ДОБАВЛЕНИЕ НОВОГО СОТРУДНИКА С КОНТАКТАМИ
        public async Task<ulong> CreateEmployeeAsync(EmployeeFullCard card)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var trans = await conn.BeginTransactionAsync())
                {
                    try
                    {
                        var names = (card.FullName ?? "").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        string ln = names.Length > 0 ? names[0] : "";
                        string fn = names.Length > 1 ? names[1] : "";
                        string pn = names.Length > 2 ? string.Join(" ", names.Skip(2)) : null;

                        string insEmp = "INSERT INTO employees (last_name, first_name, patronymic, full_name) VALUES (@LastName, @FirstName, @Patronymic, @FullName); SELECT LAST_INSERT_ID();";
                        ulong empId = await conn.ExecuteScalarAsync<ulong>(insEmp, new { LastName = ln, FirstName = fn, Patronymic = pn, FullName = card.FullName }, trans);

                        string insPeriod = "INSERT INTO employment_periods (employee_id, hire_date) VALUES (@EmployeeId, @HireDate);";
                        await conn.ExecuteAsync(insPeriod, new { EmployeeId = empId, HireDate = card.HireDate }, trans);

                        if (card.Contacts != null && card.Contacts.Count > 0)
                        {
                            string insContact = "INSERT INTO employee_contacts (employee_id, contact_type_id, contact_value) VALUES (@EmployeeId, @ContactTypeId, @ContactValue);";
                            foreach (var contact in card.Contacts)
                            {
                                if (!string.IsNullOrWhiteSpace(contact.ContactValue))
                                {
                                    await conn.ExecuteAsync(insContact, new { EmployeeId = empId, ContactTypeId = contact.ContactTypeId, ContactValue = contact.ContactValue }, trans);
                                }
                            }
                        }

                        await trans.CommitAsync();
                        return empId;
                    }
                    catch (Exception)
                    {
                        await trans.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        // 4. РЕДАКТИРОВАНИЕ ПОЛНОЙ КАРТОЧКИ (ОСНОВНЫЕ ДАННЫЕ И КОНТАКТЫ)
        public async Task<bool> UpdateEmployeeBaseCardAsync(EmployeeFullCard card)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var trans = await conn.BeginTransactionAsync())
                {
                    try
                    {
                        var names = (card.FullName ?? "").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        string ln = names.Length > 0 ? names[0] : "";
                        string fn = names.Length > 1 ? names[1] : "";
                        string pn = names.Length > 2 ? string.Join(" ", names.Skip(2)) : null;

                        string sqlUpd = "UPDATE employees SET last_name = @LastName, first_name = @FirstName, patronymic = @Patronymic, full_name = @FullName WHERE id = @Id;";
                        await conn.ExecuteAsync(sqlUpd, new { Id = card.Id, LastName = ln, FirstName = fn, Patronymic = pn, FullName = card.FullName }, trans);

                        string sqlDel = "DELETE FROM employee_contacts WHERE employee_id = @EmployeeId;";
                        await conn.ExecuteAsync(sqlDel, new { EmployeeId = card.Id }, trans);

                        if (card.Contacts != null)
                        {
                            string insContact = "INSERT INTO employee_contacts (employee_id, contact_type_id, contact_value) VALUES (@EmployeeId, @ContactTypeId, @ContactValue);";
                            foreach (var contact in card.Contacts)
                            {
                                if (!string.IsNullOrWhiteSpace(contact.ContactValue))
                                {
                                    await conn.ExecuteAsync(insContact, new { EmployeeId = card.Id, ContactTypeId = contact.ContactTypeId, ContactValue = contact.ContactValue }, trans);
                                }
                            }
                        }

                        await trans.CommitAsync();
                        return true;
                    }
                    catch (Exception)
                    {
                        await trans.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        // 5. УВОЛЬНЕНИЕ СОТРУДНИКА
        public async Task<bool> FireEmployeeAsync(ulong employeeId, DateTime fireDate)
        {
            string sql = "UPDATE employment_periods SET fire_date = @FireDate WHERE employee_id = @EmployeeId AND fire_date IS NULL ORDER BY hire_date DESC LIMIT 1;";
            using (var conn = new MySqlConnection(_connectionString))
            {
                int rows = await conn.ExecuteAsync(sql, new { EmployeeId = employeeId, FireDate = fireDate });
                return rows > 0;
            }
        }

        // 6. ПОВТОРНЫЙ НАЙМ СОТРУДНИКА
        public async Task<bool> RehireEmployeeAsync(ulong employeeId, DateTime hireDate)
        {
            string sql = "INSERT INTO employment_periods (employee_id, hire_date) VALUES (@EmployeeId, @HireDate);";
            using (var conn = new MySqlConnection(_connectionString))
            {
                int rows = await conn.ExecuteAsync(sql, new { EmployeeId = employeeId, HireDate = hireDate });
                return rows > 0;
            }
        }

        /*// 7. НАЗНАЧЕНИЕ НА НОВУЮ ДОЛЖНОСТЬ (БУФЕРНЫЙ КЛАСС В ИСТОРИЮ)
        public async Task<bool> ExecuteAssignPositionAsync(AssignPositionCommand cmd)
        {
            string sql = "INSERT INTO employee_position_assignments (employee_id, position_id, valid_from) " +
                "VALUES (@EmployeeId, @PositionId, @ValidFrom) ON DUPLICATE KEY UPDATE position_id = VALUES(position_id);";
            using (var conn = new MySqlConnection(_connectionString))
            {
                int rows = await conn.ExecuteAsync(sql, cmd);
                return rows > 0;
            }
        }

        // 8. НАЗНАЧЕНИЕ НОВОГО ГРАФИКА (БУФЕРНЫЙ КЛАСС В ИСТОРИЮ)
        public async Task<bool> ExecuteAssignScheduleAsync(AssignScheduleCommand cmd)
        {
            string sql = "INSERT INTO employee_schedule_assignments (employee_id, template_id, valid_from) " +
                "VALUES (@EmployeeId, @TemplateId, @ValidFrom) ON DUPLICATE KEY UPDATE template_id = VALUES(template_id);";
            using (var conn = new MySqlConnection(_connectionString))
            {
                int rows = await conn.ExecuteAsync(sql, cmd);
                return rows > 0;
            }
        }

        // 9. ЗАКРЕПЛЕНИЕ ЗА НОВЫМ ОБОРУДОВАНИЕМ (БУФЕРНЫЙ КЛАСС В ИСТОРИЮ)
        public async Task<bool> ExecuteAssignEquipmentAsync(AssignEquipmentCommand cmd)
        {
            string sql = "INSERT INTO employee_equipment_assignments (employee_id, equipment_id, valid_from) " +
                "VALUES (@EmployeeId, @EquipmentId, @ValidFrom) ON DUPLICATE KEY UPDATE equipment_id = VALUES(equipment_id);";
            using (var conn = new MySqlConnection(_connectionString))
            {
                int rows = await conn.ExecuteAsync(sql, cmd);
                return rows > 0;
            }
        }*/

        // 10. ВРЕМЯ С ПОСЛЕДНЕГО НАЗНАЧЕНИЯ НА СТАНОК (ДЛЯ ПРЕДУПРЕЖДЕНИЙ)
        public async Task<int?> GetDaysSinceLastEquipmentAssignmentAsync(ulong employeeId)
        {
            string sql = "SELECT DATEDIFF(CURDATE(), MAX(valid_from)) FROM employee_equipment_assignments WHERE employee_id = @EmployeeId;";
            using (var conn = new MySqlConnection(_connectionString))
            {
                return await conn.QueryFirstOrDefaultAsync<int?>(sql, new { EmployeeId = employeeId });
            }
        }

        /// <summary>
        /// ЗАПРОС ВОЗРАСТА ЗАПИСЕЙ: Считает возраст записей в днях от даты их valid_from
        /// </summary>
        public async Task<(int PositionAge, int EquipmentAge, int ScheduleAge)> GetCurrentAssignmentsAgeAsync(int employeeId)
        {
            const string qPosition = "SELECT DATEDIFF(CURDATE(), MAX(valid_from)) FROM employee_position_assignments WHERE employee_id = @EmployeeId;";
            const string qEquipment = "SELECT DATEDIFF(CURDATE(), MAX(valid_from)) FROM employee_equipment_assignments WHERE employee_id = @EmployeeId;";
            const string qSchedule = "SELECT DATEDIFF(CURDATE(), MAX(valid_from)) FROM employee_schedule_assignments WHERE employee_id = @EmployeeId;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                int positionRow = (int)await connection.QueryFirstOrDefaultAsync<int?>(qPosition, new { Id = employeeId });
                int equipmentRow = (int)await connection.QueryFirstOrDefaultAsync<int?>(qEquipment, new { Id = employeeId });
                int scheduleRow = (int)await connection.QueryFirstOrDefaultAsync<int?>(qSchedule, new { Id = employeeId });

                return (positionRow, equipmentRow, scheduleRow);
            }
        }

        /// <summary>
        /// Возвращает ленту кадровых изменений, учитывая сброс назначений при повторном найме
        /// </summary>
        public async Task<List<EmployeeCareerEventRow>> GetEmployeeCareerTimelineAsync(ulong employeeId)
        {
            string sql =
                // 1. События приема на работу
                "SELECT hire_date AS EventDate, 'Прием' AS EventType, 'Принят в штат организации' AS Details FROM employment_periods WHERE employee_id = @EmployeeId " +
                "UNION ALL " +
                // 2. События увольнения
                "SELECT fire_date AS EventDate, 'Увольнение' AS EventType, 'Трудовой договор расторгнут' AS Details FROM employment_periods WHERE employee_id = @EmployeeId AND fire_date IS NOT NULL " +
                "UNION ALL " +
                // 3. Вычисление Назначений отдельно внутри КАЖДОГО периода работы (emp.id)
                "SELECT ordered_positions.valid_from AS EventDate, " +
                "CASE WHEN ordered_positions.row_num = 1 THEN 'Назначение' ELSE 'Смена должности' END AS EventType, " +
                "ordered_positions.pos_name AS Details " +
                "FROM (" +
                "  SELECT epa.valid_from, p.name AS pos_name, " +
                "  ROW_NUMBER() OVER (PARTITION BY epa.employee_id, emp.id ORDER BY epa.valid_from ASC, epa.id ASC) AS row_num " +
                "  FROM employee_position_assignments epa " +
                "  JOIN positions p ON epa.position_id = p.id " +
                "  -- Привязываем назначение к конкретному циклу контракта " +
                "  JOIN employment_periods emp ON epa.employee_id = emp.employee_id " +
                "    AND epa.valid_from >= emp.hire_date " +
                "    AND (emp.fire_date IS NULL OR epa.valid_from <= emp.fire_date) " +
                "  WHERE epa.employee_id = @EmployeeId" +
                ") ordered_positions " +
                // Сортировка всей финальной ленты от свежих событий к старым
                "ORDER BY EventDate DESC, FIELD(EventType, 'Увольнение', 'Смена должности', 'Назначение', 'Прием') ASC;";

            using (var conn = new MySqlConnection(_connectionString))
            {
                var res = await conn.QueryAsync<EmployeeCareerEventRow>(sql, new { EmployeeId = employeeId });
                return res.ToList();
            }
        }

        /// <summary>
        /// Возвращает полную историю всех периодов работы сотрудника (приемы/увольнения)
        /// </summary>
        public async Task<List<EmployeeEmploymentHistoryRow>> GetEmployeeEmploymentHistoryAsync(ulong employeeId)
        {
            string sql = "SELECT id AS Id, hire_date AS HireDate, fire_date AS FireDate " +
                         "FROM employment_periods " +
                         "WHERE employee_id = @EmployeeId " +
                         "ORDER BY hire_date DESC, id DESC;";

            using (var conn = new MySqlConnection(_connectionString))
            {
                var res = await conn.QueryAsync<EmployeeEmploymentHistoryRow>(sql, new { EmployeeId = employeeId });
                return res.ToList();
            }
        }

        // 11. ЗАПРОС ПОЛНОЙ ИСТОРИИ НАЗНАЧЕНИЙ ДОЛЖНОСТЕЙ
        public async Task<List<EmployeePositionHistoryRow>> GetEmployeePositionHistoryAsync(ulong employeeId)
        {
            string sql = "SELECT epa.id AS Id, epa.position_id AS PositionId, p.name AS PositionName, p.system_role AS SystemRole, epa.valid_from AS ValidFrom " +
                "FROM employee_position_assignments epa JOIN positions p ON epa.position_id = p.id WHERE epa.employee_id = @EmployeeId ORDER BY epa.valid_from DESC, epa.id DESC;";
            using (var conn = new MySqlConnection(_connectionString))
            {
                var res = await conn.QueryAsync<EmployeePositionHistoryRow>(sql, new { EmployeeId = employeeId });
                return res.ToList();
            }
        }

        // 12. ЗАПРОС ПОЛНОЙ ИСТОРИИ НАЗНАЧЕНИЙ ГРАФИКОВ
        public async Task<List<EmployeeScheduleHistoryRow>> GetEmployeeScheduleHistoryAsync(ulong employeeId)
        {
            string sql = "SELECT esa.id AS Id, esa.template_id AS TemplateId, st.name AS TemplateName, esa.valid_from AS ValidFrom " +
                "FROM employee_schedule_assignments esa JOIN schedule_templates st ON esa.template_id = st.id WHERE esa.employee_id = @EmployeeId ORDER BY esa.valid_from DESC, esa.id DESC;";
            using (var conn = new MySqlConnection(_connectionString))
            {
                var res = await conn.QueryAsync<EmployeeScheduleHistoryRow>(sql, new { EmployeeId = employeeId });
                return res.ToList();
            }
        }

        // 13. ЗАПРОС ПОЛНОЙ ИСТОРИИ ЗАКРЕПЛЕНИЯ ОБОРУДОВАНИЯ
        public async Task<List<EmployeeEquipmentHistoryRow>> GetEmployeeEquipmentHistoryAsync(ulong employeeId)
        {
            string sql = "SELECT eea.id AS Id, eea.equipment_id AS EquipmentId, COALESCE(eq.name, '— Откреплен от оборудования —') AS EquipmentName, wa.name AS WorkAreaName, eea.valid_from AS ValidFrom " +
                "FROM employee_equipment_assignments eea LEFT JOIN equipment eq ON eea.equipment_id = eq.id LEFT JOIN work_areas wa ON eq.work_area_id = wa.id WHERE eea.employee_id = @EmployeeId ORDER BY eea.valid_from DESC, eea.id DESC;";
            using (var conn = new MySqlConnection(_connectionString))
            {
                var res = await conn.QueryAsync<EmployeeEquipmentHistoryRow>(sql, new { EmployeeId = employeeId });
                return res.ToList();
            }
        }

        // 14. СОЗДАНИЕ СОТРУДНИКА (РАЗДЕЛЬНЫЙ ВВОД ФИО)
        public async Task<ulong> CreateEmployeeWithAssignmentsAsync(EmployeeFullCard card)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var trans = await conn.BeginTransactionAsync())
                {
                    try
                    {
                        card.FullName = GetShortName(card.LastName, card.FirstName, card.Patronymic);

                        // Вставка в employees напрямую по полям
                        string insEmp = "INSERT INTO employees (last_name, first_name, patronymic, full_name) " +
                                        "VALUES (@LastName, @FirstName, @Patronymic, @FullName); SELECT LAST_INSERT_ID();";

                        ulong empId = await conn.ExecuteScalarAsync<ulong>(insEmp, card, trans);

                        // Стартовый период найма
                        string insPeriod = "INSERT INTO employment_periods (employee_id, hire_date) VALUES (@EmployeeId, @HireDate);";
                        await conn.ExecuteAsync(insPeriod, new { EmployeeId = empId, HireDate = card.HireDate }, trans);

                        // Стартовая должность (с даты найма)
                        if (card.PositionId.HasValue)
                        {
                            string insPos = "INSERT INTO employee_position_assignments (employee_id, position_id, valid_from) VALUES (@EmployeeId, @PositionId, @ValidFrom);";
                            await conn.ExecuteAsync(insPos, new { EmployeeId = empId, PositionId = card.PositionId.Value, ValidFrom = card.HireDate }, trans);
                        }

                        // Стартовый график
                        if (card.ScheduleTemplateId.HasValue)
                        {
                            string insSched = "INSERT INTO employee_schedule_assignments (employee_id, template_id, valid_from) VALUES (@EmployeeId, @TemplateId, @ValidFrom);";
                            await conn.ExecuteAsync(insSched, new { EmployeeId = empId, TemplateId = card.ScheduleTemplateId.Value, ValidFrom = card.HireDate }, trans);
                        }

                        // Стартовое оборудование
                        if (card.EquipmentId.HasValue)
                        {
                            string insEq = "INSERT INTO employee_equipment_assignments (employee_id, equipment_id, valid_from) VALUES (@EmployeeId, @EquipmentId, @ValidFrom);";
                            await conn.ExecuteAsync(insEq, new { EmployeeId = empId, EquipmentId = card.EquipmentId.Value, ValidFrom = card.HireDate }, trans);
                        }

                        // Запись контактов
                        if (card.Contacts != null && card.Contacts.Count > 0)
                        {
                            string insContact = "INSERT INTO employee_contacts (employee_id, contact_type_id, contact_value) VALUES (@EmployeeId, @ContactTypeId, @ContactValue);";
                            foreach (var contact in card.Contacts)
                            {
                                if (!string.IsNullOrWhiteSpace(contact.ContactValue))
                                {
                                    await conn.ExecuteAsync(insContact, new { EmployeeId = empId, ContactTypeId = contact.ContactTypeId, ContactValue = contact.ContactValue }, trans);
                                }
                            }
                        }

                        await trans.CommitAsync();
                        return empId;
                    }
                    catch (Exception)
                    {
                        await trans.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        // 15. КОМПЛЕКСНОЕ СОХРАНЕНИЕ КАРТОЧКИ С ИСПОЛЬЗОВАНИЕМ КЛАССОВ-БУФЕРОВ СМЕНЫ ИСТОРИИ
        public async Task<bool> SaveEmployeeFullCardChangesAsync(
            EmployeeFullCard card,
            PositionUpdateBuffer posBuffer,
            ScheduleUpdateBuffer schedBuffer,
            EquipmentUpdateBuffer eqBuffer)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var trans = await conn.BeginTransactionAsync())
                {
                    try
                    {
                        card.FullName = GetShortName(card.LastName, card.FirstName, card.Patronymic);

                        // 1. Обновляем личные данные в employees (Раздельный ввод ФИО)
                        string sqlEmp = "UPDATE employees SET last_name = @LastName, first_name = @FirstName, patronymic = @Patronymic, full_name = @FullName WHERE id = @Id;";
                        await conn.ExecuteAsync(sqlEmp, card, trans);

                        // 2. Обновляем текущий период найма
                        if (card.CurrentPeriodId.HasValue)
                        {
                            string sqlPeriod = "UPDATE employment_periods SET hire_date = @HireDate, fire_date = @FireDate WHERE id = @PeriodId;";
                            await conn.ExecuteAsync(sqlPeriod, new { PeriodId = card.CurrentPeriodId.Value, HireDate = card.HireDate, FireDate = card.FireDate }, trans);
                        }

                        // 2. ИСТОРИЯ ДОЛЖНОСТЕЙ (Поиск строго по первичному ключу ID строки)
                        if (!posBuffer.IsNewAssignment) // Редактируем существующую запись (можно менять и должность, и саму дату!)
                        {
                            string sqlUpd = "UPDATE employee_position_assignments SET position_id = @PositionId, valid_from = @PositionValidFrom WHERE id = @AssignmentId;";
                            await conn.ExecuteAsync(sqlUpd, card, trans);
                        }
                        else // Добавляем новую веху истории (если AssignmentId пустой)
                        {
                            string sqlIns = "INSERT INTO employee_position_assignments (employee_id, position_id, valid_from) VALUES (@EmployeeId, @NewPositionId, @NewValidFrom);";
                            await conn.ExecuteAsync(sqlIns, posBuffer, trans);
                        }

                        // 3. ИСТОРИЯ ГРАФИКОВ (Поиск строго по первичному ключу ID строки)
                        if (!schedBuffer.IsNewAssignment)
                        {
                            string sqlUpd = "UPDATE employee_schedule_assignments SET template_id = @ScheduleTemplateId, valid_from = @ScheduleValidFrom WHERE id = @AssignmentId;";
                            await conn.ExecuteAsync(sqlUpd, card, trans);
                        }
                        else
                        {
                            string sqlIns = "INSERT INTO employee_schedule_assignments (employee_id, template_id, valid_from) VALUES (@EmployeeId, @NewTemplateId, @NewValidFrom);";
                            await conn.ExecuteAsync(sqlIns, schedBuffer, trans);
                        }

                        // 4. ИСТОРИЯ ОБОРУДОВАНИЯ (Поиск строго по первичному ключу ID строки)
                        if (!eqBuffer.IsNewAssignment)
                        {
                            string sqlUpd = "UPDATE employee_equipment_assignments SET equipment_id = @EquipmentId, valid_from = @EquipmentValidFrom WHERE id = @AssignmentId;";
                            await conn.ExecuteAsync(sqlUpd, card, trans);
                        }
                        else
                        {
                            string sqlIns = "INSERT INTO employee_equipment_assignments (employee_id, equipment_id, valid_from) VALUES (@EmployeeId, @NewEquipmentId, @NewValidFrom);";
                            await conn.ExecuteAsync(sqlIns, eqBuffer, trans);
                        }

                        // 6. МАСШТАБИРУЕМЫЕ КОНТАКТЫ (Полная перезапись списка)
                        string sqlDel = "DELETE FROM employee_contacts WHERE employee_id = @Id;";
                        await conn.ExecuteAsync(sqlDel, new { Id = card.Id }, trans);

                        if (card.Contacts != null)
                        {
                            string insContact = "INSERT INTO employee_contacts (employee_id, contact_type_id, contact_value) VALUES (@EmployeeId, @ContactTypeId, @ContactValue);";
                            foreach (var contact in card.Contacts)
                            {
                                if (!string.IsNullOrWhiteSpace(contact.ContactValue))
                                {
                                    await conn.ExecuteAsync(insContact, new { EmployeeId = card.Id, ContactTypeId = contact.ContactTypeId, ContactValue = contact.ContactValue }, trans);
                                }
                            }
                        }

                        await trans.CommitAsync();
                        return true;
                    }
                    catch (Exception)
                    {
                        await trans.RollbackAsync();
                        throw;
                    }
                }
            }
        }


        // =========================================================================
        // РАЗДЕЛ 3: СПРАВОЧНИКИ ДЛЯ КОМБОБОКСОВ ФОРМЫ (ДОЛЖНОСТИ И СТАНКИ)
        // =========================================================================

        /// <summary>
        /// Возвращает список всех должностей в системе
        /// </summary>
        public async Task<List<PositionLookupDto>> GetPositionsLookupAsync()
        {
            string sql = "SELECT id AS Id, name AS Name, system_role AS SystemRole FROM positions ORDER BY name ASC;";
            using (var conn = new MySqlConnection(_connectionString))
            {
                var res = await conn.QueryAsync<PositionLookupDto>(sql);
                return res.ToList();
            }
        }

        public string GetShortName(string lastName, string firstName, string patronymic)
        {
            // Очищаем строки от случайных пробелов по краям
            string ln = (lastName ?? "").Trim();
            string fn = (firstName ?? "").Trim();
            string pn = (patronymic ?? "").Trim();

            // Если нет даже фамилии или имени, возвращаем то, что есть
            if (string.IsNullOrEmpty(ln) || string.IsNullOrEmpty(fn))
                return $"{ln} {fn}".Trim();

            // Берем первую букву имени
            string firstInitial = $"{fn[0]}.";

            // Проверяем наличие отчества
            if (!string.IsNullOrEmpty(pn))
            {
                // Если отчество есть, берем и его первую букву
                string patronymicInitial = $"{pn[0]}.";
                return $"{ln} {firstInitial} {patronymicInitial}";
            }

            // Если отчества нет, возвращаем только с одним инициалом
            return $"{ln} {firstInitial}";
        }

    }
}
