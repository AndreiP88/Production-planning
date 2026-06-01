using data;
using database;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Production_planning
{
    public partial class EditShiftForm : MaterialForm
    {
        private DateTime _date;
        private int _shiftNumber;
        private int _equipID;

        EquipmentShiftCard shiftCard;

        public EditShiftForm(DateTime date, int shiftNumber, int equipID)
        {
            InitializeComponent();

            _date = date;
            _shiftNumber = shiftNumber;
            _equipID = equipID;
        }

        private async void EditShiftForm_LoadAsync(object sender, EventArgs e)
        {
            ConnectionParameter parameter = new ConnectionParameter();

            var report = new ReportEquipShift(parameter.GetMySQLConnectionString());

            EquipmentShiftCard reportData = await report.GetEquipmentShiftCardAsync(_date, _shiftNumber, _equipID);

            this.Text = _date.ToString("D") + ", " + reportData.ShiftName;

            materialLabel1.Text = _date.ToString("D") + " Смена: " + _shiftNumber;
            materialLabel2.Text = reportData.EquipmentName;
        }
    }
}
