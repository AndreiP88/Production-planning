using System;
using System.Collections.Generic;

namespace data
{
    public class ScheduleCycleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CycleLength { get; set; }
        public List<CycleItemModel> Items { get; set; } = new List<CycleItemModel>();
    }

    public class CycleItemModel
    {
        public int DayNumber { get; set; }
        public int ShiftId { get; set; }
        public int ShiftNumber { get; set; }
        public string ShiftName { get; set; } // Для отображения в интерфейсе
    }

    // Плоская строка для чтения дней циклов из БД
    public class CycleItemDbRow
    {
        public int CycleId { get; set; }
        public string CycleName { get; set; }
        public int CycleLength { get; set; }
        public int? DayNumber { get; set; }
        public int? ShiftId { get; set; }
        public int? ShiftNumber { get; set; }
        public string ShiftName { get; set; }
    }

    // --- МОДЕЛИ ДЛЯ БРИГАД (ШАБЛОНОВ С ОПОРНОЙ ДАТОЙ) ---
    public class ScheduleTemplateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CycleId { get; set; }
        public string CycleName { get; set; } // Название используемой схемы
        public string TargetTypeRaw { get; set; } // Сюда Dapper запишет строку из ENUM (universal, equipment, employee)

        // Удобное свойство для логики приложения
        public ScheduleTargetType TargetType
        {
            get
            {
                if (string.IsNullOrEmpty(TargetTypeRaw)) return ScheduleTargetType.Universal;
                switch (TargetTypeRaw.ToLower())
                {
                    case "equipment": return ScheduleTargetType.Equipment;
                    case "employee": return ScheduleTargetType.Employee;
                    default: return ScheduleTargetType.Universal;
                }
            }
            set
            {
                TargetTypeRaw = value.ToString().ToLower();
            }
        }
        public DateTime BaseDate { get; set; }
    }

    public class DeleteResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        public static DeleteResult Success() => new DeleteResult { IsSuccess = true };
        public static DeleteResult Fail(string msg) => new DeleteResult { IsSuccess = false, ErrorMessage = msg };
    }
    public enum ScheduleTargetType
    {
        Universal,   // Универсальный
        Equipment,   // Только станки
        Employee     // Только сотрудники
    }
}
