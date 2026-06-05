using data;
using database;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Production_planning
{
    public partial class FormAddArea : MaterialForm
    {
        private readonly bool _edit;
        private WorkAreaInfo _workArea;

        public FormAddArea(WorkAreaInfo workArea = null)
        {
            InitializeComponent();

            _edit = workArea != null;
            _workArea = workArea ?? new WorkAreaInfo();
            materialButton1.Text = _edit ? "Сохранить" : "Добавить";
            
            materialButton1.DialogResult = DialogResult.OK;
            materialButton2.DialogResult = DialogResult.Cancel;
        }


        private async Task LoadWorkArea()
        {
            if (!_edit)
            {
                
            }
            else
            {
                textBoxWorkAreaName.Text = _workArea.Name;
            }
        }

        private async Task SaveWorkArea()
        {
            ConnectionParameter parameter = new ConnectionParameter();

            var workAreaService = new WorkAreaService(parameter.GetMySQLConnectionString());

            if (!_edit)
            {
                await workAreaService.CreateWorkAreaAsync(textBoxWorkAreaName.Text);
            }
            else
            {
                await workAreaService.UpdateWorkAreaNameAsync(_workArea.Id, textBoxWorkAreaName.Text);
            }
        }


        private async void FormAddCycle_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");

            await LoadWorkArea();
        }

        private async void materialButton1_Click(object sender, EventArgs e)
        {
            await SaveWorkArea();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
