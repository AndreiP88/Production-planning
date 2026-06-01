using data;
using Dapper;
using MySqlConnector;
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
    }
}
