using data;
using database;
using MaterialSkin.Controls;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Production_planning
{
    public partial class FormAddShift : MaterialForm
    {
        private readonly bool _edit;
        private ShiftDefinitionModel _shiftDefinition;

        string lastShiftNumber = string.Empty;
        string lastShiftStart = string.Empty;
        string lastShiftEnd = string.Empty;


        public FormAddShift(ShiftDefinitionModel shiftDefinition = null)
        {
            InitializeComponent();

            _edit = shiftDefinition != null;
            _shiftDefinition = shiftDefinition ?? new ShiftDefinitionModel();
            materialButton1.Text = _edit ? "Сохранить" : "Добавить";

            materialButton1.DialogResult = DialogResult.OK;
            materialButton2.DialogResult = DialogResult.Cancel;
        }

        private void FormAddShift_Load(object sender, EventArgs e)
        {
            LoadShiftDefinition();
        }

        private void LoadShiftDefinition()
        {
            if (_edit)
            {
                if (_shiftDefinition.ShiftNumber == 0)
                {
                    materialCheckbox1.Checked = true;
                }

                textBoxShiftNum.Text = _shiftDefinition.ShiftNumber.ToString();
                textBoxShiftName.Text = _shiftDefinition.Name;
                maskedTextBoxShiftStart.Text = _shiftDefinition.StartTime.ToString(@"hh\:mm");
                maskedTextBoxShiftEnd.Text = _shiftDefinition.EndTime.ToString(@"hh\:mm");
            }
        }

        private async Task SaveShiftDefinition()
        {
            ConnectionParameter parameter = new ConnectionParameter();

            var shiftService = new ShiftService(parameter.GetMySQLConnectionString());

            TimeSpan timeStart = TimeSpan.TryParse(maskedTextBoxShiftStart.Text, out TimeSpan timeS) ? timeS : TimeSpan.Zero;
            TimeSpan timeEnd = TimeSpan.TryParse(maskedTextBoxShiftEnd.Text, out TimeSpan timeE) ? timeE : TimeSpan.Zero;

            if (!_edit)
            {
                var newShift = new ShiftDefinitionModel
                {
                    ShiftNumber = Convert.ToInt32(textBoxShiftNum.Text),
                    Name = textBoxShiftName.Text,
                    StartTime = timeStart,
                    EndTime = timeEnd
                };

                await shiftService.CreateShiftAsync(newShift);
            }
            else
            {
                /*ShiftDefinitionModel newShift = _shiftDefinition;

                newShift.ShiftNumber = Convert.ToInt32(textBoxShiftNum.Text);
                newShift.Name = textBoxShiftName.Text;
                newShift.StartTime = timeStart;
                newShift.EndTime = timeEnd;*/

                var newShift = new ShiftDefinitionModel
                {
                    Id = _shiftDefinition.Id,
                    ShiftNumber = Convert.ToInt32(textBoxShiftNum.Text),
                    Name = textBoxShiftName.Text,
                    StartTime = timeStart,
                    EndTime = timeEnd
                };

                await shiftService.UpdateShiftAsync(newShift);
            }
        }

        private void materialTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем ввод только цифр и клавиши Backspace (чтобы можно было стирать)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Отменяет ввод символа
            }
        }

        private void materialTextBox1_TextChanged(object sender, EventArgs e)
        {
            // Удаляет все символы, которые не являются цифрами
            string originalText = textBoxShiftNum.Text;
            string filteredText = System.Text.RegularExpressions.Regex.Replace(originalText, @"[^\d]", "");

            if (originalText != filteredText)
            {
                textBoxShiftNum.Text = filteredText;
                // Возвращаем курсор в конец строки
                textBoxShiftNum.SelectionStart = textBoxShiftNum.Text.Length;
            }
        }

        private void materialCheckbox1_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckbox1.Checked)
            {
                lastShiftNumber = textBoxShiftNum.Text;
                lastShiftStart = maskedTextBoxShiftStart.Text;
                lastShiftEnd = maskedTextBoxShiftEnd.Text;

                textBoxShiftNum.Text = "0";
                maskedTextBoxShiftStart.Text = "00:00";
                maskedTextBoxShiftEnd.Text = "00:00";

                textBoxShiftNum.Enabled = false;
                maskedTextBoxShiftEnd.Enabled = false;
                maskedTextBoxShiftStart.Enabled = false;
            }
            else
            {
                textBoxShiftNum.Text = lastShiftNumber;
                maskedTextBoxShiftStart.Text = lastShiftStart;
                maskedTextBoxShiftEnd.Text = lastShiftEnd;

                textBoxShiftNum.Enabled = true;
                maskedTextBoxShiftEnd.Enabled = true;
                maskedTextBoxShiftStart.Enabled = true;
            }
        }

        private void maskedTextBoxShiftStart_Leave(object sender, EventArgs e)
        {
            // Очищаем текст от пробелов и маски, чтобы узнать реальное количество цифр
            string rawText = maskedTextBoxShiftStart.Text.Replace(":", "").Trim();

            // Если введены все 4 цифры (например, "12" и "30")
            if (rawText.Length == 4)
            {
                // Пытаемся распарсить корректность времени
                if (!TimeSpan.TryParse(maskedTextBoxShiftStart.Text, out TimeSpan time))
                {
                    //MessageBox.Show("Введите корректное время от 00:00 до 23:59", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    maskedTextBoxShiftStart.Text = "23:59";
                    maskedTextBoxShiftStart.Focus();
                    maskedTextBoxShiftStart.SelectAll();
                }
            }
            else if (rawText.Length > 0)
            {
                // Если пользователь начал вводить время, но заполнил поле не до конца (например, "12:__")
                //MessageBox.Show("Поле времени заполнено не полностью!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                maskedTextBoxShiftStart.Text = "23:59";
                maskedTextBoxShiftStart.Focus();
            }
        }

        private void maskedTextBoxShiftEnd_Leave(object sender, EventArgs e)
        {
            // Очищаем текст от пробелов и маски, чтобы узнать реальное количество цифр
            string rawText = maskedTextBoxShiftEnd.Text.Replace(":", "").Trim();

            // Если введены все 4 цифры (например, "12" и "30")
            if (rawText.Length == 4)
            {
                // Пытаемся распарсить корректность времени
                if (!TimeSpan.TryParse(maskedTextBoxShiftEnd.Text, out TimeSpan time))
                {
                    //MessageBox.Show("Введите корректное время от 00:00 до 23:59", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    maskedTextBoxShiftEnd.Text = "23:59";
                    maskedTextBoxShiftEnd.Focus();
                    maskedTextBoxShiftEnd.SelectAll();
                }
            }
            else if (rawText.Length > 0)
            {
                // Если пользователь начал вводить время, но заполнил поле не до конца (например, "12:__")
                //MessageBox.Show("Поле времени заполнено не полностью!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                maskedTextBoxShiftEnd.Text = "23:59";
                maskedTextBoxShiftEnd.Focus();
            }
        }

        private async void materialButton1_Click(object sender, EventArgs e)
        {
            await SaveShiftDefinition();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
