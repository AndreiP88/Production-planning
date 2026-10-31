using System;
using System.Text.RegularExpressions;

namespace data
{
    /*public class ProductionJobCard
    {
        public long IdPlanJob { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public int IdEquip { get; set; }
        public string OrderNum { get; set; } = string.Empty;
        public string OrderName { get; set; } = string.Empty;
        public string UlName { get; set; } = string.Empty;

        // РАЗДЕЛЬНЫЕ ПОЛЯ ТЕХНОЛОГИЧЕСКИХ ФАЗ ЗАКАЗА
        public double SetupCount { get; set; }        // Количество приладок (1, 2)
        public double SetupNormTime { get; set; }     // ЧИСТОЕ разделенное время приладки
        public double RunNormTime { get; set; }       // Время выполнения (как есть)
        public double RunQty { get; set; }           // Тираж (в штуках)
        public int IdOrderHead { get; set; }
        public int IdNormOperation { get; set; }
        public string ToolName { get; set; } = "—";
        public string ShortToolName
        {
            get
            {
                // Если имя оснастки пустое или прочерк — выводим как есть
                if (string.IsNullOrWhiteSpace(ToolName) || ToolName == "—")
                    return "—";

                // Сценарий 1: Ищем номер в конце строки, например "(№1557)" или "№ 1557"
                var matchNo = Regex.Match(ToolName, @"№\s*(\d+)");
                if (matchNo.Success)
                {
                    return matchNo.Groups[1].Value; // Вернет "1557"
                }

                // Сценарий 2: Если знака № нет, ищем длинный номер в скобках в самом начале строки, например "(26040856)"
                var matchStartCode = Regex.Match(ToolName, @"^\s*\(\s*(\d+)\s*\)");
                if (matchStartCode.Success)
                {
                    return matchStartCode.Groups[1].Value; // Вернет "26040856"
                }

                // Сценарий 3: Если строка не подошла ни под один шаблон, отдаем её целиком, чтобы ничего не потерять
                return ToolName;
            }
        }


        // 🌟 ВЫЧИСЛЯЕМОЕ ПОЛЕ: Автоматическая сумма приладок и времени выполнения заказа
        // 🌟 ВЫЧИСЛЯЕМОЕ ПОЛЕ: Перевод общей суммы минут (приладка + работа) в часы и минуты
        public string NormTime
        {
            get
            {
                // Складываем все минуты (умножаем на SetupCount, если время приладки указано за 1 цикл)
                double totalMinutes = (SetupNormTime * SetupCount) + RunNormTime;

                if (totalMinutes <= 0) return "00:00";

                // Вычисляем целое количество часов и остаток минут
                int hours = (int)(totalMinutes / 60);
                int minutes = (int)(totalMinutes % 60);

                // Возвращаем красивую строчку для таблицы, например: "14ч 25м"
                return $"{hours:D2}:{minutes:D2}";
            }
        }


        // ПОЛЯ ДЛЯ ПЛАНОВЫХ ПРОСТОЕВ СТАНКА (ord IS NULL)
        public bool IsIdleTime { get; set; }
        public string IdleTimeName { get; set; } = string.Empty;
    }*/

    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class ProductionJobCard
    {
        // 1. ИДЕНТИФИКАТОРЫ И СВЯЗИ БД
        public long IdPlanJob { get; set; }
        public int IdEquip { get; set; }
        public int IdOrderHead { get; set; }
        public int IdNormOperation { get; set; }
        public long ItemId { get; set; }

        // 2. БАЗОВЫЕ ПАРАМЕТРЫ ЗАКАЗА (proc_rpt_job_plan)
        public string OrderNum { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty; // Из customer_name первой процедуры
        //public string UlName { get; set; } = string.Empty;       // Историческое поле для совместимости с интерфейсом WinForms
        public string OrderName { get; set; } = string.Empty;
        public int Ord { get; set; }                             // Признак фазы (0 - приладка, 1 - выполнение)
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public double PlanOutQty { get; set; }
        public double FactOutQty { get; set; }
        public double NormTimeMinutes { get; set; }              // Время текущей строки фазы в минутах

        // 3. РАЗДЕЛЬНЫЕ ПОЛЯ ТЕХНОЛОГИЧЕСКИХ ФАЗ ИЗ ПРЕДЫДУЩИХ ВАРЯНТОВ (Сохранены!)
        public double SetupNormTime { get; set; }                 // ЧИСТОЕ разделенное время приладки
        public double SetupCount { get; set; }                    // Количество приладок (1, 2)
        public double SetupCountFact { get; set; }

        public double RunNormTime { get; set; }                   // Время выполнения (как есть)
        public double RunQty { get; set; }                       // Тираж (в штуках)
        public double RunQtyFact { get; set; }

        // 4. ТЕХНОЛОГИЧЕСКИЕ ПАРАМЕТРЫ ОПЕРАЦИИ (proc_rpt_job_plan_oper_list)
        public int MainSuboper { get; set; }
        public string ItemName { get; set; } = string.Empty;       // Элемент операции (например, "Лист 01")
        public string OperProp { get; set; } = string.Empty;       // Используемые краски / свойства операции
        public string NoteManagerTech { get; set; } = string.Empty;// Заметки технолога
        public double NormTimeTotal { get; set; }

        // 5. ОCНАСТКА И ШТАМПЫ (proc_rpt_order_tool)
        public string FullToolName { get; set; } = "—";

        // Умное извлечение короткого номера штампа (№1557 или 26040856)
        /*public string ShortToolName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FullToolName) || FullToolName == "—")
                    return "—";

                var matchNo = Regex.Match(FullToolName, @"№\s*(\d+)");
                if (matchNo.Success) return matchNo.Groups[1].Value;

                var matchStart = Regex.Match(FullToolName, @"^\s*\(\s*(\d+)\s*\)");
                if (matchStart.Success) return matchStart.Groups[1].Value;

                return FullToolName;
            }
        }*/
        public string ShortToolName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FullToolName) || FullToolName == "—")
                    return "—";

