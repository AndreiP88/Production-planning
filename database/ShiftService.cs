using data;
using Dapper;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace database
{
    public class ShiftService
    {
        private readonly string _connectionString;

        public ShiftService(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// СПИСОК: Возвращает все смены из базы данных, отсортированные по порядковому номеру
        /// </summary>
        public async Task<List<ShiftDefinitionModel>> GetAllShiftsAsync()
        {
            const string sql = @"
            SELECT 
                id AS Id, 
                shift_number AS ShiftNumber, 
                name AS Name, 
                start_time AS StartTime, 
                end_time AS EndTime
            FROM shift_definitions
            ORDER BY name;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                var shifts = await connection.QueryAsync<ShiftDefinitionModel>(sql, commandType: CommandType.Text);
                return shifts.ToList();
            }
        }

        /// <summary>
        /// ПОИСК ПО ID: Возвращает данные одной смены для формы редактирования.
        /// Если смена не найдена — вернет null.
        /// </summary>
        public async Task<ShiftDefinitionModel> GetShiftByIdAsync(ulong id)
        {
            const string sql = @"
            SELECT 
                id AS Id, 
                shift_number AS ShiftNumber, 
                name AS Name, 
                start_time AS StartTime, 
                end_time AS EndTime
            FROM shift_definitions
            WHERE id = @Id;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                // QueryFirstOrDefaultAsync вернет null, если запись с таким ID отсутствует в MySQL
                return await connection.QueryFirstOrDefaultAsync<ShiftDefinitionModel>(sql, new { Id = id });
            }
        }

        /// <summary>
        /// ДОБАВЛЕНИЕ: Создает новую смену и возвращает ее сгенерированный ID
        /// </summary>
        public async Task<ulong> CreateShiftAsync(ShiftDefinitionModel shift)
        {
            const string sql = @"
            INSERT INTO shift_definitions (shift_number, name, start_time, end_time)
            VALUES (@ShiftNumber, @Name, @StartTime, @EndTime);
            SELECT LAST_INSERT_ID();";

            using (var connection = new MySqlConnection(_connectionString))
            {
                // Передаем объект напрямую — Dapper сам сопоставит свойства с параметрами @...
                ulong newId = await connection.ExecuteScalarAsync<ulong>(sql, shift);
                return newId;
            }
        }

        /// <summary>
        /// РЕДАКТИРОВАНИЕ: Обновляет любые поля существующей смены по её ID
        /// </summary>
        public async Task<bool> UpdateShiftAsync(ShiftDefinitionModel shift)
        {
            const string sql = @"
            UPDATE shift_definitions 
            SET shift_number = @ShiftNumber,
                name = @Name,
                start_time = @StartTime,
                end_time = @EndTime
            WHERE id = @Id;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                int rowsAffected = await connection.ExecuteAsync(sql, shift);
                return rowsAffected > 0; // Возвращает true, если запись была найдена и изменена
            }
        }

        public async Task<DeleteResult> DeleteShiftAsync(int shiftId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                // 1. Проверяем использование в шаблонах циклических графиков
                const string checkCycleSql = "SELECT EXISTS(SELECT 1 FROM schedule_cycle_items WHERE shift_id = @Id);";
                bool isUsedInCycles = await connection.ExecuteScalarAsync<bool>(checkCycleSql, new { Id = shiftId });

                if (isUsedInCycles)
                {
                    return DeleteResult.Fail("Невозможно удалить смену. Она назначена в днях циклического графика (схемы смен).");
                }

                // 2. Проверяем использование в ручных заменах и корректировках
                const string checkOverridesSql = "SELECT EXISTS(SELECT 1 FROM schedule_overrides WHERE shift_id = @Id);";
                bool isUsedInOverrides = await connection.ExecuteScalarAsync<bool>(checkOverridesSql, new { Id = shiftId });

                if (isUsedInOverrides)
                {
                    return DeleteResult.Fail("Невозможно удалить смену. За ней закреплены ручные корректировки расписания сотрудников (замены/черновики).");
                }

                // 3. Проверяем использование в ежедневных планах станков
                const string checkPlansSql = "SELECT EXISTS(SELECT 1 FROM equipment_daily_plan WHERE shift_id = @Id);";
                bool isUsedInPlans = await connection.ExecuteScalarAsync<bool>(checkPlansSql, new { Id = shiftId });

                if (isUsedInPlans)
                {
                    return DeleteResult.Fail("Невозможно удалить смену. На эту смену уже сформированы и сохранены планы работы производственного оборудования.");
                }

                // Если смена нигде не используется — выполняем физическое удаление
                const string deleteSql = "DELETE FROM shift_definitions WHERE id = @Id;";
                int rowsAffected = await connection.ExecuteAsync(deleteSql, new { Id = shiftId });

                if (rowsAffected > 0)
                {
                    return DeleteResult.Success();
                }

                return DeleteResult.Fail("Смена не найдена в базе данных.");
            }
        }
    }
}
