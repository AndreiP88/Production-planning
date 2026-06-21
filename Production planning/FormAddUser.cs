using data;
using database;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Production_planning
{
    public partial class FormAddUser : MaterialForm
    {
        private int _userID;

        private EmployeeFullCard _employee;

        private List<PositionLookupDto> _positions;
        private List<EquipmentLookupDto> _equipments;
        private List<ScheduleTemplateModel> _schedules;

        PositionUpdateBuffer _positionBuffer;
        EquipmentUpdateBuffer _equipmentBuffer;
        ScheduleUpdateBuffer _scheduleBuffer;

        DateTime _originalPositionDate;
        DateTime _originalEquipmentDate;
        DateTime _originalScheduleDate;

        int? _oldPositionId;
        int? _oldEquipmentId;
        int? _oldScheduleId;

        private readonly bool _edit;

        private bool _isActive;
        private bool _isLoad;

        public FormAddUser(int userId = -1)
        {
            InitializeComponent();

            _userID = userId;
            _edit = userId != -1;

            _employee = _employee ?? new EmployeeFullCard();

            materialButton1.Text = _edit ? "Сохранить" : "Добавить";

            //materialButton1.DialogResult = DialogResult.OK;
            materialButton2.DialogResult = DialogResult.Cancel;
        }

        private async void FormAddCycle_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");

            await LoadPositions();
            await LoadEquipments();
            await LoadTemplate();
            
            await LoadUser();
        }

        private async Task LoadPositions()
        {
            ConnectionParameter parameter = new ConnectionParameter();

            _positions?.Clear();

            EmployeeManagementService employeeService = new EmployeeManagementService(parameter.GetMySQLConnectionString());

            try
            {
                _positions = await employeeService.GetPositionsLookupAsync();

                comboBoxPositions.Items.Clear();

                foreach (PositionLookupDto position in _positions)
                {
                    comboBoxPositions.Items.Add(position.DisplayText);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private async Task LoadEquipments()
        {
            ConnectionParameter parameter = new ConnectionParameter();

            _equipments?.Clear();

            WorkAreaService workAreaService = new WorkAreaService(parameter.GetMySQLConnectionString());

            try
            {
                _equipments = await workAreaService.GetEquipmentLookupAsync();

                comboBoxEquipments.Items.Clear();

                foreach (EquipmentLookupDto equipment in _equipments)
                {
                    comboBoxEquipments.Items.Add(equipment.DisplayText);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task LoadTemplate()
        {
            ConnectionParameter parameter = new ConnectionParameter();

            _schedules?.Clear();

            ScheduleTemplateService scheduleTemplate = new ScheduleTemplateService(parameter.GetMySQLConnectionString());

            try
            {
                _schedules = await scheduleTemplate.GetAllTemplatesAsync();

                comboBoxTemplates.Items.Clear();

                foreach (ScheduleTemplateModel schedule in _schedules)
                {
                    comboBoxTemplates.Items.Add(schedule.Name);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task LoadUser()
        {
            _isLoad = true;

            if (!_edit)
            {
                _isActive = true;

                dateTimePicker1.Value = DateTime.Now;

                maskedTextBoxPositionDate.Enabled = false;
                maskedTextBoxEquipmentDate.Enabled = false;
                maskedTextBoxTemplateDate.Enabled = false;
                
                buttonCalendarPosition.Enabled = false;
                buttonCalendarEquipmentAssign.Enabled = false;
                buttonCalendarTemplate.Enabled = false;

                buttonPositionAssignNew.Enabled = false;
                buttonEquipmentAssignNew.Enabled = false;
                ButtonTemplateAssignNew.Enabled = false;
            }
            else
            {
                ConnectionParameter parameter = new ConnectionParameter();

                EmployeeManagementService employeeService = new EmployeeManagementService(parameter.GetMySQLConnectionString());

                try
                {
                    _employee = await employeeService.GetEmployeeFullCardAsync((ulong)_userID);

                    _originalPositionDate = _employee.PositionValidFrom ?? _employee.HireDate;
                    _originalScheduleDate = _employee.ScheduleValidFrom ?? _employee.HireDate;
                    _originalEquipmentDate = _employee.EquipmentValidFrom ?? _employee.HireDate;

                    _oldPositionId = (int?)_employee.PositionId;
                    _oldEquipmentId = (int?)_employee.EquipmentId;
                    _oldScheduleId = (int?)_employee.ScheduleTemplateId;

                    textBoxFirstName.Text = _employee.FirstName;
                    textBoxLastName.Text = _employee.LastName;
                    textBoxPatronymic.Text = _employee.Patronymic;

                    maskedTextBoxPositionDate.Text = _employee.PositionValidFrom?.ToString("dd.MM.yyyy");
                    maskedTextBoxEquipmentDate.Text = _employee.EquipmentValidFrom?.ToString("dd.MM.yyyy");
                    maskedTextBoxTemplateDate.Text = _employee.ScheduleValidFrom?.ToString("dd.MM.yyyy");

                    textBoxDateHire.Text = _employee.HireDate.ToString("dd.MM.yyyy");

                    int positionId = _positions.FindIndex(u => u.Id == _employee.PositionId);
                    int equipmentId = _equipments.FindIndex(u => u.Id == _employee.EquipmentId);
                    int templateID = _schedules.FindIndex(u => u.Id == (int)_employee.ScheduleTemplateId);

                    comboBoxPositions.SelectedIndex = positionId;
                    comboBoxEquipments.SelectedIndex = equipmentId;
                    comboBoxTemplates.SelectedIndex = templateID;

                    if (_employee.FireDate != null)
                    {
                        _isActive = false;

                        textBoxDateFire.Visible = true;
                        buttonCalendarFire.Visible = true;
                        buttonFire.Visible = true;
                        buttonFire.Text = "Повторный найм на работу";
                        textBoxDateFire.Text = _employee.FireDate?.ToString("dd.MM.yyyy");
                    }
                    else
                    {
                        _isActive = true;

                        textBoxDateFire.Visible = false;
                        buttonCalendarFire.Visible = false;
                        buttonFire.Visible = true;
                        buttonFire.Text = "Уволить";
                    }

                    _positionBuffer = new PositionUpdateBuffer
                    {
                        AssignmentId = _employee.CurrentPositionAssignmentId, // Передали ID строки!
                        EmployeeId = _employee.Id
                    };

                    _equipmentBuffer = new EquipmentUpdateBuffer
                    {
                        AssignmentId = _employee.CurrentEquipmentAssignmentId, // Передали ID строки!
                        EmployeeId = _employee.Id
                    };

                    _scheduleBuffer = new ScheduleUpdateBuffer
                    {
                        AssignmentId = _employee.CurrentScheduleAssignmentId, // Передали ID строки!
                        EmployeeId = _employee.Id
                    };
                }
                catch (Exception ex)
                {

                }

                _isLoad = false;
            }
        }

        private async Task<bool> SaveEquipment()
        {
            bool result = true;

            ConnectionParameter parameter = new ConnectionParameter();

            try
            {
                _employee.LastName = textBoxLastName.Text;
                _employee.FirstName = textBoxFirstName.Text;
                _employee.Patronymic = textBoxPatronymic.Text;

                _employee.HireDate = Convert.ToDateTime(textBoxDateHire.Text);
                //_employee.FireDate = Convert.ToDateTime(textBoxDateFire.Text);

                if (!_isActive)
                {
                    _employee.FireDate = Convert.ToDateTime(textBoxDateFire.Text);
                }
                else
                {
                    _employee.FireDate = null;
                }

                _employee.PositionId = _positions[comboBoxPositions.SelectedIndex].Id;
                _employee.PositionValidFrom = Convert.ToDateTime(maskedTextBoxPositionDate.Text);

                _employee.EquipmentId = _equipments[comboBoxEquipments.SelectedIndex].Id;
                _employee.EquipmentValidFrom = Convert.ToDateTime(maskedTextBoxEquipmentDate.Text);

                _employee.ScheduleTemplateId = (ulong?)_schedules[comboBoxTemplates.SelectedIndex].CycleId;
                _employee.ScheduleValidFrom = Convert.ToDateTime(maskedTextBoxTemplateDate.Text);

                EmployeeManagementService employeeService = new EmployeeManagementService(parameter.GetMySQLConnectionString());

                if (!_edit)
                {
                    //_equipment.WorkAreaId = _areaID;

                    await employeeService.CreateEmployeeWithAssignmentsAsync(_employee);
                }
                else
                {
                    var (PositionAge, EquipmentAge, ScheduleAge) = await employeeService.GetCurrentAssignmentsAgeAsync(_userID);

                    if (_oldPositionId != (int)_employee.PositionId)
                    {
                        if (PositionAge > 3)
                        {
                            DialogResult dialogResult = MessageBox.Show($"С момента последнего назанение прошло {PositionAge} дня!\n\nОтредактировать актуальную должность (Да)?\nЛибо создать новое назначение с текущей даты (Нет)", "Внимание", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                            if (dialogResult == DialogResult.No)
                            {
                                _scheduleBuffer.NewTemplateId = _employee.PositionId;
                                _scheduleBuffer.NewValidFrom = _employee.PositionValidFrom;
                            }

                            if (dialogResult == DialogResult.Cancel)
                            {
                                result = false;
                                return false;
                            }
                        }
                    }

                    if (_oldEquipmentId != (int)_employee.EquipmentId)
                    {
                        if (EquipmentAge > 3)
                        {
                            DialogResult dialogResult = MessageBox.Show($"С момента последнего назанение прошло {EquipmentAge} дня!\n\nОтредактировать актуальное закрепление за рабочим местом (Да)?\nЛибо создать новое назначение с текущей даты (Нет)", "Внимание", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                            if (dialogResult == DialogResult.No)
                            {
                                _equipmentBuffer.NewEquipmentId = _employee.EquipmentId;
                                _equipmentBuffer.NewValidFrom = _employee.EquipmentValidFrom;
                            }

                            if (dialogResult == DialogResult.Cancel)
                            {
                                result = false;
                                return false;
                            }
                        }
                    }

                    if (_oldScheduleId != (int)_employee.ScheduleTemplateId)
                    {
                        if (ScheduleAge > 3)
                        {
                            DialogResult dialogResult = MessageBox.Show($"С момента последнего назанение прошло {ScheduleAge} дня!\n\nОтредактировать актуальный график (Да)?\nЛибо создать новое назначение с текущей даты (Нет)", "Внимание", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                            if (dialogResult == DialogResult.No)
                            {
                                _scheduleBuffer.NewTemplateId = _employee.ScheduleTemplateId;
                                _scheduleBuffer.NewValidFrom = _employee.ScheduleValidFrom;
                            }

                            if (dialogResult == DialogResult.Cancel)
                            {
                                result = false;
                                return false;
                            }
                        }
                    }

                    await employeeService.SaveEmployeeFullCardChangesAsync(_employee, _positionBuffer, _scheduleBuffer, _equipmentBuffer);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return result;
        }


        

        private async void materialButton1_Click(object sender, EventArgs e)
        {
            if (await SaveEquipment())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
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
            string input = textBoxDateHire.Text;

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
                maskedTextBoxPositionDate.Text = dateTimePicker1.Value.ToString("dd.MM.yyyy");
                maskedTextBoxTemplateDate.Text = dateTimePicker1.Value.ToString("dd.MM.yyyy");
                maskedTextBoxEquipmentDate.Text = dateTimePicker1.Value.ToString("dd.MM.yyyy");
            }

            // Записываем дату в нужном формате в MaterialTextBox
            textBoxDateHire.Text = dateTimePicker1.Value.ToString("dd.MM.yyyy");

            // Возвращаем фокус на текстовое поле
            textBoxDateHire.Focus();
        }

        private void textBoxDate_Leave(object sender, EventArgs e)
        {
            string input = textBoxDateHire.Text;

            // Если пользователь ничего не ввел (или ввел только точки маски "__.__.____")
            if (string.IsNullOrWhiteSpace(input) || input == "..")
            {
                textBoxDateHire.SetErrorState(false);
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
                textBoxDateHire.ErrorMessage = "Дата введена неверно";
                textBoxDateHire.SetErrorState(true);
            }
            else
            {
                // Всё ок — сбрасываем ошибку и обновляем скрытый календарь
                textBoxDateHire.SetErrorState(false);
                dateTimePicker1.Value = parsedDate;
            }
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            // Показываем выпадающий календарь стандартного DateTimePicker
            // Для этого сам datetimePicker1 должен лежать на форме (можно скрыть его)
            // Проверяем существование даты (защита от 32.01.2026, 29.02.2025 и т.д.)
            string input = textBoxDateFire.Text;

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
            textBoxDateFire.Text = dateTimePicker2.Value.ToString("dd.MM.yyyy");

            // Возвращаем фокус на текстовое поле
            textBoxDateFire.Focus();
        }

        private void textBoxDateDeComm_Leave(object sender, EventArgs e)
        {
            string input = textBoxDateFire.Text;

            // Если пользователь ничего не ввел (или ввел только точки маски "__.__.____")
            if (string.IsNullOrWhiteSpace(input) || input == "..")
            {
                textBoxDateFire.SetErrorState(false);
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
                textBoxDateFire.ErrorMessage = "Дата введена неверно";
                textBoxDateFire.SetErrorState(true);
            }
            else
            {
                // Всё ок — сбрасываем ошибку и обновляем скрытый календарь
                textBoxDateFire.SetErrorState(false);
                dateTimePicker1.Value = parsedDate;
            }
        }

        private void buttonDecommission_Click(object sender, EventArgs e)
        {
            if (_isActive)
            {
                _isActive = false;

                textBoxDateFire.Visible = true;
                buttonCalendarFire.Visible = true;

                buttonFire.Text = "Отмена";

                dateTimePicker2.Value = DateTime.Now;
            }
            else
            {
                _isActive = true;

                textBoxDateFire.Visible = false;
                buttonCalendarFire.Visible = false;

                buttonFire.Text = "Уволить";
            }
        }

        private void comboBoxTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_edit && !_isLoad)
            {
                //textBoxDateTemplateStart.Text = DateTime.Now.ToString("dd.MM.yyyy");
            }
        }

        private void comboBoxStaffingMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_edit && !_isLoad)
            {
                //textBoxDateStaffingModeStart.Text = DateTime.Now.ToString("dd.MM.yyyy");
            }
        }

        private async void ButtonNewSchedules_ClickAsync(object sender, EventArgs e)
        {
            /*FormSelectSchedule selectSchedule = new FormSelectSchedule(_equipmentID);
            selectSchedule.Owner = this;

            DialogResult result = selectSchedule.ShowDialog();

            await Task.Delay(100);

            if (result == DialogResult.OK)
            {
                var newAssignment = new PendingScheduleAssignment
                {
                    TemplateId = selectSchedule.SelectedTemplateId,
                    TemplateName = selectSchedule.SelectedTemplateName,
                    ValidFrom = selectSchedule.SelectedDate
                };

                _scheduleBuffer = new List<PendingScheduleAssignment>
                {
                    newAssignment
                };

                comboBoxTemplates.SelectedIndex = _scheduleTemplates.FindIndex(s => s.Id == selectSchedule.SelectedTemplateId);
                maskedTextBoxTemplateDate.Text = selectSchedule.SelectedDate.ToString("dd.MM.yyyy");

                comboBoxTemplates.Refresh();

                comboBoxTemplates.Enabled = false;
                maskedTextBoxTemplateDate.Enabled = false;
                buttonCalendarTemplate.Enabled = false;
            }*/
        }

        private async void buttonNewStaffing_Click(object sender, EventArgs e)
        {
            /*FormSelectStaffingMode selectSchedule = new FormSelectStaffingMode(_equipmentID);
            selectSchedule.Owner = this;

            DialogResult result = selectSchedule.ShowDialog();

            await Task.Delay(100);

            if (result == DialogResult.OK)
            {
                var newAssignment = new PendingStaffingAssignment
                {
                    StaffingMode = selectSchedule.SelectedStaffingMode,
                    ValidFrom = selectSchedule.SelectedDate
                };

                _staffingBuffer = new List<PendingStaffingAssignment>
                {
                    newAssignment
                };

                comboBoxEquipments.SelectedIndex = Array.IndexOf(_staffingMode, selectSchedule.SelectedStaffingMode);
                maskedTextBoxEquipmentDate.Text = selectSchedule.SelectedDate.ToString("dd.MM.yyyy");

                comboBoxEquipments.Refresh();

                comboBoxEquipments.Enabled = false;
                maskedTextBoxEquipmentDate.Enabled = false;
                buttonCalendarEquipmentAssign.Enabled = false;
            }*/
        }

        private void buttonPositionAssignNew_Click(object sender, EventArgs e)
        {

        }
    }
}
