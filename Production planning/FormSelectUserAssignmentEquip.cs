using data;
using database;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Production_planning
{
    public partial class FormSelectUserAssignmentEquip : MaterialForm
    {
        private List<EquipmentLookupDto> _equips;
        private int _userId;

        // ПУБЛИЧНЫЕ СВОЙСТВА: Их будет считывать основная форма после закрытия этого окна
        public int SelectedEquipId { get; private set; }
        public string SelectedEquipName { get; private set; }
        public DateTime SelectedDate { get; private set; }

        public FormSelectUserAssignmentEquip(int userId)
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
            await LoadEquips();

            textBoxDateEquipAssignmentStart.Text = DateTime.Now.ToString("dd.MM.yyyy");
        }

        private async Task LoadEquips()
        {
            ConnectionParameter parameter = new ConnectionParameter();

            _equips?.Clear();

            try
            {
                WorkAreaService workAreaService = new WorkAreaService(parameter.GetMySQLConnectionString());

                _equips = await workAreaService.GetEquipmentLookupAsync();

                comboBoxEquips.Items.Clear();

                foreach (EquipmentLookupDto equip in _equips)
                {
                    comboBoxEquips.Items.Add(equip.DisplayText);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки списка оборудования.\n" + ex.Message);
            }
            
        }

        private async Task LoadHistory()
        {
            ConnectionParameter parameter = new ConnectionParameter();

            EmployeeManagementService employeeService = new EmployeeManagementService(parameter.GetMySQLConnectionString());

            try
            {
                List<EmployeeEquipmentHistoryRow> employeeEquipHistoryes = await employeeService.GetEmployeeEquipmentHistoryAsync((ulong)_userId);

                int index = 0;

                foreach (EmployeeEquipmentHistoryRow employeeEquipHistory in employeeEquipHistoryes)
                {
                    index++;

                    ListViewItem item = new ListViewItem();

                    item.Text = index.ToString();
                    item.SubItems.Add(employeeEquipHistory.ValidFrom.ToString("dd.MM.yyyy"));
                    item.SubItems.Add(employeeEquipHistory.EquipmentName);

                    listViewHistoryEquipAssignment.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки таблиц истории:\n {ex.Message}");
                Console.WriteLine(ex.Message);
            }
        }
        private async Task<bool> SaveAssignmentEquip()
        {
            bool result = false;

            if(comboBoxEquips.SelectedItem != null)
            {
                SelectedEquipId = (int)_equips[comboBoxEquips.SelectedIndex].Id;
                SelectedEquipName = _equips[comboBoxEquips.SelectedIndex].Name;

                if (!textBoxDateEquipAssignmentStart.GetErrorState())
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
                MessageBox.Show("Выберите место работы из списка!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }

        private async void materialButton1_Click(object sender, EventArgs e)
        {
            if (await SaveAssignmentEquip())
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
            textBoxDateEquipAssignmentStart.Text = dateTimePicker1.Value.ToString("dd.MM.yyyy");

            // Возвращаем фокус на текстовое поле
            textBoxDateEquipAssignmentStart.Focus();
        }

        private void textBoxDateTemplateStart_Leave(object sender, EventArgs e)
        {
            string input = textBoxDateEquipAssignmentStart.Text;

            // Если пользователь ничего не ввел (или ввел только точки маски "__.__.____")
            if (string.IsNullOrWhiteSpace(input) || input == "..")
            {
                textBoxDateEquipAssignmentStart.SetErrorState(false);
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
                textBoxDateEquipAssignmentStart.ErrorMessage = "Дата введена неверно";
                textBoxDateEquipAssignmentStart.SetErrorState(true);
            }
            else
            {
                // Всё ок — сбрасываем ошибку и обновляем скрытый календарь
                textBoxDateEquipAssignmentStart.SetErrorState(false);
                dateTimePicker1.Value = parsedDate;
            }
        }

        private void buttonCalendarTemplate_Click(object sender, EventArgs e)
        {
            // Показываем выпадающий календарь стандартного DateTimePicker
            // Для этого сам datetimePicker1 должен лежать на форме (можно скрыть его)
            // Проверяем существование даты (защита от 32.01.2026, 29.02.2025 и т.д.)
            string input = textBoxDateEquipAssignmentStart.Text;

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
            textBoxDateEquipAssignmentStart.Text = dateTimePicker1.Value.ToString("dd.MM.yyyy");
        }

        
    }
}
