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
        public DateTime BaseDate { get; set; }
    }
}
