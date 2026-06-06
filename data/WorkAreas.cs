using System;
using System.Collections.Generic;

namespace data
{
    // Глобальный класс Участка (без изменений)
    public class WorkAreaInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; } // Добавили, так как мы им управляем
        public List<EquipmentShortInfo> Equipments { get; set; } = new List<EquipmentShortInfo>();
    }

    /// <summary>
    /// БАЗОВАЯ МОДЕЛЬ: Точно соответствует таблице `equipment` в MySQL.
    /// Используется для форм Создания/Редактирования (Write Model).
    /// </summary>
    public class EquipmentModel
    {
        public int Id { get; set; }
        public int WorkAreaId { get; set; }
        public int? TemplateId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime CommissionedAt { get; set; }
        public DateTime? DecommissionedAt { get; set; }
        public string StaffingMode { get; set; }
        public int SortOrder { get; set; }
    }

    /// <summary>
    /// РАСШИРЕННАЯ МОДЕЛЬ: Наследует всё от базовой и добавляет вычисляемые поля для интерфейса.
    /// Используется для вывода в списки/отчеты (Read Model).
    /// </summary>
    public class EquipmentShortInfo : EquipmentModel
    {
        public bool IsActive => !DecommissionedAt.HasValue;
        public string TemplateName { get; set; }

        // НОВЫЕ ПОЛЯ ДЛЯ UI
        public DateTime? StaffingModeValidFrom { get; set; }
        public DateTime? TemplateValidFrom { get; set; }
    }
    // Вспомогательный класс для плоского чтения Dapper
    public class WorkAreaEquipmentRow
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public int AreaSortOrder { get; set; }
        public int? EquipId { get; set; }
        public string EquipName { get; set; }
        public string EquipCode { get; set; }
        public int EquipSortOrder { get; set; }
        public int WorkAreaId { get; set; }
        public int? TemplateId { get; set; }
        public string TemplateName { get; set; }
        public DateTime? CommissionedAt { get; set; }
        public DateTime? DecommissionedAt { get; set; }
        public string StaffingMode { get; set; }

        // НОВЫЕ ПОЛЯ ДЛЯ ДАТ НАЧАЛА ДЕЙСТВИЯ
        public DateTime? StaffingModeValidFrom { get; set; }
        public DateTime? TemplateValidFrom { get; set; }
    }
}
