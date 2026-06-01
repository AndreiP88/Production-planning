using data;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;

namespace Production_planning
{
    public class ReportWorkAreas
    {
        private readonly string _connectionString;

        public ReportWorkAreas(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<WorkAreaInfo>> GetWorkAreasWithEquipmentAsync()
        {
            // Используем LEFT JOIN, чтобы забрать даже пустые участки
            const string sql = @"
                                SELECT 
                                    wa.id AS AreaId, 
                                    wa.name AS AreaName, 
                                    eq.id AS EquipId, 
                                    eq.name AS EquipName, 
                                    eq.code AS EquipCode,
                                    eq.decommissioned_at AS DecommissionedAt
                                FROM work_areas wa
                                LEFT JOIN equipment eq ON wa.id = eq.work_area_id
                                ORDER BY wa.sort_order, wa.id, eq.sort_order, eq.id;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                // 1. Получаем плоские строки из базы через Dapper
                var rows = await connection.QueryAsync<WorkAreaEquipmentRow>(sql, commandType: CommandType.Text);

                // 2. Группируем данные с помощью LINQ в красивое дерево
                var result = rows
                    .GroupBy(r => new { r.AreaId, r.AreaName })
                    .Select(areaGroup => new WorkAreaInfo
                    {
                        Id = areaGroup.Key.AreaId,
                        Name = areaGroup.Key.AreaName,
                        // Собираем список оборудования для текущего участка
                        Equipments = areaGroup
                            .Where(r => r.EquipId.HasValue)
                            .Select(r => new EquipmentShortInfo
                            {
                                Id = r.EquipId.Value,
                                Name = r.EquipName ?? "Без названия",
                                Code = r.EquipCode ?? "Б/К",
                                IsActive = !r.DecommissionedAt.HasValue,
                                DecommissionedAt = r.DecommissionedAt
                            })
                            .ToList()
                    })
                    .ToList();

                return result;
            }
        }
    }
}
