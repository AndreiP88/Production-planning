using data;
using database;
using MaterialSkin.Controls;
using SourceGrid;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Production_planning
{
    public partial class FormSelectUserPosition : MaterialForm
    {
        private List<PositionLookupDto> _positions;
        private int _userId;

        // ПУБЛИЧНЫЕ СВОЙСТВА: Их будет считывать основная форма после закрытия этого окна
        public int SelectedPositionId { get; private set; }
        public string SelectedPositionName { get; private set; }
        public DateTime SelectedDate { get; private set; }

        public FormSelectUserPosition(int userId)
        {
            InitializeComponent();

            _userId = userId;
            
            //materialButton1.DialogResult = DialogResult.OK;
            materialButton2.DialogResult = DialogResult.Cancel;
        }

        private async void FormSelectUserPosition_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");

            await LoadHistory();
            await LoadPositions();

            textBoxDateTemplateStart.Text = DateTime.Now.ToString("dd.MM.yyyy");
        }

        private async Task LoadPositions()
        {
            ConnectionParameter parameter = new ConnectionParameter();

            _positions?.Clear();

            try
            {
                EmployeeManagementService employeeService = new EmployeeManagementService(parameter.GetMySQLConnectionString());

                _positions = await employeeService.GetPositionsLookupAsync();

                comboBoxPositions.Items.Clear();

                foreach (PositionLookupDto position in _positions)
                {
                    comboBoxPositions.Items.Add(position.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки списка должностей.\n" + ex.Message);
            }
            
        }

        private async Task LoadHistory()
        {
            ConnectionParameter parameter = new ConnectionParameter();

            EmployeeManagementService employeeService = new EmployeeManagementService(parameter.GetMySQLConnectionString());

            try
            {
                List<EmployeeCareerEventRow> employeeCareerTimelines = await employeeService.GetEmployeeCareerTimelineAsync((ulong)_userId);

                int index = 0;

                foreach (EmployeeCareerEventRow employeeCareerTimeline in employeeCareerTimelines)
                {
                    index++;

                    ListViewItem item = new ListViewItem();

                    item.Text = index.ToString();
                    item.SubItems.Add(employeeCareerTimeline.EventDate.ToString("dd.MM.yyyy"));
                    item.SubItems.Add(employeeCareerTimeline.EventIcon + " " + employeeCareerTimeline.EventType);
                    item.SubItems.Add(employeeCareerTimeline.Details);

                    listViewPositionsHistory.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки таблиц истории:\n {ex.Message}");
                Console.WriteLine(ex.Message);
            }
        }
        private async Task<bool> SaveTemplate()
        {
            bool result = false;

            if(comboBoxPositions.SelectedItem != null)
            {
                SelectedPositionId = (int)_positions[comboBoxPositions.SelectedIndex].Id;
                SelectedPositionName = _positions[comboBoxPositions.SelectedIndex].Name;

                if (!textBoxDateTemplateStart.GetErrorState())
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
                MessageBox.Show("Выберите должность из списка!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            textBoxDateTemplateStart.Text = dateTimePicker1.Value.ToString("dd.MM.yyyy");

            // Возвращаем фокус на текстовое поле
            textBoxDateTemplateStart.Focus();
        }

        private void textBoxDateTemplateStart_Leave(object sender, EventArgs e)
        {
            string input = textBoxDateTemplateStart.Text;

            // Если пользователь ничего не ввел (или ввел только точки маски "__.__.____")
            if (string.IsNullOrWhiteSpace(input) || input == "..")
            {
                textBoxDateTemplateStart.SetErrorState(false);
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
                textBoxDateTemplateStart.ErrorMessage = "Дата введена неверно";
                textBoxDateTemplateStart.SetErrorState(true);
            }
            else
            {
                // Всё ок — сбрасываем ошибку и обновляем скрытый календарь
                textBoxDateTemplateStart.SetErrorState(false);
                dateTimePicker1.Value = parsedDate;
            }
        }

        private void buttonCalendarTemplate_Click(object sender, EventArgs e)
        {
            // Показываем выпадающий календарь стандартного DateTimePicker
            // Для этого сам datetimePicker1 должен лежать на форме (можно скрыть его)
            // Проверяем существование даты (защита от 32.01.2026, 29.02.2025 и т.д.)
            string input = textBoxDateTemplateStart.Text;

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
            textBoxDateTemplateStart.Text = dateTimePicker1.Value.ToString("dd.MM.yyyy");
        }

        
    }
}
