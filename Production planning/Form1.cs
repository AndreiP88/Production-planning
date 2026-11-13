using data;
using data.Models;
using database;
using MaterialSkin;
using MaterialSkin.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Production_planning
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme((Primary)0x00796b, (Primary)0x009688, (Primary)0x009685, Accent.DeepOrange200, TextShade.WHITE);

            gridViewAbsence.DefaultCellStyle.SelectionBackColor = gridViewAbsence.DefaultCellStyle.BackColor;
            gridViewAbsence.DefaultCellStyle.SelectionForeColor = gridViewAbsence.DefaultCellStyle.ForeColor;
        }
        private ConnectionParameter parameter = new ConnectionParameter();

        CancellationTokenSource cancelTokenSource;

        private List<PositionLookupDto> _positions;
        private List<int> _employeeShortsIndexes = new List<int>();

        List<WorkAreaInfo> workAreaInfo;
        List<ShiftDefinitionModel> shiftDefinitions;
        List<ScheduleCycleModel> scheduleCycles;
        List<ScheduleTemplateModel> scheduleTemplates;
        List<EmployeeShortRow> employeeShorts;
        private List<AbsenceType> _absenceTypes;
        private Dictionary<int, StatusColorStyle> _colorPalette = new Dictionary<int, StatusColorStyle>();
        private Dictionary<int, DataGridViewCellStyle> _cellStyles = new Dictionary<int, DataGridViewCellStyle>();
        private Dictionary<int, DataGridViewCellStyle> _sharedStyles = new Dictionary<int, DataGridViewCellStyle>();
        private int _lastSelectedRowIndex = -1;
        private int _lastSelectedColumnIndex = -1;
        private int _firstVisibleRowIndex = -1;
        //private int _firstVisibleColumnIndex = -1;
        private int _horizontalPixelOffset = 0;
        private bool _editShift = false;

        private async void Form1_Load(object sender, EventArgs e)
        {
            var service = new ReportStaffing(parameter.GetMySQLConnectionString());

            _colorPalette = await service.GetColorPaletteAsync();
            foreach (var color in _colorPalette)
            {
                var style = new DataGridViewCellStyle();
                style.BackColor = ColorTranslator.FromHtml(color.Value.HexBackColor);
                style.ForeColor = ColorTranslator.FromHtml(color.Value.HexForeColor);

                // Можно сразу задать шрифт и выравнивание, чтобы таблица не тратила на это время
                //style.Alignment = DataGridViewContentAlignment.MiddleLeft;

                _cellStyles[color.Key] = style;
            }

            _sharedStyles.Clear();
            foreach (var color in _colorPalette)
            {
                var style = new DataGridViewCellStyle();
                style.BackColor = ColorTranslator.FromHtml(color.Value.HexBackColor);
                style.ForeColor = ColorTranslator.FromHtml(color.Value.HexForeColor);

                // Присваиваем ссылку на готовый объект в кэш
                _sharedStyles[color.Key] = style;
            }

            // Жестко отключаем авто-расчет высоты строк (самый тяжелый процесс)
            //dataGridPlanning.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            // Жестко отключаем авто-расчет ширины колонок по ячейкам
            dataGridPlanning.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // Отключаем авто-размер заголовков
            dataGridPlanning.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridPlanning.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;


            // Текст выделенной ячейки будет такого же цвета, как и у обычной ячейки
            dataGridPlanning.DefaultCellStyle.SelectionForeColor = dataGridPlanning.DefaultCellStyle.ForeColor;

            DateTime date = DateTime.Now;

            int year = 2025;

            while (year <= date.Year)
            {
                planComboBoxYear.Items.Add(year);
                comboBoxAbsenceYear.Items.Add(year);

                year++;
            }

            planComboBoxYear.SelectedIndex = planComboBoxYear.Items.Count - 1;
            planComboBoxMonth.SelectedIndex = DateTime.Now.Month - 1;

            comboBoxAbsenceYear.SelectedIndex = comboBoxAbsenceYear.Items.Count - 1;
            comboBoxAbsenceMonth.SelectedIndex = DateTime.Now.Month - 1;

            materialTabControl1.SelectedIndex = 0;
            materialTabControl1_SelectedIndexChangedAsync(sender, e);
        }

        



        private async Task ViewPlanForEquipAsync()
        {
            //CreateColomnsToDataGridForPlan();
            if (workPlanAreaComboBox.SelectedItem is WorkAreaInfo workArea)
            {
                if (workPlanEquipComboBox.SelectedItem is EquipmentShortInfo equipments)
                {
                    int.TryParse(equipments.Code, out int equip);
                    
                    //List<EquipmentPlanRowItem> equipmentPlan = await PrepareEquipmentPlanAsync(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(7), equipments.Id, equip);

                    //FillEquipmentPlanGrid(equipmentPlan);

                    await LoadSingleEquipmentPlanAsync(DateTime.Now.AddDays(-5), DateTime.Now.AddDays(10), equipments.Id, equip, workArea.Settings);
                }
            }
            
        }

        private async void materialButton1_Click(object sender, EventArgs e)
        {
            await ViewPlanForEquipAsync();

/*

            var valueOrders = new ValueOrders();
            var param = new ConnectionParameter();

            // 2. Скачивание данных из двух СУБД [2026-09-14]
            List<ProductionJobCard> jobs = await valueOrders.GetProductionJobsAsync(start, end, param.GetSQLServerConnectionString());
            var filteredJobs = (jobs ?? new List<ProductionJobCard>())
                .Where(j => j.IdEquip == externalEquipId)
                .OrderBy(j => j.DateBegin).ToList();

            List<EquipmentStaffingPeriodRow> shifts = await valueOrders.GetEquipmentStaffingForPeriodAsync(start, end, mySqlEquipmentId, param.GetMySQLConnectionString());

*/
        }

        /*private async Task LoadSingleEquipmentPlanAsync(DateTime start, DateTime end, int mySqlEquipmentId, int externalEquipId, WorkAreaVisibleColumns columnsSettings)
        {
            try
            {
                this.UseWaitCursor = true;
                Cursor.Current = Cursors.WaitCursor;

                dataGridViewPlan.SuspendLayout();
                dataGridViewPlan.Rows.Clear();
                dataGridViewPlan.Columns.Clear();

                // 1. Инициализация колонок (14 столбцов)
                dataGridViewPlan.Columns.Add("Staff", "Исполнитель");              // 0
                dataGridViewPlan.Columns.Add("OrderNum", "Номер заказа");         // 1
                dataGridViewPlan.Columns.Add("Customer", "Заказчик");             // 2
                dataGridViewPlan.Columns.Add("OrderName", "Наименование заказа"); // 3
                dataGridViewPlan.Columns.Add("ItemName", "Элемент");               // 4
                dataGridViewPlan.Columns.Add("OperProp", "Краски");               // 5
                dataGridViewPlan.Columns.Add("Material", "Материал");             // 6
                dataGridViewPlan.Columns.Add("TechNote", "Заметки технолога");    // 7
                dataGridViewPlan.Columns.Add("Qty", "Плановая выработка");        // 8
                dataGridViewPlan.Columns.Add("Start", "Дата и время начала");     // 9
                dataGridViewPlan.Columns.Add("End", "Дата и время окончания");     // 10
                dataGridViewPlan.Columns.Add("Duration", "Длительность");         // 11
                dataGridViewPlan.Columns.Add("ToolName", "Штамп");                // 12
                dataGridViewPlan.Columns.Add("Cutting", "Подрезка листа");        // 13

                foreach (DataGridViewColumn column in dataGridViewPlan.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                dataGridViewPlan.SelectionMode = DataGridViewSelectionMode.CellSelect;
                dataGridViewPlan.DefaultCellStyle.SelectionBackColor = Color.White;
                dataGridViewPlan.DefaultCellStyle.SelectionForeColor = Color.Black;

                dataGridViewPlan.EnableHeadersVisualStyles = false;
                dataGridViewPlan.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
                dataGridViewPlan.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dataGridViewPlan.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridViewPlan.Font, FontStyle.Bold);
                dataGridViewPlan.ColumnHeadersHeight = 50;

                dataGridViewPlan.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                dataGridViewPlan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridViewPlan.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

                // Настройка ширины
                dataGridViewPlan.Columns["Staff"].Width = 140;
                dataGridViewPlan.Columns["OrderNum"].Width = 110;
                dataGridViewPlan.Columns["Customer"].Width = 120;
                dataGridViewPlan.Columns["OrderName"].Width = 170;
                dataGridViewPlan.Columns["ItemName"].Width = 140;
                dataGridViewPlan.Columns["OperProp"].Width = 140;
                dataGridViewPlan.Columns["Material"].Width = 160;
                dataGridViewPlan.Columns["TechNote"].Width = 140;
                dataGridViewPlan.Columns["Qty"].Width = 80;
                dataGridViewPlan.Columns["Start"].Width = 110;
                dataGridViewPlan.Columns["End"].Width = 110;
                dataGridViewPlan.Columns["Duration"].Width = 110;
                dataGridViewPlan.Columns["ToolName"].Width = 110;
                dataGridViewPlan.Columns["Cutting"].Width = 100;

                // 🌟 ИНТЕГРАЦИЯ ДИНАМИЧЕСКИХ ШАБЛОНОВ ВИДИМОСТИ УЧАСТКА
                var settings = columnsSettings ?? WorkAreaVisibleColumns.GetDefaultSettings();

                dataGridViewPlan.Columns["OrderName"].Visible = settings.OrderName;
                dataGridViewPlan.Columns["ItemName"].Visible = settings.Element;
                dataGridViewPlan.Columns["OperProp"].Visible = settings.Colors;
                dataGridViewPlan.Columns["Material"].Visible = settings.Material;
                dataGridViewPlan.Columns["TechNote"].Visible = settings.TechNotes;
                dataGridViewPlan.Columns["Qty"].Visible = settings.PlannedOutput;
                dataGridViewPlan.Columns["Start"].Visible = settings.DateTimeStart;
                dataGridViewPlan.Columns["End"].Visible = settings.DateTimeEnd;
                dataGridViewPlan.Columns["Duration"].Visible = settings.Duration;
                dataGridViewPlan.Columns["ToolName"].Visible = settings.Stamp;
                dataGridViewPlan.Columns["Cutting"].Visible = settings.SheetTrimming;

                var valueOrders = new ValueOrders();
                var param = new ConnectionParameter();

                List<ProductionJobCard> jobs = await valueOrders.GetProductionJobsAsync(start, end, param.GetSQLServerConnectionString(), externalEquipId);
                var filteredJobs = (jobs ?? new List<ProductionJobCard>()).Where(j => j.IdEquip == externalEquipId).OrderBy(j => j.DateBegin).ToList();
                List<EquipmentStaffingPeriodRow> shifts = await valueOrders.GetEquipmentStaffingForPeriodAsync(start, end, mySqlEquipmentId, param.GetMySQLConnectionString());

                var shiftIntervals = (shifts ?? new List<EquipmentStaffingPeriodRow>()).Select(s => new {
                    Start = s.Date.Date.Add(s.TimeStart),
                    End = s.TimeEnd < s.TimeStart ? s.Date.Date.AddDays(1).Add(s.TimeEnd) : s.Date.Date.Add(s.TimeEnd),
                    Data = s
                }).OrderBy(s => s.Start).ToList();

                var rowsToInsert = new List<DataGridViewRow>();
                bool isFirstRow = true;

                // ГЛОБАЛЬНЫЙ КУРСОР ТАЙМЛАЙНА
                DateTime globalTimelineCursor = start.Date;

                // 3. Распределение MES-задач по сетке смен
                foreach (var job in filteredJobs)
                {
                    DateTime currentJobCursor = job.DateBegin < globalTimelineCursor ? globalTimelineCursor : job.DateBegin;
                    DateTime jobEnd = job.DateEnd;

                    if (jobEnd <= currentJobCursor)
                    {
                        jobEnd = currentJobCursor.AddMinutes(5);
                    }

                    var crossedShifts = shiftIntervals.Where(s => s.Start < jobEnd && s.End > currentJobCursor).ToList();

                    if (crossedShifts.Count == 0)
                    {
                        var prevShift = shiftIntervals.LastOrDefault(s => s.End <= currentJobCursor);
                        var nextShift = shiftIntervals.FirstOrDefault(s => s.Start >= jobEnd);
                        DateTime gapStart = prevShift != null ? prevShift.End : start.Date;
                        DateTime gapEnd = nextShift != null ? nextShift.Start : end.Date.AddDays(1);

                        AddTimelineRow(rowsToInsert, currentJobCursor, jobEnd, job, null, $"GAP_SHIFT_{gapStart:yyyyMMddHHmm}_{gapEnd:yyyyMMmm}", ref isFirstRow);

                        if (jobEnd > globalTimelineCursor) globalTimelineCursor = jobEnd;
                        continue;
                    }

                    foreach (var shift in crossedShifts)
                    {
                        DateTime segmentStart = currentJobCursor > shift.Start ? currentJobCursor : shift.Start;
                        DateTime segmentEnd = jobEnd < shift.End ? jobEnd : shift.End;

                        if ((segmentEnd - segmentStart).TotalMinutes < 2 && segmentStart != job.DateBegin && segmentEnd != job.DateEnd) continue;

                        string currentShiftKey = $"SHIFT_{shift.Start:yyyyMMdd_HHmm}_{shift.Data.ShiftNum}";
                        EquipmentStaffingPeriodRow shiftDataToSend = shift.Data;

                        if (shift.Data.IsCancelled == 1)
                        {
                            bool hasJobBoundary = filteredJobs.Any(j => (j.DateBegin >= shift.Start && j.DateBegin <= shift.End) || (j.DateEnd >= shift.Start && j.DateEnd <= shift.End));
                            if (hasJobBoundary) currentShiftKey += "_CANCELLED";
                            else { currentJobCursor = segmentEnd; continue; }
                        }

                        AddTimelineRow(rowsToInsert, segmentStart, segmentEnd, job, shiftDataToSend, currentShiftKey, ref isFirstRow);

                        currentJobCursor = segmentEnd;

                        // 🌟 Сдвигаем глобальный курсор времени вслед за обработанным сегментом
                        if (segmentEnd > globalTimelineCursor) globalTimelineCursor = segmentEnd;

                        if (currentJobCursor >= jobEnd) break;
                    }
                }

                dataGridViewPlan.Rows.AddRange(rowsToInsert.ToArray());
                ApplySmartTimelineMerging();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка генерации: {ex.Message}");
            }
            finally
            {
                dataGridViewPlan.ResumeLayout(true);
                this.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;
                dataGridViewPlan.Refresh();
            }
        }


        private void AddTimelineRow(List<DataGridViewRow> list, DateTime start, DateTime end, ProductionJobCard job, EquipmentStaffingPeriodRow shift, string shiftKey, ref bool isFirstRow)
        {
            string staffText = "—";
            if (shift != null)
            {
                string empName = string.IsNullOrEmpty(shift.StaffNames) ? "???" : shift.StaffNames;
                staffText = shift.IsCancelled == 1
                    ? $"{shift.Date:dd.MM.yy}\n{shift.ShiftName}\n{empName}\n(Отменена)"
                    : $"{shift.Date:dd.MM.yy}\n{shift.ShiftName}\n{empName}";
            }

            isFirstRow = false;

            string orderNum = job.IsIdleTime ? "—" : job.OrderNum;
            string customer = job.IsIdleTime ? "—" : job.CustomerName;
            string orderName = job.IsIdleTime ? $"⚙ {job.IdleTimeName}" : job.OrderName;

            string itemName = (job.IsIdleTime || string.IsNullOrEmpty(job.ItemName)) ? "—" : job.ItemName;
            string operProp = (job.IsIdleTime || string.IsNullOrEmpty(job.OperProp)) ? "—" : job.OperProp;
            string materialText = job.IsIdleTime ? "—" : job.FullMaterialText;
            string techNote = (job.IsIdleTime || string.IsNullOrEmpty(job.NoteManagerTech)) ? "—" : job.NoteManagerTech;

            string formattedOrderNum = FormatOrderNumberWithTechNotes(orderNum, itemName, techNote);

            string qty;

            if (!job.IsIdleTime)
            {
                if (job.RunQtyFact <= 0)
                {
                    qty = $"{job.RunQty:N0}";
                }
                else
                {
                    qty = $"{(job.RunQty - job.RunQtyFact):N0}/{job.RunQty:N0}";
                }
            }
            else
            {
                qty = "—";
            }
            

            string durationText = job.IsIdleTime ? job.NormTime : job.NormTime;
            string toolText = job.IsIdleTime ? "—" : job.ShortToolName;

            // Новое поле подрезки листа
            string cuttingText = job.IsIdleTime ? "—" : (job.CuttingPart > 1 ? "Да" : "Нет");

            string orderKey = job.IsIdleTime ? $"IDLE_{job.IdPlanJob}" : $"JOB_{job.IdPlanJob}";

            var row = new DataGridViewRow();
            row.CreateCells(dataGridViewPlan);

            // Заполнение ячеек 0 - 13 (Штамп и Подрезка ушли в самый конец)
            row.Cells[0].Value = staffText;
            row.Cells[1].Value = formattedOrderNum;
            row.Cells[2].Value = customer;
            row.Cells[3].Value = orderName;
            row.Cells[4].Value = itemName;
            row.Cells[5].Value = operProp;
            row.Cells[6].Value = materialText;
            row.Cells[7].Value = techNote;
            row.Cells[8].Value = qty;
            row.Cells[9].Value = $"{job.DateBegin:dd.MM.yy H:mm}";
            row.Cells[10].Value = $"{job.DateEnd:dd.MM.yy H:mm}";
            row.Cells[11].Value = durationText;
            row.Cells[12].Value = toolText;
            row.Cells[13].Value = cuttingText;

            row.Cells[0].Tag = shiftKey;

            if (job.IsIdleTime)
            {
                string horizKey = $"HORIZ_{orderKey}";
                for (int c = 1; c <= 7; c++) row.Cells[c].Tag = horizKey;
                for (int c = 8; c <= 13; c++) row.Cells[c].Tag = horizKey;
            }
            else
            {
                // Прописываем оригинальные COL-теги слияния заказа
                for (int c = 1; c <= 8; c++) row.Cells[c].Tag = $"COL{c}_{orderKey}";
                row.Cells[12].Tag = $"COL12_{orderKey}"; // Штамп
                row.Cells[13].Tag = $"COL13_{orderKey}"; // Подрезка
            }

            row.Cells[9].Tag = $"COL9_{orderKey}";
            row.Cells[10].Tag = $"COL10_{orderKey}";
            row.Cells[11].Tag = $"COL11_{orderKey}";

            if (job.IsIdleTime)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
                row.DefaultCellStyle.ForeColor = Color.DimGray;
            }

            int baseHeight = 65;
            int nameLength = orderName.Length;
            int estimatedTextLines = (int)Math.Ceiling((double)nameLength / 20);
            if (estimatedTextLines > 2)
            {
                int calculatedHeight = 30 + (estimatedTextLines * 15);
                if (calculatedHeight > baseHeight) baseHeight = calculatedHeight;
            }

            row.Height = baseHeight;
            list.Add(row);
        }

        private void ApplySmartTimelineMerging()
        {
            if (dataGridViewPlan.Rows.Count == 0 || dataGridViewPlan.Columns.Count < 14) return;

            Font boldFont = new Font(dataGridViewPlan.Font, FontStyle.Bold);
            Font regularFont = new Font(dataGridViewPlan.Font, FontStyle.Regular);
            Font fontBig = new Font(new Font("Arial", 14), FontStyle.Regular);

            // -----------------------------------------------------------------
            // ШАГ 1: ВЕРТИКАЛЬНОЕ И ГОРИЗОНТАЛЬНОЕ СЛИЯНИЕ ЗАКАЗОВ (КОЛОНКИ 1 - 13)
            // -----------------------------------------------------------------
            int rIndex = 0;
            while (rIndex < dataGridViewPlan.Rows.Count)
            {
                string cell1Tag = dataGridViewPlan.Rows[rIndex].Cells[1].Tag?.ToString();
                int maxRowsMergedInThisStep = 1;

                if (cell1Tag != null && cell1Tag.StartsWith("HORIZ_"))
                {
                    string idleNameText = dataGridViewPlan.Rows[rIndex].Cells[3].Value?.ToString() ?? "⚙️ Технологический простой";

                    int idleVerticalCount = 1;
                    while (rIndex + idleVerticalCount < dataGridViewPlan.Rows.Count &&
                           dataGridViewPlan.Rows[rIndex + idleVerticalCount].Cells[1].Tag?.ToString() == cell1Tag)
                    {
                        idleVerticalCount++;
                    }

                    GridHelper.MergeCells(dataGridViewPlan, idleNameText, rIndex, 1, idleVerticalCount, 7,
                                         Color.FromArgb(248, 249, 250), Color.DimGray, fontBig, StringAlignment.Center, StringAlignment.Center);

                    int[] extraCols = { 8, 9, 10, 11, 12, 13 };
                    foreach (int col in extraCols)
                    {
                        string orderText = (col == 12 || col == 13) ? "—" : (dataGridViewPlan.Rows[rIndex].Cells[col].Value?.ToString() ?? "");
                        GridHelper.MergeCells(dataGridViewPlan, orderText, rIndex, col, idleVerticalCount, 1,
                                             Color.FromArgb(248, 249, 250), Color.DimGray, regularFont, StringAlignment.Center, StringAlignment.Center);
                    }

                    if (idleVerticalCount > maxRowsMergedInThisStep) maxRowsMergedInThisStep = idleVerticalCount;
                }
                else
                {
                    // Слияние текстовой части, выработки, штампа и подрезки (1-8, 12-13)
                    int[] orderTextCols = { 1, 2, 3, 4, 5, 6, 7, 8, 12, 13 };
                    foreach (int col in orderTextCols)
                    {
                        int startOrderRow = rIndex;
                        string orderTag = dataGridViewPlan.Rows[rIndex].Cells[col].Tag?.ToString();
                        string orderText = dataGridViewPlan.Rows[rIndex].Cells[col].Value?.ToString() ?? "";

                        int count = 1;
                        while (rIndex + count < dataGridViewPlan.Rows.Count && dataGridViewPlan.Rows[rIndex + count].Cells[col].Tag?.ToString() == orderTag)
                        {
                            count++;
                        }

                        GridHelper.MergeCells(dataGridViewPlan, orderText, startOrderRow, col, count, 1, Color.White, Color.Black, regularFont, StringAlignment.Center, StringAlignment.Center);
                        if (count > maxRowsMergedInThisStep) maxRowsMergedInThisStep = count;
                    }

                    // Слияние временной зоны дат (9 - 11)
                    for (int col = 9; col <= 11; col++)
                    {
                        int startOrderRow = rIndex;
                        string orderTag = dataGridViewPlan.Rows[rIndex].Cells[col].Tag?.ToString();
                        string orderText = dataGridViewPlan.Rows[rIndex].Cells[col].Value?.ToString() ?? "";

                        int count = 1;
                        while (rIndex + count < dataGridViewPlan.Rows.Count && dataGridViewPlan.Rows[rIndex + count].Cells[col].Tag?.ToString() == orderTag)
                        {
                            count++;
                        }

                        GridHelper.MergeCells(dataGridViewPlan, orderText, startOrderRow, col, count, 1, Color.White, Color.Black, regularFont, StringAlignment.Center, StringAlignment.Center);
                        if (count > maxRowsMergedInThisStep) maxRowsMergedInThisStep = count;
                    }
                }

                rIndex += maxRowsMergedInThisStep;
            }

            // -----------------------------------------------------------------
            // ШАГ 2: КОРРЕКТНОЕ СЛИЯНИЕ КОЛОНКИ СМЕН С ЗАЩИТОЙ ОТ НАКЛАДОК СТРОК
            // -----------------------------------------------------------------
            int startStaffRow = 0;
            while (startStaffRow < dataGridViewPlan.Rows.Count)
            {
                string currentStaffTag = dataGridViewPlan.Rows[startStaffRow].Cells[0].Tag?.ToString();

                if (string.IsNullOrEmpty(currentStaffTag))
                {
                    startStaffRow++;
                    continue;
                }

                int staffMergedCount = 1;
                string validStaffText = dataGridViewPlan.Rows[startStaffRow].Cells[0].Value?.ToString();

                while (startStaffRow + staffMergedCount < dataGridViewPlan.Rows.Count)
                {
                    string nextStaffTag = dataGridViewPlan.Rows[startStaffRow + staffMergedCount].Cells[0].Tag?.ToString();
                    string nextStaffText = dataGridViewPlan.Rows[startStaffRow + staffMergedCount].Cells[0].Value?.ToString();

                    // 🌟 ТОЧЕЧНАЯ ПРАВКА: Сливаем ячейку смены, только если совпадает код смены 
                    // И ПРИ ЭТОМ в соседней колонке 1 (Номер заказа) находится один и тот же блок!
                    // Это железно блокирует перетекание смены внутрь чужого накладывающегося заказа.
                    if (currentStaffTag == nextStaffTag &&
                        dataGridViewPlan.Rows[startStaffRow].Cells[1].Tag?.ToString() == dataGridViewPlan.Rows[startStaffRow + staffMergedCount].Cells[1].Tag?.ToString())
                    {
                        if ((string.IsNullOrEmpty(validStaffText) || validStaffText == "—") && !string.IsNullOrEmpty(nextStaffText) && nextStaffText != "—")
                        {
                            validStaffText = nextStaffText;
                        }
                        staffMergedCount++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (string.IsNullOrEmpty(validStaffText) || validStaffText == "—")
                {
                    validStaffText = "Дежурная смена\nПерсонал не назначен";
                }

                GridHelper.MergeCells(dataGridViewPlan, validStaffText, startStaffRow, 0, staffMergedCount, 1, Color.White, Color.Black, boldFont, StringAlignment.Center, StringAlignment.Center);
                startStaffRow += staffMergedCount;
            }
        }*/



        private async Task LoadSingleEquipmentPlanAsync(DateTime start, DateTime end, int mySqlEquipmentId, int externalEquipId, WorkAreaVisibleColumns columnsSettings)
        {
            try
            {
                this.UseWaitCursor = true;
                Cursor.Current = Cursors.WaitCursor;

                dataGridViewPlan.SuspendLayout();
                dataGridViewPlan.Rows.Clear();
                dataGridViewPlan.Columns.Clear();

                // Инициализация колонок
                dataGridViewPlan.Columns.Add("Staff", "Исполнитель");              // 0
                dataGridViewPlan.Columns.Add("OrderNum", "Номер заказа");         // 1
                dataGridViewPlan.Columns.Add("Customer", "Заказчик");             // 2
                dataGridViewPlan.Columns.Add("OrderName", "Наименование заказа"); // 3
                dataGridViewPlan.Columns.Add("ItemName", "Элемент");               // 4
                dataGridViewPlan.Columns.Add("OperProp", "Краски");               // 5
                dataGridViewPlan.Columns.Add("Material", "Материал");             // 6
                dataGridViewPlan.Columns.Add("TechNote", "Заметки технолога");    // 7
                dataGridViewPlan.Columns.Add("Qty", "Плановая выработка");        // 8
                dataGridViewPlan.Columns.Add("Start", "Дата и время начала");     // 9
                dataGridViewPlan.Columns.Add("End", "Дата и время окончания");     // 10
                dataGridViewPlan.Columns.Add("Duration", "Длительность");         // 11
                dataGridViewPlan.Columns.Add("ToolName", "Штамп");                // 12
                dataGridViewPlan.Columns.Add("Cutting", "Подрезка листа");        // 13

                foreach (DataGridViewColumn column in dataGridViewPlan.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                dataGridViewPlan.SelectionMode = DataGridViewSelectionMode.CellSelect;
                dataGridViewPlan.DefaultCellStyle.SelectionBackColor = Color.White;
                dataGridViewPlan.DefaultCellStyle.SelectionForeColor = Color.Black;

                dataGridViewPlan.EnableHeadersVisualStyles = false;
                dataGridViewPlan.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
                dataGridViewPlan.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dataGridViewPlan.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridViewPlan.Font, FontStyle.Bold);
                dataGridViewPlan.ColumnHeadersHeight = 50;

                dataGridViewPlan.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                dataGridViewPlan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridViewPlan.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

                // Настройка ширины
                dataGridViewPlan.Columns["Staff"].Width = 140;
                dataGridViewPlan.Columns["OrderNum"].Width = 110;
                dataGridViewPlan.Columns["Customer"].Width = 120;
                dataGridViewPlan.Columns["OrderName"].Width = 170;
                dataGridViewPlan.Columns["ItemName"].Width = 140;
                dataGridViewPlan.Columns["OperProp"].Width = 140;
                dataGridViewPlan.Columns["Material"].Width = 160;
                dataGridViewPlan.Columns["TechNote"].Width = 140;
                dataGridViewPlan.Columns["Qty"].Width = 80;
                dataGridViewPlan.Columns["Start"].Width = 110;
                dataGridViewPlan.Columns["End"].Width = 110;
                dataGridViewPlan.Columns["Duration"].Width = 110;
                dataGridViewPlan.Columns["ToolName"].Width = 110;
                dataGridViewPlan.Columns["Cutting"].Width = 100;

                // Настройка шаблонов видимости столбцов участка
                var settings = columnsSettings ?? WorkAreaVisibleColumns.GetDefaultSettings();
                dataGridViewPlan.Columns["OrderName"].Visible = settings.OrderName;
                dataGridViewPlan.Columns["ItemName"].Visible = settings.Element;
                dataGridViewPlan.Columns["OperProp"].Visible = settings.Colors;
                dataGridViewPlan.Columns["Material"].Visible = settings.Material;
                dataGridViewPlan.Columns["TechNote"].Visible = settings.TechNotes;
                dataGridViewPlan.Columns["Qty"].Visible = settings.PlannedOutput;
                dataGridViewPlan.Columns["Start"].Visible = settings.DateTimeStart;
                dataGridViewPlan.Columns["End"].Visible = settings.DateTimeEnd;
                dataGridViewPlan.Columns["Duration"].Visible = settings.Duration;
                dataGridViewPlan.Columns["ToolName"].Visible = settings.Stamp;
                dataGridViewPlan.Columns["Cutting"].Visible = settings.SheetTrimming;

                var valueOrders = new ValueOrders();
                var param = new ConnectionParameter();

                // Загружаем исходные данные
                List<ProductionJobCard> jobs = await valueOrders.GetProductionJobsAsync(start, end, param.GetSQLServerConnectionString(), externalEquipId);

                // Очищаем выборку от обедов на дальних подступах
                var filteredJobs = (jobs ?? new List<ProductionJobCard>())
                    .Where(j => j.IdEquip == externalEquipId)
                    .Where(j => !(j.OrderNum?.ToString() ?? "").Contains("***") && !(j.IdleTimeName?.ToString() ?? "").ToLower().Contains("обед"))
                    .OrderBy(j => j.DateBegin)
                    .ToList();

                // 🌟 Вычисляем начальную точку, сжимая пустые дни в начале диапазона
                DateTime effectiveStart = start;
                DateTime today = DateTime.Today;

                if (filteredJobs.Count > 0)
                {
                    DateTime firstJobDate = filteredJobs.Min(j => j.DateBegin).Date;
                    effectiveStart = firstJobDate < today ? firstJobDate : today;
                }
                else
                {
                    effectiveStart = today;
                }

                List<EquipmentStaffingPeriodRow> shifts = await valueOrders.GetEquipmentStaffingForPeriodAsync(effectiveStart.AddDays(-1), end.AddDays(1), mySqlEquipmentId, param.GetMySQLConnectionString());

                var shiftIntervals = (shifts ?? new List<EquipmentStaffingPeriodRow>()).Select(s => new {
                    Start = s.Date.Date.Add(s.TimeStart),
                    End = s.TimeEnd < s.TimeStart ? s.Date.Date.AddDays(1).Add(s.TimeEnd) : s.Date.Date.Add(s.TimeEnd),
                    Data = s
                }).Where(s => s.End > effectiveStart).OrderBy(s => s.Start).ToList();

                var rowsToInsert = new List<DataGridViewRow>();
                bool isFirstRow = true;

                DateTime globalTimelineCursor = effectiveStart;
                var usedShiftKeys = new HashSet<string>();

                // ВАШ РОДНОЙ ЦИКЛ ПО ЗАКАЗАМ
                foreach (var job in filteredJobs)
                {
                    if (job.DateEnd <= effectiveStart) continue;

                    DateTime currentJobCursor = job.DateBegin < globalTimelineCursor ? globalTimelineCursor : job.DateBegin;
                    DateTime jobEnd = job.DateEnd;

                    if (jobEnd <= currentJobCursor)
                    {
                        jobEnd = currentJobCursor.AddMinutes(5);
                    }

                    var crossedShifts = shiftIntervals.Where(s => s.Start < jobEnd && s.End > currentJobCursor).ToList();

                    if (crossedShifts.Count == 0)
                    {
                        var prevShift = shiftIntervals.LastOrDefault(s => s.End <= currentJobCursor);
                        var nextShift = shiftIntervals.FirstOrDefault(s => s.Start >= jobEnd);
                        DateTime gapStart = prevShift != null ? prevShift.End : effectiveStart;
                        DateTime gapEnd = nextShift != null ? nextShift.Start : end.Date.AddDays(1);

                        AddTimelineRow(rowsToInsert, currentJobCursor, jobEnd, job, null, $"GAP_SHIFT_{gapStart:yyyyMMddHHmm}_{gapEnd:yyyyMMmm}", ref isFirstRow);

                        if (jobEnd > globalTimelineCursor) globalTimelineCursor = jobEnd;
                        continue;
                    }

                    foreach (var shift in crossedShifts)
                    {
                        DateTime segmentStart = currentJobCursor > shift.Start ? currentJobCursor : shift.Start;
                        DateTime segmentEnd = jobEnd < shift.End ? jobEnd : shift.End;

                        if ((segmentEnd - segmentStart).TotalMinutes < 2 && segmentStart != job.DateBegin && segmentEnd != job.DateEnd) continue;

                        string currentShiftKey = $"SHIFT_{shift.Start:yyyyMMdd_HHmm}_{shift.Data.ShiftNum}";
                        usedShiftKeys.Add(currentShiftKey);

                        EquipmentStaffingPeriodRow shiftDataToSend = shift.Data;

                        if (shift.Data.IsCancelled == 1)
                        {
                            bool hasJobBoundary = filteredJobs.Any(j => (j.DateBegin >= shift.Start && j.DateBegin <= shift.End) || (j.DateEnd >= shift.Start && j.DateEnd <= shift.End));
                            if (hasJobBoundary) currentShiftKey += "_CANCELLED";
                            else { currentJobCursor = segmentEnd; continue; }
                        }

                        AddTimelineRow(rowsToInsert, segmentStart, segmentEnd, job, shiftDataToSend, currentShiftKey, ref isFirstRow);

                        currentJobCursor = segmentEnd;

                        if (segmentEnd > globalTimelineCursor) globalTimelineCursor = segmentEnd;
                        if (currentJobCursor >= jobEnd) break;
                    }
                }

                // Вывод абсолютно пустых смен диспетчера
                foreach (var shift in shiftIntervals)
                {
                    // Строго отсекаем смены, которые выходят за рамки эффективного диапазона
                    // 🌟 КЛЮЧЕВАЯ ПРАВКА: shift.Start >= effectiveStart гарантирует, 
                    // что пустые смены из "вчерашнего" дня больше не попадут на экран!
                    if (shift.End <= effectiveStart || shift.Start >= end || shift.Start < effectiveStart) continue;

                    string currentShiftKey = $"SHIFT_{shift.Start:yyyyMMdd_HHmm}_{shift.Data.ShiftNum}";

                    if (usedShiftKeys.Contains(currentShiftKey)) continue;
                    if (shift.Data.IsCancelled == 1) continue; // Не выводим отмененные

                    AddTimelineRow(rowsToInsert, shift.Start, shift.End, null, shift.Data, currentShiftKey, ref isFirstRow);
                }

                // Финальная сортировка строк по дате и времени начала (столбец 9)
                rowsToInsert = rowsToInsert.OrderBy(r => {
                    string val = r.Cells[9].Value?.ToString(); // Явно завязали на индекс 9 для надежности
                    if (DateTime.TryParseExact(val, "dd.MM.yy H:mm",
                        System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsed))
                    {
                        return parsed;
                    }
                    return DateTime.MaxValue;
                }).ToList();

                dataGridViewPlan.Rows.AddRange(rowsToInsert.ToArray());
                ApplySmartTimelineMerging();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка генерации: {ex.Message}");
            }
            finally
            {
                dataGridViewPlan.ResumeLayout(true);
                this.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;
                dataGridViewPlan.Refresh();
            }
        }

        private void AddTimelineRow(List<DataGridViewRow> list, DateTime start, DateTime end, ProductionJobCard job, EquipmentStaffingPeriodRow shift, string shiftKey, ref bool isFirstRow)
        {
            string staffText = "—";
            if (shift != null)
            {
                string empName = string.IsNullOrEmpty(shift.StaffNames) ? "???" : shift.StaffNames;
                staffText = shift.IsCancelled == 1
                    ? $"{shift.Date:dd.MM.yy}\n{shift.ShiftName}\n{empName}\n(Отменена)"
                    : $"{shift.Date:dd.MM.yy}\n{shift.ShiftName}\n{empName}";
            }

            isFirstRow = false;

            string formattedOrderNum = "—";
            string customer = "—";
            string orderName = "—";
            string itemName = "—";
            string operProp = "—";
            string materialText = "—";
            string techNote = "—";
            string qty = "—";
            string timeStartText = $"{start:dd.MM.yy H:mm}";
            string timeEndText = $"{end:dd.MM.yy H:mm}";
            string durationText = "—";
            string toolText = "—";
            string cuttingText = "—";
            string orderKey = "GAP_" + start.ToString("yyyyMMddHHmm");
            bool isJobIdle = false;

            if (job != null)
            {
                isJobIdle = job.IsIdleTime;
                orderKey = isJobIdle ? $"IDLE_{job.IdPlanJob}" : $"JOB_{job.IdPlanJob}";

                string orderNum = isJobIdle ? "—" : job.OrderNum;
                customer = isJobIdle ? "—" : job.CustomerName;
                orderName = isJobIdle ? $"⚙ {job.IdleTimeName}" : job.OrderName;

                itemName = (isJobIdle || string.IsNullOrEmpty(job.ItemName)) ? "—" : job.ItemName;
                operProp = (isJobIdle || string.IsNullOrEmpty(job.OperProp)) ? "—" : job.OperProp;
                materialText = isJobIdle ? "—" : job.FullMaterialText;
                techNote = (isJobIdle || string.IsNullOrEmpty(job.NoteManagerTech)) ? "—" : job.NoteManagerTech;

                formattedOrderNum = FormatOrderNumberWithTechNotes(orderNum, itemName, techNote);

                if (!isJobIdle)
                {
                    if (job.RunQtyFact <= 0)
                    {
                        qty = $"{job.RunQty:N0}";
                    }
                    else
                    {
                        qty = $"{(job.RunQty - job.RunQtyFact):N0}/{job.RunQty:N0}";
                    }
                }

                // 🌟 ВОЗВРАЩАЕМ РОДНОЕ СТАРЕЙШЕЕ ОТОБРАЖЕНИЕ ВРЕМЕНИ ДЛЯ ЗАКАЗОВ ИЗ БАЗЫ
                timeStartText = $"{job.DateBegin:dd.MM.yy H:mm}";
                timeEndText = $"{job.DateEnd:dd.MM.yy H:mm}";
                durationText = job.NormTime;
                toolText = isJobIdle ? "—" : job.ShortToolName;
                cuttingText = isJobIdle ? "—" : (job.CuttingPart > 1 ? "Да" : "Нет");
            }
            else
            {
                // Сценарий: Пустая смена персонала без заказов
                formattedOrderNum = "❌ НЕТ ЗАДАНИЯ";
                orderName = "Заказы не распределены диспетчером";

                TimeSpan gapDuration = end - start;
                durationText = $"{(int)gapDuration.TotalHours:D2}:{gapDuration.Minutes:D2}";
            }

            var row = new DataGridViewRow();
            row.CreateCells(dataGridViewPlan);

            // Заполнение ячеек 0 - 13
            row.Cells[0].Value = staffText;
            row.Cells[1].Value = formattedOrderNum;
            row.Cells[2].Value = customer;
            row.Cells[3].Value = orderName;
            row.Cells[4].Value = itemName;
            row.Cells[5].Value = operProp;
            row.Cells[6].Value = materialText;
            row.Cells[7].Value = techNote;
            row.Cells[8].Value = qty;
            row.Cells[9].Value = timeStartText;
            row.Cells[10].Value = timeEndText;
            row.Cells[11].Value = durationText;
            row.Cells[12].Value = toolText;
            row.Cells[13].Value = cuttingText;

            row.Cells[0].Tag = shiftKey;

            if (job != null && isJobIdle)
            {
                string horizKey = $"HORIZ_{orderKey}";
                for (int c = 1; c <= 7; c++) row.Cells[c].Tag = horizKey;
                for (int c = 8; c <= 13; c++) row.Cells[c].Tag = horizKey;
            }
            else
            {
                for (int c = 1; c <= 8; c++) row.Cells[c].Tag = $"COL{c}_{orderKey}";
                row.Cells[12].Tag = $"COL12_{orderKey}";
                row.Cells[13].Tag = $"COL13_{orderKey}";
            }

            row.Cells[9].Tag = $"COL9_{orderKey}";
            row.Cells[10].Tag = $"COL10_{orderKey}";
            row.Cells[11].Tag = $"COL11_{orderKey}";

            if (job == null)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(253, 253, 253);
                row.DefaultCellStyle.ForeColor = Color.Gray;
            }
            else if (isJobIdle)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
                row.DefaultCellStyle.ForeColor = Color.DimGray;
            }

            int baseHeight = 65;
            int nameLength = orderName.Length;
            int estimatedTextLines = (int)Math.Ceiling((double)nameLength / 20);
            if (estimatedTextLines > 2)
            {
                int calculatedHeight = 30 + (estimatedTextLines * 15);
                if (calculatedHeight > baseHeight) baseHeight = calculatedHeight;
            }

            row.Height = baseHeight;
            list.Add(row);
        }



        private void ApplySmartTimelineMerging()
        {
            if (dataGridViewPlan.Rows.Count == 0 || dataGridViewPlan.Columns.Count < 14) return;

            Font boldFont = new Font(dataGridViewPlan.Font, FontStyle.Bold);
            Font regularFont = new Font(dataGridViewPlan.Font, FontStyle.Regular);
            Font fontBig = new Font(new Font("Arial", 14), FontStyle.Regular);

            // -----------------------------------------------------------------
            // ШАГ 1: ВЕРТИКАЛЬНОЕ И ГОРИЗОНТАЛЬНОЕ СЛИЯНИЕ ЗАКАЗОВ (КОЛОНКИ 1 - 13)
            // -----------------------------------------------------------------
            int rIndex = 0;
            while (rIndex < dataGridViewPlan.Rows.Count)
            {
                string cell1Tag = dataGridViewPlan.Rows[rIndex].Cells[1].Tag?.ToString();
                int maxRowsMergedInThisStep = 1;

                if (cell1Tag != null && cell1Tag.StartsWith("HORIZ_"))
                {
                    string idleNameText = dataGridViewPlan.Rows[rIndex].Cells[3].Value?.ToString() ?? "⚙️ Технологический простой";

                    int idleVerticalCount = 1;
                    while (rIndex + idleVerticalCount < dataGridViewPlan.Rows.Count &&
                           dataGridViewPlan.Rows[rIndex + idleVerticalCount].Cells[1].Tag?.ToString() == cell1Tag)
                    {
                        idleVerticalCount++;
                    }

                    // Пул слияния текстовой части
                    GridHelper.MergeCells(dataGridViewPlan, idleNameText, rIndex, 1, idleVerticalCount, 7,
                                         dataGridViewPlan.Rows[rIndex].DefaultCellStyle.BackColor, dataGridViewPlan.Rows[rIndex].DefaultCellStyle.ForeColor, fontBig, StringAlignment.Center, StringAlignment.Center);

                    int[] extraCols = { 8, 9, 10, 11, 12, 13 };
                    foreach (int col in extraCols)
                    {
                        string orderText = (col == 12 || col == 13) ? "—" : (dataGridViewPlan.Rows[rIndex].Cells[col].Value?.ToString() ?? "");
                        GridHelper.MergeCells(dataGridViewPlan, orderText, rIndex, col, idleVerticalCount, 1,
                                             dataGridViewPlan.Rows[rIndex].DefaultCellStyle.BackColor, dataGridViewPlan.Rows[rIndex].DefaultCellStyle.ForeColor, regularFont, StringAlignment.Center, StringAlignment.Center);
                    }

                    if (idleVerticalCount > maxRowsMergedInThisStep) maxRowsMergedInThisStep = idleVerticalCount;
                }
                else
                {
                    // Слияние текстовой части, выработки, штампа и подрезки (1-8, 12-13)
                    int[] orderTextCols = { 1, 2, 3, 4, 5, 6, 7, 8, 12, 13 };
                    foreach (int col in orderTextCols)
                    {
                        int startOrderRow = rIndex;
                        string orderTag = dataGridViewPlan.Rows[rIndex].Cells[col].Tag?.ToString();
                        string orderText = dataGridViewPlan.Rows[rIndex].Cells[col].Value?.ToString() ?? "";

                        int count = 1;
                        while (rIndex + count < dataGridViewPlan.Rows.Count && dataGridViewPlan.Rows[rIndex + count].Cells[col].Tag?.ToString() == orderTag)
                        {
                            count++;
                        }

                        GridHelper.MergeCells(dataGridViewPlan, orderText, startOrderRow, col, count, 1, Color.White, Color.Black, regularFont, StringAlignment.Center, StringAlignment.Center);
                        if (count > maxRowsMergedInThisStep) maxRowsMergedInThisStep = count;
                    }

                    // Слияние временной зоны дат (9 - 11)
                    for (int col = 9; col <= 11; col++)
                    {
                        int startOrderRow = rIndex;
                        string orderTag = dataGridViewPlan.Rows[rIndex].Cells[col].Tag?.ToString();
                        string orderText = dataGridViewPlan.Rows[rIndex].Cells[col].Value?.ToString() ?? "";

                        int count = 1;
                        while (rIndex + count < dataGridViewPlan.Rows.Count && dataGridViewPlan.Rows[rIndex + count].Cells[col].Tag?.ToString() == orderTag)
                        {
                            count++;
                        }

                        GridHelper.MergeCells(dataGridViewPlan, orderText, startOrderRow, col, count, 1, Color.White, Color.Black, regularFont, StringAlignment.Center, StringAlignment.Center);
                        if (count > maxRowsMergedInThisStep) maxRowsMergedInThisStep = count;
                    }
                }

                rIndex += maxRowsMergedInThisStep;
            }

            // -----------------------------------------------------------------
            // ШАГ 2: КОРРЕКТНОЕ СЛИЯНИЕ КОЛОНКИ СМЕН И ИСПОЛНИТЕЛЕЙ (СТОЛБЕЦ 0)
            // -----------------------------------------------------------------
            int startStaffRow = 0;
            while (startStaffRow < dataGridViewPlan.Rows.Count)
            {
                string currentStaffTag = dataGridViewPlan.Rows[startStaffRow].Cells[0].Tag?.ToString();

                if (string.IsNullOrEmpty(currentStaffTag))
                {
                    startStaffRow++;
                    continue;
                }

                int staffMergedCount = 1;
                string validStaffText = dataGridViewPlan.Rows[startStaffRow].Cells[0].Value?.ToString();

                while (startStaffRow + staffMergedCount < dataGridViewPlan.Rows.Count)
                {
                    string nextStaffTag = dataGridViewPlan.Rows[startStaffRow + staffMergedCount].Cells[0].Tag?.ToString();
                    string nextStaffText = dataGridViewPlan.Rows[startStaffRow + staffMergedCount].Cells[0].Value?.ToString();

                    // 🌟 ИСПРАВЛЕНО: Сливаем ячейку смены исключительно по совпадению тега смены currentStaffTag.
                    // Теперь колонка исполнителя станет единым красивым монолитным блоком на всю длину смены!
                    if (currentStaffTag == nextStaffTag)
                    {
                        if ((string.IsNullOrEmpty(validStaffText) || validStaffText == "—") && !string.IsNullOrEmpty(nextStaffText) && nextStaffText != "—")
                        {
                            validStaffText = nextStaffText;
                        }
                        staffMergedCount++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (string.IsNullOrEmpty(validStaffText) || validStaffText == "—")
                {
                    validStaffText = "Дежурная смена\nПерсонал не назначен";
                }

                GridHelper.MergeCells(dataGridViewPlan, validStaffText, startStaffRow, 0, staffMergedCount, 1, Color.White, Color.Black, boldFont, StringAlignment.Center, StringAlignment.Center);
                startStaffRow += staffMergedCount;
            }
        }



        private string FormatOrderNumberWithTechNotes(string originalOrderNum, string elementText, string techNotesText)
        {
            if (string.IsNullOrWhiteSpace(originalOrderNum))
                return originalOrderNum;

            string suffix = "";
            string elementLower = elementText?.ToLower() ?? "";
            string notesLower = techNotesText?.ToLower() ?? "";

            // 1. Проверка поля "Элемент" (пакет или коробка)
            if (elementLower.Contains("пакет"))
            {
                suffix += "\nпакет";
            }
            else if (elementLower.Contains("коробка") || elementLower.Contains("коробочн"))
            {
                suffix += "\nкоробка";
            }

            // 2. Универсальный поиск прогонов через регулярное выражение
            // Шаблон ищет: одну или несколько цифр (\d+), за которыми могут идти дефисы, буквы (й, ой, ый) и пробелы,
            // а затем обязательно идет корень "прогон"
            var runMatch = Regex.Match(notesLower, @"(\d+)(?:\s*[-—–]?\s*[а-яисмтекхў]*\s*)?прогон");

            if (runMatch.Success)
            {
                // Вытаскиваем только чистую цифру из первой группы захвата
                string runNumber = runMatch.Groups[1].Value;
                suffix += $"\n{runNumber} прогон";
            }
            // 3. Если прогона нет, но есть отметка о присутствии технолога
            else if (notesLower.Contains("с технологом"))
            {
                suffix += "\nтехнолог";
            }

            return $"{originalOrderNum.Trim()}{suffix}";
        }

        private string PrepareCellText(ShiftInfo s)
        {
            var sb = new StringBuilder();

            // 1 строка: План (с причинами отсутствия, если есть в данных)
            if (s.PlannedStaff.Any())
                sb.AppendLine(string.Join("\n", s.PlannedStaff));

            // 2 строка: На согласовании (Черновики)
            if (s.Drafts.Any())
                sb.AppendLine(/*"📝 " + */string.Join("\n", s.Drafts));

            // 3 строка: Утвержденные (Факт)
            if (s.Assignments.Any())
            {
                if (s.PlannedStaff.Any())
                {
                    sb.Append(/*"✅ " + */string.Join("\n", s.Assignments));
                }
                else
                {
                    sb.AppendLine(/*"✅ " + */string.Join("\n", s.Assignments));
                }
            }

            return sb.ToString();
        }


        private async Task StartLoadingStaffPlanningAsync()
        {
            cancelTokenSource?.Cancel();

            cancelTokenSource = new CancellationTokenSource();

            if (planComboBoxYear.SelectedIndex < 0 || planComboBoxMonth.SelectedIndex < 0 || planComboBoxAreas.SelectedIndex < 0) return;

            int year = Convert.ToInt32(planComboBoxYear.Text);
            int month = planComboBoxMonth.SelectedIndex + 1;
            int idArea = workAreaInfo[planComboBoxAreas.SelectedIndex].Id;

            DateTime startDate = new DateTime(year, month, 1);

            //Task task = new Task(() => LoadStaffPlanningAsync(cancelTokenSource.Token, startDate, endDate, idArea), cancelTokenSource.Token);
            //task.Start();

            //await LoadStaffPlanningAsync(cancelTokenSource.Token, startDate, endDate, idArea);
            await BuildPivotGrid(startDate, idArea);
        }

        public async Task BuildPivotGrid(DateTime monthStart, int areaId)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Console.WriteLine("=== СТАРТ ДИАГНОСТИКИ ОБНОВЛЕНИЯ СЕТКИ ===");

            var service = new ReportStaffing(parameter.GetMySQLConnectionString());

            DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);
            
            long stepStart = sw.ElapsedMilliseconds;

            var reportData = await service.GetStaffingReportAsync(monthStart, monthEnd, areaId);

            long mysqlTime = sw.ElapsedMilliseconds - stepStart;
            Console.WriteLine($"Шаг 1.1: Запрос кадрового отчета (MySQL) занял: {mysqlTime} мс");


            // 1. ПЕРЕД СТАРТОМ foreach-циклов СКАЧИВАЕМ ЗАКАЗЫ ИЗ ВНЕШНЕЙ БАЗЫ С ТАЙМАУТОМ:
            var servicePlanning = new ValueOrders();

            string extConnStr = parameter.GetSQLServerConnectionString();

            stepStart = sw.ElapsedMilliseconds;
            List<ProductionJobCard> productionJobsList = await servicePlanning.GetProductionJobsForAllEquipsAsync(monthStart, monthEnd, extConnStr);
            long mssqlTime = sw.ElapsedMilliseconds - stepStart;
            Console.WriteLine($"Шаг 1.2: Запрос планов заказов (MS SQL) занял: {mssqlTime} мс");

            stepStart = sw.ElapsedMilliseconds;
            // Группируем заказы по оборудованию в Lookup для моментального поиска в памяти
            ILookup<int, ProductionJobCard> ordersLookup = productionJobsList != null
                ? productionJobsList.ToLookup(j => j.IdEquip)
                : null;
            Console.WriteLine($"Шаг 1.3: Группировка заказов в Lookup в памяти заняла: {sw.ElapsedMilliseconds - stepStart} мс");

            stepStart = sw.ElapsedMilliseconds;

            dataGridPlanning.SuspendLayout();

            dataGridPlanning.Columns.Clear();
            dataGridPlanning.Rows.Clear();

            Console.WriteLine($"Шаг 2: Очистка таблицы и SuspendLayout заняли: {sw.ElapsedMilliseconds - stepStart} мс");

            // Переменные для счетчиков тяжелых операций
            long totalCellAddAndTextTime = 0;
            long totalMergeCellsTime = 0;
            int totalCellsProcessed = 0;

            // 1. Создаем фиксированные столбцы
            dataGridPlanning.Columns.Add("Equip", "Оборудование");
            dataGridPlanning.Columns.Add("Shift", "Смена");

            dataGridPlanning.Columns["Shift"].Frozen = true;

            dataGridPlanning.Columns["Equip"].Width = 180;
            dataGridPlanning.Columns["Shift"].Width = 20;

            int columnFirst = dataGridPlanning.Rows.Add();
            int columnSecond = dataGridPlanning.Rows.Add();

            dataGridPlanning.Rows[columnSecond].Frozen = true;

            dataGridPlanning.Rows[columnFirst].MinimumHeight = 40;
            dataGridPlanning.Rows[columnSecond].MinimumHeight = 40;

            dataGridPlanning.Rows[columnFirst].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridPlanning.Rows[columnSecond].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            GridHelper.MergeCells(dataGridPlanning, "Рабочее место", 0, 0, 2, 1, Color.Gray, Color.White, dataGridPlanning.Font, StringAlignment.Center, StringAlignment.Center);
            GridHelper.MergeCells(dataGridPlanning, "Смена", 0, 1, 2, 1, Color.Gray, Color.White, dataGridPlanning.Font, StringAlignment.Center, StringAlignment.Center);

            dataGridPlanning.Rows[columnFirst].Cells[0].Value = "Рабочее место";
            dataGridPlanning.Rows[columnSecond].Cells[1].Value = "Смена";

            stepStart = sw.ElapsedMilliseconds;
            // 2. Создаем динамические столбцы для дней месяца (1, 2, 3...)
            for (int d = 1; d <= monthEnd.Day; d++)
            {
                DateTime headerDate = new DateTime(monthStart.Year, monthStart.Month, d);
                string headerText = headerDate.ToString("dd.MM.yyyy");

                var col = new DataGridViewTextBoxColumn();
                col.Name = $"day_{d}";
                col.HeaderText = headerText;
                col.Width = 300;
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True; // Разрешаем перенос строк
                dataGridPlanning.Columns.Add(col);

                int curCol = dataGridPlanning.Rows[columnFirst].Cells[$"day_{d}"].ColumnIndex;

                string fullDayName = headerDate.ToString("dddd", new CultureInfo("ru-RU"));
                string capitalizedDay = char.ToUpper(fullDayName[0]) + fullDayName.Substring(1);

                int russianIndex = ((int)headerDate.DayOfWeek == 0) ? 7 : (int)headerDate.DayOfWeek;

                Color color = Color.White;

                if (russianIndex >= 6)
                {
                    color = Color.DarkRed;
                }

                dataGridPlanning.Rows[columnFirst].Cells[curCol].Value = headerText;
                dataGridPlanning.Rows[columnSecond].Cells[curCol].Value = capitalizedDay;

                dataGridPlanning.Rows[columnFirst].Cells[curCol].Style.BackColor = Color.Gray;
                dataGridPlanning.Rows[columnFirst].Cells[curCol].Style.ForeColor = color;
                dataGridPlanning.Rows[columnFirst].Cells[curCol].Style.Font = new Font("Arial", 10, FontStyle.Bold);

                dataGridPlanning.Rows[columnSecond].Cells[curCol].Style.BackColor = Color.Gray;
                dataGridPlanning.Rows[columnSecond].Cells[curCol].Style.ForeColor = color;
                dataGridPlanning.Rows[columnSecond].Cells[curCol].Style.Font = new Font("Arial", 10, FontStyle.Bold);
            }

            Console.WriteLine($"Шаг 2.1: Добавление столбцов заняло: {sw.ElapsedMilliseconds - stepStart} мс");

            stepStart = sw.ElapsedMilliseconds;
            // 3. Заполняем строки (по 2 на каждую смену)
            var allEquip = reportData.SelectMany(d => d.Equipments)
                                     .GroupBy(e => new { e.Id, e.Name, e.Code })
                                     .ToList();

            Console.WriteLine($"Шаг 2.2: Группировка оборудования: {sw.ElapsedMilliseconds - stepStart} мс");

            stepStart = sw.ElapsedMilliseconds;

            // ===================================================================
            // 🌟 ОПТИМИЗАЦИЯ 1: ЖЕСТКО ВЫКЛЮЧАЕМ АВТОМАТИЧЕСКИЙ РАСЧЕТ РАЗМЕРОВ 🌟
            // ===================================================================
            dataGridPlanning.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridPlanning.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridPlanning.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            // Подготавливаем дефолтный стиль для строки заказов r2 один раз, чтобы не создавать его в цикле
            var r2Style = new DataGridViewCellStyle();
            r2Style.BackColor = Color.FromArgb(245, 245, 245);
            r2Style.Font = new Font(dataGridPlanning.Font.FontFamily, 8f, FontStyle.Italic);
            r2Style.ForeColor = Color.DarkSlateGray;

            // Списки для накопления строк перед добавлением (работает в десятки раз быстрее индивидуальных Rows.Add)
            var rowsToInsert = new List<DataGridViewRow>();

            // Структура для постобработки ячеек и мержинга, чтобы не дергать контрол во время создания строк
            var cellProcessingQueue = new List<Action>();

            foreach (var eq in allEquip)
            {
                // 🌟 ИСПРАВЛЕНО: Выбираем уникальные пары Id и Number для смен, чтобы разные графики не пересекались
                var shiftPairs = eq.SelectMany(s => s.Shifts)
                                   .Select(s => new { s.Id, s.Number })
                                   .Distinct()
                                   .ToList();

                int activeShiftsCount = shiftPairs.Count(x => x.Number != 0);
                int currentShiftNumber = 1;

                foreach (var shift in shiftPairs)
                {
                    if (shift.Number != 0)
                    {
                        // Создаем полноценные объекты строк WinForms в памяти БЕЗ обращения к контролу
                        var row1 = new DataGridViewRow();
                        row1.HeaderCell.Value = eq.Key.Id;

                        var row2 = new DataGridViewRow();
                        row2.HeaderCell.Value = eq.Key.Id;
                        row2.MinimumHeight = 40;

                        // Добавляем пустые ячейки согласно количеству колонок в таблице
                        row1.CreateCells(dataGridPlanning);
                        row2.CreateCells(dataGridPlanning);

                        // Заполняем базовые данные смены
                        row1.Cells[1].Value = shift.Number; // Колонка "Shift"
                        row2.Cells[1].Value = shift.Number; // Колонка "Shift"

                        row2.DefaultCellStyle = r2Style; // Присваиваем готовый Shared-стиль строке заказов

                        // Запоминаем текущие локальные переменные для лямбды
                        int currentShiftNumLocal = currentShiftNumber;
                        int sNumberLocal = shift.Number;
                        ulong sIdLocal = shift.Id; // 🌟 Локальный уникальный ID смены для замыкания
                        var currentEqKey = eq.Key;

                        // Помещаем логику заполнения данных по дням в очередь отложенной обработки
                        cellProcessingQueue.Add(() =>
                        {
                            int r1Index = row1.Index;
                            int r2Index = row2.Index;

                            if (activeShiftsCount == currentShiftNumLocal)
                            {
                                // Слияние ячеек по имени оборудования
                                GridHelper.MergeCells(dataGridPlanning, currentEqKey.Name, r1Index - (activeShiftsCount - 1) * 2, 0, activeShiftsCount * 2, 1, Color.Gray, Color.White, dataGridPlanning.Font, StringAlignment.Center, StringAlignment.Center);
                                row2.Tag = "LastEquipRow"; // Маркируем нижнюю строку станка для оранжевой линии
                            }

                            GridHelper.MergeCells(dataGridPlanning, sNumberLocal.ToString(), r1Index, 1, 2, 1, Color.Gray, Color.White, dataGridPlanning.Font, StringAlignment.Center, StringAlignment.Center);

                            // Цикл по дням месяца
                            foreach (var dayData in reportData)
                            {
                                totalCellsProcessed++;
                                int dayNum = dayData.Date.Day;
                                string colName = $"day_{dayNum}";

                                // 🌟 СИНХРОНИЗАЦИЯ: Ищем смену строго по уникальному Id (shift.Id) вместо номера
                                var currentShiftData = dayData.Equipments
                                    .FirstOrDefault(e => e.Id == currentEqKey.Id)?
                                    .Shifts.FirstOrDefault(s => s.Id == sIdLocal);

                                if (currentShiftData != null)
                                {
                                    // СТРОКА 1: Сотрудники (ФИО и цифровая раскраска)
                                    var cell1 = dataGridPlanning.Rows[r1Index].Cells[colName];
                                    cell1.Value = PrepareCellText(currentShiftData);

                                    // 🌟 СОХРАНЕНИЕ ИНДЕКСОВ: Записываем связку [ID станка, ID смены] в Tag ячейки сотрудников
                                    cell1.Tag = Tuple.Create((ulong)currentEqKey.Id, (ulong)sIdLocal);

                                    // Применение готового стиля из кэша формы
                                    if (_sharedStyles.TryGetValue(currentShiftData.StatusCode, out DataGridViewCellStyle targetStyle))
                                    {
                                        cell1.Style = targetStyle;
                                    }

                                    // СТРОКА 2: Расчет наложения временных интервалов заказов MES
                                    var orderCell = dataGridPlanning.Rows[r2Index].Cells[colName];

                                    if (productionJobsList == null)
                                    {
                                        orderCell.Value = "";
                                        orderCell.Style.ForeColor = Color.IndianRed;
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(currentEqKey.Code) && int.TryParse(currentEqKey.Code, out int externalEquipId))
                                        {
                                            DateTime shiftStartDT = dayData.Date.Date.Add(currentShiftData.TimeStart);
                                            DateTime shiftEndDT = currentShiftData.TimeEnd < currentShiftData.TimeStart
                                                ? dayData.Date.Date.AddDays(1).Add(currentShiftData.TimeEnd)
                                                : dayData.Date.Date.Add(currentShiftData.TimeEnd);

                                            var equipJobs = ordersLookup[externalEquipId];
                                            var activeJobsInShift = equipJobs
                                                .Where(j => j.DateBegin < shiftEndDT && j.DateEnd > shiftStartDT)
                                                .Select(j => new ShortOrderView(j).DisplayText)
                                                .ToList();

                                            if (activeJobsInShift.Count > 0)
                                                orderCell.Value = string.Join("\n", activeJobsInShift);
                                            else
                                                orderCell.Value = "—";
                                        }
                                        else
                                        {
                                            orderCell.Value = "—";
                                        }
                                    }
                                }
                            }
                        });

                        // Добавляем подготовленные строки в промежуточный список
                        rowsToInsert.Add(row1);
                        rowsToInsert.Add(row2);

                        currentShiftNumber++;
                    }
                }
            }


            // 🌟 ОПТИМИЗАЦИЯ 2: МАССИВНЫЙ ОДНОКРАТНЫЙ ВПРЫСК СТРОК В ТАБЛИЦУ (Rows.AddRange) 🌟
            // Это ускоряет работу в потоке UI примерно в 5 раз!
            dataGridPlanning.Rows.AddRange(rowsToInsert.ToArray());

            // Выполняем заполнение текстов, раскраску и MergeCells для уже добавленных строк
            long mergeStart = sw.ElapsedMilliseconds;
            foreach (var processAction in cellProcessingQueue)
            {
                processAction();
            }
            totalMergeCellsTime = sw.ElapsedMilliseconds - mergeStart;

            long loopTime = sw.ElapsedMilliseconds - stepStart;
            Console.WriteLine($"Шаг 3.1: Всего ячеек обработано: {totalCellsProcessed} шт.");
            Console.WriteLine($"Шаг 3.2: Суммарное время прохода всех ячеек, ФИО и Заказов: {loopTime - totalMergeCellsTime} мс");
            Console.WriteLine($"Шаг 3.3: Суммарное время работы GridHelper.MergeCells: {totalMergeCellsTime} мс");
            Console.WriteLine($"Шаг 3.4: Полный проход циклов занял: {loopTime} мс");

            // 🌟 ОПТИМИЗАЦИЯ 3: ВОЗВРАЩАЕМ ОТРИСОВКУ ЕДИНЫМ КАДРОМ 🌟
            dataGridPlanning.ResumeLayout(true);

            long resumeTime = sw.ElapsedMilliseconds - stepStart - loopTime;
            Console.WriteLine($"Шаг 4: Метод ResumeLayout(true) (Рендеринг Windows) занял: {resumeTime} мс");

            sw.Stop();
            Console.WriteLine($"=== ИТОГО: Полное время выполнения метода: {sw.ElapsedMilliseconds:F1} мс ===");

            // Автоматическая высота строк под контент
            //dataGridPlanning.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dataGridPlanning.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridPlanning.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridPlanning.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            if (_editShift)
            {
                RestoreLastSelectedCell();

                _editShift = false;
            }
            else
            {
                ScrollToCurrentDay();
            }
        }


        





        public async Task BuildPivotGrid1111111(DateTime monthStart, int areaId)
        {
            //ConnectionParameter parameter = new ConnectionParameter();

            var service = new ReportStaffing(parameter.GetMySQLConnectionString());

            DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);

            var reportData = await service.GetStaffingReportAsync(monthStart, monthEnd, areaId);

            dataGridPlanning.SuspendLayout();

            dataGridPlanning.Columns.Clear();
            dataGridPlanning.Rows.Clear();

            // 1. Создаем фиксированные столбцы
            dataGridPlanning.Columns.Add("Equip", "Оборудование");
            dataGridPlanning.Columns.Add("Shift", "Смена");

            dataGridPlanning.Columns["Shift"].Frozen = true;

            dataGridPlanning.Columns["Equip"].Width = 180;
            dataGridPlanning.Columns["Shift"].Width = 20;

            //dataGridPlanning.Columns["Equip"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            //dataGridPlanning.Columns["Shift"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

            int columnFirst = dataGridPlanning.Rows.Add();
            int columnSecond = dataGridPlanning.Rows.Add();

            dataGridPlanning.Rows[columnSecond].Frozen = true;

            dataGridPlanning.Rows[columnFirst].MinimumHeight = 40;
            dataGridPlanning.Rows[columnSecond].MinimumHeight = 40;

            dataGridPlanning.Rows[columnFirst].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridPlanning.Rows[columnSecond].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            GridHelper.MergeCells(dataGridPlanning, "Рабочее место", 0, 0, 2, 1, Color.Gray, Color.White, dataGridPlanning.Font, StringAlignment.Center, StringAlignment.Center);
            GridHelper.MergeCells(dataGridPlanning, "Смена", 0, 1, 2, 1, Color.Gray, Color.White, dataGridPlanning.Font, StringAlignment.Center, StringAlignment.Center);

            dataGridPlanning.Rows[columnFirst].Cells[0].Value = "Рабочее место";
            dataGridPlanning.Rows[columnSecond].Cells[1].Value = "Смена";

            // 2. Создаем динамические столбцы для дней месяца (1, 2, 3...)
            for (int d = 1; d <= monthEnd.Day; d++)
            {
                DateTime headerDate = new DateTime(monthStart.Year, monthStart.Month, d);
                string headerText = headerDate.ToString("dd.MM.yyyy");

                var col = new DataGridViewTextBoxColumn();
                col.Name = $"day_{d}";
                col.HeaderText = headerText;
                col.Width = 300;
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True; // Разрешаем перенос строк
                dataGridPlanning.Columns.Add(col);

                int curCol = dataGridPlanning.Rows[columnFirst].Cells[$"day_{d}"].ColumnIndex;

                //GridHelper.MergeCells(dataGridPlanning, "Смена", 0, curCol, 1, 1, Color.Gray);
                //GridHelper.MergeCells(dataGridPlanning, "Смена", 1, curCol, 1, 1, Color.Gray);

                string fullDayName = headerDate.ToString("dddd", new CultureInfo("ru-RU"));
                string capitalizedDay = char.ToUpper(fullDayName[0]) + fullDayName.Substring(1);

                int russianIndex = ((int)headerDate.DayOfWeek == 0) ? 7 : (int)headerDate.DayOfWeek;

                Color color = Color.White;

                if (russianIndex >= 6)
                {
                    color = Color.DarkRed;
                }

                dataGridPlanning.Rows[columnFirst].Cells[curCol].Value = headerText;
                dataGridPlanning.Rows[columnSecond].Cells[curCol].Value = capitalizedDay;

                dataGridPlanning.Rows[columnFirst].Cells[curCol].Style.BackColor = Color.Gray;
                dataGridPlanning.Rows[columnFirst].Cells[curCol].Style.ForeColor = color;
                dataGridPlanning.Rows[columnFirst].Cells[curCol].Style.Font = new Font("Arial", 10, FontStyle.Bold);

                dataGridPlanning.Rows[columnSecond].Cells[curCol].Style.BackColor = Color.Gray;
                dataGridPlanning.Rows[columnSecond].Cells[curCol].Style.ForeColor = color;
                dataGridPlanning.Rows[columnSecond].Cells[curCol].Style.Font = new Font("Arial", 10, FontStyle.Bold);
            }

            // 3. Заполняем строки (по 2 на каждую смену)
            // Группируем всё оборудование, чтобы знать список смен
            var allEquip = reportData.SelectMany(d => d.Equipments)
                                     .GroupBy(e => new { e.Id, e.Name })
                                     .ToList();

            foreach (var eq in allEquip)
            {
                // Для каждой смены оборудования создаем 2 строки
                var shiftNames = eq.SelectMany(s => s.Shifts).Select(s => s.Number).Distinct().ToList();
                int activeShiftsCount = shiftNames.Count(x => x != 0);
                int currentShiftNumber = 1;

                int firstRow = 0;

                foreach (var sName in shiftNames)
                {
                    if (sName != 0)
                    {
                        // Строка 1: Инфо о сотрудниках
                        int r1 = dataGridPlanning.Rows.Add(eq.Key.Id);
                        dataGridPlanning.Rows[r1].HeaderCell.Value = eq.Key.Id;
                        //dataGridPlanning.Rows[r1].HeaderCell.Tag = eq.SelectMany(s => s.Shifts).Select(s => s.Number);
                        
                        // Строка 2: Пустая (для будущего)
                        int r2 = dataGridPlanning.Rows.Add(eq.Key.Id);
                        dataGridPlanning.Rows[r2].HeaderCell.Value = eq.Key.Id;

                        dataGridPlanning.Rows[r2].MinimumHeight = 2;

                        if (currentShiftNumber == 1)
                        {
                            firstRow = r1;
                        }

                        if (activeShiftsCount == currentShiftNumber)
                        {
                            GridHelper.MergeCells(dataGridPlanning, eq.Key.Name, firstRow, 0, shiftNames.Count(x => x != 0) * 2, 1, Color.Gray, Color.White, dataGridPlanning.Font, StringAlignment.Center, StringAlignment.Center);

                            dataGridPlanning.Rows[r2].Tag = "LastEquipRow";
                        }

                        GridHelper.MergeCells(dataGridPlanning, sName.ToString(), r1, 1, 2, 1, Color.Gray, Color.White, dataGridPlanning.Font, StringAlignment.Center, StringAlignment.Center);

                        //dataGridPlanning.Rows[r1].Cells["Equip"].Value = eq.Key.Name;
                        dataGridPlanning.Rows[r1].Cells["Shift"].Value = sName;
                        //dataGridPlanning.Rows[r1].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);

                        //dataGridPlanning.Rows[r2].Cells["Equip"].Value = eq.Key.Name;
                        dataGridPlanning.Rows[r2].Cells["Shift"].Value = sName;
                        dataGridPlanning.Rows[r2].DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

                        // Заполняем данные по дням для этой смены
                        foreach (var dayData in reportData)
                        {
                            int dayNum = dayData.Date.Day;
                            var currentEq = dayData.Equipments.FirstOrDefault(e => e.Id == eq.Key.Id);
                            var currentShift = currentEq?.Shifts.FirstOrDefault(s => s.Number == sName);

                            if (currentShift != null)
                            {
                                var cell = dataGridPlanning.Rows[r1].Cells[$"day_{dayNum}"];

                                cell.Value = PrepareCellText(currentShift);

                                int statusCode = currentShift.StatusCode;

                                if (_cellStyles.TryGetValue(statusCode, out DataGridViewCellStyle targetStyle))
                                {
                                    cell.Style = targetStyle;
                                }
                            }
                        }

                        currentShiftNumber++;
                    }
                }
            }

            dataGridPlanning.ResumeLayout(true);

            //dataGridPlanning.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            if (_editShift)
            {
                RestoreLastSelectedCell();

                _editShift = false;
            }
            else
            {
                ScrollToCurrentDay();
            }
        }

        private async Task UpdateShiftsDefinitionAsync()
        {
            ConnectionParameter parameter = new ConnectionParameter();

            shiftDefinitions?.Clear();

            ShiftService shiftsDefinitionService = new ShiftService(parameter.GetMySQLConnectionString());

            shiftDefinitions = await shiftsDefinitionService.GetAllShiftsAsync();

            await AddShiftsDefinitionToListView();

            listViewShiftsDef.SelectedItems.Clear();
            listViewShiftsDef.FocusedItem = null;
        }

        private async Task AddShiftsDefinitionToListView()
        {
            if (shiftDefinitions == null) return;

            listViewShiftsDef.Items.Clear();

            int index = 1;

            foreach (var shift in shiftDefinitions)
            {
                ListViewItem lvItem = new ListViewItem();

                string startTime = (shift.ShiftNumber == 0) ? "" : shift.StartTime.ToString(@"hh\:mm");
                string endTime = (shift.ShiftNumber == 0) ? "" : shift.EndTime.ToString(@"hh\:mm");

                lvItem.Text = index.ToString();
                lvItem.SubItems.Add(shift.ShiftNumber.ToString());
                lvItem.SubItems.Add(shift.Name);
                lvItem.SubItems.Add(startTime);
                lvItem.SubItems.Add(endTime);

                listViewShiftsDef.Items.Add(lvItem);

                index++;
            }
            
        }

        private async Task UpdateScheduleCycle()
        {
            ConnectionParameter parameter = new ConnectionParameter();

            scheduleCycles?.Clear();

            ScheduleTemplateService scheduleTemplate = new ScheduleTemplateService(parameter.GetMySQLConnectionString());

            scheduleCycles = await scheduleTemplate.GetAllCyclesAsync();

            await AddScheduleCycleToListView();
        }

        private async Task AddScheduleCycleToListView()
        {
            if (scheduleCycles == null) return;

            listViewShiftCycle.Items.Clear();

            int index = 1;

            foreach (var cycle in scheduleCycles)
            {
                ListViewItem lvItem = new ListViewItem();

                string template = string.Empty;

                foreach (var item in cycle.Items)
                {
                    template += item.DayNumber + ": " + item.ShiftName.Substring(0, 1) + "; ";
                }

                lvItem.Text = index.ToString();
                lvItem.SubItems.Add(cycle.Name);
                lvItem.SubItems.Add(cycle.CycleLength.ToString());
                lvItem.SubItems.Add(template);

                listViewShiftCycle.Items.Add(lvItem);

                index++;
            }

        }

        private async Task UpdateScheduleTemplate()
        {
            ConnectionParameter parameter = new ConnectionParameter();

            scheduleTemplates?.Clear();

            ScheduleTemplateService scheduleTemplate = new ScheduleTemplateService(parameter.GetMySQLConnectionString());

            scheduleTemplates = await scheduleTemplate.GetAllTemplatesAsync();

            await AddScheduleTemplateToListView();
        }

        private async Task AddScheduleTemplateToListView()
        {
            if (scheduleTemplates == null) return;

            listViewShiftTemplate.Items.Clear();

            int index = 1;

            foreach (var template in scheduleTemplates)
            {
                ListViewItem lvItem = new ListViewItem();

                lvItem.Text = index.ToString();
                lvItem.SubItems.Add(template.Name);
                lvItem.SubItems.Add(template.CycleName);
                lvItem.SubItems.Add(template.BaseDate.ToString("dd.MM.yyyy"));

                listViewShiftTemplate.Items.Add(lvItem);

                index++;
            }

        }

        private async void materialTabControl1_SelectedIndexChangedAsync(object sender, EventArgs e)
        {
            int index = materialTabControl1.SelectedIndex;
            string namePage = materialTabControl1.TabPages[index].Text;

            this.Text = "Производственный план - " + namePage/* + " (" + index + ")"*/;

            await UpdateWorkAreasAsync();

            if (index == 0)
            {
                dataGridPlanning.Rows.Clear();
                dataGridPlanning.Columns.Clear();
                
                planComboBoxAreas.SelectedIndexChanged -= planComboBoxAreas_SelectedIndexChanged;

                planComboBoxAreas.DataSource = null;
                planComboBoxAreas.Items.Clear();

                planComboBoxAreas.DataSource = workAreaInfo;
                planComboBoxAreas.DisplayMember = "Name";
                planComboBoxAreas.ValueMember = "Id";

                planComboBoxAreas.SelectedIndex = -1;

                planComboBoxAreas.SelectedIndexChanged += planComboBoxAreas_SelectedIndexChanged;

                /*for (int i = 0; i < workAreaInfo.Count; i++)
                {
                    planComboBoxAreas.Items.Add(workAreaInfo[i].Name);
                }*/
            }

            if (index == 1)
            {
                dataGridViewPlan.Rows.Clear();
                dataGridViewPlan.Columns.Clear();

                workPlanEquipComboBox.SelectedIndexChanged -= workPlanEquipComboBox_SelectedIndexChanged;
                //workPlanAreaComboBox.Items.Clear();
                workPlanEquipComboBox.DataSource = null;
                workPlanEquipComboBox.Items.Clear();

                workPlanAreaComboBox.DataSource = null;
                workPlanAreaComboBox.Items.Clear();

                workPlanAreaComboBox.DataSource = workAreaInfo;
                workPlanAreaComboBox.DisplayMember = "Name";
                workPlanAreaComboBox.ValueMember = "Id";

                workPlanAreaComboBox.SelectedIndex = -1;

                workPlanEquipComboBox.SelectedIndexChanged += workPlanEquipComboBox_SelectedIndexChanged;

                /*for (int i = 0; i < workAreaInfo.Count; i++)
                {
                    workPlanAreaComboBox.Items.Add(workAreaInfo[i].Name);
                }*/
            }

            if (index == 3)
            {
                await UpdateUsersShortInfo();
                await LoadPositions();
                await LoadUserListToListBox();
            }

            if (index == 4)
            {
                await LoadAreaListToListBox();
            }
        }

        

        private async void materialButton2_ClickAsync(object sender, EventArgs e)
        {
            await StartLoadingStaffPlanningAsync();
        }

        private void dataGridPlanning_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Игнорируем заголовки строк и столбцов
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // Проверяем, выделена ли ячейка в данный момент
            if (e.State.HasFlag(DataGridViewElementStates.Selected))
            {
                // 1. Получаем текущий цвет ячейки (или цвет по умолчанию для этого столбца/таблицы)
                Color cellColor = e.CellStyle.BackColor;
                if (cellColor.IsEmpty || cellColor == Color.Empty)
                {
                    cellColor = dataGridPlanning.DefaultCellStyle.BackColor;
                }

                // 2. Создаем временную кисть с оригинальным цветом ячейки для закрашивания фона
                using (SolidBrush bgBrush = new SolidBrush(cellColor))
                {
                    // Закрашиваем фон, чтобы скрыть стандартное синее выделение
                    e.Graphics.FillRectangle(bgBrush, e.CellBounds);
                }

                // 3. Отрисовываем только содержимое (текст, иконки), исключая стандартный фон и рамку
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground | DataGridViewPaintParts.Focus);

                // 4. Рисуем кастомный контур выделения
                using (Pen strokePen = new Pen(Color.DarkOrange, 2))
                {
                    Rectangle rect = e.CellBounds;
                    rect.Width -= 1;
                    rect.Height -= 1;

                    e.Graphics.DrawRectangle(strokePen, rect);
                }

                // Сообщаем системе, что ячейка отрисована полностью
                e.Handled = true;
            }
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            string v = dataGridPlanning.Rows[2].Cells[0].Value.ToString();

            MessageBox.Show(v);
        }

        private void dataGridPlanning_SelectionChanged(object sender, EventArgs e)
        {
            // Проверяем каждую ячейку, которую пытается выделить пользователь
            foreach (DataGridViewCell cell in dataGridPlanning.SelectedCells)
            {
                // Условие: индекс строки меньше 2 ИЛИ индекс столбца меньше 2
                if (cell.RowIndex < 2 || cell.ColumnIndex < 2)
                {
                    // Мгновенно снимаем выделение
                    cell.Selected = false;
                }
            }
        }

        private void dataGridPlanning_Scroll(object sender, ScrollEventArgs e)
        {
            // Полностью обновляем сетку при скролле, чтобы заставить ячейки
            // динамически пересчитывать свои новые "первые видимые" координаты.
            dataGridPlanning.Invalidate();
        }

        private void workPlanAreaComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            workPlanEquipComboBox.SelectedIndexChanged -= workPlanEquipComboBox_SelectedIndexChanged;

            workPlanEquipComboBox.DataSource = null;
            workPlanEquipComboBox.Items.Clear();

            if (workPlanAreaComboBox.SelectedItem is WorkAreaInfo workArea)
            {
                List<EquipmentShortInfo> equipments = workArea.Equipments.Where(equipment => equipment.Code != "" && (equipment.DecommissionedAt == null || equipment.DecommissionedAt > DateTime.Now)).ToList();

                workPlanEquipComboBox.DataSource = equipments;
                workPlanEquipComboBox.DisplayMember = "Name";
                workPlanEquipComboBox.ValueMember = "Id";

                workPlanEquipComboBox.SelectedIndex = -1;

                workPlanEquipComboBox.SelectedIndexChanged += workPlanEquipComboBox_SelectedIndexChanged;
            }

            /*int areaID = workPlanAreaComboBox.SelectedIndex;

            for (int i = 0; i < workAreaInfo[areaID].Equipments.Count; i++)
            {
                workPlanEquipComboBox.Items.Add(workAreaInfo[areaID].Equipments[i].Name);
            }*/
        }

        private async void workPlanEquipComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            await ViewPlanForEquipAsync();
        }

        private async void planComboBoxAreas_SelectedIndexChanged(object sender, EventArgs e)
        {
            await StartLoadingStaffPlanningAsync();
        }

        private async void planComboBoxMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            await StartLoadingStaffPlanningAsync();
        }

        private async void planComboBoxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            await StartLoadingStaffPlanningAsync();
        }

        private void ScrollToCurrentDay()
        {
            // 1. Проверяем, что выбранный в фильтре месяц и год совпадают с текущей датой [2026-09-12]
            // Замените cmbMonth и cmbYear на ваши элементы управления (или переменные) фильтра дат
            int selectedMonth = planComboBoxMonth.SelectedIndex + 1;
            int selectedYear = Convert.ToInt32(planComboBoxYear.Text);

            DateTime now = DateTime.Now; // [2026-09-12]

            if (selectedMonth == now.Month && selectedYear == now.Year)
            {
                // 2. Формируем имя колонки для текущего дня (например, "day_12")
                string currentDayColumnName = $"day_{now.Day}";

                if (dataGridPlanning.Columns.Contains(currentDayColumnName))
                {
                    int columnIndex = dataGridPlanning.Columns[currentDayColumnName].Index;

                    // 3. Гарантируем, что ячейка видна на экране (прокрутка по горизонтали)
                    dataGridPlanning.FirstDisplayedScrollingColumnIndex = columnIndex;
                }
            }
        }
        private void RestoreLastSelectedCell()
        {
            // Замораживаем отрисовку формы, чтобы скрыть переходные процессы скролла
            dataGridPlanning.SuspendLayout();

            try
            {
                // 1. ВОССТАНАВЛИВАЕМ ВЕРТИКАЛЬНЫЙ СКРОЛЛ СТРОК
                if (_firstVisibleRowIndex >= 0 && _firstVisibleRowIndex < dataGridPlanning.Rows.Count)
                {
                    dataGridPlanning.FirstDisplayedScrollingRowIndex = _firstVisibleRowIndex;
                }

                // 2. 🌟 ВОССТАНАВЛИВАЕМ ГОРИЗОНТАЛЬНЫЙ СКРОЛЛ ТОЧНО В ПИКСЕЛЯХ 🌟
                // Это железно вернет левый край таблицы на то же самое место без микро-прыжков!
                if (_horizontalPixelOffset >= 0)
                {
                    dataGridPlanning.HorizontalScrollingOffset = _horizontalPixelOffset;
                }

                // 3. ВОССТАНАВЛИВАЕМ ВЫДЕЛЕНИЕ НА РЕДАКТИРУЕМУЮ ЯЧЕЙКУ
                if (_lastSelectedRowIndex >= 0 && _lastSelectedRowIndex < dataGridPlanning.Rows.Count &&
                    _lastSelectedColumnIndex >= 0 && _lastSelectedColumnIndex < dataGridPlanning.Columns.Count)
                {
                    dataGridPlanning.ClearSelection();

                    dataGridPlanning.CurrentCell = dataGridPlanning.Rows[_lastSelectedRowIndex].Cells[_lastSelectedColumnIndex];
                    dataGridPlanning.Rows[_lastSelectedRowIndex].Cells[_lastSelectedColumnIndex].Selected = true;
                }
            }
            catch (Exception)
            {
                // Страховочный блок на случай изменения размеров колонок динамически
            }
            finally
            {
                // Возвращаем отрисовку сетки
                dataGridPlanning.ResumeLayout();
            }
        }


        private async void dataGridPlanning_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 2 && e.ColumnIndex >= 2)
            {
                var cell = dataGridPlanning.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Проверяем, что в Tag лежит наш Tuple
                if (cell.Tag is Tuple<ulong, ulong> targetIds)
                {
                    int targetRow = e.RowIndex;
                    int targetCol = e.ColumnIndex;

                    _lastSelectedRowIndex = e.RowIndex;
                    _lastSelectedColumnIndex = e.ColumnIndex;

                    _firstVisibleRowIndex = dataGridPlanning.FirstDisplayedScrollingRowIndex;
                    //_firstVisibleColumnIndex = dataGridPlanning.FirstDisplayedScrollingColumnIndex;
                    _horizontalPixelOffset = dataGridPlanning.HorizontalScrollingOffset;

                    ulong equipmentId = targetIds.Item1; // ID выбранного станка
                    ulong shiftId = targetIds.Item2;      // Точный уникальный ID (индекс) смены

                    string columnNameDate = dataGridPlanning.Columns[targetCol].HeaderText;

                    // Вызываем процедуру карточки смены, передавая точный shiftId!
                    DialogResult result = DialogResult.Cancel;

                    await Task.Delay(100);

                    EditShiftForm form = new EditShiftForm(Convert.ToDateTime(columnNameDate), shiftId, equipmentId);
                    result = form.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        _editShift = true;

                        await StartLoadingStaffPlanningAsync();
                        Thread.Sleep(200);
                    }
                }
            }
        }

        

        private async Task EditSiftDefinition()
        {
            DialogResult result = DialogResult.Cancel;

            if (listViewShiftsDef.SelectedIndices.Count > 0)
            {
                int clickedIndex = listViewShiftsDef.SelectedIndices[0];

                FormAddShift formAdd = new FormAddShift(shiftDefinitions[clickedIndex]);
                result = formAdd.ShowDialog();
            }

            await Task.Delay(100);

            if (result == DialogResult.OK)
            {
                await UpdateShiftsDefinitionAsync();
            }
        }

        private async Task DeleteSiftDefinition()
        {
            if (listViewShiftsDef.SelectedIndices.Count > 0)
            {
                ConnectionParameter parameter = new ConnectionParameter();

                int clickedIndex = listViewShiftsDef.SelectedIndices[0];

                DialogResult dialogResult = MessageBox.Show($"Вы действительно хотите удалить запись: {clickedIndex + 1}: {shiftDefinitions[clickedIndex].Name}", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    var shiftService = new ShiftService(parameter.GetMySQLConnectionString());

                    DeleteResult result = await shiftService.DeleteShiftAsync((int)shiftDefinitions[clickedIndex].Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Смена не была удалена \n{result.ErrorMessage}", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        listViewShiftsDef.Items.Remove(listViewShiftsDef.SelectedItems[0]);

                        await Task.Delay(100);

                        await UpdateShiftsDefinitionAsync();
                    }
                }
            }
        }

        private async void listViewShiftsDef_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            await EditSiftDefinition();
        }
        private async void buttonShiftAdd_Click(object sender, EventArgs e)
        {
            FormAddShift formAddShift = new FormAddShift();
            DialogResult result = formAddShift.ShowDialog();

            await Task.Delay(100);

            if (result == DialogResult.OK)
            {
                await UpdateShiftsDefinitionAsync();
            }
        }
        private async void buttonShiftEdit_Click(object sender, EventArgs e)
        {
            await EditSiftDefinition();
        }

        private async void buttonShiftDelete_Click(object sender, EventArgs e)
        {
            await DeleteSiftDefinition();
        }





        private async Task EditSiftCycle()
        {
            DialogResult result = DialogResult.Cancel;

            if (listViewShiftCycle.SelectedIndices.Count > 0)
            {
                int clickedIndex = listViewShiftCycle.SelectedIndices[0];

                FormAddCycle form = new FormAddCycle(scheduleCycles[clickedIndex]);
                result = form.ShowDialog();
            }

            await Task.Delay(100);

            if (result == DialogResult.OK)
            {
                await UpdateScheduleCycle();
            }
        }
        private async Task DeleteSiftCycle()
        {
            if (listViewShiftCycle.SelectedIndices.Count > 0)
            {
                ConnectionParameter parameter = new ConnectionParameter();

                int clickedIndex = listViewShiftCycle.SelectedIndices[0];

                DialogResult dialogResult = MessageBox.Show($"Вы действительно хотите удалить запись: {clickedIndex + 1}: {scheduleCycles[clickedIndex].Name}", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    var templateService = new ScheduleTemplateService(parameter.GetMySQLConnectionString());

                    DeleteResult result = await templateService.DeleteScheduleCycleAsync((int)scheduleCycles[clickedIndex].Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Шаблон не был удален \n{result.ErrorMessage}", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        listViewShiftCycle.Items.Remove(listViewShiftCycle.SelectedItems[0]);

                        await Task.Delay(100);

                        await UpdateScheduleCycle();
                    }
                }
            }
        }
        private async void listViewShiftCycle_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            await EditSiftCycle();
        }
        private async void buttonTemplateAdd_Click(object sender, EventArgs e)
        {
            FormAddCycle form = new FormAddCycle();
            DialogResult result = form.ShowDialog();

            await Task.Delay(100);

            if (result == DialogResult.OK)
            {
                await UpdateScheduleCycle();
            }
        }
        private async void buttonTemplateEdit_Click(object sender, EventArgs e)
        {
            await EditSiftCycle();
        }

        private async void buttonTemplateDelete_Click(object sender, EventArgs e)
        {
            await DeleteSiftCycle();
        }





        private async Task EditTemplate()
        {
            DialogResult result = DialogResult.Cancel;

            if (listViewShiftTemplate.SelectedIndices.Count > 0)
            {
                int clickedIndex = listViewShiftTemplate.SelectedIndices[0];

                FormAddBrigade form = new FormAddBrigade(scheduleTemplates[clickedIndex]);
                result = form.ShowDialog();
            }

            await Task.Delay(100);

            if (result == DialogResult.OK)
            {
                await UpdateScheduleTemplate();
            }
        }
        private async Task DeleteTemplate()
        {
            if (listViewShiftTemplate.SelectedIndices.Count > 0)
            {
                ConnectionParameter parameter = new ConnectionParameter();

                int clickedIndex = listViewShiftTemplate.SelectedIndices[0];

                DialogResult dialogResult = MessageBox.Show($"Вы действительно хотите удалить запись: {clickedIndex + 1}: {scheduleTemplates[clickedIndex].Name}", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    var templateService = new ScheduleTemplateService(parameter.GetMySQLConnectionString());

                    DeleteResult result = await templateService.DeleteTemplateBindingAsync((int)scheduleTemplates[clickedIndex].Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"График не был удален \n{result.ErrorMessage}", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        listViewShiftTemplate.Items.Remove(listViewShiftTemplate.SelectedItems[0]);

                        await Task.Delay(100);

                        await UpdateScheduleTemplate();
                    }
                }
            }
        }
        private async void listViewShiftTemplate_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            await EditTemplate();
        }
        private async void buttonBrigadeAdd_Click(object sender, EventArgs e)
        {
            FormAddBrigade form = new FormAddBrigade();
            DialogResult result = form.ShowDialog();

            await Task.Delay(100);

            if (result == DialogResult.OK)
            {
                await UpdateScheduleTemplate();
            }
        }
        private async void buttonBrigadeEdit_Click(object sender, EventArgs e)
        {
            await EditTemplate();
        }

        private async void buttonBrigadeDelete_Click(object sender, EventArgs e)
        {
            await DeleteTemplate();
        }






        private void listViewShiftsDef_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool status = listViewShiftsDef.SelectedItems.Count > 0;

            buttonShiftEdit.Enabled = status;
            buttonShiftDelete.Enabled = status;
        }

        private void listViewShiftCycle_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool status = listViewShiftCycle.SelectedItems.Count > 0;

            buttonTemplateEdit.Enabled = status;
            buttonTemplateDelete.Enabled = status;
        }

        private void listViewShiftTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool status = listViewShiftTemplate.SelectedItems.Count > 0;

            buttonBrigadeEdit.Enabled = status;
            buttonBrigadeDelete.Enabled = status;
        }







        //Работа с производственными участками
        private async Task UpdateWorkAreasAsync()
        {
            try
            {
                workAreaInfo?.Clear();

                ConnectionParameter parameter = new ConnectionParameter();

                var service = new WorkAreaService(parameter.GetMySQLConnectionString());

                workAreaInfo = await service.GetWorkAreasWithEquipmentAsync();
            }
            catch
            {

            }
            
        }
        private async Task LoadAreaListToListBox()
        {
            listBoxAreas.Items.Clear();
            //MaterialListBoxItem item = new MaterialListBoxItem();

            for (int i = 0; i < workAreaInfo.Count; i++)
            {
                MaterialListBoxItem item = new MaterialListBoxItem
                {
                    Text = workAreaInfo[i].Name
                };

                listBoxAreas.Items.Add(item);
            }
        }
        private async Task LoadEquipListToListView()
        {
            listViewEquips.Items.Clear();

            //buttonEquipAdd.Enabled = false;
            buttonEquipEdit.Enabled = false;
            buttonEquipDelete.Enabled = false;
            buttonEquipMoveUp.Enabled = false;
            buttonEquipMoveDown.Enabled = false;

            int index = listBoxAreas.SelectedIndex;
            bool status = index >= 0;

            if (status)
            {
                foreach (EquipmentShortInfo shortInfo in workAreaInfo[index].Equipments)
                {
                    ListViewItem viewItem = new ListViewItem();

                    viewItem.Text = (listViewEquips.Items.Count + 1).ToString();
                    viewItem.SubItems.Add(shortInfo.Name);
                    viewItem.SubItems.Add(shortInfo.TemplateName);
                    viewItem.SubItems.Add(shortInfo.StaffingModeRus);
                    viewItem.SubItems.Add(shortInfo.IsActive ? "Активен" : "Остановлен");

                    listViewEquips.Items.Add(viewItem);
                }
            }
        }
        private async Task WorkAreaEdit()
        {
            int index = listBoxAreas.SelectedIndex;

            if (index >= 0)
            {
                FormAddArea form = new FormAddArea(workAreaInfo[index]);
                DialogResult result = form.ShowDialog();

                await Task.Delay(100);

                if (result == DialogResult.OK)
                {
                    await UpdateWorkAreasAsync();
                    await LoadAreaListToListBox();
                }
            }
        }
        private async Task WorkAreaDelete()
        {
            int index = listBoxAreas.SelectedIndex;

            if (index >= 0)
            {
                ConnectionParameter parameter = new ConnectionParameter();

                DialogResult dialogResult = MessageBox.Show($"Вы действительно хотите удалить запись: {index + 1}: {workAreaInfo[index].Name}", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    var workAreaService = new WorkAreaService(parameter.GetMySQLConnectionString());

                    DeleteResult result = await workAreaService.DeleteWorkAreaAsync(workAreaInfo[index].Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Производственный участок не был удален \n{result.ErrorMessage}", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        listBoxAreas.Items.Remove(listBoxAreas.SelectedItem);

                        await Task.Delay(100);

                        await UpdateWorkAreasAsync();
                        await LoadAreaListToListBox();
                    }
                }
            }
        }
        private async void listBoxAreas_SelectedIndexChanged(object sender, MaterialListBoxItem selectedItem)
        {
            await LoadEquipListToListView();

            bool status = listBoxAreas.SelectedIndex >= 0;

            buttonAreaEdit.Enabled = status;
            buttonAreaDelete.Enabled = status;
            //сот=ртировку добавить

            buttonEquipAdd.Enabled = status;
        }
        private async void buttonAreaAdd_Click(object sender, EventArgs e)
        {
            FormAddArea form = new FormAddArea();
            DialogResult result = form.ShowDialog();

            await Task.Delay(100);

            if (result == DialogResult.OK)
            {
                await UpdateWorkAreasAsync();
                await LoadAreaListToListBox();
            }
        }
        private async void buttonAreaEdit_Click(object sender, EventArgs e)
        {
            await WorkAreaEdit();
        }
        private async void buttonAreaDelete_Click(object sender, EventArgs e)
        {
            await WorkAreaDelete();
        }
        private void buttonAreaMoveUp_Click(object sender, EventArgs e)
        {

        }
        private void buttonAreaMoveDown_Click(object sender, EventArgs e)
        {

        }




        private async Task EditEquip()
        {
            int index = listViewEquips.SelectedIndices[0];

            if (index >= 0)
            {
                FormAddEquip form = new FormAddEquip(workAreaInfo[listBoxAreas.SelectedIndex].Id, workAreaInfo[listBoxAreas.SelectedIndex].Equipments[index].Id);
                DialogResult result = form.ShowDialog();

                await Task.Delay(100);

                if (result == DialogResult.OK)
                {
                    await UpdateWorkAreasAsync();
                    //await LoadAreaListToListBox();
                    await LoadEquipListToListView();
                }
            }
        }
        private async void buttonEquipAdd_Click(object sender, EventArgs e)
        {
            int areaID = workAreaInfo[listBoxAreas.SelectedIndex].Id;

            FormAddEquip form = new FormAddEquip(areaID);
            DialogResult result = form.ShowDialog();

            await Task.Delay(100);

            if (result == DialogResult.OK)
            {
                await UpdateWorkAreasAsync();
                await LoadEquipListToListView();
            }
        }

        private async void listViewEquips_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            await EditEquip();
        }

        private void listViewEquips_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool status = listViewEquips.SelectedItems.Count > 0;

            buttonEquipEdit.Enabled = status;
            buttonEquipDelete.Enabled = status;
            buttonEquipMoveUp.Enabled = status;
            buttonEquipMoveDown.Enabled = status;

            buttonEquipAdd.Enabled = listBoxAreas.SelectedIndex >= 0;
        }

        private async void buttonEquipEdit_Click(object sender, EventArgs e)
        {
            await EditEquip();
        }

        private async void buttonEquipDelete_Click(object sender, EventArgs e)
        {
            if (listViewEquips.SelectedIndices.Count > 0)
            {
                ConnectionParameter parameter = new ConnectionParameter();

                int clickedIndex = listViewEquips.SelectedIndices[0];

                DialogResult dialogResult = MessageBox.Show($"Вы действительно хотите удалить запись: {clickedIndex + 1}: {workAreaInfo[listBoxAreas.SelectedIndex].Equipments[clickedIndex].Name}", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    var workAreaService = new WorkAreaService(parameter.GetMySQLConnectionString());

                    DeleteResult result = await workAreaService.DeleteEquipmentAsync(workAreaInfo[listBoxAreas.SelectedIndex].Equipments[clickedIndex].Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"График не был удален \n{result.ErrorMessage}", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        listViewEquips.Items.Remove(listViewEquips.SelectedItems[0]);

                        await Task.Delay(100);

                        await UpdateWorkAreasAsync();
                        await LoadEquipListToListView();
                    }
                }
            }
        }

        private async void buttonEquipMoveUp_Click(object sender, EventArgs e)
        {
            if (listViewEquips.SelectedIndices.Count > 0)
            {
                ConnectionParameter parameter = new ConnectionParameter();

                int clickedIndex = listViewEquips.SelectedIndices[0];

                var workAreaService = new WorkAreaService(parameter.GetMySQLConnectionString());

                await workAreaService.MoveEquipmentUpAsync(workAreaInfo[listBoxAreas.SelectedIndex].Equipments[clickedIndex].Id, workAreaInfo[listBoxAreas.SelectedIndex].Id);

                await UpdateWorkAreasAsync();
                await LoadEquipListToListView();
            }
        }

        private async void buttonEquipMoveDown_Click(object sender, EventArgs e)
        {
            if (listViewEquips.SelectedIndices.Count > 0)
            {
                ConnectionParameter parameter = new ConnectionParameter();

                int clickedIndex = listViewEquips.SelectedIndices[0];

                var workAreaService = new WorkAreaService(parameter.GetMySQLConnectionString());

                await workAreaService.MoveEquipmentDownAsync(workAreaInfo[listBoxAreas.SelectedIndex].Equipments[clickedIndex].Id, workAreaInfo[listBoxAreas.SelectedIndex].Id);

                await UpdateWorkAreasAsync();
                await LoadEquipListToListView();
            }
        }

        private async Task UpdateUsersShortInfo()
        {
            employeeShorts?.Clear();

            ConnectionParameter parameter = new ConnectionParameter();

            try
            {
                var service = new EmployeeManagementService(parameter.GetMySQLConnectionString());

                employeeShorts = await service.GetEmployeeShortListAsync();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка получения списка сотрудников:\n {ex}");
            }
        }

        private async Task LoadPositions()
        {
            ConnectionParameter parameter = new ConnectionParameter();

            _positions?.Clear();

            EmployeeManagementService employeeService = new EmployeeManagementService(parameter.GetMySQLConnectionString());

            try
            {
                _positions = await employeeService.GetPositionsLookupAsync();

                comboBoxUserPosition.Items.Clear();

                foreach (PositionLookupDto position in _positions)
                {
                    comboBoxUserPosition.Items.Add(position.Name);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task LoadUserListToListBox()
        {
            listBoxUsers.Items.Clear();
            _employeeShortsIndexes?.Clear();

            //MaterialListBoxItem item = new MaterialListBoxItem();

            if (comboBoxUserPosition.SelectedIndex != -1)
            {
                for (int i = 0; i < employeeShorts?.Count; i++)
                {
                    if (employeeShorts[i].CurrentPositionID == _positions[comboBoxUserPosition.SelectedIndex].Id)
                    {
                        _employeeShortsIndexes.Add(i);

                        MaterialListBoxItem item = new MaterialListBoxItem
                        {
                            Text = employeeShorts[i].FullName
                        };

                        listBoxUsers.Items.Add(item);
                    }
                }

                listBoxUsers.SelectedIndex = -1;
                listBoxUsers.Refresh();
                ClearUserShortInfo();

                materialButtonUserViewFullCard.Enabled = false;
                buttonUserAbsenceAdd.Enabled = false;
            }
        }

        private async Task EditUser()
        {
            if (listBoxUsers.SelectedIndex == -1 || comboBoxUserPosition.SelectedIndex == -1) return;
            int index = _employeeShortsIndexes[listBoxUsers.SelectedIndex];
            ulong userId = employeeShorts[index].Id;

            await OpenUserCardAsync(userId);
        }

        private async Task OpenUserCardAsync(ulong userId = 0)
        {
            try
            {
                bool isNewUser = (userId == 0);
                ulong currentSelectedPositionId = 0;

                // Если это редактирование, запоминаем текущую открытую должность
                if (!isNewUser)
                {
                    if (listBoxUsers.SelectedIndex == -1 || comboBoxUserPosition.SelectedIndex == -1) return;
                    currentSelectedPositionId = _positions[comboBoxUserPosition.SelectedIndex].Id;
                }

                // Открываем универсальную форму (передаем 0 для нового или ID для редактирования)
                FormAddUser form = new FormAddUser(userId);
                DialogResult result = form.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // Получаем актуальный ID (для нового сотрудника берем из свойства формы, для старого оставляем прежний)
                    ulong targetUserId = isNewUser ? form.SavedUserId : userId;

                    // 1. Обновляем общий список сотрудников и должностей из базы данных
                    await UpdateUsersShortInfo();
                    await LoadPositions();

                    if (isNewUser)
                    {
                        // СЦЕНАРИЙ А: ДОБАВЛЕН НОВЫЙ СОТРУДНИК
                        // Ищем созданного сотрудника в обновленном кэше, чтобы узнать его должность
                        var createdUser = employeeShorts.Find(u => u.Id == targetUserId);

                        if (createdUser != null)
                        {
                            // Переключаем комбобокс на должность нового сотрудника
                            int newPosIndex = _positions.FindIndex(p => p.Id == createdUser.CurrentPositionID);
                            if (newPosIndex != -1)
                            {
                                comboBoxUserPosition.SelectedIndex = newPosIndex;
                            }
                        }
                    }
                    else
                    {
                        // СЦЕНАРИЙ Б: РЕДАКТИРОВАНИЕ СУЩЕСТВУЮЩЕГО СОТРУДНИКА
                        // Остаемся в той должности, которая была открыта до редактирования
                        int targetPosIndex = _positions.FindIndex(p => p.Id == currentSelectedPositionId);
                        if (targetPosIndex != -1)
                        {
                            comboBoxUserPosition.SelectedIndex = targetPosIndex;
                        }
                        else if (comboBoxUserPosition.Items.Count > 0)
                        {
                            comboBoxUserPosition.SelectedIndex = 0;
                        }
                    }

                    // 2. Перегружаем список сотрудников для активной в комбобоксе должности
                    await LoadUserListToListBox();

                    // 3. Ищем сотрудника в списке ListBox по targetUserId
                    int targetUserListBoxIndex = -1;
                    for (int i = 0; i < _employeeShortsIndexes.Count; i++)
                    {
                        int cacheIndex = _employeeShortsIndexes[i];
                        if (employeeShorts[cacheIndex].Id == targetUserId)
                        {
                            targetUserListBoxIndex = i;
                            break;
                        }
                    }

                    // 4. Выделяем сотрудника и обновляем правую панель с краткой информацией
                    if (targetUserListBoxIndex != -1)
                    {
                        listBoxUsers.SelectedIndex = targetUserListBoxIndex;
                        listBoxUsers_SelectedIndexChanged(listBoxUsers, listBoxUsers.SelectedItem);
                    }
                    else
                    {
                        // Если при редактировании сотрудника перевели на другую должность, остаемся в текущей и сбрасываем выбор
                        listBoxUsers.SelectedIndex = -1;
                        ClearUserShortInfo();
                    }

                    listBoxUsers.Refresh();
                    comboBoxUserPosition.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при работе с карточкой сотрудника:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ViewFullUserCard()
        {
            try
            {
                if (listBoxUsers.SelectedIndex == -1 || comboBoxUserPosition.SelectedIndex == -1) return;

                // 1. Запоминаем ID текущей должности и ID выбранного сотрудника
                ulong currentSelectedPositionId = _positions[comboBoxUserPosition.SelectedIndex].Id;
                int selectedUserIndex = listBoxUsers.SelectedIndex;
                int index = _employeeShortsIndexes[selectedUserIndex];
                ulong userId = employeeShorts[index].Id;

                FormAddUser form = new FormAddUser(userId);
                DialogResult result = form.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // 2. Перезагружаем данные из базы
                    await UpdateUsersShortInfo();
                    await LoadPositions();

                    // 3. Ищем индекс сохраненной должности в обновленном списке _positions
                    int targetPosIndex = _positions.FindIndex(p => p.Id == currentSelectedPositionId);

                    if (targetPosIndex != -1)
                    {
                        comboBoxUserPosition.SelectedIndex = targetPosIndex;
                    }
                    else if (comboBoxUserPosition.Items.Count > 0)
                    {
                        comboBoxUserPosition.SelectedIndex = 0;
                    }

                    // 4. Загружаем список сотрудников для выбранной должности
                    await LoadUserListToListBox();

                    // 5. Ищем отредактированного сотрудника в новом списке по его ID
                    int targetUserListBoxIndex = -1;
                    for (int i = 0; i < _employeeShortsIndexes.Count; i++)
                    {
                        int cacheIndex = _employeeShortsIndexes[i];
                        if (employeeShorts[cacheIndex].Id == userId)
                        {
                            targetUserListBoxIndex = i;
                            break;
                        }
                    }

                    // 6. Восстанавливаем выделение (или сбрасываем, если должность изменилась)
                    if (targetUserListBoxIndex != -1)
                    {
                        listBoxUsers.SelectedIndex = targetUserListBoxIndex;

                        // Вызываем обновление правых полей (замените на имя вашего обработчика клика по списку)
                        listBoxUsers_SelectedIndexChanged(listBoxUsers, listBoxUsers.SelectedItem);
                    }
                    else
                    {
                        listBoxUsers.SelectedIndex = -1;
                        ClearUserShortInfo();
                    }

                    listBoxUsers.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения полной информации сотрудника:\n {ex}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearUserShortInfo()
        {
            textBoxUserLastName.Text = "";
            textBoxUserFirstName.Text = "";
            textBoxUserPatronymic.Text = "";
            textBoxUserContactPhone.Text = "";

            textBoxUserStatus.Text = "";
            textBoxUserPosition.Text = "";
            textBoxUserAssigmentArea.Text = "";
            textBoxUserAssigmentEquip.Text = "";
            textBoxUserSchedule.Text = "";
        }

        private void listBoxUsers_SelectedIndexChanged(object sender, MaterialListBoxItem selectedItem)
        {
            try
            {
                if (listBoxUsers.SelectedIndex >= 0)
                {
                    int index = _employeeShortsIndexes[listBoxUsers.SelectedIndex];

                    textBoxUserLastName.Text = employeeShorts[index].LastName;
                    textBoxUserFirstName.Text = employeeShorts[index].FirstName;
                    textBoxUserPatronymic.Text = employeeShorts[index].Patronymic;
                    textBoxUserContactPhone.Text = employeeShorts[index].PrimaryPhone;

                    textBoxUserStatus.Text = employeeShorts[index].CurrentStatus;
                    textBoxUserPosition.Text = employeeShorts[index].CurrentPosition;
                    textBoxUserAssigmentArea.Text = employeeShorts[index].CurrentWorkArea;
                    textBoxUserAssigmentEquip.Text = employeeShorts[index].CurrentEquipment;
                    textBoxUserSchedule.Text = employeeShorts[index].CurrentSchedule;

                    materialButtonUserViewFullCard.Enabled = true;

                    if (employeeShorts[index].IsActive == 1)
                    {
                        buttonUserAbsenceAdd.Enabled = true;
                    }
                    else
                    {
                        buttonUserAbsenceAdd.Enabled = false;
                    }
                }
                else
                {
                    materialButtonUserViewFullCard.Enabled = false;
                    buttonUserAbsenceAdd.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения краткой информации сотрудника:\n {ex}");
            }
        }

        private async void materialButtonUserViewFullCard_Click(object sender, EventArgs e)
        {
            //await ViewFullUserCard();
            await EditUser();
        }

        private async void materialButtonUserAdd_Click(object sender, EventArgs e)
        {
            await OpenUserCardAsync();
        }

        private async void comboBoxUserPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            await LoadUserListToListBox();
        }

        private async void listBoxUsers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //await ViewFullUserCard();
            await EditUser();
        }

        ///Блок отображения, заполнения и редактирования отсутствий
        ///
        public List<AbsenceGridRow> PrepareDataForGrid(List<EmployeeAbsenceExtendedRow> dbRows, DateTime targetMonth)
        {
            // 1. Узнаем, сколько дней в выбранном месяце (28, 29, 30 или 31)
            int daysInMonth = DateTime.DaysInMonth(targetMonth.Year, targetMonth.Month);
            var gridRows = new List<AbsenceGridRow>();

            foreach (var dbRow in dbRows)
            {
                var gridRow = new AbsenceGridRow(daysInMonth)
                {
                    Id = dbRow.Id,
                    EmployeeFullName = dbRow.EmployeeFullName,
                    AbsenceTypeName = dbRow.AbsenceTypeName,
                    PeriodText = dbRow.PeriodText,
                    StatusColor = dbRow.StatusColor
                };

                // 2. Проверяем каждый день месяца
                for (int day = 1; day <= daysInMonth; day++)
                {
                    DateTime currentDate = new DateTime(targetMonth.Year, targetMonth.Month, day);

                    // Используем ваш готовый метод проверки активности даты!
                    if (dbRow.IsActiveOn(currentDate))
                    {
                        gridRow.Days[day - 1] = true; // Отмечаем, что в этот день человек отсутствовал
                    }
                }

                gridRows.Add(gridRow);
            }

            return gridRows;
        }

        public void FillAndColorGrid(DataGridView grid, List<AbsenceGridRow> gridData, DateTime targetMonth)
        {
            // Очищаем старые данные и колонки
            grid.Columns.Clear();
            grid.Rows.Clear();

            // 1. Создаем базовые колонки
            grid.Columns.Add("Number", "Number");
            grid.Columns.Add("FullName", "Сотрудник");
            grid.Columns.Add("AbsenceType", "Причина");
            grid.Columns.Add("Period", "Период");

            // 2. Динамически создаем колонки для дней месяца
            int daysInMonth = DateTime.DaysInMonth(targetMonth.Year, targetMonth.Month);

            int rowFirst = grid.Rows.Add();

            grid.Rows[rowFirst].Height = 40;

            GridHelper.MergeCells(grid, "№", rowFirst, 0, 1, 1, Color.Gray, Color.White, grid.Font, StringAlignment.Center, StringAlignment.Center);
            GridHelper.MergeCells(grid, "Сотрудник", rowFirst, 1, 1, 1, Color.Gray, Color.White, grid.Font, StringAlignment.Center, StringAlignment.Center);
            GridHelper.MergeCells(grid, "Причина", rowFirst, 2, 1, 1, Color.Gray, Color.White, grid.Font, StringAlignment.Center, StringAlignment.Center);
            GridHelper.MergeCells(grid, "Период, продолжительность", rowFirst, 3, 1, 1, Color.Gray, Color.White, grid.Font, StringAlignment.Center, StringAlignment.Center);

            grid.Columns[0].Width = 20;
            grid.Columns[1].Width = 220;
            grid.Columns[2].Width = 200;
            grid.Columns[3].Width = 280;

            for (int day = 1; day <= daysInMonth; day++)
            {
                // Имя колонки будет "Day1", "Day2" и т.д., а заголовок просто "1", "2"..."31"
                int colIndex = grid.Columns.Add($"Day{day}", day.ToString());
                grid.Columns[colIndex].Width = 30; // Делаем колонки дней узкими и аккуратными

                GridHelper.MergeCells(grid, day.ToString(), rowFirst, day + 3, 1, 1, Color.Gray, Color.White, grid.Font, StringAlignment.Center, StringAlignment.Center);
            }

            int colLastIndex = grid.Columns.Add($"DayCount", "sum");
            grid.Columns[colLastIndex].Width = 80;

            GridHelper.MergeCells(grid, "Дней", rowFirst, colLastIndex, 1, 1, Color.Gray, Color.White, grid.Font, StringAlignment.Center, StringAlignment.Center);

            // 3. Заполняем строки данными и красим ячейки
            foreach (var rowData in gridData)
            {
                // Создаем массив объектов для добавления строки (Сотрудник, Причина, Период)


                /*var rowValues = new List<object> { rowData.EmployeeFullName, rowData.AbsenceTypeName, rowData.PeriodText };

                // Добавляем пустые значения для ячеек-дней (чтобы текст там не писался)
                for (int i = 0; i < daysInMonth; i++) rowValues.Add("");

                // Добавляем строку в GridView и получаем её индекс
                int rowIndex = grid.Rows.Add(rowValues.ToArray());
                DataGridViewRow gridRow = grid.Rows[rowIndex];*/

                int rowIndex = grid.Rows.Add();

                grid.Rows[rowIndex].Tag = rowData.Id;
                grid.Rows[rowIndex].Height = 30;

                DataGridViewRow gridRow = grid.Rows[rowIndex];

                GridHelper.MergeCells(grid, rowIndex.ToString(), rowIndex, 0, 1, 1, Color.Gray, Color.White, grid.Font, StringAlignment.Center, StringAlignment.Center);

                gridRow.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);

                gridRow.Cells[1].Value = rowData.EmployeeFullName;
                gridRow.Cells[2].Value = rowData.AbsenceTypeName;
                gridRow.Cells[3].Value = rowData.PeriodText;

                gridRow.Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                gridRow.Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

                // 4. КРАСИМ ЯЧЕЙКИ: Идем по колонкам дней (они начинаются с индекса 3)
                Color customColor = ColorTranslator.FromHtml(rowData.StatusColor);

                int days = 0;

                for (int i = 0; i < daysInMonth; i++)
                {
                    int cellIndex = 4 + i; // Смещение, так как первые 3 колонки — это текст

                    gridRow.Cells[cellIndex].Value = "";

                    if (rowData.Days[i] == true)
                    {
                        days++;

                        /*
                         // Превращаем "#4CAF50" в Color объект для C#
                        Color customColor = ColorTranslator.FromHtml(rowData.StatusColor);
                        e.CellStyle.BackColor = customColor;
                        e.CellStyle.SelectionBackColor = customColor;

                         */
                        // Закрашиваем ячейку, если в этот день человек отсутствует
                        gridRow.Cells[cellIndex].Style.BackColor = customColor;//Color.Tomato; // Красный цвет (можно Color.Orange)
                        gridRow.Cells[cellIndex].ToolTipText = rowData.AbsenceTypeName; // Подсказка при наведении
                    }
                }

                gridRow.Cells[colLastIndex].Value = days.ToString("N0");
            }
        }

        private async void OnMonthSelected(DateTime selectedMonth)
        {
            ConnectionParameter parameter = new ConnectionParameter();
            EmployeeManagementService employeeService = new EmployeeManagementService(parameter.GetMySQLConnectionString());

            // 1. Вычисляем начало и конец выбранного месяца для SQL-запроса
            DateTime startOfMonth = new DateTime(selectedMonth.Year, selectedMonth.Month, 1);
            DateTime endOfMonth = new DateTime(selectedMonth.Year, selectedMonth.Month, DateTime.DaysInMonth(selectedMonth.Year, selectedMonth.Month));

            // 2. Получаем данные из вашей БД через созданный ранее метод
            List<EmployeeAbsenceExtendedRow> dbData = await employeeService.GetActiveAbsencesInPeriodAsync(startOfMonth, endOfMonth);

            // 3. Пересобираем данные в удобную для сетки структуру
            List<AbsenceGridRow> gridData = PrepareDataForGrid(dbData, selectedMonth);

            // 4. Отображаем и красим в DataGridView
            FillAndColorGrid(gridViewAbsence, gridData, selectedMonth);
            SetupColumnsFlexibility(gridViewAbsence);
        }

        private void DateChange()
        {
            if (comboBoxAbsenceMonth.SelectedIndex != -1 && comboBoxAbsenceYear.SelectedIndex != -1)
            {
                DateTime date = new DateTime(Convert.ToInt32(comboBoxAbsenceYear.Text), comboBoxAbsenceMonth.SelectedIndex + 1, 1);

                OnMonthSelected(date);
            }
        }

        private void comboBoxAbsenceYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateChange();
        }

        private void comboBoxAbsenceMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateChange();
        }

        public void SetupColumnsFlexibility(DataGridView grid)
        {
            // Обязательно отключаем этот режим для всей таблицы, 
            // так как мы будем настраивать каждую колонку индивидуально
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            int totalColumns = grid.Columns.Count;

            for (int i = 0; i < totalColumns; i++)
            {
                // 1. Первые 4 колонки (индексы 0, 1, 2, 3)
                if (i < 4)
                {
                    grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    //grid.Columns[i].Width = 120; // Задайте нужную вам фиксированную ширину
                }
                // 2. Самая последняя колонка
                else if (i == totalColumns - 1)
                {
                    grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    grid.Columns[i].Width = 60; // Фиксированная ширина для последней колонки
                }
                // 3. Все остальные колонки (дни месяца между ними)
                else
                {
                    // Режим Fill заставляет колонки равномерно растягиваться, 
                    // чтобы занять всё оставшееся свободное пространство
                    grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    // Задаем минимальную ширину ячейки дня, чтобы сетка не сжималась слишком сильно
                    grid.Columns[i].MinimumWidth = 25;
                }
            }
        }


        private void gridViewAbsence_SelectionChanged(object sender, EventArgs e)
        {
            // Отключаем обработку, если таблица пустая
            if (gridViewAbsence.Rows.Count == 0) return;

            // ЗАЩИТА: Если пользователь умудрился выделить самую первую строку (индекс 0)
            if (gridViewAbsence.Rows[0].Selected)
            {
                // Просто снимаем с нее выделение, НЕ трогая остальные строки!
                gridViewAbsence.Rows[0].Selected = false;
            }

            // 1. Проверяем, есть ли вообще выбранная строка
            if (gridViewAbsence.CurrentRow == null)
            {
                gridViewAbsence.Enabled = false;
                return;
            }

            int currentIndex = gridViewAbsence.CurrentRow.Index;

            // 2. БИЗНЕС-ПРАВИЛО: Кнопка активна только если выбрана строка сотрудника.
            // Если индекс равен 0 (наша первая строка с числами месяца), кнопку активировать НЕЛЬЗЯ.
            if (currentIndex > 0)
            {
                buttonAbsenceDelete.Enabled = true; // Активируем кнопку удаления
            }
            else
            {
                buttonAbsenceDelete.Enabled = false; // Блокируем для первой строки
            }
        }

        private void gridViewAbsence_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex <= 0) return;
            // Игнорируем клики по шапке таблицы (индекс -1)
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // 1. Если кликнули по первой строке (индекс 0) — запрещаем её выделять
            if (e.RowIndex == 0)
            {
                gridViewAbsence.ClearSelection();
                return;
            }

            // 2. Если кликнули по ячейкам дней (индекс столбца >= 4)
            if (e.ColumnIndex >= 4)
            {
                // Очищаем старое выделение
                gridViewAbsence.ClearSelection();

                // Принудительно выделяем ТОЛЬКО текстовую часть КЛИКНУТОЙ строки (e.RowIndex)
                for (int i = 0; i < 4; i++)
                {
                    gridViewAbsence.Rows[e.RowIndex].Cells[i].Selected = true;
                }
            }

            gridViewAbsence.Invalidate();
        }
        private void gridViewAbsence_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex == 0) return;

            var grid = (DataGridView)sender;

            // ПРОВЕРКА №2: Проверяем, является ли текущая рисуемая строка АКТИВНОЙ (выбранной пользователем)
            if (grid.CurrentRow != null && grid.CurrentRow.Index == e.RowIndex)
            {
                // Берем границы строки с небольшим отступом внутрь, чтобы линия не обрезалась
                Rectangle bounds = new Rectangle(
                    e.RowBounds.Left + 0,
                    e.RowBounds.Top + 0,
                    e.RowBounds.Width - 1,
                    e.RowBounds.Height - 1
                );

                // Рисуем рамку ярким цветом для теста
                using (Pen pen = new Pen(Color.Orange, 2))
                {
                    e.Graphics.DrawRectangle(pen, bounds);
                }
            }
        }

        private void gridViewAbsence_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 1. Проверяем, что клик был в рабочей области:
            // Игнорируем шапку (e.RowIndex < 0 или e.ColumnIndex < 0)
            // Игнорируем первые 2 верхние строки (0 и 1)
            // Игнорируем первые 2 левых столбца (0 и 1)
            if (e.RowIndex >= 1 && e.ColumnIndex < 5)
            {
                // 2. Определяем целевую строку и колонку для сбора данных.
                // Если кликнули по объединенному блоку, данные привязаны к его "мастер-ячейке" (началу блока).
                int targetRow = e.RowIndex;
                int targetCol = e.ColumnIndex;

                if (gridViewAbsence.Rows[e.RowIndex].Cells[e.ColumnIndex] is BiMergedCell clickedCell)
                {
                    targetRow = clickedCell.TopRow;
                    targetCol = clickedCell.LeftColumn;
                }

                // Получаем содержимое столбца с индексом 1 для этой строки (Номер смены)
                object rowName = gridViewAbsence.Rows[targetRow].Tag;
                int id = rowName != null ? Convert.ToInt32(rowName) : -1;

                FormAddAbsence form = new FormAddAbsence(1, id);
                DialogResult = form.ShowDialog();

                if (DialogResult == DialogResult.OK)
                {
                    DateChange();
                }
            }
        }

        private void buttonAbsenceNew_Click(object sender, EventArgs e)
        {
            FormAddAbsence form = new FormAddAbsence();
            DialogResult = form.ShowDialog();

            if (DialogResult == DialogResult.OK)
            {
                DateChange();
            }
        }

        private void buttonAbsenceCurrentMonth_Click(object sender, EventArgs e)
        {
            comboBoxAbsenceYear.SelectedIndex = comboBoxAbsenceYear.Items.Count - 1;
            comboBoxAbsenceMonth.SelectedIndex = DateTime.Now.Month - 1;

            comboBoxAbsenceYear.Refresh();
            comboBoxAbsenceMonth.Refresh();

            DateChange();
        }

        private async void buttonAbsenceDelete_Click(object sender, EventArgs e)
        {
            // 1. Защитная проверка: есть ли выделенная строка и лежит ли в её Tag наш ulong ID
            if (gridViewAbsence.CurrentRow == null || !(gridViewAbsence.CurrentRow.Tag is ulong absenceId))
            {
                MessageBox.Show("Не удалось определить ID выбранного отсутствия.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2. Запрашиваем подтверждение удаления, чтобы пользователь не нажал кнопку случайно
            var confirmResult = MessageBox.Show(
                "Вы уверены, что хотите безвозвратно удалить этот период отсутствия сотрудника?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    // Блокируем элементы на время запроса к БД
                    buttonAbsenceDelete.Enabled = false;

                    // 3. Вызываем метод репозитория
                    ConnectionParameter parameter = new ConnectionParameter();
                    EmployeeManagementService employeeService = new EmployeeManagementService(parameter.GetMySQLConnectionString());
                    bool isDeleted = await employeeService.DeleteAbsenceAsync(absenceId);

                    if (isDeleted)
                    {
                        MessageBox.Show("Период отсутствия успешно удален.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 4. ОБНОВЛЕНИЕ ЭКРАНА: вызываем ваш метод загрузки данных, 
                        // чтобы таблица мгновенно перерисовалась уже без удаленного сотрудника
                        DateChange();
                    }
                    else
                    {
                        MessageBox.Show("Запись не найдена в базе данных. Возможно, она уже была удалена.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении из базы данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void buttonUserAbsenceAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int selecredIndex = listBoxUsers.SelectedIndex;
                int index = _employeeShortsIndexes[selecredIndex];

                FormAddAbsence form = new FormAddAbsence(2, (int)employeeShorts[index].Id);
                DialogResult = form.ShowDialog();

                if (DialogResult == DialogResult.OK)
                {
                    await LoadUserListToListBox();

                    listBoxUsers.SelectedIndex = -1;
                    listBoxUsers.Update();
                    listBoxUsers.Refresh();
                    listBoxUsers.SelectedIndex = selecredIndex;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления периода:\n {ex}");
            }
        }

        private void dataGridPlanning_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridPlanning_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // Проверяем, что это не шапка и строка содержит наш маркер последней строки станка
            if (e.RowIndex >= 0 && dataGridPlanning.Rows[e.RowIndex].Tag?.ToString() == "LastEquipRow")
            {
                // Вычисляем координаты для линии (под текущей строкой по всей ширине таблицы)
                int rowBottom = e.RowBounds.Bottom - 1;
                int leftCoord = e.RowBounds.Left;
                int rightCoord = e.RowBounds.Right;

                // Создаем оранжевое перо толщиной в 2 пикселя (можете настроить толщину и оттенок)
                using (Pen orangePen = new Pen(Color.DarkOrange, 2))
                {
                    // Рисуем разделительную линию под ячейками
                    e.Graphics.DrawLine(orangePen, leftCoord, rowBottom, rightCoord, rowBottom);
                }
            }
        }

        private async void materialTabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = materialTabControl2.SelectedIndex;

            if (index == 0)
            {
                await UpdateShiftsDefinitionAsync();
                await UpdateScheduleCycle();
                await UpdateScheduleTemplate();
            }

            if (index == 1)
            {
                await LoadAbsenceTypesGridAsync();
                dataGridViewAbsences_SelectionChanged(null, null);
            }
        }

        // =========================================================================
        // МОДУЛЬ УПРАВЛЕНИЯ ТИПАМИ ОТСУТСТВИЙ (ABSENCES)
        // =========================================================================
        private async Task LoadAbsenceTypesGridAsync()
        {
            try
            {
                var param = new ConnectionParameter();
                var service = new EmployeeManagementService(param.GetMySQLConnectionString());

                // Читаем положение переключателя MaterialSwitch "отображать все"
                bool showAll = materialSwitchAbsencesTypeShowAll.Checked;

                _absenceTypes = await service.GetAbsenceTypesAsync(showAll);

                DataGridView grid = dataGridViewAbsences;

                grid.DefaultCellStyle.SelectionBackColor = grid.DefaultCellStyle.BackColor;
                grid.DefaultCellStyle.SelectionForeColor = grid.DefaultCellStyle.ForeColor;
                grid.AllowUserToResizeRows = false;
                grid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;



                grid.SuspendLayout();
                grid.Rows.Clear();
                grid.Columns.Clear();

                // Настройка колонок
                grid.Columns.Add("Id", "ID");
                grid.Columns.Add("Number", "№");
                grid.Columns.Add("Name", "Наименование");
                grid.Columns.Add("EndDateReq", "Обязательность даты окончания");
                grid.Columns.Add("Color", "Цвет индикатора");
                grid.Columns.Add("Status", "Статус");
                grid.Columns.Add("Description", "Описание / Примечание");

                grid.Columns["Id"].Visible = false; // Прячем системный ID

                int rowFirst = grid.Rows.Add();

                grid.Rows[rowFirst].Height = 40;

                GridHelper.MergeCells(grid, "№", rowFirst, 1, 1, 1, Color.Gray, Color.White, grid.Font, StringAlignment.Center, StringAlignment.Center);
                GridHelper.MergeCells(grid, "Наименование", rowFirst, 2, 1, 1, Color.Gray, Color.White, grid.Font, StringAlignment.Center, StringAlignment.Center);
                GridHelper.MergeCells(grid, "Обязательность даты окончания", rowFirst, 3, 1, 1, Color.Gray, Color.White, grid.Font, StringAlignment.Center, StringAlignment.Center);
                GridHelper.MergeCells(grid, "Цвет индикатора", rowFirst, 4, 1, 1, Color.Gray, Color.White, grid.Font, StringAlignment.Center, StringAlignment.Center);
                GridHelper.MergeCells(grid, "Статус", rowFirst, 5, 1, 1, Color.Gray, Color.White, grid.Font, StringAlignment.Center, StringAlignment.Center);
                GridHelper.MergeCells(grid, "Описание / Примечание", rowFirst, 6, 1, 1, Color.Gray, Color.White, grid.Font, StringAlignment.Center, StringAlignment.Center);

                grid.Columns[1].Width = 20;
                grid.Columns[2].Width = 220;
                grid.Columns[3].Width = 200;
                grid.Columns[4].Width = 280;
                grid.Columns[5].Width = 280;

                grid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grid.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                grid.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                grid.Columns[6].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                foreach (var item in _absenceTypes)
                {
                    int rowIndex = dataGridViewAbsences.Rows.Add();
                    var row = dataGridViewAbsences.Rows[rowIndex];

                    grid.Rows[rowIndex].Height = 30;

                    GridHelper.MergeCells(grid, rowIndex.ToString(), rowIndex, 1, 1, 1, Color.Gray, Color.White, grid.Font, StringAlignment.Center, StringAlignment.Center);

                    row.Tag = item.Id; // Запоминаем ID в Tag строки
                    row.Cells["Name"].Value = item.Name;
                    row.Cells["EndDateReq"].Value = item.IsEndDateRequired ? "Да" : "Нет";
                    row.Cells["Color"].Value = item.StatusColor;
                    row.Cells["Status"].Value = item.IsActive ? "Активен" : "Деактивирован";
                    row.Cells["Description"].Value = item.Description;

                    // Выполняем раскраску ячейки или текста цветом из базы данных
                    try
                    {
                        Color parseColor = ColorTranslator.FromHtml(item.StatusColor);
                        row.Cells["Color"].Style.BackColor = parseColor;
                        row.Cells["Color"].Style.ForeColor = parseColor.GetBrightness() < 0.5 ? Color.White : Color.Black;
                    }
                    catch { /* Защита от некорректного HEX в БД */ }

                    // Визуально приглушаем деактивированные строки
                    if (!item.IsActive)
                    {
                        row.DefaultCellStyle.ForeColor = Color.Gray;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки справочника: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dataGridViewAbsences.ResumeLayout();
            }
        }
        private async void materialButtonAbsencesTypeAdd_Click(object sender, EventArgs e)
        {
            using (var form = new FormAddAbsenceType())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var param = new ConnectionParameter();
                    var service = new EmployeeManagementService(param.GetMySQLConnectionString());

                    await service.CreateAbsenceTypeAsync(form.ResultModel);
                    await LoadAbsenceTypesGridAsync(); // Обновляем Grid
                }
            }
        }

        private async void materialButtonAbsencesTypeEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewAbsences.CurrentRow == null || dataGridViewAbsences.CurrentRow.Tag == null) return;

            ulong id = (ulong)dataGridViewAbsences.CurrentRow.Tag;
            var selectedItem = _absenceTypes.Find(x => x.Id == id);
            if (selectedItem == null) return;

            using (var form = new FormAddAbsenceType(selectedItem))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var param = new ConnectionParameter();
                    var service = new EmployeeManagementService(param.GetMySQLConnectionString());

                    await service.UpdateAbsenceTypeAsync(form.ResultModel);
                    await LoadAbsenceTypesGridAsync();
                }
            }
        }

        private async void materialButtonAbsencesTypeDeactivate_Click(object sender, EventArgs e)
        {
            if (dataGridViewAbsences.CurrentRow == null || dataGridViewAbsences.CurrentRow.Tag == null)
            {
                MessageBox.Show("Выберите строку для изменения статуса!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ulong id = (ulong)dataGridViewAbsences.CurrentRow.Tag;
            var currentItem = _absenceTypes.FirstOrDefault(x => x.Id == id);

            if (currentItem == null) return;

            var param = new ConnectionParameter();
            var service = new EmployeeManagementService(param.GetMySQLConnectionString());

            // Инвертируем текущее состояние активности
            bool newStatus = !currentItem.IsActive;

            bool success = await service.UpdateAbsenceTypeStatusAsync(id, newStatus);
            if (success)
            {
                await LoadAbsenceTypesGridAsync();
            }
        }

        private async void materialButtonAbsencesTypeDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewAbsences.CurrentRow == null || dataGridViewAbsences.CurrentRow.Tag == null) return;

            ulong id = (ulong)dataGridViewAbsences.CurrentRow.Tag;
            var selectedItem = _absenceTypes.Find(x => x.Id == id);
            if (selectedItem == null) return;

            var param = new ConnectionParameter();
            var service = new EmployeeManagementService(param.GetMySQLConnectionString());

            // 1. Проверяем, используется ли запись в системе
            bool isUsed = await service.IsAbsenceTypeUsedAsync(id);

            if (isUsed)
            {
                // 🔒 Запись используется: блокируем удаление и предлагаем деактивацию
                var result = MessageBox.Show(
                    $"Невозможно удалить причину \"{selectedItem.Name}\", так как она уже закреплена за сотрудниками в архиве расстановок!\n\n" +
                    "Хотите деактивировать её? Она останется в старых отчетах, но исчезнет из списков выбора для новых документов.",
                    "Защита целостности данных",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    await service.UpdateAbsenceTypeStatusAsync(id, false); // Деактивируем
                    await LoadAbsenceTypesGridAsync();
                }
            }
            else
            {
                // Запись чистая: можно безопасно удалить окончательно
                var result = MessageBox.Show(
                    $"Вы уверены, что хотите навсегда удалить причину \"{selectedItem.Name}\"? Эту операцию нельзя будет отменить.",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    await service.DeleteAbsenceTypeAsync(id);
                    await LoadAbsenceTypesGridAsync();
                }
            }
        }

        private async void materialSwitchAbsencesTypeShowAll_CheckedChanged(object sender, EventArgs e)
        {
            await LoadAbsenceTypesGridAsync();
        }

        private void dataGridViewAbsences_SelectionChanged(object sender, EventArgs e)
        {
            // 1. Проверяем, выбрана ли хоть одна строка в таблице
            bool isRowSelected = dataGridViewAbsences.CurrentRow != null && dataGridViewAbsences.CurrentRow.Tag != null;

            // Управляем доступностью кнопок
            materialButtonAbsencesTypeEdit.Enabled = isRowSelected;
            materialButtonAbsencesTypeDelete.Enabled = isRowSelected;
            materialButtonAbsencesTypeDeactivate.Enabled = isRowSelected;

            if (isRowSelected)
            {
                // 2. Получаем ID выбранной строки и находим объект в нашем кэш-списке
                ulong selectedId = (ulong)dataGridViewAbsences.CurrentRow.Tag;
                var selectedItem = _absenceTypes.Find(x => x.Id == selectedId);

                if (selectedItem != null)
                {
                    // 3. 🌟 УМНАЯ СМЕНА НАДПИСИ: Меняем текст кнопки в зависимости от текущего состояния IsActive
                    if (selectedItem.IsActive)
                    {
                        materialButtonAbsencesTypeDeactivate.Text = "Деактивировать";
                        materialButtonAbsencesTypeDeactivate.UseAccentColor = true;
                        // Опционально: можно задать цвет предупреждения, если библиотека поддерживает
                    }
                    else
                    {
                        materialButtonAbsencesTypeDeactivate.Text = "Активировать";
                        materialButtonAbsencesTypeDeactivate.UseAccentColor = false;
                    }
                }
            }
            else
            {
                // Дефолтное состояние, если в таблице пусто
                materialButtonAbsencesTypeDeactivate.Text = "Деактивировать";
                materialButtonAbsencesTypeDeactivate.UseAccentColor = true;
            }
        }

        /*private void dataGridViewAbsences_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex <= 0) return;
            // Игнорируем клики по шапке таблицы (индекс -1)
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var grid = (DataGridView)sender;

            // 1. Если кликнули по первой строке (индекс 0) — запрещаем её выделять
            if (e.RowIndex == 0)
            {
                grid.ClearSelection();
                return;
            }

            *//*// 2. Если кликнули по ячейкам дней (индекс столбца >= 4)
            if (e.ColumnIndex >= 4)
            {
                // Очищаем старое выделение
                grid.ClearSelection();

                // Принудительно выделяем ТОЛЬКО текстовую часть КЛИКНУТОЙ строки (e.RowIndex)
                for (int i = 0; i < 4; i++)
                {
                    grid.Rows[e.RowIndex].Cells[i].Selected = true;
                }
            }*//*

            grid.Invalidate();
        }*/
        private void dataGridViewAbsences_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex <= 0) return;
            // Игнорируем клики по шапке таблицы (индекс -1)
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var grid = (DataGridView)sender;

            // 1. Если кликнули по первой строке (индекс 0) — запрещаем её выделять
            if (e.RowIndex == 0)
            {
                grid.ClearSelection();
                return;
            }

            /*// 2. Если кликнули по ячейкам дней (индекс столбца >= 4)
            if (e.ColumnIndex >= 4)
            {
                // Очищаем старое выделение
                grid.ClearSelection();

                // Принудительно выделяем ТОЛЬКО текстовую часть КЛИКНУТОЙ строки (e.RowIndex)
                for (int i = 0; i < 4; i++)
                {
                    grid.Rows[e.RowIndex].Cells[i].Selected = true;
                }
            }*/

            grid.Invalidate();
        }

        private void dataGridViewAbsences_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex == 0) return;

            var grid = (DataGridView)sender;

            // ПРОВЕРКА №2: Проверяем, является ли текущая рисуемая строка АКТИВНОЙ (выбранной пользователем)
            if (grid.CurrentRow != null && grid.CurrentRow.Index == e.RowIndex)
            {
                // Берем границы строки с небольшим отступом внутрь, чтобы линия не обрезалась
                Rectangle bounds = new Rectangle(
                    e.RowBounds.Left + 0,
                    e.RowBounds.Top + 0,
                    e.RowBounds.Width - 1,
                    e.RowBounds.Height - 1
                );

                // Рисуем рамку ярким цветом для теста
                using (Pen pen = new Pen(Color.Orange, 2))
                {
                    e.Graphics.DrawRectangle(pen, bounds);
                }
            }
        }

        private void dataGridViewAbsences_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Игнорируем шапку таблицы и пустые строки
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // Находим ячейки, у которых задан кастомный цвет (например, колонка "Цвет индикатора" или дни в плане)
            // Укажите здесь индекс или имя вашей цветной колонки (например, "Color" или индекс >= 4 для дней)
            if (e.ColumnIndex == 3 || e.ColumnIndex >= 4)
            {
                // 🌟 ГЛАВНЫЙ СЕКРЕТ: Если ячейка выделена, мы принудительно заставляем WinForms
                // использовать её оригинальный BackColor вместо системного SelectionBackColor!
                if ((e.State & DataGridViewElementStates.Selected) != 0)
                {
                    // Задаем цвет фона выделения равным её собственному цвету ячейки
                    e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;

                    // Цвет текста выделения делаем контрастным (черным)
                    e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
                }
            }
        }

        private async void dataGridViewAbsences_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 1)
            {
                var grid = (DataGridView)sender;

                if (dataGridViewAbsences.CurrentRow == null || dataGridViewAbsences.CurrentRow.Tag == null) return;

                ulong id = (ulong)grid.CurrentRow.Tag;
                var selectedItem = _absenceTypes.Find(x => x.Id == id);

                if (selectedItem == null) return;

                using (var form = new FormAddAbsenceType(selectedItem))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        var param = new ConnectionParameter();
                        var service = new EmployeeManagementService(param.GetMySQLConnectionString());

                        await service.UpdateAbsenceTypeAsync(form.ResultModel);
                        await LoadAbsenceTypesGridAsync();
                    }
                }


                /*// 2. Определяем целевую строку и колонку для сбора данных.
                // Если кликнули по объединенному блоку, данные привязаны к его "мастер-ячейке" (началу блока).
                int targetRow = e.RowIndex;
                int targetCol = e.ColumnIndex;

                if (grid.Rows[e.RowIndex].Cells[e.ColumnIndex] is BiMergedCell clickedCell)
                {
                    targetRow = clickedCell.TopRow;
                    targetCol = clickedCell.LeftColumn;
                }

                // Получаем содержимое столбца с индексом 1 для этой строки (Номер смены)
                object rowName = grid.Rows[targetRow].Tag;
                int id = rowName != null ? Convert.ToInt32(rowName) : -1;

                FormAddAbsence form = new FormAddAbsence(1, id);
                DialogResult = form.ShowDialog();

                if (DialogResult == DialogResult.OK)
                {
                    DateChange();
                }*/
            }
        }

        
    }
}
