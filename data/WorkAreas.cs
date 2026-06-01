using System;
using System.Collections.Generic;

namespace data
{
    public class WorkAreaEquipmentRow
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public int? EquipId { get; set; } // Для int? это работает всегда
        public string EquipName { get; set; }
        public string EquipCode { get; set; }
        public DateTime? DecommissionedAt { get; set; }
    }

    public class WorkAreaInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<EquipmentShortInfo> Equipments { get; set; } = new List<EquipmentShortInfo>();
    }

    public class EquipmentShortInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DecommissionedAt { get; set; }
    }

}