                // Сценарий 1: Ищем знак № с цифрами (например, "№1557")
                var matchNo = Regex.Match(FullToolName, @"№\s*(\d+)");
                if (matchNo.Success)
                    return matchNo.Groups[1].Value;

                // Сценарий 2: Ищем ВСЕ совпадения внутри круглых скобок (например, "(26091807_1)" и "(26091807_2)")
                // Вытаскивает цифры и знаки подчеркивания внутри скобок
                var bracketMatches = Regex.Matches(FullToolName, @"\(([^)]+)\)");
                if (bracketMatches.Count > 0)
                {
                    var uniqueCodes = new List<string>();
                    foreach (Match m in bracketMatches)
                    {
                        string code = m.Groups[1].Value.Trim();
                        // Добавляем только уникальные артикулы, чтобы не дублировать
                        if (!uniqueCodes.Contains(code))
                        {
                            uniqueCodes.Add(code);
                        }
                    }

                    if (uniqueCodes.Count > 0)
                    {
                        // Соединяем найденные номера обратно через запятую
                        return string.Join(", ", uniqueCodes);
                    }
                }

                // Сценарий 3: Если скобок нет, но строка является просто коротким номером (например, "186" или "590")
                string cleanTool = FullToolName.Trim();
                if (cleanTool.Length <= 15)
                    return cleanTool;

