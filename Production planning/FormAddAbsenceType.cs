using data;
using MaterialSkin.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Production_planning
{
    public partial class FormAddAbsenceType : MaterialForm
    {
        private readonly bool _edit;
        private readonly AbsenceType _model;
        private string _selectedHexColor = "#FF6347"; // Цвет по умолчанию (Tomato)

        public AbsenceType ResultModel { get; private set; }

        public FormAddAbsenceType(AbsenceType model = null)
        {
            InitializeComponent();

            _edit = model != null;
            _model = model ?? new AbsenceType();

            materialButtonSave.Text = _edit ? "Сохранить" : "Добавить";
            this.Text = _edit ? "Редактирование причины отсутствия" : "Добавление причины отсутствия";
        }

        private void FormAddAbsenceType_Load(object sender, EventArgs e)
        {
            if (_edit)
            {
                textBoxName.Text = _model.Name;
                textBoxDescription.Text = _model.Description;
                switchEndDateRequired.Checked = _model.IsEndDateRequired;
                _selectedHexColor = string.IsNullOrWhiteSpace(_model.StatusColor) ? "#FF6347" : _model.StatusColor;
            }

            // Устанавливаем превью цвета
            UpdateColorPreview(_selectedHexColor);
        }

        private void btnSelectColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                try
                {
                    colorDialog.Color = ColorTranslator.FromHtml(_selectedHexColor);
                }
                catch { colorDialog.Color = Color.Tomato; }

                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    // Конвертируем Color обратно в HEX для MySQL
                    _selectedHexColor = "#" + (colorDialog.Color.ToArgb() & 0x00FFFFFF).ToString("X6");
                    UpdateColorPreview(_selectedHexColor);
                }
            }
        }

        private void UpdateColorPreview(string hexColor)
        {
            try
            {
                panelColorPreview.BackColor = ColorTranslator.FromHtml(hexColor);
            }
            catch { panelColorPreview.BackColor = Color.Tomato; }
        }

        private void materialButtonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                MessageBox.Show("Наименование причины является обязательным полем!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None; // Не закрываем форму
                return;
            }

            ResultModel = new AbsenceType
            {
                Id = _model.Id,
                Name = textBoxName.Text.Trim(),
                Description = textBoxDescription.Text.Trim(),
                IsEndDateRequired = switchEndDateRequired.Checked,
                StatusColor = _selectedHexColor,
                IsActive = _edit ? _model.IsActive : true // Новая запись всегда активна
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
