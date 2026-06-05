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
    public class ScheduleTemplateService
    {
        private readonly string _connectionString;

        public ScheduleTemplateService(string connectionString)
        {
            _connectionString = connectionString;
        }

        // =========================================================================
        // БЛОК 1: РАБОТА С ЦИКЛИЧЕСКИМИ ГРАФИКАМИ (СХЕМАМИ)
        // =========================================================================

        /// <summary>
        /// ПОЛУЧИТЬ ВСЕ ГРАФИКИ: Возвращает список всех схем с вложенными днями и сменами
        /// </summary>
        public async Task<List<ScheduleCycleModel>> GetAllCyclesAsync()
        {
            const string sql = @"
            SELECT sc.id AS CycleId, sc.name AS CycleName, sc.cycle_length AS CycleLength,
                   sci.day_number AS DayNumber, sci.shift_id AS ShiftId, sd.shift_number AS ShiftNumber, sd.name AS ShiftName
            FROM schedule_cycles sc
            LEFT JOIN schedule_cycle_items sci ON sc.id = sci.cycle_id
            LEFT JOIN shift_definitions sd ON sci.shift_id = sd.id
            ORDER BY sc.name, sci.day_number;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                var rows = await connection.QueryAsync<CycleItemDbRow>(sql);
                return rows.GroupBy(r => new { r.CycleId, r.CycleName, r.CycleLength })
                    .Select(g => new ScheduleCycleModel
                    {
                        Id = g.Key.CycleId,
                        Name = g.Key.CycleName,
                        CycleLength = g.Key.CycleLength,
                        Items = g.Where(r => r.DayNumber.HasValue)
                                 .Select(r => new CycleItemModel { DayNumber = r.DayNumber.Value, ShiftId = r.ShiftId.Value, ShiftNumber = r.ShiftNumber.Value, ShiftName = r.ShiftName })
                                 .ToList()
                    }).ToList();
            }
        }

        /// <summary>
        /// СОЗДАТЬ ГРАФИК: Вставляет схему и ее дни в рамках одной транзакции
        /// </summary>
        public async Task<int> CreateScheduleCycleAsync(ScheduleCycleModel cycle)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var tx = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        int cycleId = await connection.ExecuteScalarAsync<int>(
                            "INSERT INTO schedule_cycles (name, cycle_length) VALUES (@Name, @CycleLength); SELECT LAST_INSERT_ID();", cycle, tx);

                        if (cycle.Items != null && cycle.Items.Any())
                        {
                            await connection.ExecuteAsync(
                                "INSERT INTO schedule_cycle_items (cycle_id, day_number, shift_id) VALUES (@CycleId, @DayNumber, @ShiftId);",
                                cycle.Items.Select(i => new { CycleId = cycleId, i.DayNumber, i.ShiftId }), tx);
                        }
                        await tx.CommitAsync();
                        return cycleId;
                    }
                    catch { await tx.RollbackAsync(); throw; }
                }
            }
        }

        /// <summary>
        /// РЕДАКТИРОВАТЬ ГРАФИК: Обновляет шапку схемы, удаляет старые дни и записывает новые
        /// </summary>
        public async Task<bool> UpdateScheduleCycleAsync(ScheduleCycleModel cycle)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var tx = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        // 1. Обновляем имя и длину цикла
                        await connection.ExecuteAsync(
                            "UPDATE schedule_cycles SET name = @Name, cycle_length = @CycleLength WHERE id = @Id;", cycle, tx);

                        // 2. Очищаем старую сетку дней для этого цикла
                        await connection.ExecuteAsync("DELETE FROM schedule_cycle_items WHERE cycle_id = @Id;", new { Id = cycle.Id }, tx);

                        // 3. Записываем обновленную сетку дней
                        if (cycle.Items != null && cycle.Items.Any())
                        {
                            await connection.ExecuteAsync(
                                "INSERT INTO schedule_cycle_items (cycle_id, day_number, shift_id) VALUES (@CycleId, @DayNumber, @ShiftId);",
                                cycle.Items.Select(i => new { CycleId = cycle.Id, i.DayNumber, i.ShiftId }), tx);
                        }
                        await tx.CommitAsync();
                        return true;
                    }
                    catch { await tx.RollbackAsync(); return false; }
                }
            }
        }

        // =========================================================================
        // БЛОК 2: РАБОТА С БРИГАДАМИ (ШАБЛОНАМИ С ОПОРНОЙ ДАТОЙ)
        // =========================================================================

        /// <summary>
        /// ПОЛУЧИТЬ ВСЕ БРИГАДЫ: Возвращает список бригад с названиями их базовых графиков
        /// </summary>
        public async Task<List<ScheduleTemplateModel>> GetAllTemplatesAsync()
        {
            const string sql = @"
            SELECT st.id AS Id, st.name AS Name, st.base_date AS BaseDate, 
                   st.cycle_id AS CycleId, sc.name AS CycleName
            FROM schedule_templates st
            JOIN schedule_cycles sc ON st.cycle_id = sc.id
            ORDER BY st.name;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<ScheduleTemplateModel>(sql);
                return result.ToList();
            }
        }

        /// <summary>
        /// ПОЛУЧИТЬ БРИГАДУ ПО ID: Для формы редактирования конкретной бригады
        /// </summary>
        public async Task<ScheduleTemplateModel> GetTemplateByIdAsync(int id)
        {
            const string sql = @"
            SELECT id AS Id, name AS Name, base_date AS BaseDate, cycle_id AS CycleId 
            FROM schedule_templates WHERE id = @Id;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<ScheduleTemplateModel>(sql, new { Id = id });
            }
        }

        /// <summary>
        /// СОЗДАТЬ БРИГАДУ: Привязывает выбранный график к имени бригады и опорной дате
        /// </summary>
        public async Task<int> CreateTemplateBindingAsync(ScheduleTemplateModel template)
        {
            const string sql = @"
            INSERT INTO schedule_templates (name, cycle_id, base_date) 
            VALUES (@Name, @CycleId, @BaseDate);
            SELECT LAST_INSERT_ID();";

            using (var connection = new MySqlConnection(_connectionString))
            {
                return await connection.ExecuteScalarAsync<int>(sql, template);
            }
        }

        /// <summary>
        /// РЕДАКТИРОВАТЬ БРИГАДУ: Позволяет сменить имя бригады, перепривязать к другой схеме или сдвинуть опорную дату
        /// </summary>
        public async Task<bool> UpdateTemplateBindingAsync(ScheduleTemplateModel template)
        {
            const string sql = @"
            UPDATE schedule_templates 
            SET name = @Name, 
                cycle_id = @CycleId, 
                base_date = @BaseDate 
            WHERE id = @Id;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                int rows = await connection.ExecuteAsync(sql, template);
                return rows > 0;
            }
        }

        /// <summary>
        /// БЕЗОПАСНОЕ УДАЛЕНИЕ БРИГАДЫ (Шаблона с опорной датой)
        /// </summary>
        public async Task<DeleteResult> DeleteTemplateBindingAsync(int templateId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                // 1. Проверяем, назначен ли этот шаблон (бригада) хоть одному сотруднику
                const string checkSql = "SELECT EXISTS(SELECT 1 FROM employee_schedule_assignments WHERE template_id = @Id);";
                bool isUsedByEmployees = await connection.ExecuteScalarAsync<bool>(checkSql, new { Id = templateId });

                if (isUsedByEmployees)
                {
                    return DeleteResult.Fail("Невозможно удалить бригаду. На неё всё еще назначены сотрудники в истории расписаний.");
                }

                // 2. Проверяем, привязан ли этот шаблон к какому-либо оборудованию
                const string checkEquipmentSql = "SELECT EXISTS(SELECT 1 FROM equipment WHERE template_id = @Id OR id IN (SELECT equipment_id FROM equipment_schedule_history WHERE template_id = @Id));";
                bool isUsedByEquipment = await connection.ExecuteScalarAsync<bool>(checkEquipmentSql, new { Id = templateId });

                if (isUsedByEquipment)
                {
                    return DeleteResult.Fail("Невозможно удалить бригаду. Данный шаблон зафиксирован за производственным оборудованием.");
                }

                // Если проверок нет — удаляем
                const string deleteSql = "DELETE FROM schedule_templates WHERE id = @Id;";
                int rows = await connection.ExecuteAsync(deleteSql, new { Id = templateId });

                return rows > 0 ? DeleteResult.Success() : DeleteResult.Fail("Запись не найдена в базе данных.");
            }
        }

        /// <summary>
        /// БЕЗОПАСНОЕ УДАЛЕНИЕ ЦИКЛИЧЕСКОГО ГРАФИКА (Схемы дней)
        /// </summary>
        public async Task<DeleteResult> DeleteScheduleCycleAsync(int cycleId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var tx = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        // 1. Проверяем, созданы ли бригады (шаблоны) на основе этого циклического графика
                        const string checkSql = "SELECT EXISTS(SELECT 1 FROM schedule_templates WHERE cycle_id = @Id);";
                        bool isUsedInTemplates = await connection.ExecuteScalarAsync<bool>(checkSql, new { Id = cycleId }, tx);

                        if (isUsedInTemplates)
                        {
                            return DeleteResult.Fail("Невозможно удалить схему графика. На её основе созданы и функционируют бригады.");
                        }

                        // 2. Сначала очищаем сетку дней в подчиненной таблице schedule_cycle_items
                        const string deleteItemsSql = "DELETE FROM schedule_cycle_items WHERE cycle_id = @Id;";
                        await connection.ExecuteAsync(deleteItemsSql, new { Id = cycleId }, tx);

                        // 3. Удаляем сам циклический график из schedule_cycles
                        const string deleteCycleSql = "DELETE FROM schedule_cycles WHERE id = @Id;";
                        int rows = await connection.ExecuteAsync(deleteCycleSql, new { Id = cycleId }, tx);

                        await tx.CommitAsync();

                        return rows > 0 ? DeleteResult.Success() : DeleteResult.Fail("График не найден.");
                    }
                    catch (Exception ex)
                    {
                        await tx.RollbackAsync();
                        return DeleteResult.Fail($"Ошибка транзакции базы данных: {ex.Message}");
                    }
                }
            }
        }
    }
}
