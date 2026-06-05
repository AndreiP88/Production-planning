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

        public FormAddBrigade(ScheduleTemplateModel scheduleTemplate = null)
        {
            InitializeComponent();

            _edit = scheduleTemplate != null;
            _scheduleTemplate = scheduleTemplate ?? new ScheduleTemplateModel();
            materialButton1.Text = _edit ? "Сохранить" : "Добавить";
            
            materialButton1.DialogResult = DialogResult.OK;
            materialButton2.DialogResult = DialogResult.Cancel;
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
                
            }
            else
            {
                int id = _scheduleCycles.FindIndex(u => u.Id == _scheduleTemplate.CycleId);

                textBoxBrigadeName.Text = _scheduleTemplate.Name;
                textBoxDate.Text = _scheduleTemplate.BaseDate.ToString("dd.MM.yyyy");
                comboBoxCycles.SelectedIndex = id;
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

            if (!_edit)
            {
                await shiftService.CreateTemplateBindingAsync(newTemplate);
            }
            else
            {
                /*ShiftDefinitionModel newShift = _shiftDefinition;

                newShift.ShiftNumber = Convert.ToInt32(textBoxShiftNum.Text);
                newShift.Name = textBoxShiftName.Text;
                newShift.StartTime = timeStart;
                newShift.EndTime = timeEnd;*/

                /*var newCycle = new ScheduleCycleModel
                {
                    
                };*/

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
            // Показываем выпадающий календарь стандартного DateTimePicker
            // Для этого сам datetimePicker1 должен лежать на форме (можно скрыть его)
            // Проверяем существование даты (защита от 32.01.2026, 29.02.2025 и т.д.)
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
            // Записываем дату в нужном формате в MaterialTextBox
            textBoxDate.Text = dateTimePicker1.Value.ToString("dd.MM.yyyy");

            // Возвращаем фокус на текстовое поле
            textBoxDate.Focus();
        }

        private void textBoxDate_Leave(object sender, EventArgs e)
        {
            string input = textBoxDate.Text;

            // Если пользователь ничего не ввел (или ввел только точки маски "__.__.____")
            if (string.IsNullOrWhiteSpace(input) || input == "..")
            {
                textBoxDate.SetErrorState(false);
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
                textBoxDate.ErrorMessage = "Дата введена неверно";
                textBoxDate.SetErrorState(true);
            }
            else
            {
                // Всё ок — сбрасываем ошибку и обновляем скрытый календарь
                textBoxDate.SetErrorState(false);
                dateTimePicker1.Value = parsedDate;
            }
        }
    }
}
