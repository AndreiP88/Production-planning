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
        public ulong Shift_id { get; set; }
        public string Shift_name { get; set; }
        public string Time_start { get; set; }
        public string Time_end { get; set; }
        public ulong? Edp_id { get; set; }
        public int Is_equipment_cancelled { get; set; }
        public ulong? Plan_employee_id { get; set; }
        public string Plan_employee_name { get; set; }
        public string Plan_status { get; set; }
        public ulong? Plan_override_id { get; set; }
        public ulong? Draft_override_id { get; set; }
        public string Draft_employee_name { get; set; }
        public ulong? Approved_override_id { get; set; }
        public string Approved_employee_name { get; set; }
        public string Final_fact_status { get; set; }
        public int Is_equipment_working_by_plan { get; set; } // Придет как 1 или 0
        public string Staffing_requirement { get; set; }
        public string Active_staffing_mode { get; set; } = string.Empty;
        public int Staffing_requirement_code { get; set; } // Поле из MySQL
        public int Active_staff_count { get; set; }
        public int Plan_status_code { get; set; }
    }

    // Удобная древовидная модель карточки для логики приложения
    public class EquipmentShiftCard
    {
        public string EquipmentName { get; set; }
        public ulong ShiftID { get; set; }
        public string ShiftName { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public ulong? EdpId { get; set; }
        public bool IsEquipmentCancelled { get; set; }
        public bool IsWorkingByPlan { get; set; }       // 🌟 Добавлено
        public string ActiveStaffingMode { get; set; } = string.Empty; // 🌟 Добавлено
        public string StaffingRequirement { get; set; } = string.Empty;
        public int StaffingRequirementCode { get; set; } // Свойство объекта-контейнера
        public int ActiveStaffCount { get; set; }

        public List<PlannedEmployeeInfo> PlannedStaff { get; set; } = new List<PlannedEmployeeInfo>();
        public List<DraftEmployeeInfo> DraftStaff { get; set; } = new List<DraftEmployeeInfo>();
        public List<AssignedEmployeeInfo> ApprovedStaff { get; set; } = new List<AssignedEmployeeInfo>();
    }

    public class PlannedEmployeeInfo
    {
        public ulong EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string PlanStatus { get; set; }
        public string FinalFactStatus { get; set; }
        public ulong? PlanOverrideId { get; set; }
        public int PlanStatusCode { get; set; }
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

    public enum EquipmentShiftState
    {
        CancelledManually = 0, // Станок принудительно остановлен (в таблице edp.is_cancelled = 1)
        ManualActive = 1,      // Активная ручная смена (для manual_only станка)
        ActiveByPlan = 2,      // Станок работает (по штатному плану или запущен вне плана для автосмен)
        IdleByPlan = 3,        // Плановый простой станка (выходной по циклу автоплана)
        ManualWaiting = 4      // Ручной станок ждет активации смены (в таблице daily_plan пусто)
    }

    public enum StaffingRequirementType
    {
        EquipmentCancelled = 0, // Не требуется (Остановка станка)
        Staffed = 1,            // ✅ Укомплектовано
        StaffNeeded = 2,        // 🚨 ТРЕБУЕТСЯ ПЕРСОНАЛ
        IdleBySchedule = 3,     // Не требуется (Вне графика)
        ManualWaiting = 4       // ⚪ Ожидание назначения
    }

}
