using data;
using database;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Production_planning
{
    public partial class FormAddAbsence : MaterialForm
    {
        //private bool _isEdit = false;
        private int _loadType;
        private int _absenceId;
        private int _userId = -1;
        private EmployeeAbsenceRow _absence;
        private List<ActiveEmployeeRow> _employees;
        private List<AbsenceType> _types;

        /// <summary>
        /// 0 - Новое отсутствие
        /// 1 - Редактировать отсутствие
        /// 2 - Добавить отутствие для конкретного сотрудника
        /// </summary>
        /// <param name="typeLoad"></param>
        /// <param name="incertId"></param>
        public FormAddAbsence(int typeLoad = 0, int incertId = -1)
        {
            InitializeComponent();

            _loadType = typeLoad;

            if (typeLoad < 0 && typeLoad >2)
                return;

            if (typeLoad == 1)
            {
                _absenceId = incertId;
            }

            if (typeLoad == 2)
            {
                _userId = incertId;
            }

            //materialButton1.DialogResult = DialogResult.OK;
            materialButton2.DialogResult = DialogResult.Cancel;
        }

        private async Task LoadUsers()
        {
            _employees?.Clear();

            ConnectionParameter parameter = new ConnectionParameter();

            try
            {
                var service = new EmployeeManagementService(parameter.GetMySQLConnectionString());

                _employees = await service.GetCurrentActiveEmployeesAsync();

                comboBoxUsers.Items.Clear();

                comboBoxUsers.DisplayMember = "FullName";
                comboBoxUsers.ValueMember = "Id";
                comboBoxUsers.DataSource = _employees;

                comboBoxUsers.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения списка сотрудников:\n {ex}");
            }
        }

        private async Task LoadTypes()
        {
            _types?.Clear();

            ConnectionParameter parameter = new ConnectionParameter();

            try
            {
                var service = new EmployeeManagementService(parameter.GetMySQLConnectionString());

                _types = await service.GetAllAbsenceTypesAsync();

                comboBoxAbsence.Items.Clear();

                comboBoxAbsence.DisplayMember = "Name";
                comboBoxAbsence.ValueMember = "Id";
                comboBoxAbsence.DataSource = _types;

                comboBoxAbsence.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения списка:\n {ex}");
            }
        }

        private async Task LoadAbsence()
        {
            if (_absenceId != -1)
            {
                ConnectionParameter parameter = new ConnectionParameter();

                try
                {
                    var service = new EmployeeManagementService(parameter.GetMySQLConnectionString());

                    _absence = await service.GetAbsenceByIdAsync((ulong)_absenceId);

                    comboBoxAbsence.SelectedValue = _absence.TypeId;

                    comboBoxUsers.SelectedValue = _absence.EmployeeId;

                    dateTimeStart.Value = _absence.StartDate;
                    //textBoxDateStart.Text = _absence.StartDate.ToString("dd.MM.yyyy");

                    if (_absence.EndDate != null)
                    {
                        dateTimeEnd.Value = (DateTime)_absence.EndDate;
                        //textBoxDateEnd.Text = _absence.EndDate.Value.ToString("dd.MM.yyyy");
                    }
                    else
                    {
                        switchOpenDate.Checked = true;
                        dateTimeEnd.Value = DateTime.Now;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка получения информации:\n {ex}");
                }
            }
        }

        private async Task NewAbsenceForUser()
        {
            if (_userId != -1)
            {
                comboBoxUsers.SelectedValue = (ulong)_userId;
            }
        }

        private async Task<bool> SaveAbsence()
        {
            bool result = false;

            if (comboBoxUsers.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите сотрудника из списка", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }

            if (comboBoxAbsence.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите причину из списка", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }

            if (textBoxDateStart.GetErrorState())
            {
                MessageBox.Show("Укажите начальную дату", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }

            if (comboBoxAbsence.SelectedItem is AbsenceType selectedType)
            {
                bool isRequired = selectedType.IsEndDateRequired;
                
                if (isRequired && switchOpenDate.Checked)
                {
                    MessageBox.Show("Дата завершения обязательна. Укажите конечную дату", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return result;
                }
            }

            if (textBoxDateEnd.GetErrorState() && !switchOpenDate.Checked)
            {
                MessageBox.Show("Укажите конечную дату", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }

            DateTime? endDate = !switchOpenDate.Checked ? dateTimeEnd.Value : (DateTime?)null;

            RegisterAbsenceCommand registerAbsence = new RegisterAbsenceCommand
            {
                EmployeeId = (ulong)comboBoxUsers.SelectedValue,
                TypeId = (ulong)comboBoxAbsence.SelectedValue,
                StartDate = dateTimeStart.Value,
                EndDate = endDate
            };

            ConnectionParameter parameter = new ConnectionParameter();

            try
            {
                var service = new EmployeeManagementService(parameter.GetMySQLConnectionString());

                if (_loadType == 0 || _loadType == 2)
                {
                    await service.TryRegisterAbsenceAsync(registerAbsence);
                }
                
                if (_loadType == 1)
                {
                    await service.UpdateAbsenceAsync((ulong)_absenceId, registerAbsence);
                }

                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления нового периода:\n {ex}");
            }

            return result;
        }

        private async void materialButton1_Click(object sender, EventArgs e)
        {
            if (await SaveAbsence())
            {
                materialButton1.Enabled = false;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ChangeMethodInputDateEndValue()
        {
            if (radioButtonDateEnd.Checked)
            {
                tableLayoutPanel5.Enabled = true;
                tableLayoutPanel6.Enabled = false;
            }

            if (radioButtonCountDays.Checked)
            {
                tableLayoutPanel5.Enabled = false;
                tableLayoutPanel6.Enabled = true;
            }
        }

        private async void FormAddAbsence_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");

            await LoadUsers();
            await LoadTypes();

            if (_loadType == 0)
            {
                comboBoxUsers.Enabled = true;
                textBoxDateStart.Text = DateTime.Now.ToString("dd.MM.yyyy");
            }

            if (_loadType == 1)
            {
                await LoadAbsence();

                comboBoxUsers.Enabled = false;
            }

            if (_loadType == 2)
            {
                await NewAbsenceForUser();

                comboBoxUsers.Enabled = false;
            }
        }

        private void switchOpenDate_CheckedChanged(object sender, EventArgs e)
        {
            tableLayoutPanel7.Enabled = !switchOpenDate.Checked;
        }

        private void radioButtonDateEnd_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonCountDays.Checked = !radioButtonDateEnd.Checked;

            ChangeMethodInputDateEndValue();
        }

        private void radioButtonCountDays_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonDateEnd.Checked = !radioButtonCountDays.Checked;

            ChangeMethodInputDateEndValue();
        }

        private void materialTextBoxDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем ввод только цифр (\d) и клавиш управления (например, Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // Передаем true, чтобы отменить (подавить) нажатие клавиши
                e.Handled = true;
            }
        }

        private void materialTextBoxDays_TextChanged(object sender, EventArgs e)
        {
            // Если в поле появились любые символы, кроме цифр (например, после вставки мышки)
            if (Regex.IsMatch(materialTextBoxDays.Text, "[^0-9]"))
            {
                // Очищаем строку от букв, оставляя только цифры
                materialTextBoxDays.Text = Regex.Replace(materialTextBoxDays.Text, "[^0-9]", "");

                // Возвращаем курсор в самый конец строки
                materialTextBoxDays.SelectionStart = materialTextBoxDays.Text.Length;
            }

            if (radioButtonCountDays.Checked)
            {
                int days = Convert.ToInt32(materialTextBoxDays.Text);

                if (days < 999999)
                {
                    dateTimeEnd.Value = dateTimeStart.Value.AddDays(days - 1);
                }
            }
        }

        private void comboBoxAbsence_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loadType == 0)
            {
                if (comboBoxAbsence.SelectedIndex != -1)
                {
                    if (comboBoxAbsence.SelectedItem is AbsenceType selectedType)
                    {
                        switchOpenDate.Checked = !selectedType.IsEndDateRequired;
                    }
                }
                else
                {
                    switchOpenDate.Checked = false;
                }
            }
        }

        private void CalculateDays()
        {
            DateTime start = dateTimeStart.Value.Date;
            DateTime end = dateTimeEnd.Value.Date;

            if (start > end)
            {
                textBoxDateEnd.SetErrorState(true);
                textBoxDateEnd.ErrorMessage = "Неверная дата";
            }
            else
            {
                textBoxDateEnd.SetErrorState(false);
            }

            if (radioButtonDateEnd.Checked && end >= start)
            {
                int days = (int)(end - start).TotalDays + 1;

                materialTextBoxDays.Text = days.ToString();
            }
        }

        private void dateTimeStart_ValueChanged(object sender, EventArgs e)
        {
            // Записываем дату в нужном формате в MaterialTextBox
            textBoxDateStart.Text = dateTimeStart.Value.ToString("dd.MM.yyyy");

            CalculateDays();
        }

        private void dateTimeStart_CloseUp(object sender, EventArgs e)
        {
            // Возвращаем фокус на текстовое поле
            textBoxDateStart.Focus();
        }

        private void textBoxDateStart_Leave(object sender, EventArgs e)
        {
            string input = textBoxDateStart.Text;

            // Если пользователь ничего не ввел (или ввел только точки маски "__.__.____")
            if (string.IsNullOrWhiteSpace(input) || input == "..")
            {
                textBoxDateStart.SetErrorState(false);
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
                textBoxDateStart.ErrorMessage = "Дата введена неверно";
                textBoxDateStart.SetErrorState(true);
            }
            else
            {
                // Всё ок — сбрасываем ошибку и обновляем скрытый календарь
                textBoxDateStart.SetErrorState(false);
                dateTimeStart.Value = parsedDate;
            }
        }

        private void buttonCalendarStart_Click(object sender, EventArgs e)
        {
            // Показываем выпадающий календарь стандартного DateTimePicker
            // Для этого сам datetimePicker1 должен лежать на форме (можно скрыть его)
            // Проверяем существование даты (защита от 32.01.2026, 29.02.2025 и т.д.)
            string input = textBoxDateStart.Text;

            DateTime parsedDate;
            bool isValidDate = DateTime.TryParseExact(input, "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out parsedDate);

            if (isValidDate)
            {
                dateTimeStart.Value = parsedDate;
            }

            dateTimeStart.Focus();

            SendKeys.Send("%{DOWN}");
        }
        
        private void dateTimeEnd_ValueChanged(object sender, EventArgs e)
        {
            // Записываем дату в нужном формате в MaterialTextBox
            textBoxDateEnd.Text = dateTimeEnd.Value.ToString("dd.MM.yyyy");

            CalculateDays();
        }

        private void dateTimeEnd_CloseUp(object sender, EventArgs e)
        {
            // Возвращаем фокус на текстовое поле
            textBoxDateEnd.Focus();
        }

        private void textBoxDateEnd_Leave(object sender, EventArgs e)
        {
            string input = textBoxDateEnd.Text;

            // Если пользователь ничего не ввел (или ввел только точки маски "__.__.____")
            if (string.IsNullOrWhiteSpace(input) || input == "..")
            {
                textBoxDateEnd.SetErrorState(false);
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
                textBoxDateEnd.ErrorMessage = "Дата введена неверно";
                textBoxDateEnd.SetErrorState(true);
            }
            else
            {
                // Всё ок — сбрасываем ошибку и обновляем скрытый календарь
                textBoxDateEnd.SetErrorState(false);
                dateTimeEnd.Value = parsedDate;
            }
        }

        private void buttonCalendarEnd_Click(object sender, EventArgs e)
        {
            // Показываем выпадающий календарь стандартного DateTimePicker
            // Для этого сам datetimePicker1 должен лежать на форме (можно скрыть его)
            // Проверяем существование даты (защита от 32.01.2026, 29.02.2025 и т.д.)
            string input = textBoxDateEnd.Text;

            DateTime parsedDate;
            bool isValidDate = DateTime.TryParseExact(input, "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out parsedDate);

            if (isValidDate)
            {
                dateTimeEnd.Value = parsedDate;
            }

            dateTimeEnd.Focus();

            SendKeys.Send("%{DOWN}");
        }

        private void buttonDayDecrement_Click(object sender, EventArgs e)
        {
            try
            {
                int input = Convert.ToInt32(materialTextBoxDays.Text);

                if (input > 1)
                {
                    input--;
                }

                materialTextBoxDays.Text = input.ToString();
            }
            catch (Exception ex)
            {
                materialTextBoxDays.ErrorMessage = "Неверный ввод";
                materialTextBoxDays.SetErrorState(true);
            }
        }

        private void buttonDayIncrement_Click(object sender, EventArgs e)
        {
            try
            {
                int input = Convert.ToInt32(materialTextBoxDays.Text);

                if (input < 999999)
                {
                    input++;
                }

                materialTextBoxDays.Text = input.ToString();
            }
            catch (Exception ex)
            {
                materialTextBoxDays.ErrorMessage = "Неверный ввод";
                materialTextBoxDays.SetErrorState(true);
            }
        }

        private void materialTextBoxDays_Click(object sender, EventArgs e)
        {

        }

        
    }
}
