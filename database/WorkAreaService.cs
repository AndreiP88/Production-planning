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
        /// ДОБАВЛЕНИЕ: Создает станок и сразу пишет начальную точку в историю на основе дат из модели
        /// </summary>
        public async Task<int> CreateEquipmentAsync(EquipmentShortInfo model)
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
        /// ОБНОВЛЕНИЕ С УМНОЙ ИСТОРИЕЙ: Корректно обрабатывает создание новых точек истории, 
        /// обновление значений на ту же дату и редактирование дат для существующих записей.
        /// </summary>
        public async Task<bool> UpdateEquipmentWithHistoryAsync(
            EquipmentShortInfo model,
            DateTime oldTemplateValidFrom,
            DateTime oldStaffingModeValidFrom)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var tx = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        // 1. Получаем базовые дефолты из таблицы equipment на случай, если истории нет
                        const string getBaseSql = "SELECT template_id, staffing_mode FROM equipment WHERE id = @Id;";
                        var baseData = await connection.QueryFirstOrDefaultAsync(getBaseSql, new { Id = model.Id }, tx);

                        // 2. Обновляем базовые текстовые поля станка
                        const string updateEquipSql = @"
                            UPDATE equipment 
                            SET work_area_id = @WorkAreaId,
                                name = @Name,
                                code = @Code,
                                commissioned_at = @CommissionedAt,
                                decommissioned_at = @DecommissionedAt
                            WHERE id = @Id;";
                        await connection.ExecuteAsync(updateEquipSql, model, tx);

                        // Целевые даты из UI формы
                        DateTime newScheduleDate = model.TemplateValidFrom ?? DateTime.Today;
                        DateTime newStaffingDate = model.StaffingModeValidFrom ?? DateTime.Today;

                        // =========================================================================
                        // БЛОК ГРАФИКА (SCHEDULE HISTORY)
                        // =========================================================================

                        // Пытаемся взять значение из истории по старой дате
                        const string getActualSched = "SELECT template_id FROM equipment_schedule_history WHERE equipment_id = @Id AND valid_from = @OldValidFrom;";
                        int? lastSavedTemplateId = await connection.QueryFirstOrDefaultAsync<int?>(getActualSched, new { Id = model.Id, OldValidFrom = oldTemplateValidFrom }, tx);

                        // Если в истории записей нет — подставляем дефолтное значение из самого станка
                        if (!lastSavedTemplateId.HasValue && baseData != null)
                        {
                            lastSavedTemplateId = baseData.template_id;
                        }

                        // Проверяем, есть ли запись в истории строго на НОВУЮ выбранную дату
                        const string checkSchedSql = "SELECT template_id FROM equipment_schedule_history WHERE equipment_id = @Id AND valid_from = @ValidFrom;";
                        int? existingTemplateId = await connection.QueryFirstOrDefaultAsync<int?>(checkSchedSql, new { Id = model.Id, ValidFrom = newScheduleDate }, tx);

                        if (existingTemplateId.HasValue)
                        {
                            // Сценарий 1: На эту дату запись есть — обновляем (UPDATE)
                            const string updateSched = "UPDATE equipment_schedule_history SET template_id = @TemplateId WHERE equipment_id = @EquipmentId AND valid_from = @ValidFrom;";
                            await connection.ExecuteAsync(updateSched, new { TemplateId = model.TemplateId, EquipmentId = model.Id, ValidFrom = newScheduleDate }, tx);
                        }
                        else
                        {
                            // Сценарий 2: График ИЗМЕНИЛСЯ — создаем точку истории (INSERT)
                            if (lastSavedTemplateId != model.TemplateId)
                            {
                                const string insertSched = "INSERT INTO equipment_schedule_history (equipment_id, template_id, valid_from) VALUES (@EquipmentId, @TemplateId, @ValidFrom);";
                                await connection.ExecuteAsync(insertSched, new { EquipmentId = model.Id, TemplateId = model.TemplateId, ValidFrom = newScheduleDate }, tx);
                            }
                            // Сценарий 3: График тот же, но поменялась дата (и только если в истории БЫЛО что двигать!)
                            else if (newScheduleDate != oldTemplateValidFrom && oldTemplateValidFrom != model.CommissionedAt)
                            {
                                const string shiftSchedDate = "UPDATE equipment_schedule_history SET valid_from = @NewValidFrom WHERE equipment_id = @EquipmentId AND valid_from = @OldValidFrom;";
                                await connection.ExecuteAsync(shiftSchedDate, new { NewValidFrom = newScheduleDate, EquipmentId = model.Id, OldValidFrom = oldTemplateValidFrom }, tx);
                            }
                            // Сценарий 4: Истории не было вообще, график не менялся, но дату сдвинули — создаем ПЕРВУЮ запись истории
                            else if (newScheduleDate != oldTemplateValidFrom && oldTemplateValidFrom == model.CommissionedAt)
                            {
                                const string insertFirstSched = "INSERT INTO equipment_schedule_history (equipment_id, template_id, valid_from) VALUES (@EquipmentId, @TemplateId, @ValidFrom);";
                                await connection.ExecuteAsync(insertFirstSched, new { EquipmentId = model.Id, TemplateId = model.TemplateId, ValidFrom = newScheduleDate }, tx);
                            }
                        }

                        // =========================================================================
                        // БЛОК РЕЖИМА (STAFFING HISTORY)
                        // =========================================================================

                        // Пытаемся взять значение из истории по старой дате
                        const string getActualStaff = "SELECT staffing_mode FROM equipment_staffing_history WHERE equipment_id = @Id AND valid_from = @OldValidFrom;";
                        string lastSavedStaffMode = await connection.QueryFirstOrDefaultAsync<string>(getActualStaff, new { Id = model.Id, OldValidFrom = oldStaffingModeValidFrom }, tx);

                        // Если в истории пусто — подставляем дефолтный режим работы из станка
                        if (lastSavedStaffMode == null && baseData != null)
                        {
                            lastSavedStaffMode = baseData.staffing_mode;
                        }

                        // Проверяем существование записи на новую дату
                        const string checkStaffSql = "SELECT staffing_mode FROM equipment_staffing_history WHERE equipment_id = @Id AND valid_from = @ValidFrom;";
                        string existingStaffingMode = await connection.QueryFirstOrDefaultAsync<string>(checkStaffSql, new { Id = model.Id, ValidFrom = newStaffingDate }, tx);

                        if (existingStaffingMode != null)
                        {
                            // Сценарий 1: На эту дату запись есть — обновляем (UPDATE)
                            const string updateStaff = "UPDATE equipment_staffing_history SET staffing_mode = @StaffingMode WHERE equipment_id = @EquipmentId AND valid_from = @ValidFrom;";
                            await connection.ExecuteAsync(updateStaff, new { StaffingMode = model.StaffingMode, EquipmentId = model.Id, ValidFrom = newStaffingDate }, tx);
                        }
                        else
                        {
                            // Сценарий 2: Режим ИЗМЕНИЛСЯ — создаем точку истории (INSERT)
                            if (lastSavedStaffMode != model.StaffingMode)
                            {
                                const string insertStaff = "INSERT INTO equipment_staffing_history (equipment_id, staffing_mode, valid_from) VALUES (@EquipmentId, @StaffingMode, @ValidFrom);";
                                await connection.ExecuteAsync(insertStaff, new { EquipmentId = model.Id, StaffingMode = model.StaffingMode, ValidFrom = newStaffingDate }, tx);
                            }
                            // Сценарий 3: Режим тот же, изменилась дата (если в истории была запись)
                            else if (newStaffingDate != oldStaffingModeValidFrom && oldStaffingModeValidFrom != model.CommissionedAt)
                            {
                                const string shiftStaffDate = "UPDATE equipment_staffing_history SET valid_from = @NewValidFrom WHERE equipment_id = @EquipmentId AND valid_from = @OldValidFrom;";
                                await connection.ExecuteAsync(shiftStaffDate, new { NewValidFrom = newStaffingDate, EquipmentId = model.Id, OldValidFrom = oldStaffingModeValidFrom }, tx);
                            }
                            // Сценарий 4: Истории не было, режим тот же, но дату изменили — создаем первую точку истории
                            else if (newStaffingDate != oldStaffingModeValidFrom && oldStaffingModeValidFrom == model.CommissionedAt)
                            {
                                const string insertFirstStaff = "INSERT INTO equipment_staffing_history (equipment_id, staffing_mode, valid_from) VALUES (@EquipmentId, @StaffingMode, @ValidFrom);";
                                await connection.ExecuteAsync(insertFirstStaff, new { EquipmentId = model.Id, StaffingMode = model.StaffingMode, ValidFrom = newStaffingDate }, tx);
                            }
                        }

                        await tx.CommitAsync();
                        return true;
                    }
                    catch
                    {
                        await tx.RollbackAsync(); return false;
                    }
                }
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

    }
}
