using Dapper;
using data.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace database
{
    public class ReportStaffing
    {
        private readonly string _connectionString;

        public ReportStaffing(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Вспомогательный метод
        private static List<string> SplitCsv(string val) =>
            string.IsNullOrEmpty(val) ? new List<string>() : val.Split(new[] { ", ", " | " }, StringSplitOptions.RemoveEmptyEntries).ToList();

        public async Task<List<DailyReport>> GetStaffingReportAsync(DateTime start, DateTime end, int areaId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                // 1. Вызываем процедуру
                var rows = await connection.QueryAsync<ShiftReportRow>(
                    "GetEquipmentStaffingReport",
                    new
                    {
                        p_start_date = start,
                        p_end_date = end,
                        p_target_area_id = areaId
                    },
                    commandType: CommandType.StoredProcedure
                );

                // 2. Группируем данные с помощью LINQ
                var report = rows
                    .GroupBy(r => r.Date)
                    .Select(dateGroup => new DailyReport
                    {
                        Date = dateGroup.Key,
                        Equipments = dateGroup
                            .GroupBy(r => new { r.EquipId, r.EquipName, r.EquipCode })
                            .Select(eqGroup => new EquipmentInfo
                            {
                                Id = eqGroup.Key.EquipId,
                                Name = eqGroup.Key.EquipName,
                                Code = eqGroup.Key.EquipCode,
                                Shifts = eqGroup.Select(s => new ShiftInfo
                                {
                                    Number = s.ShiftNum,
                                    Name = s.Shift,
                                    Status = s.NeedStatus,
                                    PlannedStaff = SplitCsv(s.PlanAndStatuses),
                                    Assignments = SplitCsv(s.Assignments),
                                    Drafts = SplitCsv(s.Drafts),
                                    FinalStaff = SplitCsv(s.ApprovedFact)
                                }).ToList()
                            }).ToList()
                    })
                    .OrderBy(r => r.Date)
                    .ToList();

                return report;
            }
        }
    }
}
