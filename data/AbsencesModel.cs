using System;

namespace data
{
    internal class AbsencesModel
    {
    }

    /// <summary>
    /// Строка списка отсутствий сотрудника для DataGrid
    /// </summary>
    public class EmployeeAbsenceRow
    {
        public ulong Id { get; set; }
        public ulong EmployeeId { get; set; }
        public ulong TypeId { get; set; }
        public string AbsenceTypeName { get; set; } // Название из справочника absence_types
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Понятный текст периода для интерфейса
        public string PeriodText => EndDate.HasValue
            ? $"{StartDate:dd.MM.yyyy} — {EndDate:dd.MM.yyyy}"
            : $"{StartDate:dd.MM.yyyy} — Открытая дата";
    }

    /// <summary>
    /// Буферная команда регистрации нового отсутствия
    /// </summary>
    public class RegisterAbsenceCommand
    {
        public ulong EmployeeId { get; set; }
        public ulong TypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    /// <summary>
    /// Полная строка отсутствия с ФИО сотрудника для общих списков и отчетов
    /// </summary>
    public class EmployeeAbsenceExtendedRow
    {
        public ulong Id { get; set; }
        public ulong EmployeeId { get; set; }
        public string EmployeeFullName { get; set; } // ФИО сотрудника
        public ulong TypeId { get; set; }
        public string AbsenceTypeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StatusColor { get; set; } = "#FF6347";

        public string PeriodText => EndDate.HasValue
            ? $"{StartDate:dd.MM.yyyy} — {EndDate:dd.MM.yyyy}. Всего дней: {(EndDate.Value - StartDate).TotalDays + 1}"
            : $"{StartDate:dd.MM.yyyy} — Открытый больничный";

        // Свойство для C# 7.3: проверка активности на переданную дату
        public bool IsActiveOn(DateTime targetDate)
        {
            return targetDate.Date >= StartDate.Date &&
                   (!EndDate.HasValue || targetDate.Date <= EndDate.Value.Date);
        }
    }

    public class AbsenceGridRow
    {
        public ulong Id { get; set; }
        public string EmployeeFullName { get; set; } = string.Empty;
        public string AbsenceTypeName { get; set; } = string.Empty;
        public string PeriodText { get; set; } = string.Empty;
        public string StatusColor { get; set; } = "#FF9800"; // Мапится из status_color

        // Массив булевых значений: true - человек отсутствует в этот день, false - работает
        public bool[] Days { get; set; }

        public AbsenceGridRow(int daysInMonth)
        {
            Days = new bool[daysInMonth];
        }
    }

    public class AbsenceType
    {
        public ulong Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsEndDateRequired { get; set; } // Мапится из is_end_date_required
        public string StatusColor { get; set; } = "#FF6347"; // Мапится из status_color
    }

}
