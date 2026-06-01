using System;
using System.Collections.Generic;

namespace data
{
    internal class EquipmentShift
    {
    }

    

    // Плоская строка. Названия свойств точно соответствуют вашему новому SELECT
    public class EquipmentShiftCardRow
    {
        public string Equipment_name { get; set; }
        public string Shift_name { get; set; }
        public string Time_start { get; set; }
        public string Time_end { get; set; }
        public ulong? Edp_id { get; set; }
        public int Is_equipment_cancelled { get; set; }
        public string Staffing_requirement { get; set; }
        public int? Plan_employee_id { get; set; }
        public string Plan_employee_name { get; set; }
        public string Plan_status { get; set; }
        public ulong? Draft_override_id { get; set; }
        public string Draft_employee_name { get; set; }
        public ulong? Approved_override_id { get; set; }
        public string Approved_employee_name { get; set; }
        public string Final_fact_status { get; set; }
    }

    // Удобная древовидная модель карточки для логики приложения
    public class EquipmentShiftCard
    {
        public string EquipmentName { get; set; }
        public string ShiftName { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public ulong? EdpId { get; set; }
        public bool IsEquipmentCancelled { get; set; }
        public string StaffingRequirement { get; set; }

        public List<PlannedEmployeeInfo> PlannedStaff { get; set; } = new List<PlannedEmployeeInfo>();
        public List<DraftEmployeeInfo> DraftStaff { get; set; } = new List<DraftEmployeeInfo>();
        public List<AssignedEmployeeInfo> ApprovedStaff { get; set; } = new List<AssignedEmployeeInfo>();
    }

    public class PlannedEmployeeInfo
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string PlanStatus { get; set; }
        public string FinalFactStatus { get; set; }
    }

    public class DraftEmployeeInfo
    {
        public ulong OverrideId { get; set; }
        public string EmployeeName { get; set; }
    }

    public class AssignedEmployeeInfo
    {
        public ulong OverrideId { get; set; }
        public string EmployeeName { get; set; }
        public string FinalFactStatus { get; set; }
    }

}
