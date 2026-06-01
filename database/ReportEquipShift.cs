using data;
using Dapper;
using MySqlConnector;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace database
{
    public class ReportEquipShift
    {
        private readonly string _connectionString;

        public ReportEquipShift(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<EquipmentShiftCard> GetEquipmentShiftCardAsync(DateTime targetDate, int shiftNumber, int equipmentId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                // 1. Вызываем процедуру с обновленными именами параметров
                var rows = await connection.QueryAsync<EquipmentShiftCardRow>(
                    "GetEquipmentShiftCard",
                    new
                    {
                        p_target_date = targetDate,
                        p_target_shift_number = shiftNumber,
                        p_target_equipment_id = equipmentId
                    },
                    commandType: CommandType.StoredProcedure
                );

                if (!rows.Any()) return null;

                var firstRow = rows.First();

                // Формируем заголовочную информацию карточки
                var card = new EquipmentShiftCard
                {
                    EquipmentName = firstRow.Equipment_name,
                    ShiftName = firstRow.Shift_name,
                    TimeStart = firstRow.Time_start,
                    TimeEnd = firstRow.Time_end,
                    EdpId = firstRow.Edp_id,
                    IsEquipmentCancelled = firstRow.Is_equipment_cancelled == 1,
                    StaffingRequirement = firstRow.Staffing_requirement
                };

                // 2. Группируем списки сотрудников по категориям
                foreach (var r in rows)
                {
                    // Плановый персонал
                    if (r.Plan_employee_id.HasValue)
                    {
                        card.PlannedStaff.Add(new PlannedEmployeeInfo
                        {
                            EmployeeId = r.Plan_employee_id.Value,
                            EmployeeName = r.Plan_employee_name,
                            PlanStatus = r.Plan_status,
                            FinalFactStatus = r.Final_fact_status
                        });
                    }

                    // Черновики ручных назначений
                    if (r.Draft_override_id.HasValue)
                    {
                        card.DraftStaff.Add(new DraftEmployeeInfo
                        {
                            OverrideId = r.Draft_override_id.Value,
                            EmployeeName = r.Draft_employee_name
                        });
                    }

                    // Утвержденные ручные назначения
                    if (r.Approved_override_id.HasValue)
                    {
                        card.ApprovedStaff.Add(new AssignedEmployeeInfo
                        {
                            OverrideId = r.Approved_override_id.Value,
                            EmployeeName = r.Approved_employee_name,
                            FinalFactStatus = r.Final_fact_status
                        });
                    }
                }

                return card;
            }
        }
    }
}
