using System;
using System.Collections.Generic;
//using System.Xml;
using Newtonsoft.Json;

namespace data
{
    public class ExternalOrgDirectoryModel
    {
        // Первичный ключ записи цеха/участка
        public long id_common_org_directory { get; set; }

        // Наименование подразделения (например, "2.Печатный цех")
        public string org_name { get; set; }

        // Ссылка на родительский узел
        public long? parentid { get; set; }
    }

    public class ExternalEquipmentRow
    {
        // Данные из таблицы станков
        public long EquipId { get; set; }
        public string EquipName { get; set; }
        public decimal EquipType { get; set; }

        // Связующие данные из таблицы структуры компании
        public long OrgId { get; set; }
        public string OrgName { get; set; }

        // 🌟 ФОРМАТНЫЙ ВЫВОД ДЛЯ КОМБОБОКСА: "Печатный цех — Roland"
        // Дополнительно очищаем имя цеха от технологических индексов вроде "2."
        public string DisplayText
        {
            get
            {
                string cleanOrgName = string.IsNullOrEmpty(OrgName)
                    ? "Без цеха"
                    : OrgName.Substring(OrgName.IndexOf('.') + 1).Trim();

                return $"{cleanOrgName} — {EquipName}";
            }
        }
    }

    // Глобальный класс Участка (без изменений)
    public class WorkAreaInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Сюда Dapper или Entity Framework запишет сырой JSON-текст из базы данных
        public string VisibleColumnsJson { get; set; }

        // Удобное свойство для работы в коде программы
        public WorkAreaVisibleColumns Settings => WorkAreaVisibleColumns.FromJson(VisibleColumnsJson);
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
        public string StaffingModeRus => StaffingMode == "strict_schedule"
            ? "По строгому графику"
            : "Ручное назначение (Вне плана)";
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
        public string AreaVisibleColumnsJson { get; set; } // Имя свойства совпадает с AS AreaVisibleColumnsJson
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

    public class WorkAreaVisibleColumns
    {
        // ==========================================
        // 🔒 ГРУППА 1. ОБЯЗАТЕЛЬНЫЕ СИСТЕМНЫЕ КОЛОНКИ
        // ==========================================

        [JsonIgnore] // Не сериализуем в JSON, так как они всегда true
        public bool Executor => true; // Исполнитель

        [JsonIgnore]
        public bool OrderNumber => true; // Номер заказа

        [JsonIgnore]
        public bool Customer => true; // Заказчик


        // ==========================================
        // ⚙️ ГРУППА 2. ОПЦИОНАЛЬНЫЕ ТЕХНОЛОГИЧЕСКИЕ КОЛОНКИ
        // ==========================================

        [JsonProperty("order_name")]
        public bool OrderName { get; set; } = true; // Наименование заказа

        [JsonProperty("element")]
        public bool Element { get; set; } = true; // Элемент тиража

        [JsonProperty("colors")]
        public bool Colors { get; set; } = true; // Красочность (Лицо/Оборот)

        [JsonProperty("material")]
        public bool Material { get; set; } = true; // Материал (Бумага)

        [JsonProperty("tech_notes")]
        public bool TechNotes { get; set; } = true; // Заметки технолога

        [JsonProperty("planned_output")]
        public bool PlannedOutput { get; set; } = true; // Плановая выработка

        [JsonProperty("date_time_start")]
        public bool DateTimeStart { get; set; } = true; // Дата/время начала

        [JsonProperty("date_time_end")]
        public bool DateTimeEnd { get; set; } = true; // Дата/время окончания

        [JsonProperty("duration")]
        public bool Duration { get; set; } = true; // Длительность

        [JsonProperty("stamp")]
        public bool Stamp { get; set; } = false; // Штамп (оснастка)

        [JsonProperty("sheet_trimming")]
        public bool SheetTrimming { get; set; } = false; // Подрезка листа


        // ==========================================
        // 🛠️ МЕТОДЫ ДЛЯ СЕРИАЛИЗАЦИИ И ДЕФОЛТНЫХ НАСТРОЕК
        // ==========================================

        /// <summary>
        /// Конвертирует текущий объект настроек в JSON-строку для сохранения в MySQL visible_columns
        /// </summary>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }

        /// <summary>
        /// Создает объект настроек из JSON-строки, полученной из базы данных MySQL
        /// </summary>
        public static WorkAreaVisibleColumns FromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return GetDefaultSettings(); // Если в базе NULL, отдаем настройки по умолчанию
            }

            try
            {
                return JsonConvert.DeserializeObject<WorkAreaVisibleColumns>(json);
            }
            catch
            {
                return GetDefaultSettings(); // Защита от битого JSON в БД
            }
        }

        /// <summary>
        /// Возвращает базовые настройки по умолчанию (например, для новых участков)
        /// </summary>
        public static WorkAreaVisibleColumns GetDefaultSettings()
        {
            return new WorkAreaVisibleColumns
            {
                OrderName = true,
                Element = true,
                Colors = true,
                Material = true,
                TechNotes = true,
                PlannedOutput = true,
                DateTimeStart = true,
                DateTimeEnd = true,
                Duration = true,
                Stamp = false,        // По умолчанию штамп выключен (включим для вырубки)
                SheetTrimming = false  // По умолчанию подрезка выключена
            };
        }
    }
}
