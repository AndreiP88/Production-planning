using data;
using database;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Production_planning
{
    public partial class FormAddEquip : MaterialForm
    {
        private readonly bool _edit;
        private List<ScheduleTemplateModel> _scheduleTemplates;
        private List<WorkAreaInfo> _workAreas;
        private EquipmentShortInfo _equipment;
        private int _equipmentID;
        private int _areaID;
        private bool _isActive;
        private bool _isLoad;

        private string[] _staffingMode = { "strict_schedule", "manual_only" };

        public FormAddEquip(int areaID = -1, int equipmentID = -1)
        {
            InitializeComponent();

            _areaID = areaID;
            _equipmentID = equipmentID;
            _edit = equipmentID != -1;
            _equipment = _equipment ?? new EquipmentShortInfo();
            materialButton1.Text = _edit ? "Сохранить" : "Добавить";
            
            materialButton1.DialogResult = DialogResult.OK;
            materialButton2.DialogResult = DialogResult.Cancel;
        }

        private async void FormAddCycle_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");

            await LoadTemplate();
            await LoadAreas();
            await LoadEquipment();
        }

        private async Task LoadTemplate()
        {
            ConnectionParameter parameter = new ConnectionParameter();

            _scheduleTemplates?.Clear();

            ScheduleTemplateService scheduleTemplate = new ScheduleTemplateService(parameter.GetMySQLConnectionString());

            _scheduleTemplates = await scheduleTemplate.GetAllTemplatesAsync();

            comboBoxTemplates.Items.Clear();

            foreach (ScheduleTemplateModel template in _scheduleTemplates)
            {
                comboBoxTemplates.Items.Add(template.Name);
            }
        }

        private async Task LoadAreas()
        {
            ConnectionParameter parameter = new ConnectionParameter();

            WorkAreaService workAreaService = new WorkAreaService(parameter.GetMySQLConnectionString());

            _workAreas = await workAreaService.GetWorkAreasWithEquipmentAsync();

            foreach(WorkAreaInfo workArea in _workAreas)
            {
                comboBoxAreas.Items.Add(workArea.Name);
            }
        }

        private async Task LoadEquipment()
        {
            _isLoad = true;

            int areaID = _workAreas.FindIndex(u => u.Id == _areaID);
            comboBoxAreas.SelectedIndex = areaID;

            this.Text += " - " + _workAreas[areaID].Name;

            if (!_edit)
            {
                _isActive = true;

                dateTimePicker1.Value = DateTime.Now;

                textBoxDateTemplateStart.Enabled = false;
                textBoxDateStaffingModeStart.Enabled = false;

                buttonCalendarTemplate.Enabled = false;
                buttonCalendarStaffingMode.Enabled = false;
            }
            else
            {
                ConnectionParameter parameter = new ConnectionParameter();

                WorkAreaService workAreaService = new WorkAreaService(parameter.GetMySQLConnectionString());

                _equipment = await workAreaService.GetEquipmentByIdAsync(_equipmentID);

                textBoxEquipName.Text = _equipment.Name;
                textBoxEquipCode.Text = _equipment.Code;

                textBoxDateTemplateStart.Text = _equipment.TemplateValidFrom?.ToString("dd.MM.yyyy");
                textBoxDateStaffingModeStart.Text = _equipment.StaffingModeValidFrom?.ToString("dd.MM.yyyy");

                textBoxDateComm.Text = _equipment.CommissionedAt.ToString("dd.MM.yyyy");

                int templatesID = _scheduleTemplates.FindIndex(u => u.Id == _equipment.TemplateId);
                comboBoxTemplates.SelectedIndex = templatesID;

                int staffingID = Array.IndexOf(_staffingMode, _equipment.StaffingMode);

                comboBoxStaffingMode.SelectedIndex = staffingID;

                if (_equipment.DecommissionedAt != null)
                {
                    _isActive = false;

                    textBoxDateDeComm.Visible = true;
                    buttonCalendarDeComm.Visible = true;
                    //buttonDecommission.Visible = false;
                    buttonDecommission.Text = "Восстановить работу";
                    textBoxDateDeComm.Text = _equipment.DecommissionedAt?.ToString("dd.MM.yyyy");
                }
                else
                {
                    _isActive = true;

                    textBoxDateDeComm.Visible = false;
                    buttonCalendarDeComm.Visible = false;
                    buttonDecommission.Visible = true;
                }
            }

            _isLoad = false;
        }

        private async Task SaveEquipment()
        {
            ConnectionParameter parameter = new ConnectionParameter();

            var workAreaService = new WorkAreaService(parameter.GetMySQLConnectionString());

            _equipment.WorkAreaId = _workAreas[comboBoxAreas.SelectedIndex].Id;
            _equipment.Name = textBoxEquipName.Text;
            _equipment.Code = textBoxEquipCode.Text;
            _equipment.TemplateId = _scheduleTemplates[comboBoxTemplates.SelectedIndex].Id;
            _equipment.CommissionedAt = Convert.ToDateTime(textBoxDateComm.Text);
            _equipment.StaffingMode = _staffingMode[comboBoxStaffingMode.SelectedIndex];

            if (!_isActive)
            {
                _equipment.DecommissionedAt = Convert.ToDateTime(textBoxDateDeComm.Text);
            }
            else
            {
                _equipment.DecommissionedAt = null;
            }

            if (!_edit)
            {
                //_equipment.WorkAreaId = _areaID;
                _equipment.TemplateValidFrom = Convert.ToDateTime(textBoxDateTemplateStart.Text);
                _equipment.StaffingModeValidFrom = Convert.ToDateTime(textBoxDateStaffingModeStart.Text);

                await workAreaService.CreateEquipmentAsync(_equipment);
            }
            else
            {
                DateTime oldTemplateValidFrom = (DateTime)_equipment.TemplateValidFrom;
                DateTime oldStaffingModeValidFrom = (DateTime)_equipment.StaffingModeValidFrom;

                _equipment.TemplateValidFrom = Convert.ToDateTime(textBoxDateTemplateStart.Text);
                _equipment.StaffingModeValidFrom = Convert.ToDateTime(textBoxDateStaffingModeStart.Text);

                await workAreaService.UpdateEquipmentWithHistoryAsync(_equipment, oldTemplateValidFrom, oldStaffingModeValidFrom);
            }
        }


        

        private async void materialButton1_Click(object sender, EventArgs e)
        {
            await SaveEquipment();

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
            string input = textBoxDateComm.Text;

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
            if (!_edit)
            {
                textBoxDateTemplateStart.Text = dateTimePicker1.Value.ToString("dd.MM.yyyy");
                textBoxDateStaffingModeStart.Text = dateTimePicker1.Value.ToString("dd.MM.yyyy");
            }

            // Записываем дату в нужном формате в MaterialTextBox
            textBoxDateComm.Text = dateTimePicker1.Value.ToString("dd.MM.yyyy");

            // Возвращаем фокус на текстовое поле
            textBoxDateComm.Focus();
        }

        private void textBoxDate_Leave(object sender, EventArgs e)
        {
            string input = textBoxDateComm.Text;

            // Если пользователь ничего не ввел (или ввел только точки маски "__.__.____")
            if (string.IsNullOrWhiteSpace(input) || input == "..")
            {
                textBoxDateComm.SetErrorState(false);
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
                textBoxDateComm.ErrorMessage = "Дата введена неверно";
                textBoxDateComm.SetErrorState(true);
            }
            else
            {
                // Всё ок — сбрасываем ошибку и обновляем скрытый календарь
                textBoxDateComm.SetErrorState(false);
                dateTimePicker1.Value = parsedDate;
            }
        }

        

        private void materialButton3_Click(object sender, EventArgs e)
        {
            // Показываем выпадающий календарь стандартного DateTimePicker
            // Для этого сам datetimePicker1 должен лежать на форме (можно скрыть его)
            // Проверяем существование даты (защита от 32.01.2026, 29.02.2025 и т.д.)
            string input = textBoxDateDeComm.Text;

            DateTime parsedDate;
            bool isValidDate = DateTime.TryParseExact(input, "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out parsedDate);

            if (isValidDate)
            {
                dateTimePicker2.Value = parsedDate;
            }

            dateTimePicker2.Focus();

            SendKeys.Send("%{DOWN}");
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            // Записываем дату в нужном формате в MaterialTextBox
            textBoxDateDeComm.Text = dateTimePicker2.Value.ToString("dd.MM.yyyy");

            // Возвращаем фокус на текстовое поле
            textBoxDateDeComm.Focus();
        }

        private void textBoxDateDeComm_Leave(object sender, EventArgs e)
        {
            string input = textBoxDateDeComm.Text;

            // Если пользователь ничего не ввел (или ввел только точки маски "__.__.____")
            if (string.IsNullOrWhiteSpace(input) || input == "..")
            {
                textBoxDateDeComm.SetErrorState(false);
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
                textBoxDateDeComm.ErrorMessage = "Дата введена неверно";
                textBoxDateDeComm.SetErrorState(true);
            }
            else
            {
                // Всё ок — сбрасываем ошибку и обновляем скрытый календарь
                textBoxDateDeComm.SetErrorState(false);
                dateTimePicker1.Value = parsedDate;
            }
        }

        private void buttonDecommission_Click(object sender, EventArgs e)
        {
            if (_isActive)
            {
                _isActive = false;

                textBoxDateDeComm.Visible = true;
                buttonCalendarDeComm.Visible = true;

                dateTimePicker2.Value = DateTime.Now;
            }
            else
            {
                _isActive = true;

                textBoxDateDeComm.Visible = false;
                buttonCalendarDeComm.Visible = false;
            }
        }

        private void comboBoxTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_edit && !_isLoad)
            {
                textBoxDateTemplateStart.Text = DateTime.Now.ToString("dd.MM.yyyy");
            }
        }

        private void comboBoxStaffingMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_edit && !_isLoad)
            {
                textBoxDateStaffingModeStart.Text = DateTime.Now.ToString("dd.MM.yyyy");
            }
        }
    }
}
