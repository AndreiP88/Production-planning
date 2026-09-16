using System;

namespace data
{
    public class ProductionJobCard
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

        // ПОЛЯ ДЛЯ ПЛАНОВЫХ ПРОСТОЕВ СТАНКА (ord IS NULL)
        public bool IsIdleTime { get; set; }
        public string IdleTimeName { get; set; } = string.Empty;
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
                    qtyText = $" ({Math.Round(card.RunQty / 1000, 1)}к)";
                }
                else if (card.RunQty > 0)
                {
                    qtyText = $" ({card.RunQty} шт)";
                }

                // 3. Выводим маркер, если приладка была повторной
                string setupAlert = card.SetupCount > 1 ? $" [Прл: {card.SetupCount}х]" : "";

                // Итог: "1141/П1 (3.5к) Фармтехнология ООО"
                DisplayText = $"{shortOrderNum}{qtyText}{setupAlert} {card.UlName}";
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
