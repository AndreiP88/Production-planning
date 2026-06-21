using data;
using database;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Production_planning
{
    public partial class FormSelectStaffingMode : MaterialForm
    {
        private int _equipmentId;

        // ПУБЛИЧНЫЕ СВОЙСТВА: Их будет считывать основная форма после закрытия этого окна
        //public int SelectedTemplateId { get; private set; }
        public string SelectedStaffingMode { get; private set; }
        public DateTime SelectedDate { get; private set; }

        private string[] _staffingMode = { "strict_schedule", "manual_only" };
        private string[] _staffingModeName = { "Назначение сотрудников по графику", "Ручное назначение отрудников" };

        public FormSelectStaffingMode(int equipmentId)
        {
            InitializeComponent();

            _equipmentId = equipmentId;
            
            //materialButton1.DialogResult = DialogResult.OK;
            materialButton2.DialogResult = DialogResult.Cancel;
        }

        private async void FormAddCycle_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");

            comboBoxStaffingMode.Items.AddRange(_staffingModeName);

            await LoadHistory();

            textBoxDateStaffingModeStart.Text = DateTime.Now.ToString("dd.MM.yyyy");
        }

        private async Task LoadHistory()
        {
            ConnectionParameter parameter = new ConnectionParameter();

            WorkAreaService workAreaService = new WorkAreaService(parameter.GetMySQLConnectionString());

            try
            {
                List<EquipmentStaffingHistoryRow> staffingHistory = await workAreaService.GetEquipmentStaffingHistoryAsync(_equipmentId);

                int index = 0;

                foreach (EquipmentStaffingHistoryRow staffing in staffingHistory)
                {
                    index++;

                    ListViewItem item = new ListViewItem();

                    item.Text = index.ToString();
                    item.SubItems.Add(_staffingModeName[Array.IndexOf(_staffingMode, staffing.StaffingMode)]);
                    item.SubItems.Add(staffing.ValidFrom.ToString("dd.MM.yyyy"));

                    listViewScheduleHistory.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки таблиц истории: {ex.Message}");
            }
        }
        private async Task<bool> SaveTemplate()
        {
            bool result = false;

            if(comboBoxStaffingMode.SelectedItem != null)
            {
                SelectedStaffingMode = _staffingMode[comboBoxStaffingMode.SelectedIndex];

                if (!textBoxDateStaffingModeStart.GetErrorState())
                {
                    SelectedDate = dateTimePicker1.Value;
                    result = true;
                }
                else
                {
                    MessageBox.Show("Укажите корректную дату!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите режим работы из списка!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }

        private async void materialButton1_Click(object sender, EventArgs e)
        {
            if (await SaveTemplate())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // Записываем дату в нужном формате в MaterialTextBox
            textBoxDateStaffingModeStart.Text = dateTimePicker1.Value.ToString("dd.MM.yyyy");

            // Возвращаем фокус на текстовое поле
            textBoxDateStaffingModeStart.Focus();
        }

        private void textBoxDateTemplateStart_Leave(object sender, EventArgs e)
        {
            string input = textBoxDateStaffingModeStart.Text;

            // Если пользователь ничего не ввел (или ввел только точки маски "__.__.____")
            if (string.IsNullOrWhiteSpace(input) || input == "..")
            {
                textBoxDateStaffingModeStart.SetErrorState(false);
                return;
            }

            // Проверяем существование даты (защита от 32.01.2026, 29.02.2025 и т.д.)
            DateTime parsedDate;
            bool isValidDate = DateTime.TryParseExact(input, "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out parsedDate);

            if (!isValidDate)
            {
                // Включаем ошибку в стиле Material
                textBoxDateStaffingModeStart.ErrorMessage = "Дата введена неверно";
                textBoxDateStaffingModeStart.SetErrorState(true);
            }
            else
            {
                // Всё ок — сбрасываем ошибку и обновляем скрытый календарь
                textBoxDateStaffingModeStart.SetErrorState(false);
                dateTimePicker1.Value = parsedDate;
            }
        }

        private void buttonCalendarTemplate_Click(object sender, EventArgs e)
        {
            // Показываем выпадающий календарь стандартного DateTimePicker
            // Для этого сам datetimePicker1 должен лежать на форме (можно скрыть его)
            // Проверяем существование даты (защита от 32.01.2026, 29.02.2025 и т.д.)
            string input = textBoxDateStaffingModeStart.Text;

            DateTime parsedDate;
            bool isValidDate = DateTime.TryParseExact(input, "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out parsedDate);

            if (isValidDate)
            {
                dateTimePicker1.Value = parsedDate;
            }

            dateTimePicker1.Focus();

            SendKeys.Send("%{DOWN}");
        }

        private void dateTimePicker1_ValueChanged_1(object sender, EventArgs e)
        {
            textBoxDateStaffingModeStart.Text = dateTimePicker1.Value.ToString("dd.MM.yyyy");
        }
    }
}
