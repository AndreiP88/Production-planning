using data;
using database;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Production_planning
{
    public partial class FormSelectAssignmentEquipUser : MaterialForm
    {
        ConnectionParameter parameter = new ConnectionParameter();
        private List<EquipmentLookupDto> _equips;
        private DateTime _date;
        private ulong _shiftId;
        private ulong _userId;
        private ulong _currentValue;
        private string _sourceEquipmentName;
        private ShiftDefinitionModel _shiftDefinition;

        public FormSelectAssignmentEquipUser(DateTime date, ulong shiftId, ulong userId, string sourceEquipmentName, ulong currentValue)
        {
            InitializeComponent();

            _date = date;
            _shiftId = shiftId;
            _userId = userId;
            _sourceEquipmentName = sourceEquipmentName;
            _currentValue = currentValue;
            
            //materialButton1.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;
        }

        private async void FormSelectUserPosition_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");

            var report = new ShiftService(parameter.GetMySQLConnectionString());

            _shiftDefinition = await  report.GetShiftByIdAsync(_shiftId);

            labelDate.Text = _date.ToString("D");
            labelShift.Text = "Смена №" + _shiftDefinition.ShiftNumber.ToString() + ". " + _shiftDefinition.Name;

            await LoadEquips();        
        }

        private async Task LoadEquips()
        {
            _equips?.Clear();

            try
            {
                WorkAreaService workAreaService = new WorkAreaService(parameter.GetMySQLConnectionString());

                _equips = await workAreaService.GetEquipmentLookupAsync();
                
                comboBoxAssignment.Items.Clear();

                comboBoxAssignment.DisplayMember = "DisplayText";
                comboBoxAssignment.ValueMember = "Id";
                comboBoxAssignment.DataSource = _equips;

                comboBoxAssignment.SelectedValue = (ulong)_currentValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки списка рабочих мест.\n" + ex.Message);
            }
            
        }

        
        private async Task<bool> SaveAssignment()
        {
            bool result = false;

            // 1. Проверяем, выбран ли станок в комбобоксе
            if (comboBoxAssignment.SelectedValue == null)
            {
                MessageBox.Show("Пожалуйста, выберите станок из списка!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var report = new EmployeeManagementService(parameter.GetMySQLConnectionString());

            // Вытаскиваем ulong ID выбранного оборудования
            ulong newEquipmentId = (ulong)comboBoxAssignment.SelectedValue;

            try
            {
                // Выключаем кнопку, чтобы избежать повторных кликов при медленной сети
                buttonAdd.Enabled = false;

                // 2. ВЫПОЛНЯЕМ ИНСЕРТ ОВЕРРАЙДА ПРЯМО ОТСЮДА!
                await report.AssignEmployeeToShiftAsync(_date, _shiftId, newEquipmentId, _userId, 2, textBoxComment.Text);

                // 3. Если всё прошло гладко — ставим статус OK и закрываем окошко
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при переназначении в базе данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                buttonAdd.Enabled = true;
            }

            return result;
        }

        private async Task SaveAssignmentEquip()
        {
            if (comboBoxAssignment.SelectedValue == null || comboBoxTargetShifts.SelectedValue == null)
            {
                MessageBox.Show("Выберите оборудование и целевую смену!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var report = new EmployeeManagementService(parameter.GetMySQLConnectionString());

            var selectedEquipment = (EquipmentLookupDto)comboBoxAssignment.SelectedItem;
            var selectedShift = (ShiftDefinitionModel)comboBoxTargetShifts.SelectedItem;

            ulong newEquipmentId = selectedEquipment.Id;
            string newEquipmentName = selectedEquipment.Name; // Название нового станка

            ulong targetShiftId = selectedShift.Id; // ID новой смены [2026-09-05]
            string newShiftName = selectedShift.Name; // Название новой смены (например, "1 смена (08:00 - 20:00)")

            // Проверяем, отличается ли целевой shift_id от исходного, откуда переводим
            // (_sourceShiftId — это ID смены старого станка)
            if (targetShiftId != _shiftId)
            {
                var warnResult = MessageBox.Show(
                    "Внимание! График выбранного рабочего места отличается от текущего графика сотрудника.\n\n" +
                    "Будет произведена автоматическая отмена плановой смены сотрудника на старом рабочем месте " +
                    "и создание нового назначения на выбранное рабочее место. Продолжить?",
                    "Переназначение смены",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (warnResult == DialogResult.No) return; // Мастер передумал
            }

            try
            {
                buttonAdd.Enabled = false;

                // --- МАГИЯ ОВЕРРАЙДОВ (Вариант Б) ---

                // ЗАПРОС 1: Гасим сотрудника на старой смене станка (ставим принудительную отмену)
                string cancelComment = $"🔄 ПЕРЕВЕДЕН на {newEquipmentName}, {newShiftName}";
                await report.CancelEmployeeShiftAsync(_date, _shiftId, _userId, cancelComment);

                // ЗАПРОС 2: Назначаем его на новую смену нового станка с утвержденным статусом (2)
                string assignComment = $"🔄 Перевод с {_sourceEquipmentName}";
                await report.AssignEmployeeToShiftAsync(_date, targetShiftId, newEquipmentId, _userId, 2, assignComment);

                // Успешно завершаем диалог
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении перевода: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                buttonAdd.Enabled = true;
            }
        }


        private async void materialButton1_Click(object sender, EventArgs e)
        {
            await SaveAssignmentEquip();
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void comboBoxAssignment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxAssignment.SelectedValue == null) return;

            var report = new ShiftService(parameter.GetMySQLConnectionString());

            ulong selectedEquipId = (ulong)comboBoxAssignment.SelectedValue;

            // -----------------------------------------------------------------
            // ПРОВЕРКА 1: Запрет перевода на тот же самый станок
            // -----------------------------------------------------------------
            if (selectedEquipId == _currentValue)
            {
                /*MessageBox.Show(
                    $"Сотрудник уже запланирован на станцию '{_sourceEquipmentName}'.\n" +
                    "Перевод на тот же самый станок невозможен. Выберите другое оборудование.",
                    "Внимание",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );*/

                buttonAdd.Enabled = false; // Блокируем кнопку сохранения
                comboBoxTargetShifts.Enabled = false;
                comboBoxTargetShifts.DataSource = null;
                return;
            }

            // Если выбрали другой станок — возвращаем кнопке "ОК" активность
            buttonAdd.Enabled = true;

            // Загружаем из репозитория смены, которые есть у нового выбранного станка [2026-09-04]
            List<ShiftDefinitionModel> availableShifts = await report.GetEquipmentShiftsAsync(selectedEquipId, _date);

            comboBoxTargetShifts.DataSource = null;
            comboBoxTargetShifts.DisplayMember = "Name";
            comboBoxTargetShifts.ValueMember = "Id";
            comboBoxTargetShifts.DataSource = availableShifts;

            // -----------------------------------------------------------------
            // ПРОВЕРКА 2: Умная блокировка смены, если графики оборудования совпадают
            // -----------------------------------------------------------------

            // Ищем, есть ли у нового станка смена с ТЕМ ЖЕ внутренним id, что и текущая смена сотрудника
            // (_sourceShiftId — это уникальный индекс текущей смены старого станка)
            var identicalShift = availableShifts.FirstOrDefault(s => s.Id == _shiftId);

            if (identicalShift != null)
            {
                // ЕСЛИ ИНДЕКС СОВПАЛ (Станки работают по абсолютно одинаковому графику и шаблону смен)
                comboBoxTargetShifts.SelectedValue = identicalShift.Id;

                // 🔒 БЛОКИРУЕМ список смен! Концепция перевода "в рамках одной смены" работает автоматически.
                comboBoxTargetShifts.Enabled = false;
                comboBoxTargetShifts.Visible = true;
            }
            else
            {
                // ЕСЛИ ИНДЕКСА НЕТ, но совпадает хотя бы НОМЕР смены (например, Смена №2 на 8-час и 12-час графиках)
                var matchingNumberShift = availableShifts.FirstOrDefault(s => s.ShiftNumber == _shiftDefinition.ShiftNumber);

                if (matchingNumberShift != null)
                {
                    comboBoxTargetShifts.SelectedValue = matchingNumberShift.Id;

                    // Раз графики и индексы разные — РАЗБЛОКИРУЕМ комбобокс, чтобы мастер мог скорректировать смену
                    comboBoxTargetShifts.Enabled = true;
                    comboBoxTargetShifts.Visible = true;
                }
                else
                {
                    // Если графики совсем не пересекаются — просто ставим первую доступную смену станка
                    if (availableShifts.Count > 0) comboBoxTargetShifts.SelectedIndex = 0;

                    comboBoxTargetShifts.Enabled = true;
                    comboBoxTargetShifts.Visible = true;
                }
            }
        }
    }
}
