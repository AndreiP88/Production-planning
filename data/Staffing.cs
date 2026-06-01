using System;
using System.Collections.Generic;

namespace data.Models
{
    // 1. "Плоская" модель для получения данных из Dapper
    public class ShiftReportRow
    {
        public DateTime Date { get; set; }
        public long EquipId { get; set; }
        public string EquipName { get; set; }
        public string EquipCode { get; set; }
        public int ShiftNum { get; set; }
        public string Shift { get; set; }
        public string NeedStatus { get; set; }
        public string PlanAndStatuses { get; set; }
        public string Assignments { get; set; }
        public string Drafts { get; set; }
        public string ApprovedFact { get; set; }
    }

    // 2. Иерархические модели для удобного отображения в UI
    public class DailyReport
    {
        public DateTime Date { get; set; }
        public List<EquipmentInfo> Equipments { get; set; } = new List<EquipmentInfo>();
    }

    public class EquipmentInfo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public List<ShiftInfo> Shifts { get; set; } = new List<ShiftInfo>();
    }

    public class ShiftInfo
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public List<string> PlannedStaff { get; set; } // План
        public List<string> Assignments { get; set; }  // Ручные назначения
        public List<string> Drafts { get; set; }       // Черновики
        public List<string> FinalStaff { get; set; }   // Итоговый факт
    }
}
