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
    /// БАЗОВАЯ МОДЕЛЬ: Полное соответствие полям таблицы `equipment` в MySQL.
    /// Используется для создания и базового изменения записи.
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
    /// ПОЛНАЯ КАРТОЧКА: Используется для детального просмотра,
    /// исправления анкетных данных и вывода текущих статусов истории.
    /// </summary>
    public class EquipmentFullCard : EquipmentModel
    {
        public string TemplateName { get; set; }
        public DateTime? TemplateValidFrom { get; set; }
        public DateTime? StaffingModeValidFrom { get; set; }
    }

    /// <summary>
    /// РАСШИРЕННАЯ МОДЕЛЬ РЕЕСТРА: Наследует базовую модель, используется 
    /// для вывода дерева Участки -> Оборудование на главном экране.
    /// </summary>
    public class EquipmentShortInfo : EquipmentModel
    {
        public bool IsActive => !DecommissionedAt.HasValue;
        public string TemplateName { get; set; }
        public DateTime? StaffingModeValidFrom { get; set; }
        public DateTime? TemplateValidFrom { get; set; }
    }

    /// <summary>
    /// Вспомогательный класс для плоского чтения строк через Dapper LEFT JOIN
    /// </summary>
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
        public DateTime? StaffingModeValidFrom { get; set; }
        public DateTime? TemplateValidFrom { get; set; }
    }

    /// <summary>
    /// Модель оборудования
    /// </summary>
    public class EquipmentLookupDto
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public ulong WorkAreaId { get; set; }
        public string WorkAreaName { get; set; } // Подтянем название участка для удобной группировки

        // Отображение в комбобоксе: Название [Код] (Участок)
        public string DisplayText => $"{WorkAreaName}: {Name}";
    }

    /// <summary>
    /// Буфер памяти формы: Отложенное назначение НОВОГО ГРАФИКА
    /// </summary>
    public class PendingScheduleAssignment
    {
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public DateTime ValidFrom { get; set; }
    }

    /// <summary>
    /// Буфер памяти формы: Отложенное назначение НОВОГО РЕЖИМА
    /// </summary>
    public class PendingStaffingAssignment
    {
        public string StaffingMode { get; set; }
        public DateTime ValidFrom { get; set; }
    }

    /// <summary>
    /// Строка полной истории графиков станка
    /// </summary>
    public class EquipmentScheduleHistoryRow
    {
        public ulong Id { get; set; }
        public int TemplateId { get; set; }
        public string TemplateName { get; set; } // Текстовое название бригады
        public DateTime ValidFrom { get; set; }  // Дата начала действия
    }

    /// <summary>
    /// Строка полной истории режимов работы станка
    /// </summary>
    public class EquipmentStaffingHistoryRow
    {
        public ulong Id { get; set; }
        public string StaffingMode { get; set; } // 'strict_schedule' или 'manual_only'
        public DateTime ValidFrom { get; set; }   // Дата начала действия

        // Удобное свойство для красивого вывода режима в интерфейс на русском языке
        public string StaffingModeRus => StaffingMode == "strict_schedule"
            ? "По строгому графику"
            : "Ручное назначение (Вне плана)";
    }
}
