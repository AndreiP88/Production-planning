using System;
using System.Collections.Generic;

namespace data
{
    /// <summary>
    /// Модель для общего списка сотрудников (Краткая сводка)
    /// </summary>
    public class EmployeeShortRow
    {
        public ulong Id { get; set; }
        public string FullName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public int IsActive { get; set; }
        public string CurrentStatus => IsActive == 1 ? "Работает" : "Уволен";     // "Работает" или "Уволен"
        public string CurrentPosition { get; set; }   // На текущую дату
        public string CurrentSchedule { get; set; }   // На текущую дату
        public string CurrentEquipment { get; set; }  // Закрепленный станок
        public string CurrentWorkArea { get; set; }   // Авто-вычисляемый участок
        public string PrimaryPhone { get; set; }      // Основной телефон для связи
    }

    /// <summary>
    /// Элемент масштабируемого списка контактов
    /// </summary>
    public class EmployeeContactDto
    {
        public ulong Id { get; set; }
        public uint ContactTypeId { get; set; }
        public string ContactTypeCode { get; set; } // 'phone', 'email', 'telegram'
        public string ContactTypeName { get; set; } // 'Телефон', 'Эл. почта'
        public string ContactValue { get; set; }
    }

    /// <summary>
    /// Максимально полная карточка одного сотрудника со всеми текущими вехами
    /// </summary>
    public class EmployeeFullCard
    {
        // Базовые данные сотрудника
        public ulong Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string FullName { get; set; }

        // Текущий период найма (из employment_periods)
        public ulong? CurrentPeriodId { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? FireDate { get; set; }

        // Текущая должность (из employee_position_assignments)
        public ulong? CurrentPositionAssignmentId { get; set; }
        public ulong? PositionId { get; set; }
        public string PositionName { get; set; }
        public string SystemRole { get; set; }
        public DateTime? PositionValidFrom { get; set; }

        // Текущий график (из employee_schedule_assignments)
        public ulong? CurrentScheduleAssignmentId { get; set; }
        public ulong? ScheduleTemplateId { get; set; }
        public string ScheduleName { get; set; }
        public DateTime? ScheduleValidFrom { get; set; }

        // Текущее оборудование (из employee_equipment_assignments)
        public ulong? CurrentEquipmentAssignmentId { get; set; }
        public ulong? EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public DateTime? EquipmentValidFrom { get; set; }

        // Масштабируемая коллекция контактов
        public List<EmployeeContactDto> Contacts { get; set; } = new List<EmployeeContactDto>();
    }

    /// <summary>
    /// Модель должности
    /// </summary>
    public class PositionLookupDto
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string SystemRole { get; set; } // 'worker', 'master', 'chief'

        // Понятное отображение в интерфейсе, если нужно вывести роль рядом
        public string DisplayText => $"{Name} ({SystemRole})";
    }

    public class PositionUpdateBuffer
    {
        public ulong? AssignmentId { get; set; } // Ключевое поле! ID строки из базы данных
        public ulong EmployeeId { get; set; }
        public ulong? NewPositionId { get; set; }
        public DateTime? NewValidFrom { get; set; }

        public bool IsNewAssignment => !NewPositionId.HasValue || NewValidFrom.HasValue;
    }

    public class ScheduleUpdateBuffer
    {
        public ulong? AssignmentId { get; set; } // Ключевое поле! ID строки из базы данных
        public ulong EmployeeId { get; set; }
        public ulong? NewTemplateId { get; set; }
        public DateTime? NewValidFrom { get; set; }

        public bool IsNewAssignment => NewTemplateId.HasValue && NewValidFrom.HasValue;
    }

    public class EquipmentUpdateBuffer
    {
        public ulong? AssignmentId { get; set; } // Ключевое поле! ID строки из базы данных
        public ulong EmployeeId { get; set; }
        public ulong? NewEquipmentId { get; set; }
        public DateTime? NewValidFrom { get; set; }

        public bool IsNewAssignment => !NewEquipmentId.HasValue || NewValidFrom.HasValue;
    }

    /// <summary>
    /// Строка полной истории изменения должностей сотрудника
    /// </summary>
    public class EmployeePositionHistoryRow
    {
        public ulong Id { get; set; }
        public ulong PositionId { get; set; }
        public string PositionName { get; set; }
        public string SystemRole { get; set; }
        public DateTime ValidFrom { get; set; }
    }

    /// <summary>
    /// Строка полной истории изменения графиков работы сотрудника
    /// </summary>
    public class EmployeeScheduleHistoryRow
    {
        public ulong Id { get; set; }
        public ulong TemplateId { get; set; }
        public string TemplateName { get; set; } // Текстовое название графика/бригады
        public DateTime ValidFrom { get; set; }
    }

    /// <summary>
    /// Строка полной истории закрепления оборудования за сотрудником
    /// </summary>
    public class EmployeeEquipmentHistoryRow
    {
        public ulong Id { get; set; }
        public ulong? EquipmentId { get; set; }
        public string EquipmentName { get; set; } // Название станка
        public string WorkAreaName { get; set; }  // Авто-вычисляемое название участка станка
        public DateTime ValidFrom { get; set; }
    }

    /// <summary>
    /// Строка истории найма и увольнений сотрудника
    /// </summary>
    public class EmployeeEmploymentHistoryRow
    {
        public ulong Id { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? FireDate { get; set; }

        // Полезное текстовое свойство для вывода красивого статуса в таблицу
        public string PeriodStatus => FireDate.HasValue
            ? "Уволен"
            : "Работает в настоящий момент";

        // Вычисляемое свойство для вывода общего стажа за этот конкретный период
        public string DurationText
        {
            get
            {
                DateTime end = FireDate ?? DateTime.Today;
                TimeSpan span = end - HireDate;
                int totalDays = span.Days;

                if (totalDays < 0) return "—";
                return $"{totalDays} дн.";
            }
        }
    }

    /// <summary>
    /// Строка единой ленты кадровых событий сотрудника
    /// </summary>
    public class EmployeeCareerEventRow
    {
        public DateTime EventDate { get; set; }  // Дата события
        public string EventType { get; set; }    // 'Прием', 'Смена должности', 'Увольнение'
        public string Details { get; set; }      // Название должности или пояснение

        // Визуальный anchor-значок для scannability в UI таблице
        public string EventIcon
        {
            get
            {
                switch (EventType)
                {
                    case "Прием":
                        return "📥";
                    case "Назначение":
                        return "📌";
                    case "Увольнение":
                        return "📤";
                    default:
                        return "🔄"; // Для смены должности
                }
            }
        }
    }
}
