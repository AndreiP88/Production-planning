using Dapper;
using data;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace database
{
    public class WorkAreaService
    {
        private readonly string _connectionString;

        public WorkAreaService(string connectionString)
        {
            _connectionString = connectionString;
        }

        // =========================================================================
        // 1. БЛОК ПОЛУЧЕНИЯ ДАННЫХ (Ваш оригинальный метод с исправленным ORDER BY)
        // =========================================================================

        /// <summary>
        /// ПОЛУЧЕНИЕ ВСЕЙ СТРУКТУРЫ: Теперь возвращает полную информацию по каждому станку
        /// </summary>
        public async Task<List<WorkAreaInfo>> GetWorkAreasWithEquipmentAsync()
        {
            var todayStr = DateTime.Today.ToString("yyyy-MM-dd");

            string sql = $@"
        SELECT 
            wa.id AS AreaId, 
            wa.name AS AreaName, 
            wa.sort_order AS AreaSortOrder,
            eq.id AS EquipId, 
            eq.name AS EquipName, 
            eq.code AS EquipCode,
            eq.sort_order AS EquipSortOrder,
            eq.work_area_id AS WorkAreaId,
            eq.commissioned_at AS CommissionedAt,
            eq.decommissioned_at AS DecommissionedAt,
            
            -- 1. Режим работы
            COALESCE(
                (SELECT staffing_mode FROM equipment_staffing_history 
                 WHERE equipment_id = eq.id AND valid_from <= '{todayStr}' 
                 ORDER BY valid_from DESC LIMIT 1),
                eq.staffing_mode
            ) AS StaffingMode,
            
            -- Дата начала действия актуального режима работы
            COALESCE(
                (SELECT valid_from FROM equipment_staffing_history 
                 WHERE equipment_id = eq.id AND valid_from <= '{todayStr}' 
                 ORDER BY valid_from DESC LIMIT 1),
                eq.commissioned_at
            ) AS StaffingModeValidFrom,
            
            -- 2. Шаблон/График
            COALESCE(
                (SELECT template_id FROM equipment_schedule_history 
                 WHERE equipment_id = eq.id AND valid_from <= '{todayStr}' 
                 ORDER BY valid_from DESC LIMIT 1),
                eq.template_id
            ) AS TemplateId,
            
            -- Дата начала действия актуального графика
            COALESCE(
                (SELECT valid_from FROM equipment_schedule_history 
                 WHERE equipment_id = eq.id AND valid_from <= '{todayStr}' 
                 ORDER BY valid_from DESC LIMIT 1),
                eq.commissioned_at
            ) AS TemplateValidFrom,
            
            -- Название шаблона
            (SELECT name FROM schedule_templates 
             WHERE id = COALESCE(
                (SELECT template_id FROM equipment_schedule_history 
                 WHERE equipment_id = eq.id AND valid_from <= '{todayStr}' 
                 ORDER BY valid_from DESC LIMIT 1),
                eq.template_id
             )
            ) AS TemplateName
            
        FROM work_areas wa
        LEFT JOIN equipment eq ON wa.id = eq.work_area_id
        ORDER BY wa.sort_order, wa.name, eq.sort_order, eq.name;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                var rows = await connection.QueryAsync<WorkAreaEquipmentRow>(sql, commandType: CommandType.Text);

                var result = rows
                    .GroupBy(r => new { r.AreaId, r.AreaName, r.AreaSortOrder })
                    .Select(areaGroup => new WorkAreaInfo
                    {
                        Id = areaGroup.Key.AreaId,
                        Name = areaGroup.Key.AreaName,
                        SortOrder = areaGroup.Key.AreaSortOrder,
                        Equipments = areaGroup
                            .Where(r => r.EquipId.HasValue)
                            .Select(r => new EquipmentShortInfo
                            {
                                Id = r.EquipId.Value,
                                WorkAreaId = r.WorkAreaId,
                                TemplateId = r.TemplateId,
                                Name = r.EquipName ?? "Без названия",
                                Code = r.EquipCode ?? "Б/К",
                                CommissionedAt = r.CommissionedAt ?? DateTime.MinValue,
                                DecommissionedAt = r.DecommissionedAt,
                                SortOrder = r.EquipSortOrder,
                                StaffingMode = r.StaffingMode ?? "strict_schedule",

                                // Передаем новые данные в расширенную модель
                                TemplateName = r.TemplateName ?? "График не назначен",
                                StaffingModeValidFrom = r.StaffingModeValidFrom,
                                TemplateValidFrom = r.TemplateValidFrom
                            })
                            .ToList()
                    })
                    .ToList();

                return result;
            }
        }




        // =========================================================================
        // 2. БЛОК ИЗМЕНЕНИЯ И УПРАВЛЕНИЯ УЧАСТКАМИ
        // =========================================================================

        /// <summary>
        /// ДОБАВЛЕНИЕ: Создает новый участок и ставит его в конец списка по sort_order
        /// </summary>
        public async Task<int> CreateWorkAreaAsync(string name)
        {
            // COALESCE находит максимальный текущий sort_order и делает +1. Если база пуста — ставит 1.
            const string sql = @"
                INSERT INTO work_areas (name, sort_order) 
                VALUES (@Name, COALESCE((SELECT MAX(sort_order) FROM work_areas wa) + 1, 1));
                SELECT LAST_INSERT_ID();";

            using (var connection = new MySqlConnection(_connectionString))
            {
                return await connection.ExecuteScalarAsync<int>(sql, new { Name = name });
            }
        }

        /// <summary>
        /// РЕДАКТИРОВАНИЕ: Меняет только название существующего участка
        /// </summary>
        public async Task<bool> UpdateWorkAreaNameAsync(int id, string newName)
        {
            const string sql = "UPDATE work_areas SET name = @Name WHERE id = @Id;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                int rowsAffected = await connection.ExecuteAsync(sql, new { Id = id, Name = newName });
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// СОРТИРОВКА: Меняет приоритет (порядок) отображения участка на экране
        /// </summary>
        public async Task<bool> UpdateWorkAreaSortOrderAsync(int id, int newSortOrder)
        {
            const string sql = "UPDATE work_areas SET sort_order = @SortOrder WHERE id = @Id;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                int rowsAffected = await connection.ExecuteAsync(sql, new { Id = id, SortOrder = newSortOrder });
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// БЕЗОПАСНОЕ УДАЛЕНИЕ: Удаляет участок только в том случае, если на нем нет оборудования
        /// </summary>
        public async Task<DeleteResult> DeleteWorkAreaAsync(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                // 1. Проверяем, привязано ли оборудование к этому участку
                const string checkSql = "SELECT EXISTS(SELECT 1 FROM equipment WHERE work_area_id = @Id);";
                bool hasEquipment = await connection.ExecuteScalarAsync<bool>(checkSql, new { Id = id });

                if (hasEquipment)
                {
                    return DeleteResult.Fail("Невозможно удалить участок. На нем все еще числится оборудование. Сначала перенесите оборудование на другой участок.");
                }

                // 2. Если участок пуст — удаляем его
                const string deleteSql = "DELETE FROM work_areas WHERE id = @Id;";
                int rowsAffected = await connection.ExecuteAsync(deleteSql, new { Id = id });

                if (rowsAffected > 0)
                {
                    return DeleteResult.Success();
                }

                return DeleteResult.Fail("Участок не найден в базе данных.");
            }
        }

        // =========================================================================
        // 3. БЛОК УПРАВЛЕНИЯ ОБОРУДОВАНИЕМ (CRUD)
        // =========================================================================

        /*/// <summary>
        /// ДОБАВЛЕНИЕ: Создает новую единицу оборудования с авто-расчетом sort_order для своего участка
        /// </summary>
        public async Task<int> CreateEquipmentAsync(EquipmentModel model)
        {
            // Мы обернули SELECT MAX во вложенный SELECT ... FROM ( ... ) AS temp
            const string sql = @"
                INSERT INTO equipment (work_area_id, template_id, name, code, commissioned_at, decommissioned_at, staffing_mode, sort_order)
                VALUES (@WorkAreaId, @TemplateId, @Name, @Code, @CommissionedAt, @DecommissionedAt, @StaffingMode, 
                        COALESCE((SELECT max_order FROM (SELECT MAX(sort_order) AS max_order FROM equipment WHERE work_area_id = @WorkAreaId) AS temp) + 1, 1));
                SELECT LAST_INSERT_ID();";

            using (var connection = new MySqlConnection(_connectionString))
            {
                return await connection.ExecuteScalarAsync<int>(sql, model);
            }
        }*/

        /// <summary>
        /// Возвращает список всего несписанного оборудования с указанием их участков
        /// </summary>
        public async Task<List<EquipmentLookupDto>> GetEquipmentLookupAsync()
        {
            string sql = "SELECT e.id AS Id, e.name AS Name, e.code AS Code, e.work_area_id AS WorkAreaId, wa.name AS WorkAreaName " +
                         "FROM equipment e " +
                         "JOIN work_areas wa ON e.work_area_id = wa.id " +
                         "WHERE e.decommissioned_at IS NULL OR e.decommissioned_at > CURDATE() " +
                         "ORDER BY wa.name ASC, e.sort_order, e.name ASC;";

            using (var conn = new MySqlConnection(_connectionString))
            {
                var res = await conn.QueryAsync<EquipmentLookupDto>(sql);
                return res.ToList();
            }
        }

        /// <summary>
        /// ПОЛУЧЕНИЕ СТАНКА ПО ID: Возвращает полную информацию о станке, включая актуальный график, режим и даты их начала действия
        /// </summary>
        public async Task<EquipmentShortInfo> GetEquipmentByIdAsync(int id)
        {
            var todayStr = DateTime.Today.ToString("yyyy-MM-dd");

            string sql = $@"
                SELECT 
                    eq.id AS EquipId, 
                    eq.work_area_id AS WorkAreaId, 
                    eq.name AS EquipName, 
                    eq.code AS EquipCode, 
                    eq.commissioned_at AS CommissionedAt, 
                    eq.decommissioned_at AS DecommissionedAt, 
                    eq.sort_order AS EquipSortOrder,
                    
                    -- Актуальный режим работы и дата его начала
                    COALESCE(
                        (SELECT staffing_mode FROM equipment_staffing_history 
                         WHERE equipment_id = eq.id AND valid_from <= '{todayStr}' 
                         ORDER BY valid_from DESC LIMIT 1),
                        eq.staffing_mode
                    ) AS StaffingMode,
                    COALESCE(
                        (SELECT valid_from FROM equipment_staffing_history 
                         WHERE equipment_id = eq.id AND valid_from <= '{todayStr}' 
                         ORDER BY valid_from DESC LIMIT 1),
                        eq.commissioned_at
                    ) AS StaffingModeValidFrom,
                    
                    -- Актуальный ID графика и дата его начала
                    COALESCE(
                        (SELECT template_id FROM equipment_schedule_history 
                         WHERE equipment_id = eq.id AND valid_from <= '{todayStr}' 
                         ORDER BY valid_from DESC LIMIT 1),
                        eq.template_id
                    ) AS TemplateId,
                    COALESCE(
                        (SELECT valid_from FROM equipment_schedule_history 
                         WHERE equipment_id = eq.id AND valid_from <= '{todayStr}' 
                         ORDER BY valid_from DESC LIMIT 1),
                        eq.commissioned_at
                    ) AS TemplateValidFrom,
                    
                    -- Текстовое название графика
                    (SELECT name FROM schedule_templates 
                     WHERE id = COALESCE(
                        (SELECT template_id FROM equipment_schedule_history 
                         WHERE equipment_id = eq.id AND valid_from <= '{todayStr}' 
                         ORDER BY valid_from DESC LIMIT 1),
                        eq.template_id
                     )
                    ) AS TemplateName
                FROM equipment eq 
                WHERE eq.id = @Id;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                // Читаем плоскую строку из базы данных
                var row = await connection.QueryFirstOrDefaultAsync<WorkAreaEquipmentRow>(sql, new { Id = id });

                if (row == null) return null;

                // Маппим данные в расширенную модель с датами
                return new EquipmentShortInfo
                {
                    Id = row.EquipId.Value,
                    WorkAreaId = row.WorkAreaId,
                    TemplateId = row.TemplateId,
                    Name = row.EquipName ?? "Без названия",
                    Code = row.EquipCode ?? "Б/К",
                    CommissionedAt = row.CommissionedAt ?? DateTime.MinValue,
                    DecommissionedAt = row.DecommissionedAt,
                    SortOrder = row.EquipSortOrder,
                    StaffingMode = row.StaffingMode ?? "strict_schedule",

                    TemplateName = row.TemplateName ?? "График не назначен",
                    StaffingModeValidFrom = row.StaffingModeValidFrom,
                    TemplateValidFrom = row.TemplateValidFrom
                };
            }
        }

        /// <summary>
        /// ПОЛНАЯ КАРТОЧКА ПО ID: Считывает все данные станка с актуальной историей на сегодня
        /// </summary>
        public async Task<EquipmentFullCard> GetEquipmentFullCardAsync(int id)
        {
            var todayStr = DateTime.Today.ToString("yyyy-MM-dd");

            string sql = $@"
                SELECT 
                    eq.id AS Id, eq.work_area_id AS WorkAreaId, eq.name AS Name, eq.code AS Code, 
                    eq.commissioned_at AS CommissionedAt, eq.decommissioned_at AS DecommissionedAt, 
                    eq.sort_order AS SortOrder,
                    COALESCE(
                        (SELECT staffing_mode FROM equipment_staffing_history 
                         WHERE equipment_id = eq.id AND valid_from <= '{todayStr}' 
                         ORDER BY valid_from DESC LIMIT 1),
                        eq.staffing_mode
                    ) AS StaffingMode,
                    COALESCE(
                        (SELECT valid_from FROM equipment_staffing_history 
                         WHERE equipment_id = eq.id AND valid_from <= '{todayStr}' 
                         ORDER BY valid_from DESC LIMIT 1),
                        eq.commissioned_at
                    ) AS StaffingModeValidFrom,
                    COALESCE(
                        (SELECT template_id FROM equipment_schedule_history 
                         WHERE equipment_id = eq.id AND valid_from <= '{todayStr}' 
                         ORDER BY valid_from DESC LIMIT 1),
                        eq.template_id
                    ) AS TemplateId,
                    COALESCE(
                        (SELECT valid_from FROM equipment_schedule_history 
                         WHERE equipment_id = eq.id AND valid_from <= '{todayStr}' 
                         ORDER BY valid_from DESC LIMIT 1),
                        eq.commissioned_at
                    ) AS TemplateValidFrom,
                    (SELECT name FROM schedule_templates 
                     WHERE id = COALESCE(
                        (SELECT template_id FROM equipment_schedule_history 
                         WHERE equipment_id = eq.id AND valid_from <= '{todayStr}' 
                         ORDER BY valid_from DESC LIMIT 1),
                        eq.template_id
                     )
                    ) AS TemplateName
                FROM equipment eq 
                WHERE eq.id = @Id;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<EquipmentFullCard>(sql, new { Id = id });
            }
        }

        /// <summary>
        /// ДОБАВЛЕНИЕ: Создает станок и сразу пишет начальную точку в историю на основе дат из модели
        /// </summary>
        public async Task<int> CreateEquipmentAsync(EquipmentFullCard model)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var tx = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        // 1. Вставляем станок с авто-порядком
                        const string insertEquipSql = @"
                            INSERT INTO equipment (work_area_id, template_id, name, code, commissioned_at, decommissioned_at, staffing_mode, sort_order)
                            VALUES (@WorkAreaId, @TemplateId, @Name, @Code, @CommissionedAt, @DecommissionedAt, @StaffingMode, 
                                    COALESCE((SELECT max_order FROM (SELECT MAX(sort_order) AS max_order FROM equipment WHERE work_area_id = @WorkAreaId) AS temp) + 1, 1));
                            SELECT LAST_INSERT_ID();";

                        int equipmentId = await connection.ExecuteScalarAsync<int>(insertEquipSql, model, tx);

                        // 2. Стартовая запись графиков (берем дату из TemplateValidFrom, если пустая — из ввода в эксплуатацию)
                        const string insertSchedHistory = @"
                            INSERT INTO equipment_schedule_history (equipment_id, template_id, valid_from)
                            VALUES (@EquipmentId, @TemplateId, @ValidFrom);";

                        await connection.ExecuteAsync(insertSchedHistory, new
                        {
                            EquipmentId = equipmentId,
                            TemplateId = model.TemplateId,
                            ValidFrom = model.TemplateValidFrom ?? model.CommissionedAt
                        }, tx);

                        // 3. Стартовая запись режимов (берем дату из StaffingModeValidFrom, если пустая — из ввода в эксплуатацию)
                        const string insertStaffHistory = @"
                            INSERT INTO equipment_staffing_history (equipment_id, staffing_mode, valid_from)
                            VALUES (@EquipmentId, @StaffingMode, @ValidFrom);";

                        await connection.ExecuteAsync(insertStaffHistory, new
                        {
                            EquipmentId = equipmentId,
                            StaffingMode = model.StaffingMode,
                            ValidFrom = model.StaffingModeValidFrom ?? model.CommissionedAt
                        }, tx);

                        await tx.CommitAsync();
                        return equipmentId;
                    }
                    catch
                    {
                        await tx.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// ГЛОБАЛЬНОЕ СОХРАНЕНИЕ КАРТОЧКИ СТАНКА С УМНЫМ УЧЕТОМ БУФЕРА
        /// </summary>
        public async Task<bool> SaveEquipmentTransactionAsync(
            EquipmentFullCard card,
            DateTime oldTemplateValidFrom,
            DateTime oldStaffingValidFrom,
            List<PendingScheduleAssignment> pendingSchedules,
            List<PendingStaffingAssignment> pendingStaffing)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var tx = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        // 1. Обновляем анкетные поля
                        const string updateEquipSql = @"
                            UPDATE equipment SET work_area_id = @WorkAreaId, name = @Name, code = @Code,
                                commissioned_at = @CommissionedAt, decommissioned_at = @DecommissionedAt WHERE id = @Id;";
                        await connection.ExecuteAsync(updateEquipSql, card, tx);

                        bool hasNewSchedules = pendingSchedules != null && pendingSchedules.Any();
                        bool hasNewStaffing = pendingStaffing != null && pendingStaffing.Any();

                        // 2. ГРАФИК: Исправляем старую запись, только если буфер пуст
                        if (!hasNewSchedules)
                        {
                            const string correctSchedSql = @"
                                UPDATE equipment_schedule_history SET template_id = @TemplateId, valid_from = @NewValidFrom 
                                WHERE equipment_id = @EquipmentId AND valid_from = @OldValidFrom;";
                            await connection.ExecuteAsync(correctSchedSql, new { TemplateId = card.TemplateId, NewValidFrom = card.TemplateValidFrom?.Date, EquipmentId = card.Id, OldValidFrom = oldTemplateValidFrom.Date }, tx);
                        }

                        // 3. РЕЖИМ: Исправляем старую запись, только если буфер пуст
                        if (!hasNewStaffing)
                        {
                            const string correctStaffSql = @"
                                UPDATE equipment_staffing_history SET staffing_mode = @StaffingMode, valid_from = @NewValidFrom 
                                WHERE equipment_id = @EquipmentId AND valid_from = @OldValidFrom;";
                            await connection.ExecuteAsync(correctStaffSql, new { StaffingMode = card.StaffingMode, NewValidFrom = card.StaffingModeValidFrom?.Date, EquipmentId = card.Id, OldValidFrom = oldStaffingValidFrom.Date }, tx);
                        }

                        // 4. Накатываем новые независимые ГРАФИКИ из буфера
                        if (hasNewSchedules)
                        {
                            const string insSched = "INSERT INTO equipment_schedule_history (equipment_id, template_id, valid_from) VALUES (@EquipmentId, @TemplateId, @ValidFrom) ON DUPLICATE KEY UPDATE template_id = @TemplateId;";
                            foreach (var sched in pendingSchedules)
                            {
                                await connection.ExecuteAsync(insSched, new { EquipmentId = card.Id, TemplateId = sched.TemplateId, ValidFrom = sched.ValidFrom.Date }, tx);
                            }
                        }

                        // 5. Накатываем новые независимые РЕЖИМЫ из буфера
                        if (hasNewStaffing)
                        {
                            const string insStaff = "INSERT INTO equipment_staffing_history (equipment_id, staffing_mode, valid_from) VALUES (@EquipmentId, @StaffingMode, @ValidFrom) ON DUPLICATE KEY UPDATE staffing_mode = @StaffingMode;";
                            foreach (var staff in pendingStaffing)
                            {
                                await connection.ExecuteAsync(insStaff, new { EquipmentId = card.Id, StaffingMode = staff.StaffingMode, ValidFrom = staff.ValidFrom.Date }, tx);
                            }
                        }

                        await tx.CommitAsync();
                        return true;
                    }
                    catch { await tx.RollbackAsync(); return false; }
                }
            }
        }

        /// <summary>
        /// ЗАПРОС ВОЗРАСТА ЗАПИСЕЙ: Считает возраст записей в днях от даты их valid_from
        /// </summary>
        public async Task<(int ScheduleAge, int StaffingAge)> GetCurrentAssignmentsAgeAsync(int equipmentId, DateTime scheduleDate, DateTime staffingDate)
        {
            const string qSched = "SELECT id FROM equipment_schedule_history WHERE equipment_id = @Id AND valid_from = @OldDate;";
            const string qStaff = "SELECT id FROM equipment_staffing_history WHERE equipment_id = @Id AND valid_from = @OldDate;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                var schedRow = await connection.QueryFirstOrDefaultAsync<int?>(qSched, new { Id = equipmentId, OldDate = scheduleDate.Date });
                var staffRow = await connection.QueryFirstOrDefaultAsync<int?>(qStaff, new { Id = equipmentId, OldDate = staffingDate.Date });

                int schedAge = schedRow.HasValue ? (DateTime.Today - scheduleDate.Date).Days : 0;
                int staffAge = staffRow.HasValue ? (DateTime.Today - staffingDate.Date).Days : 0;

                return (schedAge, staffAge);
            }
        }

        /// <summary>
        /// БЕЗОПАСНОЕ УДАЛЕНИЕ: Проверяет использование оборудования в планах, назначениях и истории до удаления
        /// </summary>
        public async Task<DeleteResult> DeleteEquipmentAsync(int equipmentId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                // 1. Проверяем историю расписаний оборудования
                bool inHistory = await connection.ExecuteScalarAsync<bool>(
                    "SELECT EXISTS(SELECT 1 FROM equipment_schedule_history WHERE equipment_id = @Id);", new { Id = equipmentId });
                if (inHistory) return DeleteResult.Fail("Невозможно удалить оборудование. Оно задействовано в истории шаблонов расписаний.");

                // 2. Проверяем планы работы оборудования
                bool inPlans = await connection.ExecuteScalarAsync<bool>(
                    "SELECT EXISTS(SELECT 1 FROM equipment_daily_plan WHERE equipment_id = @Id);", new { Id = equipmentId });
                if (inPlans) return DeleteResult.Fail("Невозможно удалить оборудование. По нему уже составлены ежедневные планы работы.");

                // 3. Проверяем назначения сотрудников на это оборудование
                bool inEmployeeAssignments = await connection.ExecuteScalarAsync<bool>(
                    "SELECT EXISTS(SELECT 1 FROM employee_equipment_assignments WHERE equipment_id = @Id);", new { Id = equipmentId });
                if (inEmployeeAssignments) return DeleteResult.Fail("Невозможно удалить оборудование. За ним закреплены сотрудники в истории назначений.");

                // 4. Проверяем ручные корректировки (замены)
                bool inOverrides = await connection.ExecuteScalarAsync<bool>(
                    "SELECT EXISTS(SELECT 1 FROM schedule_overrides WHERE equipment_id = @Id);", new { Id = equipmentId });
                if (inOverrides) return DeleteResult.Fail("Невозможно удалить оборудование. Оно фигурирует в ручных корректировках смен.");

                // Если проверок нет — удаляем
                int rows = await connection.ExecuteAsync("DELETE FROM equipment WHERE id = @Id;", new { Id = equipmentId });
                return rows > 0 ? DeleteResult.Success() : DeleteResult.Fail("Оборудование не найдено.");
            }
        }

        // =========================================================================
        // 4. БЛОК СОРТИРОВКИ ОБОРУДОВАНИЯ (КНОПКИ ВВЕРХ / ВНИЗ)
        // =========================================================================

        /// <summary>
        /// ПЕРЕМЕСТИТЬ ВЫШЕ: Меняет местами текущее оборудование с тем, что стоит перед ним на этом же участке
        /// </summary>
        public async Task<bool> MoveEquipmentUpAsync(int equipmentId, int workAreaId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var tx = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        // 1. Получаем текущий sort_order выбранного станка
                        int currentOrder = await connection.ExecuteScalarAsync<int>(
                            "SELECT sort_order FROM equipment WHERE id = @Id;", new { Id = equipmentId }, tx);

                        // 2. Находим станок, у которого sort_order максимальный среди тех, что МЕНЬШЕ текущего (то есть прямо над ним)
                        const string neighborSql = @"
                            SELECT id FROM equipment 
                            WHERE work_area_id = @WorkAreaId AND sort_order < @CurrentOrder 
                            ORDER BY sort_order DESC LIMIT 1;";

                        int? neighborId = await connection.QueryFirstOrDefaultAsync<int?>(neighborSql, new { WorkAreaId = workAreaId, CurrentOrder = currentOrder }, tx);

                        if (neighborId == null) return false; // Выше никого нет, станок уже самый первый

                        // 3. Получаем sort_order соседа
                        int neighborOrder = await connection.ExecuteScalarAsync<int>(
                            "SELECT sort_order FROM equipment WHERE id = @Id;", new { Id = neighborId }, tx);

                        // 4. Меняем их местами в базе данных
                        await connection.ExecuteAsync("UPDATE equipment SET sort_order = @NeighborOrder WHERE id = @CurrentId;", new { NeighborOrder = neighborOrder, CurrentId = equipmentId }, tx);
                        await connection.ExecuteAsync("UPDATE equipment SET sort_order = @CurrentOrder WHERE id = @NeighborId;", new { CurrentOrder = currentOrder, NeighborId = neighborId }, tx);

                        await tx.CommitAsync();
                        return true;
                    }
                    catch { await tx.RollbackAsync(); return false; }
                }
            }
        }

        /// <summary>
        /// ПЕРЕМЕСТИТЬ НИЖЕ: Меняет местами текущее оборудование с тем, что стоит после него на этом же участке
        /// </summary>
        public async Task<bool> MoveEquipmentDownAsync(int equipmentId, int workAreaId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var tx = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        // 1. Получаем текущий sort_order выбранного станка
                        int currentOrder = await connection.ExecuteScalarAsync<int>(
                            "SELECT sort_order FROM equipment WHERE id = @Id;", new { Id = equipmentId }, tx);

                        // 2. Находим станок, у которого sort_order минимальный среди тех, что БОЛЬШЕ текущего (то есть прямо под ним)
                        const string neighborSql = @"
                            SELECT id FROM equipment 
                            WHERE work_area_id = @WorkAreaId AND sort_order > @CurrentOrder 
                            ORDER BY sort_order ASC LIMIT 1;";

                        int? neighborId = await connection.QueryFirstOrDefaultAsync<int?>(neighborSql, new { WorkAreaId = workAreaId, CurrentOrder = currentOrder }, tx);

                        if (neighborId == null) return false; // Ниже никого нет, станок уже последний

                        // 3. Получаем sort_order соседа
                        int neighborOrder = await connection.ExecuteScalarAsync<int>(
                            "SELECT sort_order FROM equipment WHERE id = @Id;", new { Id = neighborId }, tx);

                        // 4. Меняем их местами в базе данных
                        await connection.ExecuteAsync("UPDATE equipment SET sort_order = @NeighborOrder WHERE id = @CurrentId;", new { NeighborOrder = neighborOrder, CurrentId = equipmentId }, tx);
                        await connection.ExecuteAsync("UPDATE equipment SET sort_order = @CurrentOrder WHERE id = @NeighborId;", new { CurrentOrder = currentOrder, NeighborId = neighborId }, tx);

                        await tx.CommitAsync();
                        return true;
                    }
                    catch { await tx.RollbackAsync(); return false; }
                }
            }
        }

        // =========================================================================
        // РАЗДЕЛ 5: ПРОСМОТР ПОЛНОЙ ИСТОРИИ ИЗМЕНЕНИЙ СТАНКА (ТАБЛИЦЫ ИСТОРИИ)
        // =========================================================================

        /// <summary>
        /// ИСТОРИЯ ГРАФИКОВ: Возвращает хронологический список всех назначенных станoutput графиков
        /// </summary>
        public async Task<List<EquipmentScheduleHistoryRow>> GetEquipmentScheduleHistoryAsync(int equipmentId)
        {
            const string sql = @"
                SELECT 
                    esh.id AS Id,
                    esh.template_id AS TemplateId,
                    st.name AS TemplateName,
                    esh.valid_from AS ValidFrom
                FROM equipment_schedule_history esh
                JOIN schedule_templates st ON esh.template_id = st.id
                WHERE esh.equipment_id = @EquipmentId
                ORDER BY esh.valid_from DESC, esh.id DESC;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<EquipmentScheduleHistoryRow>(sql, new { EquipmentId = equipmentId });
                return result.ToList();
            }
        }

        /// <summary>
        /// ИСТОРИЯ РЕЖИМОВ: Возвращает хронологический список всех изменений режима работы станка
        /// </summary>
        public async Task<List<EquipmentStaffingHistoryRow>> GetEquipmentStaffingHistoryAsync(int equipmentId)
        {
            const string sql = @"
                SELECT 
                    id AS Id,
                    staffing_mode AS StaffingMode,
                    valid_from AS ValidFrom
                FROM equipment_staffing_history
                WHERE equipment_id = @EquipmentId
                ORDER BY valid_from DESC, id DESC;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<EquipmentStaffingHistoryRow>(sql, new { EquipmentId = equipmentId });
                return result.ToList();
            }
        }

    }
}
