using data;
using database;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Production_planning
{
    public partial class FormSelectAssignmentUser : MaterialForm
    {
        ConnectionParameter parameter = new ConnectionParameter();
        private List<AvailableEmployeeRow> _employees;
        private DateTime _date;
        private ulong _shiftId;
        private ulong _currentValue;
        private ShiftDefinitionModel _shiftDefinition;

        public FormSelectAssignmentUser(DateTime date, ulong shiftId, ulong currentValue)
        {
            InitializeComponent();

            _date = date;
            _shiftId = shiftId;
            _currentValue = currentValue;
            
            //materialButton1.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;
        }

        private async void FormSelectAssignmentUser_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");

            var report = new ShiftService(parameter.GetMySQLConnectionString());

            _shiftDefinition = await report.GetShiftByIdAsync(_shiftId);

            labelDate.Text = _date.ToString("D");
            labelShift.Text = "Смена №" + _shiftDefinition.ShiftNumber.ToString() + ". " + _shiftDefinition.Name;

            await LoadAvailableEmployeesGrid();
        }

        private async Task LoadAvailableEmployeesGrid()
        {
            try
            {
                // 1. Вызываем метод репозитория (который теперь дергает Хранимую процедуру)
                var report = new EmployeeManagementService(parameter.GetMySQLConnectionString());
                _employees = await report.GetAvailableEmployeesForShiftAsync(_date, _shiftDefinition.ShiftNumber);

                // 2. СТРОИМ СТРУКТУРУ КОЛОНОК С НУЛЯ (чтобы исключить баги дизайнера)
                gridEmployees.Columns.Clear();
                gridEmployees.Rows.Clear();

/*                gridEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                gridEmployees.MultiSelect = false;
                gridEmployees.ReadOnly = true;
                gridEmployees.RowHeadersVisible = false;
                gridEmployees.AllowUserToAddRows = false;*/

                // Создаем 4 нужных колонки
                int colName = gridEmployees.Columns.Add("EmployeeName", "Сотрудник");
                int colStatus = gridEmployees.Columns.Add("CurrentStatus", "Статус доступности");
                int colActivity = gridEmployees.Columns.Add("CurrentActivity", "Текущая активность");
                int colAdjacentBefore = gridEmployees.Columns.Add("AdjacentBefore", "Смежная смена (ДО)");
                int colAdjacentAfter = gridEmployees.Columns.Add("AdjacentAfter", "Смежная смена (ПОСЛЕ)");

                // Настраиваем адаптивную ширину колонок под размеры окна
                gridEmployees.Columns[colName].Width = 140;
                gridEmployees.Columns[colStatus].Width = 220;
                gridEmployees.Columns[colActivity].Width = 230;
                gridEmployees.Columns[colAdjacentBefore].Width = 230;
                gridEmployees.Columns[colAdjacentAfter].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                // 3. НАПОЛНЯЕМ ТАБЛИЦУ СТРОКАМИ
                foreach (var emp in _employees)
                {
                    // Поскольку сами данные из MySQL приходят на русском ("🟢 ДОСТУПЕН", "⛔ ЗАНЯТ"),
                    // мы берем их напрямую из вашей английской модели DTO
                    string statusText = emp.CurrentStatus ?? string.Empty;

                    // Принудительно добавляем строку во вновь созданные колонки!
                    int rowIndex = gridEmployees.Rows.Add(
                        emp.EmployeeName,
                        statusText,
                        emp.CurrentActivity ?? "---",
                        emp.AdjacentBefore ?? "---",
                        emp.AdjacentAfter ?? "---"
                    );

                    gridEmployees.Rows[rowIndex].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);

                    // Бережно прячем EmployeeId в Tag созданной строки [2026-09-04]
                    gridEmployees.Rows[rowIndex].Tag = emp.EmployeeId;

                    EmployeeShiftStatus status = (EmployeeShiftStatus)emp.StatusCode;

                    // Процессор обрабатывает переключение чисел (switch) в тысячи раз быстрее, чем текстовый Contains!
                    switch (status)
                    {
                        case EmployeeShiftStatus.Available: // 3
                            gridEmployees.Rows[rowIndex].Cells[1].Style.ForeColor = Color.ForestGreen;
                            break;

                        case EmployeeShiftStatus.AvailableAdjacent: // 2
                            gridEmployees.Rows[rowIndex].Cells[1].Style.ForeColor = Color.Orange;
                            break;

                        case EmployeeShiftStatus.Busy: // 1
                        case EmployeeShiftStatus.Absent: // 0
                            gridEmployees.Rows[rowIndex].Cells[1].Style.ForeColor = Color.Red;
                            gridEmployees.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Gray;
                            break;
                    }
                }

                // 5. ОЧИСТКА: Снимаем автоматический фокус с первой строки при старте [2026-09-01]
                gridEmployees.ClearSelection();
                gridEmployees.CurrentCell = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка заполнения таблицы сотрудников: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SaveAssignmentUser()
        {
            var report = new EmployeeManagementService(parameter.GetMySQLConnectionString());

            if (gridEmployees.CurrentRow == null || gridEmployees.CurrentRow.Tag == null)
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника из таблицы!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int rowId = gridEmployees.CurrentRow.Index;
            ulong employeeId = (ulong)gridEmployees.CurrentRow.Tag;

            AvailableEmployeeRow employee = _employees[rowId];

            string statusCurrent = employee.CurrentStatus;

            EmployeeShiftStatus status = (EmployeeShiftStatus)employee.StatusCode;

            // 🛑 ЖЕСТКАЯ ЗАЩИТА ТК: Запрещаем назначать занятых!
            if (status == EmployeeShiftStatus.Busy)
            {
                MessageBox.Show(
                    $"Невозможно назначить данного сотрудника! Текущий статус: {statusCurrent}.\n" +
                    "Выберите человека с зеленым или желтым статусом доступности.",
                    "Кадровое ограничение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop
                );
                return;
            }

            // Если статус желтый (смежные смены), выводим предупреждение, но разрешаем (по усмотрению мастера)
            if (status == EmployeeShiftStatus.Absent)
            {
                var warn = MessageBox.Show(
                    $"Внимание! Текущий статус сотрудника: {statusCurrent}.\n" +
                    "Вы уверены, что хотите назначить его сверхурочно?",
                    "Предупреждение о переработке",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                if (warn == DialogResult.No) return;
            }

            // Если статус желтый (смежные смены), выводим предупреждение, но разрешаем (по усмотрению мастера)
            if (status == EmployeeShiftStatus.AvailableAdjacent)
            {
                var warn = MessageBox.Show(
                    "Внимание! У сотрудника есть смежные смены (интервал отдыха менее 8 часов).\n" +
                    "Вы уверены, что хотите назначить его сверхурочно?",
                    "Предупреждение о переработке",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                if (warn == DialogResult.No) return;
            }

            // Сохраняем оверрайд назначения на текущий станок карточки смены [2026-09-04]
            string comment = textBoxComment.Text.Trim();
            await report.AssignEmployeeToShiftAsync(_date, _shiftId, _currentValue, employeeId, 2, comment);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private async void materialButton1_Click(object sender, EventArgs e)
        {
            await SaveAssignmentUser();
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
