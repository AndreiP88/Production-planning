using data;
using database;
using MaterialSkin.Controls;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Production_planning
{
    public partial class EditShiftForm : MaterialForm
    {
        ConnectionParameter parameter = new ConnectionParameter();
        private DateTime _date;
        private int _shiftNumber;
        private ulong _shiftID;
        private ulong _equipID;

        EquipmentLookupDto _equipment;
        EquipmentShiftCard _shiftCard;

        private EquipmentShiftState _currentEquipmentState;

        public EditShiftForm(DateTime date, int shiftNumber, ulong equipID)
        {
            InitializeComponent();

            _date = date;
            _shiftNumber = shiftNumber;
            _equipID = equipID;
        }

        private async void EditShiftForm_LoadAsync(object sender, EventArgs e)
        {
            //ConnectionParameter parameter = new ConnectionParameter();

            try
            {
                var workAreaShift = new WorkAreaService(parameter.GetMySQLConnectionString());
                _equipment = await workAreaShift.GetEquipmentLookupByIdAsync((ulong)_equipID);

                this.Text = _equipment.DisplayText;
            }
            catch
            {

            }

            await RefreshCardDataAsync();
        }

        private async Task RefreshCardDataAsync()
        {
            //ConnectionParameter parameter = new ConnectionParameter();

            try
            {
                var report = new ReportEquipShift(parameter.GetMySQLConnectionString());

                _shiftCard = await report.GetEquipmentShiftCardAsync(_date, _shiftNumber, _equipID);

                _shiftID = _shiftCard.ShiftID;

                FillGeneralShiftInfo(_shiftCard);
                FillEmployeesTables(_shiftCard);

                /*this.Text = _equipment.DisplayText;

                labelShiftDate.Text = _date.ToString("D") + ". " + _shiftCard.ShiftName + " (" + _shiftCard.TimeStart + " - " + _shiftCard.TimeEnd + ")";
                //labelShiftName.Text = _shiftCard.ShiftName + " (" + _shiftCard.TimeStart + " - " + _shiftCard.TimeEnd + ")";

                labelStatus.Text = _shiftCard.StaffingRequirement;

                FillPlannedEmployeesGrid(gridViewUserSchedule, _shiftCard.PlannedStaff);*/
            }
            catch
            {

            }
        }

        public void FillGeneralShiftInfo(EquipmentShiftCard card)
        {
            if (card == null) return;

            // 1. Заполняем текстовые заголовки на форме
            labelShiftDate.Text = $"{_date:D}, {card.ShiftName} ({card.TimeStart} — {card.TimeEnd})";

            // 2. Управляем кнопками самого оборудования (Остановка / Запуск станка)
            /*buttonEquipCancel.Enabled = !card.IsEquipmentCancelled; // Активна, если станок РАБОТАЕТ
            buttonEquipCancel.Enabled = card.IsEquipmentCancelled; // Активна, если станок ОСТАНОВЛЕН*/

            // Включаем кнопку ручного добавления сотрудников по умолчанию
            btnAssignEmployee.Enabled = true;
            lblStaffingRequirement.Text = card.StaffingRequirement;

            _currentEquipmentState = (EquipmentShiftState)card.StaffingRequirementCode;
            StaffingRequirementType requirement = (StaffingRequirementType)card.StaffingRequirementCode;

            // ВЫСТАВЛЯЕМ ИНТЕРФЕЙС СТРОГО ПО КОДУ СОСТОЯНИЯ ИЗ БД
            switch (_currentEquipmentState)
            {
                case EquipmentShiftState.CancelledManually: // Код 0
                    lblEquipmentState.Text = "ℹ️ ПРИНУДИТЕЛЬНО ОСТАНОВЛЕНО";
                    lblEquipmentState.ForeColor = Color.Red;
                    lblStaffingRequirement.ForeColor = Color.Red;

                    btnManageEquipment.Text = "🔄 Вернуть в график";
                    btnManageEquipment.UseAccentColor = false;
                    btnAssignEmployee.Enabled = false;
                    break;

                case EquipmentShiftState.ManualActive: // Код 1
                    lblEquipmentState.Text = "ℹ️ РУЧНАЯ СМЕНА АКТИВИРОВАНА";
                    lblEquipmentState.ForeColor = Color.ForestGreen;
                    //lblStaffingRequirement.ForeColor = card.StaffingRequirement.Contains("✅") ? Color.ForestGreen : Color.Red;

                    btnManageEquipment.Text = "🔄 Отменить смену"; // Сразу предлагает СТЕРЕТЬ запись (DELETE)
                    btnManageEquipment.UseAccentColor = false;
                    break;

                case EquipmentShiftState.ActiveByPlan: // Код 2
                                                       // Проверяем: если есть EdpId, значит это внеплановый запуск для автосмен
                                                       // Если смена создана вручную (есть EdpId), но по штатному графику шаблона у станка ВЫХОДНОЙ
                    if (card.EdpId.HasValue && !card.IsWorkingByPlan)
                    {
                        // Переключаем в состояние удаления, чтобы по клику сразу выполнился DELETE!
                        _currentEquipmentState = EquipmentShiftState.ManualActive;

                        lblEquipmentState.Text = "ℹ️ СМЕНА АКТИВИРОВАНА ВНЕ ПЛАНА";
                        btnManageEquipment.Text = "🔄 Отменить внеплановую смену"; // Текст кнопки
                        btnManageEquipment.UseAccentColor = false; // Обычный цвет возврата
                    }
                    // Если смена запущена вне плана для станка, у которого по графику сейчас ДОЛЖНА БЫТЬ работа
                    else if (card.EdpId.HasValue && card.IsWorkingByPlan)
                    {
                        _currentEquipmentState = EquipmentShiftState.ActiveByPlan;
                        lblEquipmentState.Text = "ℹ️ СМЕНА ВОССТАНОВЛЕНА ВНЕ ПЛАНА";
                        btnManageEquipment.Text = "🛑 Снять (отменить) смену";
                        btnManageEquipment.UseAccentColor = true;
                    }
                    // Обычная штатная работа по циклическому графику шаблона
                    else
                    {
                        _currentEquipmentState = EquipmentShiftState.ActiveByPlan;
                        lblEquipmentState.Text = "ℹ️ РАБОТАЕТ ПО ШТАТНОМУ ГРАФИКУ";
                        btnManageEquipment.Text = "🛑 Отменить работу";
                        btnManageEquipment.UseAccentColor = true;
                    }

                    lblEquipmentState.ForeColor = Color.ForestGreen;
                    break;

                case EquipmentShiftState.IdleByPlan: // Код 3
                    lblEquipmentState.Text = "ℹ️ ПЛАНОВЫЙ ПРОСТОЙ (Выходной)";
                    lblEquipmentState.ForeColor = Color.DimGray;
                    lblStaffingRequirement.ForeColor = Color.DimGray;

                    btnManageEquipment.Text = "🟢 Активировать работу (Вне плана)";
                    btnManageEquipment.UseAccentColor = true;
                    btnAssignEmployee.Enabled = false;
                    break;

                case EquipmentShiftState.ManualWaiting: // Код 4
                    lblEquipmentState.Text = "ℹ️ РУЧНОЕ НАЗНАЧЕНИЕ (Смена не активна)";
                    lblEquipmentState.ForeColor = Color.DarkOrange;
                    lblStaffingRequirement.ForeColor = Color.DimGray;

                    btnManageEquipment.Text = "🟢 Назначить (активировать) смену";
                    btnManageEquipment.UseAccentColor = true;
                    btnAssignEmployee.Enabled = false;
                    break;
            }

            switch (requirement)
            {
                case StaffingRequirementType.EquipmentCancelled: // Код 0 (Станок принудительно остановлен)
                    lblStaffingRequirement.ForeColor = Color.Firebrick; // Темно-красный
                    break;

                // ИСПРАВЛЕНО: Заменили несуществующий ManualActive на правильный Staffed (Код 1)
                case StaffingRequirementType.Staffed:     // Код 1 (Ручная смена запущена)
                case StaffingRequirementType.StaffNeeded: // Код 2 (Станок работает по автоплану)

                    // Наша умная проверка по количеству фактически присутствующих сотрудников [2026-09-11]
                    if (card.ActiveStaffCount > 0)
                    {
                        lblStaffingRequirement.ForeColor = Color.ForestGreen; // Спокойный зеленый
                    }
                    else
                    {
                        lblStaffingRequirement.ForeColor = Color.Red; // Яркий красный alert!
                    }
                    break;

                case StaffingRequirementType.IdleBySchedule: // Код 3 (Законный выходной станка)
                case StaffingRequirementType.ManualWaiting:  // Код 4 (Ручной станок ждет активации)
                    lblStaffingRequirement.ForeColor = Color.DimGray; // Серый цвет
                    break;
            }
        }

        /// <summary>
        /// Шаг 2: Инициализация и наполнение списков персонала (План по графику и Назначения/Замены)
        /// </summary>
        public void FillEmployeesTables(EquipmentShiftCard card)
        {
            gridViewUserSchedule.SelectionChanged -= gridViewUserSchedule_SelectionChanged;
            gridViewUserOverride.SelectionChanged -= gridViewUserOverride_SelectionChanged;

            // Очищаем строки и колонки обеих таблиц, чтобы избежать наложений при обновлении
            gridViewUserSchedule.Columns.Clear();
            gridViewUserSchedule.Rows.Clear();

            gridViewUserOverride.Columns.Clear();
            gridViewUserOverride.Rows.Clear();

            if (card == null) return;

            // Настраиваем структуру колонок для двух DataGridView (вспомогательный метод ниже)
            ConfigureGridStyle(gridViewUserSchedule, "Персонал по графику", "Статус плана");
            ConfigureGridStyle(gridViewUserOverride, "Фактически в смене", "Текущий статус");

            // -----------------------------------------------------------------
            // 1. НАПОЛНЯЕМ ТАБЛИЦУ ПЛАНА (gridPlan)
            // -----------------------------------------------------------------
            if (card.PlannedStaff != null && card.PlannedStaff.Count > 0)
            {
                foreach (var employee in card.PlannedStaff)
                {
                    string finalStatusText = employee.PlanStatus;

                    // Приводим числовой код из базы к нашему строгому Enum [2026-09-09]
                    PlannedEmployeeStatusType statusType = (PlannedEmployeeStatusType)employee.PlanStatusCode;

                    // 1. ЦИФРОВОЕ ПРАВИЛО: Если станок остановлен, а человек здоров (Active) [2026-09-09]
                    if (card.IsEquipmentCancelled && statusType == PlannedEmployeeStatusType.Active)
                    {
                        finalStatusText = "⚠️ Без рабочего места";
                    }

                    // 2. Добавляем строку в верхнюю таблицу плана [2026-09-01]
                    int rowIndex = gridViewUserSchedule.Rows.Add(employee.EmployeeName, finalStatusText);
                    gridViewUserSchedule.Rows[rowIndex].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);

                    // Упаковываем комбинированный тег (ID_сотрудника;ID_оверрайда) [2026-09-04]
                    ulong overrideId = employee.PlanOverrideId ?? 0;
                    gridViewUserSchedule.Rows[rowIndex].Tag = $"{employee.EmployeeId};{overrideId}";

                    // 3. СТРОГАЯ ЦИФРОВАЯ РАСКРАСКА ЦВЕТОВ (БЕЗ ПАРСИНГА СТРОК) [2026-09-09]
                    var statusCell = gridViewUserSchedule.Rows[rowIndex].Cells[1]; // Вторая колонка — Статус

                    // Если сработал алерт остановки станка для здорового человека
                    if (card.IsEquipmentCancelled && statusType == PlannedEmployeeStatusType.Active)
                    {
                        statusCell.Style.ForeColor = Color.DarkOrange;
                        statusCell.Style.Font = new Font(gridViewUserSchedule.Font, FontStyle.Bold);
                    }
                    else
                    {
                        // Раскрашиваем строго по системному Enum [2026-09-09]
                        switch (statusType)
                        {
                            case PlannedEmployeeStatusType.Active: // Код 0
                                statusCell.Style.ForeColor = Color.ForestGreen; // ✅ В графике
                                break;

                            case PlannedEmployeeStatusType.Absence: // Код 1
                                statusCell.Style.ForeColor = Color.Red; // ❌ Больничный / Отпуск
                                statusCell.Style.Font = new Font(gridViewUserSchedule.Font, FontStyle.Bold);
                                break;

                            case PlannedEmployeeStatusType.Cancelled: // Код 2
                                statusCell.Style.ForeColor = Color.DarkOrange; // 🚫 Отмена
                                break;

                            case PlannedEmployeeStatusType.Transferred: // Код 3
                                statusCell.Style.ForeColor = Color.DodgerBlue; // ➡️ Переведен
                                break;
                        }
                    }
                }
            }
            else
            {
                gridViewUserSchedule.Rows.Add("По графику никто не запланирован", "---");
            }

            // -----------------------------------------------------------------
            // 2. НАПОЛНЯЕМ ТАБЛИЦУ ФАКТИЧЕСКОГО ПРИСУТСТВИЯ (gridFact)
            // -----------------------------------------------------------------

            // Вариант А: Выводим утвержденные ручные замены (Approved)
            if (card.ApprovedStaff != null && card.ApprovedStaff.Count > 0)
            {
                foreach (var approved in card.ApprovedStaff)
                {
                    // Добавляем строку (ФИО, статус по умолчанию)
                    int rowIndex = gridViewUserOverride.Rows.Add(approved.EmployeeName, "Замена (Утверждено)");
                    gridViewUserOverride.Rows[rowIndex].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                    gridViewUserOverride.Rows[rowIndex].Tag = approved.OverrideId; // Храним ID оверрайда для удаления

                    // 🌟 НАША НОВАЯ ПРОВЕРКА: Если станок остановили, а ручная замена уже была назначена
                    if (card.IsEquipmentCancelled)
                    {
                        // Точно так же бьем тревогу и красим ячейку в глубокий оранжевый цвет!
                        gridViewUserOverride.Rows[rowIndex].Cells[1].Value = "⚠️ Без рабочего места";
                        gridViewUserOverride.Rows[rowIndex].Cells[1].Style.ForeColor = Color.DarkOrange;
                        gridViewUserOverride.Rows[rowIndex].Cells[1].Style.Font = new Font(gridViewUserOverride.Font, FontStyle.Bold);
                    }
                    else
                    {
                        // Если станок работает штатно — замена горит стандартным зеленым цветом
                        gridViewUserOverride.Rows[rowIndex].Cells[1].Style.ForeColor = Color.ForestGreen;
                    }
                }
            }

            /*// Вариант Б: Выводим плановых сотрудников, которые вышли и имеют итоговый статус "Работает"
            if (card.PlannedStaff != null && card.PlannedStaff.Count > 0)
            {
                foreach (var employee in card.PlannedStaff)
                {
                    if (employee.FinalFactStatus == "✅ РАБОТАЕТ")
                    {
                        int rowIndex = gridViewUserOverride.Rows.Add(employee.EmployeeName, "По графику");
                        gridViewUserOverride.Rows[rowIndex].Tag = employee.EmployeeId;

                        // БИЗНЕС-ЛОГИКА: Если станок остановили, а человек пришел (остался без рабочего места)
                        if (card.IsEquipmentCancelled)
                        {
                            gridViewUserOverride.Rows[rowIndex].Cells[1].Value = "⚠️ Без рабочего места (Станок стопорится)";
                            gridViewUserOverride.Rows[rowIndex].Cells[1].Style.ForeColor = Color.DarkOrange;
                            gridViewUserOverride.Rows[rowIndex].Cells[1].Style.Font = new Font(gridViewUserOverride.Font, FontStyle.Bold);
                        }
                        else
                        {
                            gridViewUserOverride.Rows[rowIndex].Cells[1].Style.ForeColor = Color.ForestGreen;
                        }
                    }
                }
            }*/

            // Вариант В: Добавляем в конец таблицы факта черновики назначений
            if (card.DraftStaff != null && card.DraftStaff.Count > 0)
            {
                foreach (var draft in card.DraftStaff)
                {
                    int rowIndex = gridViewUserOverride.Rows.Add(draft.EmployeeName, "📝 Черновик назначения");
                    gridViewUserOverride.Rows[rowIndex].Tag = draft.OverrideId; // Храним ID черновика для утверждения/удаления
                    gridViewUserOverride.Rows[rowIndex].Cells[1].Style.ForeColor = Color.Orange;
                }
            }

            // Защитная заглушка, если таблица факта осталась пустой
            if (gridViewUserOverride.Rows.Count == 0)
            {
                gridViewUserOverride.Rows.Add("Нет назначенных сотрудников в смене", "---");
            }

            gridViewUserSchedule.ClearSelection();
            gridViewUserOverride.ClearSelection();

            gridViewUserSchedule.SelectionChanged += gridViewUserSchedule_SelectionChanged;
            gridViewUserOverride.SelectionChanged += gridViewUserOverride_SelectionChanged;
        }

        /// <summary>
        /// Вспомогательный метод базовой настройки внешнего вида DataGridView
        /// </summary>
        private void ConfigureGridStyle(DataGridView grid, string col1Text, string col2Text)
        {
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.AllowUserToAddRows = false;

            int c1 = grid.Columns.Add("Col1", col1Text);
            int c2 = grid.Columns.Add("Col2", col2Text);

            grid.Columns[c1].Width = 160;
            grid.Columns[c2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        /// <summary>
        /// Вспомогательный метод раскраски текстовых статусов планового персонала
        /// </summary>
        private void ColorizeStatusCell(DataGridViewCellCollection cells, string status)
        {
            if (string.IsNullOrEmpty(status) || cells.Count < 2) return;

            var cell = cells[1]; // Вторая колонка — Статус

            if (status.Contains("❌") || status.Contains("Больничный"))
            {
                cell.Style.ForeColor = Color.Red;
                cell.Style.Font = new Font(cell.InheritedStyle.Font, FontStyle.Bold);
            }
            else if (status.Contains("🚫") || status.Contains("Отмена"))
            {
                // ИСПРАВЛЕНО: Применяем корректный цвет системы
                cell.Style.ForeColor = Color.OrangeRed;
            }
            else if (status.Contains("➡️") || status.Contains("Переведен"))
            {
                cell.Style.ForeColor = Color.DodgerBlue;
            }
            else
            {
                cell.Style.ForeColor = Color.ForestGreen;
            }
        }

        private void gridViewUserSchedule_SelectionChanged(object sender, EventArgs e)
        {
            // 1. ЗАЩИТА: Если строка не выбрана или это пустая заглушка ("Никто не запланирован")
            if (gridViewUserSchedule.CurrentRow == null || gridViewUserSchedule.CurrentRow.Tag == null || gridViewUserSchedule.CurrentRow.Index < 0)
            {
                buttonScheduleShiftCancel.Enabled = false;
                buttonScheduleEquipReplace.Enabled = false;
                buttonScheduleReturn.Enabled = false;
                return;
            }

            // 2. РАЗБИРАЕМ КОМБИНИРОВАННЫЙ ТЕГ СТРОКИ
            string[] idParts = gridViewUserSchedule.CurrentRow.Tag.ToString().Split(';');
            ulong employeeId = ulong.Parse(idParts[0]);
            ulong overrideId = ulong.Parse(idParts[1]); // ID оверрайда из базы или 0

            // 3. НАХОДИМ ОБЪЕКТ СОТРУДНИКА В СПИСКЕ КАРТОЧКИ, ЧТОБЫ УЗНАТЬ ЕГО ЦИФРОВОЙ СТАТУС
            // (_currentCard — сохраненный объект EquipmentShiftCard текущего окна)
            var employee = _shiftCard.PlannedStaff.FirstOrDefault(x => x.EmployeeId == employeeId);
            if (employee == null) return;

            // Приводим числовой код из базы к нашему строгому Enum [2026-09-09, 2026-09-11]
            PlannedEmployeeStatusType statusType = (PlannedEmployeeStatusType)employee.PlanStatusCode;

            // ===================================================================
            // 🌟 ЖЕСТКАЯ ЦИФРОВАЯ БЛОКИРОВКА КНОПОК ПЛАНА (ЗАЩИТА ОТ ДУРАКА) 🌟
            // ===================================================================
            switch (statusType)
            {
                case PlannedEmployeeStatusType.Active:
                    // Сотрудник здоров и в графике: можно отменить или перевести. Возвращать нечего.
                    buttonScheduleShiftCancel.Enabled = true;
                    buttonScheduleEquipReplace.Enabled = true;
                    buttonScheduleReturn.Enabled = false;
                    break;

                case PlannedEmployeeStatusType.Absence:
                    // Сотрудник официально БОЛЕЕТ или в ОТПУСКЕ: 
                    // Жестко блокируем ВСЕ кнопки! Корректировать системное отсутствие из этого окна нельзя.
                    buttonScheduleShiftCancel.Enabled = false;
                    buttonScheduleEquipReplace.Enabled = false;
                    buttonScheduleReturn.Enabled = false;
                    break;

                case PlannedEmployeeStatusType.Cancelled:
                case PlannedEmployeeStatusType.Transferred:
                    // Сотрудник УЖЕ ручками отменен или переведен мастером (есть overrideId > 0):
                    // Повторно отменять или переводить нельзя. Разрешаем только СБРОСИТЬ изменения (Вернуть по графику).
                    buttonScheduleShiftCancel.Enabled = false;
                    buttonScheduleEquipReplace.Enabled = false;
                    buttonScheduleReturn.Enabled = overrideId > 0; // Активна, если оверрайд физически существует
                    break;
            }
            /*if (gridViewUserSchedule.CurrentRow != null)
            {
                int rowIndex = gridViewUserSchedule.CurrentRow.Index;

                object tagValue = gridViewUserSchedule.CurrentRow.Tag;

                if (tagValue != null)
                {
                    // 1. Разбиваем строку по разделителю ';'
                    string[] idParts = gridViewUserSchedule.CurrentRow.Tag.ToString().Split(';');

                    // 2. Парсим обе части в чистые ulong переменные
                    ulong employeeId = ulong.Parse(idParts[0]);
                    ulong overrideId = ulong.Parse(idParts[1]);

                    //int id = tagValue != null ? Convert.ToInt32(tagValue) : -1;
                    if (overrideId == 0)
                    {
                        buttonScheduleShiftCancel.Enabled = true;
                        buttonScheduleEquipReplace.Enabled = true;
                        buttonScheduleReturn.Enabled = false;
                    }

                    if (overrideId > 0)
                    {
                        buttonScheduleShiftCancel.Enabled = false;
                        buttonScheduleEquipReplace.Enabled = false;
                        buttonScheduleReturn.Enabled = true;
                    }
                }
            }
            else
            {
                buttonScheduleShiftCancel.Enabled = false;
                buttonScheduleEquipReplace.Enabled = false;
                buttonScheduleReturn.Enabled = false;
            }*/
        }

        private void gridViewUserOverride_SelectionChanged(object sender, EventArgs e)
        {
            // 1. ЗАЩИТА: Если строка не выбрана или таблица пустая — тушим кнопку
            if (gridViewUserOverride.CurrentRow == null || gridViewUserOverride.CurrentRow.Tag == null)
            {
                buttonAssignReturn.Enabled = false;
                return;
            }
            else
            {
                buttonAssignReturn.Enabled = true;
                return;
            }

            // 2. Извлекаем текст статуса из второй колонки (индекс 1) [2026-08-31, 2026-09-05]
            string currentStatusText = gridViewUserOverride.CurrentRow.Cells[1].Value?.ToString() ?? string.Empty;

            // 3. БИЗНЕС-ПРАВИЛО: Кнопка активна ТОЛЬКО для ручных замен или черновиков
            // Если строка содержит "По графику" или "Без рабочего места" — удалять тут нечего, это базовый план!
            /*if (currentStatusText.Contains("По графику") || currentStatusText.Contains("Без рабочего места"))
            {
                buttonAssignReturn.Enabled = false; // Блокируем кнопку удаления
            }
            else
            {
                // Если статус содержит "Замена" или "Черновик" — разрешаем удаление оверрайда! [2026-09-05]
                buttonAssignReturn.Enabled = true; // Активируем кнопку
            }*/
        }

        private async void buttonScheduleShiftCancel_Click(object sender, EventArgs e)
        {
            if (gridViewUserSchedule.CurrentRow == null || gridViewUserSchedule.CurrentRow.Tag == null) return;

            try
            {
                var report = new EmployeeManagementService(parameter.GetMySQLConnectionString());

                string[] idParts = gridViewUserSchedule.CurrentRow.Tag.ToString().Split(';');
                ulong employeeId = ulong.Parse(idParts[0]);

                string comment = "Отмена мастером в карточке смены";

                var confirmResult = MessageBox.Show(
                "Вы уверены, что хотите отменить смену сотрудника?",
                "Подтверждение отмены",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
                );

                if (confirmResult == DialogResult.Yes)
                {
                    // Пишем оверрайд отмены (is_cancellation = 1)
                    await report.CancelEmployeeShiftAsync(_date, _shiftID, employeeId, comment);

                    await RefreshCardDataAsync();
                }
            }
            catch
            {

            }
        }

        private async void buttonScheduleEquipReplace_Click(object sender, EventArgs e)
        {
            if (gridViewUserSchedule.CurrentRow == null || gridViewUserSchedule.CurrentRow.Tag == null) return;

            // 1. Извлекаем ID сотрудника из нашего комбинированного тега (id_empl;id_ovr) [2026-09-04]
            string[] idParts = gridViewUserSchedule.CurrentRow.Tag.ToString().Split(';');
            ulong employeeId = ulong.Parse(idParts[0]);

            // 2. Открываем окно выбора, передавая в него ВСЕ параметры для инсерта
            using (var selectEquipForm = new FormSelectAssignmentEquipUser(_date, _shiftID, employeeId, _equipment.Name, _equipID))
            {
                // Если в дочернем окне нажали "ОК" и инсерт в БД прошел успешно
                if (selectEquipForm.ShowDialog() == DialogResult.OK)
                {
                    // Нам остается просто обновить сетку на главной форме!
                    await RefreshCardDataAsync();
                }
            }
        }

        private async void buttonScheduleReturn_Click(object sender, EventArgs e)
        {
            if (gridViewUserSchedule.CurrentRow == null || gridViewUserSchedule.CurrentRow.Tag == null) return;

            try
            {
                var report = new EmployeeManagementService(parameter.GetMySQLConnectionString());

                string[] idParts = gridViewUserSchedule.CurrentRow.Tag.ToString().Split(';');
                ulong overrideId = ulong.Parse(idParts[1]);

                // Защита: если оверрайда нет (равен 0), возвращать нечего
                if (overrideId == 0)
                {
                    MessageBox.Show("Сотрудник и так работает по штатному графику.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var confirm = MessageBox.Show("Вернуть сотрудника на плановое рабочее место?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    // Вызываем точечный DELETE по id записи оверрайда
                    await report.DeleteOverrideByIdAsync(overrideId);

                    await RefreshCardDataAsync();
                }
            }
            catch
            {

            }
        }

        private async void buttonAssignEmployee_Click(object sender, EventArgs e)
        {
            using (var selectEquipForm = new FormSelectAssignmentUser(_date, _shiftID, _equipID))
            {
                // Если в дочернем окне нажали "ОК" и инсерт в БД прошел успешно
                if (selectEquipForm.ShowDialog() == DialogResult.OK)
                {
                    // Нам остается просто обновить сетку на главной форме!
                    await RefreshCardDataAsync();
                }
            }
        }

        private async void buttonAssignReturn_Click(object sender, EventArgs e)
        {
            // 1. Проверяем, выбрана ли строка в нижней таблице факта
            if (gridViewUserOverride.CurrentRow == null || gridViewUserOverride.CurrentRow.Tag == null) return;

            var report = new EmployeeManagementService(parameter.GetMySQLConnectionString());

            // Вытаскиваем ID, сохраненный в Tag строки
            ulong idFromTag = (ulong)gridViewUserOverride.CurrentRow.Tag;

            // Получаем текст статуса из второй колонки (индекс 1), чтобы понять, КТО это [2026-08-31]
            string currentStatusText = gridViewUserOverride.CurrentRow.Cells[1].Value?.ToString() ?? string.Empty;

            // 2. ЗАЩИТНАЯ ПРОВЕРКА: Если строка "По графику" — отправляем мастера в верхний блок кнопок!
            if (currentStatusText.Contains("По графику"))
            {
                MessageBox.Show(
                    "Этот сотрудник работает по своему постоянному плановому графику.\n\n" +
                    "Вы не можете 'удалить' его замену, так как её нет. Чтобы снять планового сотрудника со смены, " +
                    "выберите его в ВЕРХНЕЙ таблице плана и нажмите кнопку 'Отменить смену'.",
                    "Внимание",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }

            // 3. Если это реальная ручная замена или черновик (статус содержит "Замена" или "Черновик")
            var confirm = MessageBox.Show(
                "Вы уверены, что хотите снять данное ручное назначение?",
                "Подтверждение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    buttonAssignReturn.Enabled = false;

                    // Вызываем наш точечный метод DELETE по ID оверрайда [2026-09-04]
                    bool isDeleted = await report.DeleteOverrideByIdAsync(idFromTag);

                    if (isDeleted)
                    {
                        MessageBox.Show("Ручное назначение успешно аннулировано.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Перерисовываем форму карточки смены
                        await RefreshCardDataAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении назначения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    buttonAssignReturn.Enabled = true;
                }
            }
        }

        private async void buttonEquipCancel_Click(object sender, EventArgs e)
        {
            if (_shiftCard == null) return;

            var report = new ReportEquipShift(parameter.GetMySQLConnectionString());

            try
            {
                btnManageEquipment.Enabled = false;

                switch (_currentEquipmentState)
                {
                    case EquipmentShiftState.ActiveByPlan:
                        // Действие для автоплана: создаем оверрайд остановки станка (is_cancelled = true)
                        var confirmPlan = MessageBox.Show("Вы уверены, что хотите остановить работу на эту смену?", "Остановка", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmPlan == DialogResult.Yes)
                        {
                            await report.SaveEquipmentOverrideAsync(_date, _shiftID, _equipID, isCancelled: true);
                        }
                        break;

                    case EquipmentShiftState.CancelledManually:
                        // Действие для планового станка: удаляем ручную остановку (DELETE), возвращая к автоплану
                        var confirmReset = MessageBox.Show("Вернуть к стандартному плановому графику работы?", "Возврат к графику", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (confirmReset == DialogResult.Yes)
                        {
                            await report.RemoveEquipmentOverrideAsync(_date, _shiftID, _equipID);
                        }
                        break;

                    case EquipmentShiftState.ManualActive:
                        string msg = _shiftCard.ActiveStaffingMode == "manual_only"
                        ? "Удалить созданную вручную смену?"
                        : "Удалить внеплановую смену? Возврат в состояние планового простоя (выходного).";

                        var confirmManual = MessageBox.Show(msg, "Отмена смены", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (confirmManual == DialogResult.Yes)
                        {
                            // Выполняется чистый DELETE, никаких лишних флагов! [2026-09-09]
                            await report.RemoveEquipmentOverrideAsync(_date, _shiftID, _equipID);
                        }
                        break;

                    case EquipmentShiftState.IdleByPlan:
                    case EquipmentShiftState.ManualWaiting:
                        // Действие для простоя или ожидания: активируем станок в работу (isCancelled = false)
                        await report.SaveEquipmentOverrideAsync(_date, _shiftID, _equipID, isCancelled: false);
                        break;
                }

                await RefreshCardDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выполнения команды: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnManageEquipment.Enabled = true;
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
