using data;
using database;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Production_planning
{
    public partial class FormAddCycle : MaterialForm
    {
        private readonly bool _edit;
        private ScheduleCycleModel _scheduleCycle;
        private List<ShiftDefinitionModel> _shiftDefinitions;

        public FormAddCycle(ScheduleCycleModel scheduleCycle = null)
        {
            InitializeComponent();

            _edit = scheduleCycle != null;
            _scheduleCycle = scheduleCycle ?? new ScheduleCycleModel();
            materialButton1.Text = _edit ? "Сохранить" : "Добавить";

            materialButton1.DialogResult = DialogResult.OK;
            materialButton2.DialogResult = DialogResult.Cancel;
        }

        private void LoadScheduleCycle()
        {
            listViewCycleItem.Items.Clear();

            if (_edit)
            {
                textBoxCycleName.Text = _scheduleCycle.Name;
                labelCycleLeght.Text = _scheduleCycle.CycleLength.ToString();

                /*int index = 1;

                foreach (CycleItemModel item in _scheduleCycle.Items)
                {
                    ListViewItem lvItem = new ListViewItem();

                    lvItem.Text = index.ToString();
                    lvItem.SubItems.Add(item.DayNumber.ToString());
                    lvItem.SubItems.Add(item.ShiftNumber == 0 ? "" : item.ShiftNumber.ToString());
                    lvItem.SubItems.Add(item.ShiftName);

                    listViewCycleItem.Items.Add(lvItem);

                    index++;
                }*/
            }
            else
            {
                //textBoxCycleName.Text = _scheduleCycle.Name;
                labelCycleLeght.Text = _scheduleCycle.CycleLength.ToString();

                /*int index = 1;

                foreach (CycleItemModel item in _scheduleCycle.Items)
                {
                    ListViewItem lvItem = new ListViewItem();

                    lvItem.Text = index.ToString();
                    lvItem.SubItems.Add(item.DayNumber.ToString());
                    lvItem.SubItems.Add(item.ShiftNumber == 0 ? "" : item.ShiftNumber.ToString());
                    lvItem.SubItems.Add(item.ShiftName);

                    listViewCycleItem.Items.Add(lvItem);

                    index++;
                }*/
            }

            int index = 1;

            foreach (CycleItemModel item in _scheduleCycle.Items)
            {
                ListViewItem lvItem = new ListViewItem();

                lvItem.Text = index.ToString();
                lvItem.SubItems.Add(item.DayNumber.ToString());
                lvItem.SubItems.Add(item.ShiftNumber == 0 ? "" : item.ShiftNumber.ToString());
                lvItem.SubItems.Add(item.ShiftName);

                listViewCycleItem.Items.Add(lvItem);

                index++;
            }
        }

        private async Task LoadShiftsAsync()
        {
            ConnectionParameter parameter = new ConnectionParameter();

            _shiftDefinitions?.Clear();

            ShiftService shiftsDefinitionService = new ShiftService(parameter.GetMySQLConnectionString());

            _shiftDefinitions = await shiftsDefinitionService.GetAllShiftsAsync();

            comboBoxShifts.Items.Clear();

            foreach (ShiftDefinitionModel shiftDefinition in _shiftDefinitions)
            {
                comboBoxShifts.Items.Add(shiftDefinition.Name);
            }
        }

        private async Task SaveShiftDefinition()
        {
            ConnectionParameter parameter = new ConnectionParameter();

            var shiftService = new ScheduleTemplateService(parameter.GetMySQLConnectionString());

            _scheduleCycle.Name = textBoxCycleName.Text;

            if (!_edit)
            {
                /*var newCycle = new ScheduleCycleModel
                {
                    
                };*/

                

                ScheduleCycleModel newCycle = _scheduleCycle;

                await shiftService.CreateScheduleCycleAsync(newCycle);
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

                ScheduleCycleModel newCycle = _scheduleCycle;

                await shiftService.UpdateScheduleCycleAsync(newCycle);
            }
        }
        private void AddNewCicleItem(bool newDay)
        {
            if (comboBoxShifts.SelectedIndex >= 0)
            {
                int day = _scheduleCycle.CycleLength; // == 0 ? 1 : _scheduleCycle.CycleLength;
                ShiftDefinitionModel shiftSelected = _shiftDefinitions[comboBoxShifts.SelectedIndex];

                //_scheduleCycle.Name = textBoxCycleName.Text;

                if (newDay)
                {
                    _scheduleCycle.CycleLength++;
                    day++;
                }

                _scheduleCycle.Items.Add(
                    new CycleItemModel
                    {
                        DayNumber = day,
                        ShiftId = (int)shiftSelected.Id,
                        ShiftNumber = shiftSelected.ShiftNumber,
                        ShiftName = shiftSelected.Name
                    }
                );
            }
        }

        private void DeleteLastCycleItem()
        {
            if(_scheduleCycle.Items.Count > 0)
            {
                _scheduleCycle.CycleLength--;

                _scheduleCycle.Items.RemoveAt(_scheduleCycle.Items.Count - 1);
            }
        }

        private async void FormAddCycle_Load(object sender, EventArgs e)
        {
            await LoadShiftsAsync();
            LoadScheduleCycle();
        }

        private void buttonShiftAndDayAdd_Click(object sender, EventArgs e)
        {
            AddNewCicleItem(true);
            LoadScheduleCycle();
        }

        private void buttonShiftAdd_Click(object sender, EventArgs e)
        {
            AddNewCicleItem(false);
            LoadScheduleCycle();
        }

        private void buttonShiftLastDelete_Click(object sender, EventArgs e)
        {
            DeleteLastCycleItem();
            LoadScheduleCycle();
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
