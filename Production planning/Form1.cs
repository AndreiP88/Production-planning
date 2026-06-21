using data;
using data.Models;
using database;
using DevAge.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            materialSkinManager.ColorScheme = new ColorScheme((Primary)0x00796b, (Primary)0x009688, (Primary)0x009685, (Accent)0x009686, TextShade.WHITE);
        }

        CancellationTokenSource cancelTokenSource;

        List<WorkAreaInfo> workAreaInfo;
        List<ShiftDefinitionModel> shiftDefinitions;
        List<ScheduleCycleModel> scheduleCycles;
        List<ScheduleTemplateModel> scheduleTemplates;
        List<EmployeeShortRow> employeeShorts;

        private void Form1_Load(object sender, EventArgs e)
        {
            // Текст выделенной ячейки будет такого же цвета, как и у обычной ячейки
            dataGridPlanning.DefaultCellStyle.SelectionForeColor = dataGridPlanning.DefaultCellStyle.ForeColor;

            DateTime date = DateTime.Now;

            int year = 2025;

            while (year <= date.Year)
            {
                planComboBoxYear.Items.Add(year);

                year++;
            }

            planComboBoxYear.SelectedIndex = planComboBoxYear.Items.Count - 1;
            planComboBoxMonth.SelectedIndex = DateTime.Now.Month - 1;

            materialTabControl1.SelectedIndex = 0;
            materialTabControl1_SelectedIndexChangedAsync(sender, e);
        }

        



        private async Task ViewPlanForEquipAsync()
        {
            CreateColomnsToDataGridForSearchedOrder();

            if (workPlanEquipComboBox.SelectedIndex >= 0)
            {
                int equip = Convert.ToInt32(workAreaInfo[workPlanAreaComboBox.SelectedIndex].Equipments[workPlanEquipComboBox.SelectedIndex].Code);

                StartLoading(equip, true);
            }
        }

        private async void materialButton1_Click(object sender, EventArgs e)
        {
            await ViewPlanForEquipAsync();
        }

        private void ClearColomnsFromDataGridForSearchedOrder()
        {
            dataGridViewPlan.Rows.Clear();
            dataGridViewPlan.Columns.Clear();
        }

        private void CreateColomnsToDataGridForSearchedOrder()
        {
            ClearColomnsFromDataGridForSearchedOrder();

            dataGridViewPlan.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //dataGridViewOneShift.AllowUserToResizeColumns = false;
            dataGridViewPlan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewPlan.AllowUserToResizeRows = false;
            dataGridViewPlan.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            string[] colNames = { "№", "Начало", "Завершение", "Заказ", "Заказчик", "Описание", "Приладка", "Работа", "Тираж", "Статус" };
            int[] colWidth = { 30, 200, 200, 100, 180, 180, 100, 100, 100, 100 };
            DataGridViewContentAlignment[] colAligment = { DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleLeft,
                DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleCenter,
                DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleLeft };

            for (int i = 0; i < colNames.Length; i++)
            {
                int indexCol = dataGridViewPlan.Columns.Add(colNames[i], colNames[i]);
                dataGridViewPlan.Columns[indexCol].Width = colWidth[i];
                dataGridViewPlan.Columns[indexCol].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridViewPlan.Columns[indexCol].DefaultCellStyle.Alignment = colAligment[i];
            }

            //dataGridViewPlan.Columns[0].Frozen = true;
            //dataGridViewPlan.Columns[1].Frozen = true;

            dataGridViewPlan.Rows.Add();
            dataGridViewPlan.Rows[0].Height = 30;
            dataGridViewPlan.Rows[0].Frozen = true;

            for (int i = 0; i < colNames.Length; i++)
            {
                AddCellToGrid(dataGridViewPlan, 0, i);
                dataGridViewPlan.Rows[0].Cells[i].Value = colNames[i];
            }
        }

        private void AddCellToGrid(DataGridView dataGrid, int indexRow, int indexCell, int collSpan = 1)
        {
            HMergedCell pCell;

            //int nOffset = indexCell;

            for (int j = indexCell; j < indexCell + collSpan; j++)
            {
                dataGrid.Rows[indexRow].Cells[j] = new HMergedCell();
                pCell = (HMergedCell)dataGrid.Rows[indexRow].Cells[j];
                pCell.LeftColumn = indexCell;
                pCell.RightColumn = indexCell + collSpan - 1;
            }
            //nOffset += collSpan + 1;
        }

        private void StartLoading(int idMachine, bool loadAllOrders)
        {
            cancelTokenSource?.Cancel();

            cancelTokenSource = new CancellationTokenSource();

            Task task = new Task(() => LoadPlan(cancelTokenSource.Token, idMachine, loadAllOrders), cancelTokenSource.Token);
            task.Start();

            //LoadPlan(cancelTokenSource.Token, idMachine, loadAllOrders);
        }

        private void LoadPlan(CancellationToken token, int idMachine, bool loadAllOrders)
        {
            ValueOrders valueOrders = new ValueOrders();

            List<OrdersLoad> orders = valueOrders.GetPlan(idMachine, token);

            int indexRow;

            try
            {
                for (int i = 0; i < orders.Count; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    Invoke(new Action(() =>
                    {
                        indexRow = dataGridViewPlan.Rows.Add();
                        
                        dataGridViewPlan.Rows[indexRow].Cells[0].Value = i.ToString("D2");
                        dataGridViewPlan.Rows[indexRow].Cells[1].Value = orders[i].TimeStartOrder;
                        dataGridViewPlan.Rows[indexRow].Cells[2].Value = orders[i].TimeEndOrder;
                        dataGridViewPlan.Rows[indexRow].Cells[3].Value = orders[i].numberOfOrder;
                        dataGridViewPlan.Rows[indexRow].Cells[4].Value = orders[i].nameCustomer;
                        dataGridViewPlan.Rows[indexRow].Cells[5].Value = orders[i].nameItem;
                        dataGridViewPlan.Rows[indexRow].Cells[6].Value = orders[i].makereadyTime;
                        dataGridViewPlan.Rows[indexRow].Cells[7].Value = orders[i].workTime;
                        dataGridViewPlan.Rows[indexRow].Cells[8].Value = orders[i].amountOfOrder.ToString("N0");
                        dataGridViewPlan.Rows[indexRow].Cells[9].Value = orders[i].stamp;
                    }));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                //Logger.WriteLine(ex.Message);
            }
        }

        private string PrepareCellText(ShiftInfo s)
        {
            var sb = new StringBuilder();

            // 1 строка: План (с причинами отсутствия, если есть в данных)
            if (s.PlannedStaff.Any())
                sb.AppendLine(string.Join(", ", s.PlannedStaff));

            // 2 строка: На согласовании (Черновики)
            if (s.Drafts.Any())
                sb.AppendLine(/*"📝 " + */string.Join(", ", s.Drafts));

            // 3 строка: Утвержденные (Факт)
            if (s.Assignments.Any())
                sb.Append(/*"✅ " + */string.Join(", ", s.Assignments));

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
            BuildPivotGrid(startDate, idArea);
        }

        public async void BuildPivotGrid(DateTime monthStart, int areaId)
        {
            ConnectionParameter parameter = new ConnectionParameter();

            var service = new ReportStaffing(parameter.GetMySQLConnectionString());

            DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);

            var reportData = await service.GetStaffingReportAsync(monthStart, monthEnd, areaId);

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

            GridHelper.MergeCells(dataGridPlanning, "Оборудование", 0, 0, 2, 1, Color.Gray);
            GridHelper.MergeCells(dataGridPlanning, "Смена", 0, 1, 2, 1, Color.Gray);

            dataGridPlanning.Rows[columnFirst].Cells[0].Value = "Оборудование";
            dataGridPlanning.Rows[columnSecond].Cells[1].Value = "Смена";

            // 2. Создаем динамические столбцы для дней месяца (1, 2, 3...)
            for (int d = 1; d <= monthEnd.Day; d++)
            {
                DateTime headerDate = new DateTime(monthStart.Year, monthStart.Month, d);
                string headerText = headerDate.ToString("dd.MM.yyyy");

                var col = new DataGridViewTextBoxColumn();
                col.Name = $"day_{d}";
                col.HeaderText = headerText;
                col.Width = 220;
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
                int currentShiftNumber = 1;

                int firstRow = 0;

                foreach (var sName in shiftNames)
                {
                    // Строка 1: Инфо о сотрудниках
                    int r1 = dataGridPlanning.Rows.Add(eq.Key.Id);
                    dataGridPlanning.Rows[r1].HeaderCell.Value = eq.Key.Id;

                    // Строка 2: Пустая (для будущего)
                    int r2 = dataGridPlanning.Rows.Add(eq.Key.Id);
                    dataGridPlanning.Rows[r2].HeaderCell.Value = eq.Key.Id;

                    dataGridPlanning.Rows[r2].MinimumHeight = 60;

                    if (currentShiftNumber == 1)
                    {
                        firstRow = r1;
                    }

                    if (shiftNames.Count == currentShiftNumber)
                    {
                        GridHelper.MergeCells(dataGridPlanning, eq.Key.Name, firstRow, 0, shiftNames.Count * 2, 1, Color.Gray);
                    }

                    GridHelper.MergeCells(dataGridPlanning, sName.ToString(), r1, 1, 2, 1, Color.Gray);

                    //dataGridPlanning.Rows[r1].Cells["Equip"].Value = eq.Key.Name;
                    dataGridPlanning.Rows[r1].Cells["Shift"].Value = sName;
                    dataGridPlanning.Rows[r1].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);

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
                            dataGridPlanning.Rows[r1].Cells[$"day_{dayNum}"].Value = PrepareCellText(currentShift);
                        }
                    }

                    currentShiftNumber++;
                }
            }

            // Автоматическая высота строк под контент
            dataGridPlanning.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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

            this.Text = "Производственный план - " + namePage + " (" + index + ")";

            await UpdateWorkAreasAsync();

            if (index == 0)
            {
                planComboBoxAreas.Items.Clear();

                dataGridPlanning.Rows.Clear();
                dataGridPlanning.Columns.Clear();

                for (int i = 0; i < workAreaInfo.Count; i++)
                {
                    planComboBoxAreas.Items.Add(workAreaInfo[i].Name);
                }
            }

            if (index == 1)
            {
                workPlanAreaComboBox.Items.Clear();
                workPlanEquipComboBox.Items.Clear();

                dataGridViewPlan.Rows.Clear();
                dataGridViewPlan.Columns.Clear();

                for (int i = 0; i < workAreaInfo.Count; i++)
                {
                    workPlanAreaComboBox.Items.Add(workAreaInfo[i].Name);
                }
            }

            if (index == 3)
            {
                await UpdateUsersShortInfo();
                await LoadUserListToListBox();
            }

            if (index == 4)
            {
                await LoadAreaListToListBox();
            }

            if (index == 5)
            {
                await UpdateShiftsDefinitionAsync();
                await UpdateScheduleCycle();
                await UpdateScheduleTemplate();
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
                using (Pen strokePen = new Pen(Color.DarkOrange, 2)) // Синяя рамка толщиной 2 пикселя
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
            workPlanEquipComboBox.Items.Clear();

            int areaID = workPlanAreaComboBox.SelectedIndex;

            for (int i = 0; i < workAreaInfo[areaID].Equipments.Count; i++)
            {
                workPlanEquipComboBox.Items.Add(workAreaInfo[areaID].Equipments[i].Name);
            }
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

        private void dataGridPlanning_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 1. Проверяем, что клик был в рабочей области:
            // Игнорируем шапку (e.RowIndex < 0 или e.ColumnIndex < 0)
            // Игнорируем первые 2 верхние строки (0 и 1)
            // Игнорируем первые 2 левых столбца (0 и 1)
            if (e.RowIndex >= 2 && e.ColumnIndex >= 2)
            {
                // 2. Определяем целевую строку и колонку для сбора данных.
                // Если кликнули по объединенному блоку, данные привязаны к его "мастер-ячейке" (началу блока).
                int targetRow = e.RowIndex;
                int targetCol = e.ColumnIndex;

                if (dataGridPlanning.Rows[e.RowIndex].Cells[e.ColumnIndex] is BiMergedCell clickedCell)
                {
                    targetRow = clickedCell.TopRow;
                    targetCol = clickedCell.LeftColumn;
                }

                // 3. Получаем название столбца (Дата), по которому кликнули
                string columnNameDate = dataGridPlanning.Columns[targetCol].HeaderText;

                // 4. Получаем содержимое столбца с индексом 1 для этой строки (Номер смены)
                object shiftValue = dataGridPlanning.Rows[targetRow].Cells[1].Value;
                int shiftNumber = shiftValue != null ? Convert.ToInt32(shiftValue) : 0;

                // 5. Получаем название строки (Код станка). 
                // Обычно код станка находится в самом первом столбце (индекс 0). 
                // Замените индекс 0 на ваш индекс столбца, если код станка лежит в другом месте.
                object machineValue = dataGridPlanning.Rows[targetRow].HeaderCell.Value;
                int machineID = machineValue != null ? Convert.ToInt32(machineValue) : 0;

                // 6. Получаем текст самого объединенного блока (задачи/плана)
                object blockValue = dataGridPlanning.Rows[targetRow].Cells[targetCol].Value;
                string taskText = blockValue != null ? blockValue.ToString() : string.Empty;

                EditShiftForm form = new EditShiftForm(Convert.ToDateTime(columnNameDate), shiftNumber, machineID);
                form.ShowDialog();

                // --- ВАШ КОД ОБРАБОТКИ ---
                // Теперь у вас есть все переменные. Вы можете передать их в новую форму или обработать.
                // Пример вывода собранных данных:
                string message = $"Клик по блоку: \"{taskText}\"\n\n" +
                                 $"📅 Дата (Столбец): {columnNameDate}\n" +
                                 $"🔄 Смена (Столбец [1]): {shiftNumber}\n" +
                                 $"⚙️ Код станка (Строка): {machineID}";

                //MessageBox.Show(message, "Данные планирования", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            workAreaInfo?.Clear();

            ConnectionParameter parameter = new ConnectionParameter();

            var service = new WorkAreaService(parameter.GetMySQLConnectionString());

            workAreaInfo = await service.GetWorkAreasWithEquipmentAsync();
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

            buttonEquipAdd.Enabled = false;
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
                    viewItem.SubItems.Add(shortInfo.StaffingMode.ToString());
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

        private async Task LoadUserListToListBox()
        {
            listBoxUsers.Items.Clear();
            //MaterialListBoxItem item = new MaterialListBoxItem();

            for (int i = 0; i < employeeShorts?.Count; i++)
            {
                MaterialListBoxItem item = new MaterialListBoxItem
                {
                    Text = employeeShorts[i].FullName
                };

                listBoxUsers.Items.Add(item);
            }

            listBoxUsers.SelectedIndex = -1;
            listBoxUsers.Refresh();
            ClearUserShortInfo();
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
                int index = listBoxUsers.SelectedIndex;

                textBoxUserLastName.Text = employeeShorts[index].LastName;
                textBoxUserFirstName.Text = employeeShorts[index].FirstName;
                textBoxUserPatronymic.Text = employeeShorts[index].Patronymic;
                textBoxUserContactPhone.Text = employeeShorts[index].PrimaryPhone;

                textBoxUserStatus.Text = employeeShorts[index].CurrentStatus;
                textBoxUserPosition.Text = employeeShorts[index].CurrentPosition;
                textBoxUserAssigmentArea.Text = employeeShorts[index].CurrentWorkArea;
                textBoxUserAssigmentEquip.Text = employeeShorts[index].CurrentEquipment;
                textBoxUserSchedule.Text = employeeShorts[index].CurrentSchedule;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения краткой информации сотрудника:\n {ex}");
            }
            


        }

        private void materialButtonUserViewFullCard_Click(object sender, EventArgs e)
        {
            try
            {
                int index = listBoxUsers.SelectedIndex;

                FormAddUser form = new FormAddUser((int)employeeShorts[index].Id);
                DialogResult = form.ShowDialog();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения полной информации сотрудника:\n {ex}");
            }
        }

        private void materialButtonUserAdd_Click(object sender, EventArgs e)
        {
            try
            {
                FormAddUser form = new FormAddUser();
                DialogResult = form.ShowDialog();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения полной информации сотрудника:\n {ex}");
            }
        }

        ///
    }
}
