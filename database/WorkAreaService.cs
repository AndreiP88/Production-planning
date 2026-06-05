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
            const string sql = @"
                                SELECT 
                                    wa.id AS AreaId, 
                                    wa.name AS AreaName, 
                                    wa.sort_order AS AreaSortOrder,
                                    eq.id AS EquipId, 
                                    eq.name AS EquipName, 
                                    eq.code AS EquipCode,
                                    eq.sort_order AS EquipSortOrder,
                                    eq.work_area_id AS WorkAreaId,
                                    eq.template_id AS TemplateId,
                                    eq.commissioned_at AS CommissionedAt,
                                    eq.decommissioned_at AS DecommissionedAt,
                                    eq.staffing_mode AS StaffingMode,
                                    st.name AS TemplateName
                                FROM work_areas wa
                                LEFT JOIN equipment eq ON wa.id = eq.work_area_id
                                LEFT JOIN schedule_templates st ON eq.template_id = st.id
                                ORDER BY wa.sort_order, wa.name, eq.sort_order, eq.name;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                var rows = await connection.QueryAsync<WorkAreaEquipmentRow>(sql);

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
                                // Заполняем базовые свойства (наследованы от EquipmentModel)
                                Id = r.EquipId.Value,
                                WorkAreaId = r.WorkAreaId,
                                TemplateId = r.TemplateId,
                                Name = r.EquipName ?? "Без названия",
                                Code = r.EquipCode ?? "Б/К",
                                CommissionedAt = r.CommissionedAt ?? DateTime.MinValue,
                                DecommissionedAt = r.DecommissionedAt,
                                StaffingMode = r.StaffingMode ?? "strict_schedule",
                                SortOrder = r.EquipSortOrder,

                                // Заполняем расширенные свойства (кастомные для UI)
                                TemplateName = r.TemplateName ?? "График не назначен"
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

        /// <summary>
        /// ДОБАВЛЕНИЕ: Создает новую единицу оборудования с авто-расчетом sort_order для своего участка
        /// </summary>
        public async Task<int> CreateEquipmentAsync(EquipmentModel model)
        {
            const string sql = @"
                INSERT INTO equipment (work_area_id, template_id, name, code, commissioned_at, decommissioned_at, staffing_mode, sort_order)
                VALUES (@WorkAreaId, @TemplateId, @Name, @Code, @CommissionedAt, @DecommissionedAt, @StaffingMode, 
                        COALESCE((SELECT MAX(sort_order) FROM equipment WHERE work_area_id = @WorkAreaId) + 1, 1));
                SELECT LAST_INSERT_ID();";

            using (var connection = new MySqlConnection(_connectionString))
            {
                return await connection.ExecuteScalarAsync<int>(sql, model);
            }
        }

        /// <summary>
        /// ПОЛУЧЕНИЕ ОДНОГО СТАНКА ПО ID: Если нужно открыть форму редактирования отдельно
        /// </summary>
        public async Task<EquipmentModel> GetEquipmentByIdAsync(int id)
        {
            const string sql = @"
                SELECT id AS Id, work_area_id AS WorkAreaId, template_id AS TemplateId, 
                       name AS Name, code AS Code, commissioned_at AS CommissionedAt, 
                       decommissioned_at AS DecommissionedAt, staffing_mode AS StaffingMode, 
                       sort_order AS SortOrder 
                FROM equipment WHERE id = @Id;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                // Возвращает чистую базовую модель, готовую для отправки обратно через UPDATE
                return await connection.QueryFirstOrDefaultAsync<EquipmentModel>(sql, new { Id = id });
            }
        }

        /// <summary>
        /// СОХРАНЕНИЕ ИЗМЕНЕНИЙ: Принимает базовый класс и обновляет MySQL
        /// </summary>
        public async Task<bool> UpdateEquipmentAsync(EquipmentModel model)
        {
            const string sql = @"
                UPDATE equipment 
                SET work_area_id = @WorkAreaId,
                    template_id = @TemplateId,
                    name = @Name,
                    code = @Code,
                    commissioned_at = @CommissionedAt,
                    decommissioned_at = @DecommissionedAt,
                    staffing_mode = @StaffingMode
                WHERE id = @Id;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                // Сюда можно передавать как EquipmentModel, так и EquipmentShortInfo (благодаря наследованию!)
                int rows = await connection.ExecuteAsync(sql, model);
                return rows > 0;
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
