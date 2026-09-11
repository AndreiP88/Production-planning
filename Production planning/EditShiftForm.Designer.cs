using System.Windows.Forms;

namespace Production_planning
{
    partial class EditShiftForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        class DoubleBufferedDataGridView : DataGridView
        {
            protected override bool DoubleBuffered { get => true; }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnManageEquipment = new MaterialSkin.Controls.MaterialButton();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.labelShiftDate = new MaterialSkin.Controls.MaterialLabel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.gridViewUserSchedule = new Production_planning.EditShiftForm.DoubleBufferedDataGridView();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonScheduleReturn = new MaterialSkin.Controls.MaterialButton();
            this.buttonScheduleEquipReplace = new MaterialSkin.Controls.MaterialButton();
            this.buttonScheduleShiftCancel = new MaterialSkin.Controls.MaterialButton();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.gridViewUserOverride = new Production_planning.EditShiftForm.DoubleBufferedDataGridView();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonAssignReturn = new MaterialSkin.Controls.MaterialButton();
            this.btnAssignEmployee = new MaterialSkin.Controls.MaterialButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblStaffingRequirement = new System.Windows.Forms.Label();
            this.lblEquipmentState = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewUserSchedule)).BeginInit();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewUserOverride)).BeginInit();
            this.tableLayoutPanel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.Controls.Add(this.btnManageEquipment, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.materialLabel1, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.materialLabel2, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.labelShiftDate, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblStaffingRequirement, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblEquipmentState, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 11);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 88);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 12;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(702, 614);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnManageEquipment
            // 
            this.btnManageEquipment.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnManageEquipment.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnManageEquipment.Depth = 0;
            this.btnManageEquipment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnManageEquipment.HighEmphasis = true;
            this.btnManageEquipment.Icon = null;
            this.btnManageEquipment.Location = new System.Drawing.Point(19, 126);
            this.btnManageEquipment.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnManageEquipment.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnManageEquipment.Name = "btnManageEquipment";
            this.btnManageEquipment.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnManageEquipment.Size = new System.Drawing.Size(664, 38);
            this.btnManageEquipment.TabIndex = 2;
            this.btnManageEquipment.Text = "🚫 Отменить смену на рабочем месте";
            this.btnManageEquipment.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnManageEquipment.UseAccentColor = false;
            this.btnManageEquipment.UseVisualStyleBackColor = true;
            this.btnManageEquipment.Click += new System.EventHandler(this.buttonEquipCancel_Click);
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto Medium", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.FontType = MaterialSkin.MaterialSkinManager.fontType.H6;
            this.materialLabel1.Location = new System.Drawing.Point(18, 178);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(666, 30);
            this.materialLabel1.TabIndex = 8;
            this.materialLabel1.Text = "Запланировано";
            this.materialLabel1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto Medium", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel2.FontType = MaterialSkin.MaterialSkinManager.fontType.H6;
            this.materialLabel2.Location = new System.Drawing.Point(18, 356);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(666, 30);
            this.materialLabel2.TabIndex = 9;
            this.materialLabel2.Text = "Назначено";
            this.materialLabel2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelShiftDate
            // 
            this.labelShiftDate.AutoSize = true;
            this.labelShiftDate.Depth = 0;
            this.labelShiftDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelShiftDate.Font = new System.Drawing.Font("Roboto Medium", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.labelShiftDate.FontType = MaterialSkin.MaterialSkinManager.fontType.H6;
            this.labelShiftDate.Location = new System.Drawing.Point(18, 0);
            this.labelShiftDate.MouseState = MaterialSkin.MouseState.HOVER;
            this.labelShiftDate.Name = "labelShiftDate";
            this.labelShiftDate.Size = new System.Drawing.Size(666, 40);
            this.labelShiftDate.TabIndex = 0;
            this.labelShiftDate.Text = "Дата";
            this.labelShiftDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel5, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel6, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(18, 211);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(666, 134);
            this.tableLayoutPanel2.TabIndex = 10;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.gridViewUserSchedule, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(615, 128);
            this.tableLayoutPanel5.TabIndex = 6;
            // 
            // gridViewUserSchedule
            // 
            this.gridViewUserSchedule.AllowUserToAddRows = false;
            this.gridViewUserSchedule.AllowUserToDeleteRows = false;
            this.gridViewUserSchedule.AllowUserToResizeColumns = false;
            this.gridViewUserSchedule.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gridViewUserSchedule.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gridViewUserSchedule.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridViewUserSchedule.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridViewUserSchedule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewUserSchedule.ColumnHeadersVisible = false;
            this.gridViewUserSchedule.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Aquamarine;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridViewUserSchedule.DefaultCellStyle = dataGridViewCellStyle3;
            this.gridViewUserSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridViewUserSchedule.Location = new System.Drawing.Point(3, 6);
            this.gridViewUserSchedule.MultiSelect = false;
            this.gridViewUserSchedule.Name = "gridViewUserSchedule";
            this.gridViewUserSchedule.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridViewUserSchedule.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gridViewUserSchedule.RowHeadersVisible = false;
            this.gridViewUserSchedule.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridViewUserSchedule.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridViewUserSchedule.Size = new System.Drawing.Size(609, 119);
            this.gridViewUserSchedule.TabIndex = 4;
            this.gridViewUserSchedule.SelectionChanged += new System.EventHandler(this.gridViewUserSchedule_SelectionChanged);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Controls.Add(this.buttonScheduleReturn, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.buttonScheduleEquipReplace, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.buttonScheduleShiftCancel, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(624, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 4;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(39, 128);
            this.tableLayoutPanel6.TabIndex = 7;
            // 
            // buttonScheduleReturn
            // 
            this.buttonScheduleReturn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonScheduleReturn.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonScheduleReturn.Depth = 0;
            this.buttonScheduleReturn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonScheduleReturn.Enabled = false;
            this.buttonScheduleReturn.HighEmphasis = true;
            this.buttonScheduleReturn.Icon = null;
            this.buttonScheduleReturn.Location = new System.Drawing.Point(4, 86);
            this.buttonScheduleReturn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonScheduleReturn.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonScheduleReturn.Name = "buttonScheduleReturn";
            this.buttonScheduleReturn.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonScheduleReturn.Size = new System.Drawing.Size(31, 28);
            this.buttonScheduleReturn.TabIndex = 7;
            this.buttonScheduleReturn.Text = "↩️";
            this.buttonScheduleReturn.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonScheduleReturn.UseAccentColor = false;
            this.buttonScheduleReturn.UseVisualStyleBackColor = true;
            this.buttonScheduleReturn.Click += new System.EventHandler(this.buttonScheduleReturn_Click);
            // 
            // buttonScheduleEquipReplace
            // 
            this.buttonScheduleEquipReplace.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonScheduleEquipReplace.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonScheduleEquipReplace.Depth = 0;
            this.buttonScheduleEquipReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonScheduleEquipReplace.Enabled = false;
            this.buttonScheduleEquipReplace.HighEmphasis = true;
            this.buttonScheduleEquipReplace.Icon = null;
            this.buttonScheduleEquipReplace.Location = new System.Drawing.Point(4, 46);
            this.buttonScheduleEquipReplace.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonScheduleEquipReplace.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonScheduleEquipReplace.Name = "buttonScheduleEquipReplace";
            this.buttonScheduleEquipReplace.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonScheduleEquipReplace.Size = new System.Drawing.Size(31, 28);
            this.buttonScheduleEquipReplace.TabIndex = 6;
            this.buttonScheduleEquipReplace.Text = "➡️";
            this.buttonScheduleEquipReplace.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonScheduleEquipReplace.UseAccentColor = false;
            this.buttonScheduleEquipReplace.UseVisualStyleBackColor = true;
            this.buttonScheduleEquipReplace.Click += new System.EventHandler(this.buttonScheduleEquipReplace_Click);
            // 
            // buttonScheduleShiftCancel
            // 
            this.buttonScheduleShiftCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonScheduleShiftCancel.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonScheduleShiftCancel.Depth = 0;
            this.buttonScheduleShiftCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonScheduleShiftCancel.Enabled = false;
            this.buttonScheduleShiftCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonScheduleShiftCancel.HighEmphasis = true;
            this.buttonScheduleShiftCancel.Icon = null;
            this.buttonScheduleShiftCancel.Location = new System.Drawing.Point(4, 6);
            this.buttonScheduleShiftCancel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonScheduleShiftCancel.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonScheduleShiftCancel.Name = "buttonScheduleShiftCancel";
            this.buttonScheduleShiftCancel.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonScheduleShiftCancel.Size = new System.Drawing.Size(31, 28);
            this.buttonScheduleShiftCancel.TabIndex = 5;
            this.buttonScheduleShiftCancel.Text = "🚫";
            this.buttonScheduleShiftCancel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonScheduleShiftCancel.UseAccentColor = false;
            this.buttonScheduleShiftCancel.UseVisualStyleBackColor = true;
            this.buttonScheduleShiftCancel.Click += new System.EventHandler(this.buttonScheduleShiftCancel_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel7, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel8, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(18, 389);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 134F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(666, 134);
            this.tableLayoutPanel4.TabIndex = 11;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Controls.Add(this.gridViewUserOverride, 0, 1);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(615, 128);
            this.tableLayoutPanel7.TabIndex = 1;
            // 
            // gridViewUserOverride
            // 
            this.gridViewUserOverride.AllowUserToAddRows = false;
            this.gridViewUserOverride.AllowUserToDeleteRows = false;
            this.gridViewUserOverride.AllowUserToResizeColumns = false;
            this.gridViewUserOverride.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gridViewUserOverride.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.gridViewUserOverride.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridViewUserOverride.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.gridViewUserOverride.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewUserOverride.ColumnHeadersVisible = false;
            this.gridViewUserOverride.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridViewUserOverride.DefaultCellStyle = dataGridViewCellStyle7;
            this.gridViewUserOverride.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridViewUserOverride.Location = new System.Drawing.Point(3, 6);
            this.gridViewUserOverride.MultiSelect = false;
            this.gridViewUserOverride.Name = "gridViewUserOverride";
            this.gridViewUserOverride.ReadOnly = true;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridViewUserOverride.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.gridViewUserOverride.RowHeadersVisible = false;
            this.gridViewUserOverride.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridViewUserOverride.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridViewUserOverride.Size = new System.Drawing.Size(609, 119);
            this.gridViewUserOverride.TabIndex = 4;
            this.gridViewUserOverride.SelectionChanged += new System.EventHandler(this.gridViewUserOverride_SelectionChanged);
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.buttonAssignReturn, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.btnAssignEmployee, 0, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(624, 3);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 4;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(39, 128);
            this.tableLayoutPanel8.TabIndex = 2;
            // 
            // buttonAssignReturn
            // 
            this.buttonAssignReturn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonAssignReturn.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonAssignReturn.Depth = 0;
            this.buttonAssignReturn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAssignReturn.Enabled = false;
            this.buttonAssignReturn.HighEmphasis = true;
            this.buttonAssignReturn.Icon = null;
            this.buttonAssignReturn.Location = new System.Drawing.Point(4, 46);
            this.buttonAssignReturn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonAssignReturn.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonAssignReturn.Name = "buttonAssignReturn";
            this.buttonAssignReturn.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonAssignReturn.Size = new System.Drawing.Size(31, 28);
            this.buttonAssignReturn.TabIndex = 1;
            this.buttonAssignReturn.Text = "🗑";
            this.buttonAssignReturn.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonAssignReturn.UseAccentColor = false;
            this.buttonAssignReturn.UseVisualStyleBackColor = true;
            this.buttonAssignReturn.Click += new System.EventHandler(this.buttonAssignReturn_Click);
            // 
            // btnAssignEmployee
            // 
            this.btnAssignEmployee.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAssignEmployee.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnAssignEmployee.Depth = 0;
            this.btnAssignEmployee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAssignEmployee.Enabled = false;
            this.btnAssignEmployee.HighEmphasis = true;
            this.btnAssignEmployee.Icon = null;
            this.btnAssignEmployee.Location = new System.Drawing.Point(4, 6);
            this.btnAssignEmployee.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnAssignEmployee.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnAssignEmployee.Name = "btnAssignEmployee";
            this.btnAssignEmployee.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnAssignEmployee.Size = new System.Drawing.Size(31, 28);
            this.btnAssignEmployee.TabIndex = 0;
            this.btnAssignEmployee.Text = "👤➕";
            this.btnAssignEmployee.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnAssignEmployee.UseAccentColor = false;
            this.btnAssignEmployee.UseVisualStyleBackColor = true;
            this.btnAssignEmployee.Click += new System.EventHandler(this.buttonAssignEmployee_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(18, 173);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(666, 2);
            this.panel1.TabIndex = 12;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(18, 351);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(666, 2);
            this.panel2.TabIndex = 13;
            // 
            // lblStaffingRequirement
            // 
            this.lblStaffingRequirement.AutoSize = true;
            this.lblStaffingRequirement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStaffingRequirement.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStaffingRequirement.Location = new System.Drawing.Point(18, 80);
            this.lblStaffingRequirement.Name = "lblStaffingRequirement";
            this.lblStaffingRequirement.Size = new System.Drawing.Size(666, 40);
            this.lblStaffingRequirement.TabIndex = 14;
            this.lblStaffingRequirement.Text = "Статус";
            this.lblStaffingRequirement.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEquipmentState
            // 
            this.lblEquipmentState.AutoSize = true;
            this.lblEquipmentState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEquipmentState.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblEquipmentState.Location = new System.Drawing.Point(18, 40);
            this.lblEquipmentState.Name = "lblEquipmentState";
            this.lblEquipmentState.Size = new System.Drawing.Size(666, 40);
            this.lblEquipmentState.TabIndex = 15;
            this.lblEquipmentState.Text = "Статус рабочегом места: неизвестно";
            this.lblEquipmentState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(18, 562);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(666, 49);
            this.tableLayoutPanel3.TabIndex = 16;
            // 
            // EditShiftForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 705);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DrawerUseColors = true;
            this.FormStyle = MaterialSkin.Controls.MaterialForm.FormStyles.ActionBar_64;
            this.MaximizeBox = false;
            this.Name = "EditShiftForm";
            this.Padding = new System.Windows.Forms.Padding(3, 88, 3, 3);
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Информация о смене";
            this.Load += new System.EventHandler(this.EditShiftForm_LoadAsync);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewUserSchedule)).EndInit();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewUserOverride)).EndInit();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private MaterialSkin.Controls.MaterialLabel labelShiftDate;
        private MaterialSkin.Controls.MaterialButton btnManageEquipment;
        private DoubleBufferedDataGridView gridViewUserSchedule;
        private MaterialSkin.Controls.MaterialButton buttonScheduleShiftCancel;
        private DoubleBufferedDataGridView gridViewUserOverride;
        private MaterialSkin.Controls.MaterialButton btnAssignEmployee;
        private MaterialSkin.Controls.MaterialButton buttonAssignReturn;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel4;
        private TableLayoutPanel tableLayoutPanel5;
        private TableLayoutPanel tableLayoutPanel7;
        private Panel panel1;
        private Panel panel2;
        private Label lblStaffingRequirement;
        private MaterialSkin.Controls.MaterialButton buttonScheduleEquipReplace;
        private MaterialSkin.Controls.MaterialButton buttonScheduleReturn;
        private TableLayoutPanel tableLayoutPanel8;
        private TableLayoutPanel tableLayoutPanel6;
        private Label lblEquipmentState;
        private TableLayoutPanel tableLayoutPanel3;
    }
}