                // Дефолтный сценарий: возвращаем оригинал, если структура не подошла
                return FullToolName;
            }
        }
        // 6. ПАРАМЕТРЫ ЗАПЕЧАТЫВАЕМОГО МАТЕРИАЛА (proc_rpt_order_paper_BY)
        public string PaperName { get; set; } = string.Empty;       // Наименование бумаги / картона
        public string FormatPaper { get; set; } = string.Empty;     // Формат операции
        public string FormatCutting { get; set; } = string.Empty;   // Формат флатовки / кроя
        public double CuttingPart { get; set; }                     // Доля реза
        public double Density { get; set; }                         // Плотность материала (г/м2)
        public double Width { get; set; }                           // Ширина роля

        // Красивый собранный текст материала для вывода в таблицу
        public string FullMaterialText
        {
            get
            {
                if (IsIdleTime || string.IsNullOrEmpty(PaperName)) return "—";
                string formatText = !string.IsNullOrEmpty(FormatCutting) ? $" {FormatCutting}" : "";
                return $"{PaperName}, {Density}г/м2{formatText}";
            }
        }

        // 7. ПОЛЯ ДЛЯ ПЛАНОВЫХ ПРОСТОЕВ СТАНКА
        public bool IsIdleTime { get; set; }
        public string IdleTimeName { get; set; } = string.Empty;

        /*public string NormTime
        {
            get
            {
                // Складываем все минуты (умножаем на SetupCount, если время приладки указано за 1 цикл)
                double totalMinutes = (SetupNormTime * SetupCount) + RunNormTime;

                if (totalMinutes <= 0) return "00:00";

                // Вычисляем целое количество часов и остаток минут
                int hours = (int)(totalMinutes / 60);
                int minutes = (int)(totalMinutes % 60);

                // Возвращаем красивую строчку для таблицы, например: "14ч 25м"
                return $"{hours:D2}:{minutes:D2}";
            }
        }*/

        // 8. АВТОМАТИЧЕСКОЕ ИНТЕЛЛЕКТУАЛЬНОЕ СВОЙСТВО ВРЕМЕНИ В ФОРМАТЕ "Хч ХХм"
        public string NormTime
        {
            get
            {
                // Если время пришло в минутах из первой процедуры (построчно), переводим его в часы
                double totalMinutes = NormTimeMinutes;

                // Если по какой-то причине в строке пусто, берем тотал из технологической карты операции
                if (totalMinutes <= 0) totalMinutes = NormTimeTotal;
                if (totalMinutes <= 0) return "00:00";

                int hours = (int)(totalMinutes / 60);
                int minutes = (int)(totalMinutes % 60);

                return $"{hours:D2}:{minutes:D2}";
            }
        }
    }


    public class ShortOrderView
    {
        public string DisplayText { get; private set; }

        public ShortOrderView(ProductionJobCard card)
        {
            if (card.IsIdleTime)
            {
                // Случай 1: Плановый простой станка (например, ТО или ремонт)
                DisplayText = $"💤 {card.IdleTimeName}";
            }
            else
            {
                // Случай 2: Активное задание (выполнение или приладка)
                // 1. Обрезаем первые 4 символа (год и месяц создания заказа)
                string shortOrderNum = card.OrderNum.Length > 4
                    ? card.OrderNum.Substring(4)
                    : card.OrderNum;

                // 2. Сжимаем большие тиражи в тысячи (65000 оттисков -> 65к)
                string qtyText = string.Empty;
                if (card.RunQty >= 1000)
                {
                    qtyText = $"({Math.Round(card.RunQty / 1000, 1)}к)";
                }
                else if (card.RunQty > 0)
                {
                    qtyText = $"({card.RunQty} шт)";
                }

                // 3. Выводим маркер, если приладка была повторной
                string setupAlert = card.SetupCount > 1 ? $" [Прл: {card.SetupCount}х]" : "";

                // Итог: "1141/П1 (3.5к) Фармтехнология ООО"
                DisplayText = $"📝 {shortOrderNum}: {setupAlert} {card.CustomerName} {qtyText}";
            }
        }
    }

    public class OrdersLoad
    {
        public int TypeJob;
        public int IDManPlanJob;
        public string TimeStartOrder;
        public string TimeEndOrder;
        public string numberOfOrder;
        public string nameCustomer;
        public string nameItem;
        public int makereadyTime;
        public int workTime;
        public int amountOfOrder;
        public string stamp;
        public string headOrder;

        public OrdersLoad(int typeJob, int idManPlanJob, string timeStartOrder, string timeEndOrder, string number, string customer, string item, int mkTime, int wkTime, int amount, string orderStamp, string head)
        {
            this.TypeJob = typeJob;
            this.IDManPlanJob = idManPlanJob;
            this.TimeStartOrder = timeStartOrder;
            this.TimeEndOrder = timeEndOrder;
            this.numberOfOrder = number;
            this.nameCustomer = customer;
            this.nameItem = item;
            this.makereadyTime = mkTime;
            this.workTime = wkTime;
            this.amountOfOrder = amount;
            this.stamp = orderStamp;
            this.headOrder = head;
        }

        public OrdersLoad(string number, string customer, string item, int mkTime, int wkTime, int amount, string orderStamp, string head)
        {
            this.TypeJob = 0;
            this.IDManPlanJob = -1;
            this.TimeStartOrder = "";
            this.TimeEndOrder = "";
            this.numberOfOrder = number;
            this.nameCustomer = customer;
            this.nameItem = item;
            this.makereadyTime = mkTime;
            this.workTime = wkTime;
            this.amountOfOrder = amount;
            this.stamp = orderStamp;
            this.headOrder = head;
        }
    }
}
