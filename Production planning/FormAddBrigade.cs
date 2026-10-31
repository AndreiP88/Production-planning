using data;
using database;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Production_planning
{
    public partial class FormAddBrigade : MaterialForm
    {
        private readonly bool _edit;
        private ScheduleTemplateModel _scheduleTemplate;
        private List<ScheduleCycleModel> _scheduleCycles;

        // Вспомогательный словарь для связи текста комбобокса с перечислением
        private readonly Dictionary<string, ScheduleTargetType> _targetTypeMapping = new Dictionary<string, ScheduleTargetType>
        {
            { "Универсальный", ScheduleTargetType.Universal },
            { "Для рабочих мест", ScheduleTargetType.Equipment },
            { "Для сотрудников", ScheduleTargetType.Employee }
        };

        public FormAddBrigade(ScheduleTemplateModel scheduleTemplate = null)
        {
            InitializeComponent();

            _edit = scheduleTemplate != null;
            _scheduleTemplate = scheduleTemplate ?? new ScheduleTemplateModel();
            materialButton1.Text = _edit ? "Сохранить" : "Добавить";

            materialButton1.DialogResult = DialogResult.OK;
            materialButton2.DialogResult = DialogResult.Cancel;

            // Наполняем комбобокс вариантов назначения графика
            PopulateTargetTypeComboBox();
        }

        private void PopulateTargetTypeComboBox()
        {
            comboBoxTargetType.Items.Clear();
            foreach (var key in _targetTypeMapping.Keys)
            {
                comboBoxTargetType.Items.Add(key);
            }
            comboBoxTargetType.SelectedIndex = 0; // По умолчанию Универсальный
        }

        private async Task LoadCycles()
        {
            ConnectionParameter parameter = new ConnectionParameter();

            _scheduleCycles?.Clear();

            ScheduleTemplateService scheduleTemplate = new ScheduleTemplateService(parameter.GetMySQLConnectionString());

            _scheduleCycles = await scheduleTemplate.GetAllCyclesAsync();

            comboBoxCycles.Items.Clear();

            foreach (ScheduleCycleModel scheduleCycle in _scheduleCycles)
            {
                comboBoxCycles.Items.Add(scheduleCycle.Name);
            }
        }

        private async Task LoadTemplate()
        {
            if (!_edit)
            {
                // Для нового графика выставляем текущую дату по умолчанию
                textBoxDate.Text = DateTime.Today.ToString("dd.MM.yyyy");
                dateTimePicker1.Value = DateTime.Today;
            }
            else
            {
                int id = _scheduleCycles.FindIndex(u => u.Id == _scheduleTemplate.CycleId);

                textBoxBrigadeName.Text = _scheduleTemplate.Name;
                textBoxDate.Text = _scheduleTemplate.BaseDate.ToString("dd.MM.yyyy");
                comboBoxCycles.SelectedIndex = id;

                // 🌟 НАХОДИМ И ВЫСТАВЛЯЕМ ТЕКУЩИЙ ТИП НАЗНАЧЕНИЯ ГРАФИКА
                foreach (var pair in _targetTypeMapping)
                {
                    if (pair.Value == _scheduleTemplate.TargetType)
                    {
                        comboBoxTargetType.SelectedItem = pair.Key;
                        break;
                    }
                }
            }
        }

        private async Task SaveTemplate()
        {
            ConnectionParameter parameter = new ConnectionParameter();
            var shiftService = new ScheduleTemplateService(parameter.GetMySQLConnectionString());

            ScheduleTemplateModel newTemplate = _scheduleTemplate;

            newTemplate.Name = textBoxBrigadeName.Text;
            newTemplate.CycleId = _scheduleCycles[comboBoxCycles.SelectedIndex].Id;
            newTemplate.BaseDate = Convert.ToDateTime(textBoxDate.Text);

            // 🌟 ЧИТАЕМ ТИП НАЗНАЧЕНИЯ ГРАФИКА ИЗ КОМБОБОКСА И ПЕРЕДАЕМ В МОДЕЛЬ
            if (comboBoxTargetType.SelectedItem != null &&
                _targetTypeMapping.TryGetValue(comboBoxTargetType.SelectedItem.ToString(), out ScheduleTargetType selectedType))
            {
                newTemplate.TargetType = selectedType; // Автоматически запишет нужную строку enum в TargetTypeRaw
            }

            if (!_edit)
            {
                await shiftService.CreateTemplateBindingAsync(newTemplate);
            }
            else
            {
                await shiftService.UpdateTemplateBindingAsync(newTemplate);
            }
        }


        private async void FormAddCycle_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");

            await LoadCycles();
            await LoadTemplate();
        }

        private async void materialButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxBrigadeName.Text))
            {
                MessageBox.Show("Укажите наименование графика!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }

            if (comboBoxCycles.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите схему цикла для графика!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }

            if (textBoxDate.Text.Contains("_") || string.IsNullOrWhiteSpace(textBoxDate.Text))
            {
                MessageBox.Show("Укажите корректную базовую дату начала отсчета!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }

            await SaveTemplate();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonCalendar_Click(object sender, EventArgs e)
        {
            string input = textBoxDate.Text;

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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            textBoxDate.Text = dateTimePicker1.Value.ToString("dd.MM.yyyy");
            textBoxDate.Focus();
        }

        private void textBoxDate_Leave(object sender, EventArgs e)
        {
            string input = textBoxDate.Text;

            if (string.IsNullOrWhiteSpace(input) || input == "..")
            {
                textBoxDate.SetErrorState(false);
                return;
            }

            DateTime parsedDate;
            bool isValidDate = DateTime.TryParseExact(input, "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out parsedDate);

            if (!isValidDate)
            {
                textBoxDate.ErrorMessage = "Дата введена неверно";
                textBoxDate.SetErrorState(true);
            }
            else
            {
                textBoxDate.SetErrorState(false);
                dateTimePicker1.Value = parsedDate;
            }
        }
    }
}
