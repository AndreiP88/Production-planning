using System.Windows.Forms;

namespace Production_planning
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Код, автоматически созданный конструктором форм Windows

        public class MyListView : System.Windows.Forms.ListView
        {
            public MyListView()
            {
                DoubleBuffered = true;
            }
        }

        class DoubleBufferedDataGridView : DataGridView
        {
            protected override bool DoubleBuffered { get => true; }
        }

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.materialTabControl1 = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridPlanning = new Production_planning.Form1.DoubleBufferedDataGridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.planComboBoxYear = new MaterialSkin.Controls.MaterialComboBox();
            this.planComboBoxMonth = new MaterialSkin.Controls.MaterialComboBox();
            this.planComboBoxAreas = new MaterialSkin.Controls.MaterialComboBox();
            this.materialButton2 = new MaterialSkin.Controls.MaterialButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.materialButton1 = new MaterialSkin.Controls.MaterialButton();
            this.workPlanAreaComboBox = new MaterialSkin.Controls.MaterialComboBox();
            this.workPlanEquipComboBox = new MaterialSkin.Controls.MaterialComboBox();
            this.dataGridViewPlan = new Production_planning.Form1.DoubleBufferedDataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.listBoxAreas = new MaterialSkin.Controls.MaterialListBox();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonAreaMoveDown = new MaterialSkin.Controls.MaterialButton();
            this.buttonAreaMoveUp = new MaterialSkin.Controls.MaterialButton();
            this.buttonAreaDelete = new MaterialSkin.Controls.MaterialButton();
            this.buttonAreaEdit = new MaterialSkin.Controls.MaterialButton();
            this.buttonAreaAdd = new MaterialSkin.Controls.MaterialButton();
            this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonEquipMoveDown = new MaterialSkin.Controls.MaterialButton();
            this.buttonEquipMoveUp = new MaterialSkin.Controls.MaterialButton();
            this.buttonEquipDelete = new MaterialSkin.Controls.MaterialButton();
            this.buttonEquipEdit = new MaterialSkin.Controls.MaterialButton();
            this.buttonEquipAdd = new MaterialSkin.Controls.MaterialButton();
            this.listViewEquips = new MaterialSkin.Controls.MaterialListView();
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonShiftEdit = new MaterialSkin.Controls.MaterialButton();
            this.buttonShiftAdd = new MaterialSkin.Controls.MaterialButton();
            this.buttonShiftDelete = new MaterialSkin.Controls.MaterialButton();
            this.listViewShiftsDef = new MaterialSkin.Controls.MaterialListView();
            this.ColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonTemplateDelete = new MaterialSkin.Controls.MaterialButton();
            this.buttonTemplateEdit = new MaterialSkin.Controls.MaterialButton();
            this.buttonTemplateAdd = new MaterialSkin.Controls.MaterialButton();
            this.listViewShiftCycle = new MaterialSkin.Controls.MaterialListView();
            this.columnHeader21 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader22 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader23 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader24 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonBrigadeDelete = new MaterialSkin.Controls.MaterialButton();
            this.buttonBrigadeEdit = new MaterialSkin.Controls.MaterialButton();
            this.buttonBrigadeAdd = new MaterialSkin.Controls.MaterialButton();
            this.listViewShiftTemplate = new MaterialSkin.Controls.MaterialListView();
            this.columnHeader31 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader32 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader33 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader34 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.materialCheckbox1 = new MaterialSkin.Controls.MaterialCheckbox();
            this.materialCheckbox2 = new MaterialSkin.Controls.MaterialCheckbox();
            this.materialCheckbox3 = new MaterialSkin.Controls.MaterialCheckbox();
            this.materialCheckbox4 = new MaterialSkin.Controls.MaterialCheckbox();
            this.materialCheckbox5 = new MaterialSkin.Controls.MaterialCheckbox();
            this.materialCheckbox6 = new MaterialSkin.Controls.MaterialCheckbox();
            this.materialCheckbox7 = new MaterialSkin.Controls.MaterialCheckbox();
            this.materialCheckbox8 = new MaterialSkin.Controls.MaterialCheckbox();
            this.materialCheckbox9 = new MaterialSkin.Controls.MaterialCheckbox();
            this.materialTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPlanning)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPlan)).BeginInit();
            this.tabPage5.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.tableLayoutPanel16.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.SuspendLayout();
            // 
            // materialTabControl1
            // 
            this.materialTabControl1.Controls.Add(this.tabPage1);
            this.materialTabControl1.Controls.Add(this.tabPage2);
            this.materialTabControl1.Controls.Add(this.tabPage3);
            this.materialTabControl1.Controls.Add(this.tabPage4);
            this.materialTabControl1.Controls.Add(this.tabPage5);
            this.materialTabControl1.Controls.Add(this.tabPage6);
            this.materialTabControl1.Controls.Add(this.tabPage7);
            this.materialTabControl1.Depth = 0;
            this.materialTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialTabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.materialTabControl1.ImageList = this.imageList;
            this.materialTabControl1.Location = new System.Drawing.Point(3, 88);
            this.materialTabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabControl1.Multiline = true;
            this.materialTabControl1.Name = "materialTabControl1";
            this.materialTabControl1.SelectedIndex = 0;
            this.materialTabControl1.Size = new System.Drawing.Size(1248, 522);
            this.materialTabControl1.TabIndex = 0;
            this.materialTabControl1.SelectedIndexChanged += new System.EventHandler(this.materialTabControl1_SelectedIndexChangedAsync);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.ImageKey = "free-icon-subtitles-3916608.png";
            this.tabPage1.Location = new System.Drawing.Point(4, 39);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1240, 479);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Расстановка персонала";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridPlanning, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1234, 473);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // dataGridPlanning
            // 
            this.dataGridPlanning.AllowUserToAddRows = false;
            this.dataGridPlanning.AllowUserToDeleteRows = false;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dataGridPlanning.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridPlanning.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridPlanning.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridPlanning.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridPlanning.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridPlanning.ColumnHeadersVisible = false;
            this.dataGridPlanning.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dataGridPlanning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridPlanning.Location = new System.Drawing.Point(3, 63);
            this.dataGridPlanning.MultiSelect = false;
            this.dataGridPlanning.Name = "dataGridPlanning";
            this.dataGridPlanning.ReadOnly = true;
            this.dataGridPlanning.RowHeadersVisible = false;
            this.dataGridPlanning.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridPlanning.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridPlanning.Size = new System.Drawing.Size(1228, 407);
            this.dataGridPlanning.TabIndex = 2;
            this.dataGridPlanning.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridPlanning_CellDoubleClick);
            this.dataGridPlanning.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridPlanning_CellPainting);
            this.dataGridPlanning.SelectionChanged += new System.EventHandler(this.dataGridPlanning_SelectionChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 280F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.planComboBoxYear, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.planComboBoxMonth, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.planComboBoxAreas, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.materialButton2, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1228, 54);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // planComboBoxYear
            // 
            this.planComboBoxYear.AutoResize = false;
            this.planComboBoxYear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.planComboBoxYear.Depth = 0;
            this.planComboBoxYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.planComboBoxYear.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.planComboBoxYear.DropDownHeight = 174;
            this.planComboBoxYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.planComboBoxYear.DropDownWidth = 121;
            this.planComboBoxYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.planComboBoxYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.planComboBoxYear.FormattingEnabled = true;
            this.planComboBoxYear.IntegralHeight = false;
            this.planComboBoxYear.ItemHeight = 43;
            this.planComboBoxYear.Location = new System.Drawing.Point(3, 3);
            this.planComboBoxYear.MaxDropDownItems = 4;
            this.planComboBoxYear.MouseState = MaterialSkin.MouseState.OUT;
            this.planComboBoxYear.Name = "planComboBoxYear";
            this.planComboBoxYear.Size = new System.Drawing.Size(174, 49);
            this.planComboBoxYear.StartIndex = 0;
            this.planComboBoxYear.TabIndex = 0;
            this.planComboBoxYear.SelectedIndexChanged += new System.EventHandler(this.planComboBoxYear_SelectedIndexChanged);
            // 
            // planComboBoxMonth
            // 
            this.planComboBoxMonth.AutoResize = false;
            this.planComboBoxMonth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.planComboBoxMonth.Depth = 0;
            this.planComboBoxMonth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.planComboBoxMonth.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.planComboBoxMonth.DropDownHeight = 174;
            this.planComboBoxMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.planComboBoxMonth.DropDownWidth = 121;
            this.planComboBoxMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.planComboBoxMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.planComboBoxMonth.FormattingEnabled = true;
            this.planComboBoxMonth.IntegralHeight = false;
            this.planComboBoxMonth.ItemHeight = 43;
            this.planComboBoxMonth.Items.AddRange(new object[] {
            "Январь",
            "Февраль",
            "Март",
            "Апрель",
            "Май",
            "Июнь",
            "Июль",
            "Август",
            "Сентябрь",
            "Октябрь",
            "Ноябрь",
            "Декабрь"});
            this.planComboBoxMonth.Location = new System.Drawing.Point(183, 3);
            this.planComboBoxMonth.MaxDropDownItems = 4;
            this.planComboBoxMonth.MouseState = MaterialSkin.MouseState.OUT;
            this.planComboBoxMonth.Name = "planComboBoxMonth";
            this.planComboBoxMonth.Size = new System.Drawing.Size(174, 49);
            this.planComboBoxMonth.StartIndex = 0;
            this.planComboBoxMonth.TabIndex = 1;
            this.planComboBoxMonth.SelectedIndexChanged += new System.EventHandler(this.planComboBoxMonth_SelectedIndexChanged);
            // 
            // planComboBoxAreas
            // 
            this.planComboBoxAreas.AutoResize = false;
            this.planComboBoxAreas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.planComboBoxAreas.Depth = 0;
            this.planComboBoxAreas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.planComboBoxAreas.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.planComboBoxAreas.DropDownHeight = 174;
            this.planComboBoxAreas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.planComboBoxAreas.DropDownWidth = 121;
            this.planComboBoxAreas.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.planComboBoxAreas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.planComboBoxAreas.FormattingEnabled = true;
            this.planComboBoxAreas.Hint = "Участок...";
            this.planComboBoxAreas.IntegralHeight = false;
            this.planComboBoxAreas.ItemHeight = 43;
            this.planComboBoxAreas.Location = new System.Drawing.Point(363, 3);
            this.planComboBoxAreas.MaxDropDownItems = 4;
            this.planComboBoxAreas.MouseState = MaterialSkin.MouseState.OUT;
            this.planComboBoxAreas.Name = "planComboBoxAreas";
            this.planComboBoxAreas.Size = new System.Drawing.Size(274, 49);
            this.planComboBoxAreas.StartIndex = 0;
            this.planComboBoxAreas.TabIndex = 2;
            this.planComboBoxAreas.SelectedIndexChanged += new System.EventHandler(this.planComboBoxAreas_SelectedIndexChanged);
            // 
            // materialButton2
            // 
            this.materialButton2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialButton2.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButton2.Depth = 0;
            this.materialButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialButton2.HighEmphasis = true;
            this.materialButton2.Icon = null;
            this.materialButton2.Location = new System.Drawing.Point(644, 6);
            this.materialButton2.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialButton2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton2.Name = "materialButton2";
            this.materialButton2.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButton2.Size = new System.Drawing.Size(172, 43);
            this.materialButton2.TabIndex = 3;
            this.materialButton2.Text = "Обновить";
            this.materialButton2.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButton2.UseAccentColor = false;
            this.materialButton2.UseVisualStyleBackColor = true;
            this.materialButton2.Click += new System.EventHandler(this.materialButton2_ClickAsync);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel3);
            this.tabPage2.ImageKey = "free-icon-form-3914182.png";
            this.tabPage2.Location = new System.Drawing.Point(4, 39);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(1240, 479);
            this.tabPage2.TabIndex = 5;
            this.tabPage2.Text = "План работы";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.dataGridViewPlan, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1240, 479);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 280F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 280F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.materialButton1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.workPlanAreaComboBox, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.workPlanEquipComboBox, 2, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1234, 54);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // materialButton1
            // 
            this.materialButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialButton1.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButton1.Depth = 0;
            this.materialButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialButton1.HighEmphasis = true;
            this.materialButton1.Icon = null;
            this.materialButton1.Location = new System.Drawing.Point(4, 6);
            this.materialButton1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton1.Name = "materialButton1";
            this.materialButton1.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButton1.Size = new System.Drawing.Size(172, 42);
            this.materialButton1.TabIndex = 0;
            this.materialButton1.Text = "Обновить";
            this.materialButton1.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButton1.UseAccentColor = false;
            this.materialButton1.UseVisualStyleBackColor = true;
            this.materialButton1.Click += new System.EventHandler(this.materialButton1_Click);
            // 
            // workPlanAreaComboBox
            // 
            this.workPlanAreaComboBox.AutoResize = false;
            this.workPlanAreaComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.workPlanAreaComboBox.Depth = 0;
            this.workPlanAreaComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workPlanAreaComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.workPlanAreaComboBox.DropDownHeight = 174;
            this.workPlanAreaComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.workPlanAreaComboBox.DropDownWidth = 121;
            this.workPlanAreaComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.workPlanAreaComboBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.workPlanAreaComboBox.FormattingEnabled = true;
            this.workPlanAreaComboBox.Hint = "Участок";
            this.workPlanAreaComboBox.IntegralHeight = false;
            this.workPlanAreaComboBox.ItemHeight = 43;
            this.workPlanAreaComboBox.Location = new System.Drawing.Point(183, 3);
            this.workPlanAreaComboBox.MaxDropDownItems = 4;
            this.workPlanAreaComboBox.MouseState = MaterialSkin.MouseState.OUT;
            this.workPlanAreaComboBox.Name = "workPlanAreaComboBox";
            this.workPlanAreaComboBox.Size = new System.Drawing.Size(274, 49);
            this.workPlanAreaComboBox.StartIndex = 0;
            this.workPlanAreaComboBox.TabIndex = 1;
            this.workPlanAreaComboBox.SelectedIndexChanged += new System.EventHandler(this.workPlanAreaComboBox_SelectedIndexChanged);
            // 
            // workPlanEquipComboBox
            // 
            this.workPlanEquipComboBox.AutoResize = false;
            this.workPlanEquipComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.workPlanEquipComboBox.Depth = 0;
            this.workPlanEquipComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workPlanEquipComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.workPlanEquipComboBox.DropDownHeight = 174;
            this.workPlanEquipComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.workPlanEquipComboBox.DropDownWidth = 121;
            this.workPlanEquipComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.workPlanEquipComboBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.workPlanEquipComboBox.FormattingEnabled = true;
            this.workPlanEquipComboBox.Hint = "Оборудование";
            this.workPlanEquipComboBox.IntegralHeight = false;
            this.workPlanEquipComboBox.ItemHeight = 43;
            this.workPlanEquipComboBox.Location = new System.Drawing.Point(463, 3);
            this.workPlanEquipComboBox.MaxDropDownItems = 4;
            this.workPlanEquipComboBox.MouseState = MaterialSkin.MouseState.OUT;
            this.workPlanEquipComboBox.Name = "workPlanEquipComboBox";
            this.workPlanEquipComboBox.Size = new System.Drawing.Size(274, 49);
            this.workPlanEquipComboBox.StartIndex = 0;
            this.workPlanEquipComboBox.TabIndex = 2;
            this.workPlanEquipComboBox.SelectedIndexChanged += new System.EventHandler(this.workPlanEquipComboBox_SelectedIndexChanged);
            // 
            // dataGridViewPlan
            // 
            this.dataGridViewPlan.AllowUserToAddRows = false;
            this.dataGridViewPlan.AllowUserToDeleteRows = false;
            this.dataGridViewPlan.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridViewPlan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPlan.ColumnHeadersVisible = false;
            this.dataGridViewPlan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPlan.Location = new System.Drawing.Point(3, 63);
            this.dataGridViewPlan.MultiSelect = false;
            this.dataGridViewPlan.Name = "dataGridViewPlan";
            this.dataGridViewPlan.ReadOnly = true;
            this.dataGridViewPlan.RowHeadersVisible = false;
            this.dataGridViewPlan.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPlan.Size = new System.Drawing.Size(1234, 413);
            this.dataGridViewPlan.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.ImageKey = "free-icon-user-delete-3914336.png";
            this.tabPage3.Location = new System.Drawing.Point(4, 39);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1240, 479);
            this.tabPage3.TabIndex = 10;
            this.tabPage3.Text = "Отсутствия";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.ImageKey = "free-icon-user-3917688.png";
            this.tabPage4.Location = new System.Drawing.Point(4, 39);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1240, 479);
            this.tabPage4.TabIndex = 6;
            this.tabPage4.Text = "Сотрудники";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.tableLayoutPanel5);
            this.tabPage5.ImageKey = "free-icon-database-3914565.png";
            this.tabPage5.Location = new System.Drawing.Point(4, 39);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1240, 479);
            this.tabPage5.TabIndex = 7;
            this.tabPage5.Text = "Оборудование";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 4;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel7, 2, 2);
            this.tableLayoutPanel5.Controls.Add(this.materialLabel1, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.materialLabel2, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.listBoxAreas, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel15, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel16, 3, 1);
            this.tableLayoutPanel5.Controls.Add(this.listViewEquips, 2, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 3;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1240, 479);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 4;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 437);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(450, 39);
            this.tableLayoutPanel6.TabIndex = 0;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 4;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(509, 437);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(678, 39);
            this.tableLayoutPanel7.TabIndex = 1;
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.Location = new System.Drawing.Point(3, 0);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(450, 30);
            this.materialLabel1.TabIndex = 2;
            this.materialLabel1.Text = "Производственные участки";
            this.materialLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel2.Location = new System.Drawing.Point(509, 0);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(678, 30);
            this.materialLabel2.TabIndex = 3;
            this.materialLabel2.Text = "Оборудование";
            this.materialLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listBoxAreas
            // 
            this.listBoxAreas.BackColor = System.Drawing.Color.White;
            this.listBoxAreas.BorderColor = System.Drawing.Color.LightGray;
            this.listBoxAreas.Depth = 0;
            this.listBoxAreas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxAreas.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBoxAreas.Location = new System.Drawing.Point(3, 33);
            this.listBoxAreas.MouseState = MaterialSkin.MouseState.HOVER;
            this.listBoxAreas.Name = "listBoxAreas";
            this.listBoxAreas.SelectedIndex = -1;
            this.listBoxAreas.SelectedItem = null;
            this.listBoxAreas.Size = new System.Drawing.Size(450, 398);
            this.listBoxAreas.TabIndex = 4;
            this.listBoxAreas.SelectedIndexChanged += new MaterialSkin.Controls.MaterialListBox.SelectedIndexChangedEventHandler(this.listBoxAreas_SelectedIndexChanged);
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.ColumnCount = 1;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Controls.Add(this.buttonAreaMoveDown, 0, 4);
            this.tableLayoutPanel15.Controls.Add(this.buttonAreaMoveUp, 0, 3);
            this.tableLayoutPanel15.Controls.Add(this.buttonAreaDelete, 0, 2);
            this.tableLayoutPanel15.Controls.Add(this.buttonAreaEdit, 0, 1);
            this.tableLayoutPanel15.Controls.Add(this.buttonAreaAdd, 0, 0);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(459, 33);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 6;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(44, 398);
            this.tableLayoutPanel15.TabIndex = 6;
            // 
            // buttonAreaMoveDown
            // 
            this.buttonAreaMoveDown.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonAreaMoveDown.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonAreaMoveDown.Depth = 0;
            this.buttonAreaMoveDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAreaMoveDown.Enabled = false;
            this.buttonAreaMoveDown.HighEmphasis = true;
            this.buttonAreaMoveDown.Icon = null;
            this.buttonAreaMoveDown.Location = new System.Drawing.Point(4, 186);
            this.buttonAreaMoveDown.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonAreaMoveDown.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonAreaMoveDown.Name = "buttonAreaMoveDown";
            this.buttonAreaMoveDown.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonAreaMoveDown.Size = new System.Drawing.Size(36, 33);
            this.buttonAreaMoveDown.TabIndex = 5;
            this.buttonAreaMoveDown.Text = "↓";
            this.buttonAreaMoveDown.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonAreaMoveDown.UseAccentColor = false;
            this.buttonAreaMoveDown.UseVisualStyleBackColor = true;
            this.buttonAreaMoveDown.Click += new System.EventHandler(this.buttonAreaMoveDown_Click);
            // 
            // buttonAreaMoveUp
            // 
            this.buttonAreaMoveUp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonAreaMoveUp.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonAreaMoveUp.Depth = 0;
            this.buttonAreaMoveUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAreaMoveUp.Enabled = false;
            this.buttonAreaMoveUp.HighEmphasis = true;
            this.buttonAreaMoveUp.Icon = null;
            this.buttonAreaMoveUp.Location = new System.Drawing.Point(4, 141);
            this.buttonAreaMoveUp.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonAreaMoveUp.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonAreaMoveUp.Name = "buttonAreaMoveUp";
            this.buttonAreaMoveUp.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonAreaMoveUp.Size = new System.Drawing.Size(36, 33);
            this.buttonAreaMoveUp.TabIndex = 4;
            this.buttonAreaMoveUp.Text = "↑";
            this.buttonAreaMoveUp.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonAreaMoveUp.UseAccentColor = false;
            this.buttonAreaMoveUp.UseVisualStyleBackColor = true;
            this.buttonAreaMoveUp.Click += new System.EventHandler(this.buttonAreaMoveUp_Click);
            // 
            // buttonAreaDelete
            // 
            this.buttonAreaDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonAreaDelete.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonAreaDelete.Depth = 0;
            this.buttonAreaDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAreaDelete.Enabled = false;
            this.buttonAreaDelete.HighEmphasis = true;
            this.buttonAreaDelete.Icon = null;
            this.buttonAreaDelete.Location = new System.Drawing.Point(4, 96);
            this.buttonAreaDelete.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonAreaDelete.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonAreaDelete.Name = "buttonAreaDelete";
            this.buttonAreaDelete.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonAreaDelete.Size = new System.Drawing.Size(36, 33);
            this.buttonAreaDelete.TabIndex = 3;
            this.buttonAreaDelete.Text = "❌";
            this.buttonAreaDelete.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonAreaDelete.UseAccentColor = false;
            this.buttonAreaDelete.UseVisualStyleBackColor = true;
            this.buttonAreaDelete.Click += new System.EventHandler(this.buttonAreaDelete_Click);
            // 
            // buttonAreaEdit
            // 
            this.buttonAreaEdit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonAreaEdit.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonAreaEdit.Depth = 0;
            this.buttonAreaEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAreaEdit.Enabled = false;
            this.buttonAreaEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAreaEdit.HighEmphasis = true;
            this.buttonAreaEdit.Icon = null;
            this.buttonAreaEdit.Location = new System.Drawing.Point(4, 51);
            this.buttonAreaEdit.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonAreaEdit.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonAreaEdit.Name = "buttonAreaEdit";
            this.buttonAreaEdit.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonAreaEdit.Size = new System.Drawing.Size(36, 33);
            this.buttonAreaEdit.TabIndex = 2;
            this.buttonAreaEdit.Text = "📝";
            this.buttonAreaEdit.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonAreaEdit.UseAccentColor = false;
            this.buttonAreaEdit.UseVisualStyleBackColor = true;
            this.buttonAreaEdit.Click += new System.EventHandler(this.buttonAreaEdit_Click);
            // 
            // buttonAreaAdd
            // 
            this.buttonAreaAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonAreaAdd.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonAreaAdd.Depth = 0;
            this.buttonAreaAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAreaAdd.HighEmphasis = true;
            this.buttonAreaAdd.Icon = null;
            this.buttonAreaAdd.Location = new System.Drawing.Point(4, 6);
            this.buttonAreaAdd.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonAreaAdd.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonAreaAdd.Name = "buttonAreaAdd";
            this.buttonAreaAdd.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonAreaAdd.Size = new System.Drawing.Size(36, 33);
            this.buttonAreaAdd.TabIndex = 1;
            this.buttonAreaAdd.Text = "➕";
            this.buttonAreaAdd.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonAreaAdd.UseAccentColor = false;
            this.buttonAreaAdd.UseVisualStyleBackColor = true;
            this.buttonAreaAdd.Click += new System.EventHandler(this.buttonAreaAdd_Click);
            // 
            // tableLayoutPanel16
            // 
            this.tableLayoutPanel16.ColumnCount = 1;
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel16.Controls.Add(this.buttonEquipMoveDown, 0, 4);
            this.tableLayoutPanel16.Controls.Add(this.buttonEquipMoveUp, 0, 3);
            this.tableLayoutPanel16.Controls.Add(this.buttonEquipDelete, 0, 2);
            this.tableLayoutPanel16.Controls.Add(this.buttonEquipEdit, 0, 1);
            this.tableLayoutPanel16.Controls.Add(this.buttonEquipAdd, 0, 0);
            this.tableLayoutPanel16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel16.Location = new System.Drawing.Point(1193, 33);
            this.tableLayoutPanel16.Name = "tableLayoutPanel16";
            this.tableLayoutPanel16.RowCount = 6;
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel16.Size = new System.Drawing.Size(44, 398);
            this.tableLayoutPanel16.TabIndex = 7;
            // 
            // buttonEquipMoveDown
            // 
            this.buttonEquipMoveDown.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonEquipMoveDown.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonEquipMoveDown.Depth = 0;
            this.buttonEquipMoveDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEquipMoveDown.Enabled = false;
            this.buttonEquipMoveDown.HighEmphasis = true;
            this.buttonEquipMoveDown.Icon = null;
            this.buttonEquipMoveDown.Location = new System.Drawing.Point(4, 186);
            this.buttonEquipMoveDown.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonEquipMoveDown.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonEquipMoveDown.Name = "buttonEquipMoveDown";
            this.buttonEquipMoveDown.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonEquipMoveDown.Size = new System.Drawing.Size(36, 33);
            this.buttonEquipMoveDown.TabIndex = 5;
            this.buttonEquipMoveDown.Text = "↓";
            this.buttonEquipMoveDown.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonEquipMoveDown.UseAccentColor = false;
            this.buttonEquipMoveDown.UseVisualStyleBackColor = true;
            // 
            // buttonEquipMoveUp
            // 
            this.buttonEquipMoveUp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonEquipMoveUp.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonEquipMoveUp.Depth = 0;
            this.buttonEquipMoveUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEquipMoveUp.Enabled = false;
            this.buttonEquipMoveUp.HighEmphasis = true;
            this.buttonEquipMoveUp.Icon = null;
            this.buttonEquipMoveUp.Location = new System.Drawing.Point(4, 141);
            this.buttonEquipMoveUp.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonEquipMoveUp.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonEquipMoveUp.Name = "buttonEquipMoveUp";
            this.buttonEquipMoveUp.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonEquipMoveUp.Size = new System.Drawing.Size(36, 33);
            this.buttonEquipMoveUp.TabIndex = 4;
            this.buttonEquipMoveUp.Text = "↑";
            this.buttonEquipMoveUp.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonEquipMoveUp.UseAccentColor = false;
            this.buttonEquipMoveUp.UseVisualStyleBackColor = true;
            // 
            // buttonEquipDelete
            // 
            this.buttonEquipDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonEquipDelete.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonEquipDelete.Depth = 0;
            this.buttonEquipDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEquipDelete.Enabled = false;
            this.buttonEquipDelete.HighEmphasis = true;
            this.buttonEquipDelete.Icon = null;
            this.buttonEquipDelete.Location = new System.Drawing.Point(4, 96);
            this.buttonEquipDelete.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonEquipDelete.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonEquipDelete.Name = "buttonEquipDelete";
            this.buttonEquipDelete.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonEquipDelete.Size = new System.Drawing.Size(36, 33);
            this.buttonEquipDelete.TabIndex = 3;
            this.buttonEquipDelete.Text = "❌";
            this.buttonEquipDelete.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonEquipDelete.UseAccentColor = false;
            this.buttonEquipDelete.UseVisualStyleBackColor = true;
            // 
            // buttonEquipEdit
            // 
            this.buttonEquipEdit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonEquipEdit.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonEquipEdit.Depth = 0;
            this.buttonEquipEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEquipEdit.Enabled = false;
            this.buttonEquipEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonEquipEdit.HighEmphasis = true;
            this.buttonEquipEdit.Icon = null;
            this.buttonEquipEdit.Location = new System.Drawing.Point(4, 51);
            this.buttonEquipEdit.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonEquipEdit.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonEquipEdit.Name = "buttonEquipEdit";
            this.buttonEquipEdit.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonEquipEdit.Size = new System.Drawing.Size(36, 33);
            this.buttonEquipEdit.TabIndex = 2;
            this.buttonEquipEdit.Text = "📝";
            this.buttonEquipEdit.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonEquipEdit.UseAccentColor = false;
            this.buttonEquipEdit.UseVisualStyleBackColor = true;
            // 
            // buttonEquipAdd
            // 
            this.buttonEquipAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonEquipAdd.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonEquipAdd.Depth = 0;
            this.buttonEquipAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEquipAdd.HighEmphasis = true;
            this.buttonEquipAdd.Icon = null;
            this.buttonEquipAdd.Location = new System.Drawing.Point(4, 6);
            this.buttonEquipAdd.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonEquipAdd.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonEquipAdd.Name = "buttonEquipAdd";
            this.buttonEquipAdd.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonEquipAdd.Size = new System.Drawing.Size(36, 33);
            this.buttonEquipAdd.TabIndex = 1;
            this.buttonEquipAdd.Text = "➕";
            this.buttonEquipAdd.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonEquipAdd.UseAccentColor = false;
            this.buttonEquipAdd.UseVisualStyleBackColor = true;
            this.buttonEquipAdd.Click += new System.EventHandler(this.buttonEquipAdd_Click);
            // 
            // listViewEquips
            // 
            this.listViewEquips.AutoSizeTable = false;
            this.listViewEquips.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.listViewEquips.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewEquips.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader15,
            this.columnHeader16,
            this.columnHeader6});
            this.listViewEquips.Depth = 0;
            this.listViewEquips.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewEquips.FullRowSelect = true;
            this.listViewEquips.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewEquips.HideSelection = false;
            this.listViewEquips.Location = new System.Drawing.Point(509, 33);
            this.listViewEquips.MinimumSize = new System.Drawing.Size(200, 100);
            this.listViewEquips.MouseLocation = new System.Drawing.Point(-1, -1);
            this.listViewEquips.MouseState = MaterialSkin.MouseState.OUT;
            this.listViewEquips.MultiSelect = false;
            this.listViewEquips.Name = "listViewEquips";
            this.listViewEquips.OwnerDraw = true;
            this.listViewEquips.Size = new System.Drawing.Size(678, 398);
            this.listViewEquips.TabIndex = 8;
            this.listViewEquips.UseCompatibleStateImageBehavior = false;
            this.listViewEquips.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "№";
            this.columnHeader11.Width = 30;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Название";
            this.columnHeader12.Width = 220;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "График работы";
            this.columnHeader15.Width = 200;
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "Тип работы";
            this.columnHeader16.Width = 160;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Статус";
            this.columnHeader6.Width = 120;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.tableLayoutPanel8);
            this.tabPage6.ImageKey = "free-icon-indent-3917045.png";
            this.tabPage6.Location = new System.Drawing.Point(4, 39);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(1240, 479);
            this.tabPage6.TabIndex = 8;
            this.tabPage6.Text = "График смен";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 3;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel9, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel11, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel13, 2, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 2;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(1240, 479);
            this.tableLayoutPanel8.TabIndex = 0;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel10, 1, 1);
            this.tableLayoutPanel9.Controls.Add(this.listViewShiftsDef, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.materialLabel3, 0, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 3;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(604, 233);
            this.tableLayoutPanel9.TabIndex = 0;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 1;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Controls.Add(this.buttonShiftEdit, 0, 1);
            this.tableLayoutPanel10.Controls.Add(this.buttonShiftAdd, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.buttonShiftDelete, 0, 2);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(557, 23);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 5;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(44, 187);
            this.tableLayoutPanel10.TabIndex = 0;
            // 
            // buttonShiftEdit
            // 
            this.buttonShiftEdit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonShiftEdit.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonShiftEdit.Depth = 0;
            this.buttonShiftEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShiftEdit.Enabled = false;
            this.buttonShiftEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonShiftEdit.HighEmphasis = true;
            this.buttonShiftEdit.Icon = null;
            this.buttonShiftEdit.Location = new System.Drawing.Point(4, 46);
            this.buttonShiftEdit.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonShiftEdit.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonShiftEdit.Name = "buttonShiftEdit";
            this.buttonShiftEdit.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonShiftEdit.Size = new System.Drawing.Size(36, 28);
            this.buttonShiftEdit.TabIndex = 1;
            this.buttonShiftEdit.Text = "📝";
            this.buttonShiftEdit.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonShiftEdit.UseAccentColor = false;
            this.buttonShiftEdit.UseVisualStyleBackColor = true;
            this.buttonShiftEdit.Click += new System.EventHandler(this.buttonShiftEdit_Click);
            // 
            // buttonShiftAdd
            // 
            this.buttonShiftAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonShiftAdd.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonShiftAdd.Depth = 0;
            this.buttonShiftAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShiftAdd.HighEmphasis = true;
            this.buttonShiftAdd.Icon = null;
            this.buttonShiftAdd.Location = new System.Drawing.Point(4, 6);
            this.buttonShiftAdd.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonShiftAdd.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonShiftAdd.Name = "buttonShiftAdd";
            this.buttonShiftAdd.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonShiftAdd.Size = new System.Drawing.Size(36, 28);
            this.buttonShiftAdd.TabIndex = 0;
            this.buttonShiftAdd.Text = "➕";
            this.buttonShiftAdd.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonShiftAdd.UseAccentColor = false;
            this.buttonShiftAdd.UseVisualStyleBackColor = true;
            this.buttonShiftAdd.Click += new System.EventHandler(this.buttonShiftAdd_Click);
            // 
            // buttonShiftDelete
            // 
            this.buttonShiftDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonShiftDelete.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonShiftDelete.Depth = 0;
            this.buttonShiftDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShiftDelete.Enabled = false;
            this.buttonShiftDelete.HighEmphasis = true;
            this.buttonShiftDelete.Icon = null;
            this.buttonShiftDelete.Location = new System.Drawing.Point(4, 86);
            this.buttonShiftDelete.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonShiftDelete.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonShiftDelete.Name = "buttonShiftDelete";
            this.buttonShiftDelete.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonShiftDelete.Size = new System.Drawing.Size(36, 28);
            this.buttonShiftDelete.TabIndex = 2;
            this.buttonShiftDelete.Text = "❌";
            this.buttonShiftDelete.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonShiftDelete.UseAccentColor = false;
            this.buttonShiftDelete.UseVisualStyleBackColor = true;
            this.buttonShiftDelete.Click += new System.EventHandler(this.buttonShiftDelete_Click);
            // 
            // listViewShiftsDef
            // 
            this.listViewShiftsDef.AutoSizeTable = false;
            this.listViewShiftsDef.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.listViewShiftsDef.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewShiftsDef.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1,
            this.ColumnHeader2,
            this.ColumnHeader3,
            this.ColumnHeader4,
            this.ColumnHeader5});
            this.listViewShiftsDef.Depth = 0;
            this.listViewShiftsDef.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewShiftsDef.FullRowSelect = true;
            this.listViewShiftsDef.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewShiftsDef.HideSelection = false;
            this.listViewShiftsDef.Location = new System.Drawing.Point(3, 23);
            this.listViewShiftsDef.MinimumSize = new System.Drawing.Size(200, 100);
            this.listViewShiftsDef.MouseLocation = new System.Drawing.Point(-1, -1);
            this.listViewShiftsDef.MouseState = MaterialSkin.MouseState.OUT;
            this.listViewShiftsDef.MultiSelect = false;
            this.listViewShiftsDef.Name = "listViewShiftsDef";
            this.listViewShiftsDef.OwnerDraw = true;
            this.listViewShiftsDef.Size = new System.Drawing.Size(548, 187);
            this.listViewShiftsDef.TabIndex = 1;
            this.listViewShiftsDef.UseCompatibleStateImageBehavior = false;
            this.listViewShiftsDef.View = System.Windows.Forms.View.Details;
            this.listViewShiftsDef.SelectedIndexChanged += new System.EventHandler(this.listViewShiftsDef_SelectedIndexChanged);
            this.listViewShiftsDef.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewShiftsDef_MouseDoubleClick);
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "№";
            this.ColumnHeader1.Width = 25;
            // 
            // ColumnHeader2
            // 
            this.ColumnHeader2.Text = "Смена";
            // 
            // ColumnHeader3
            // 
            this.ColumnHeader3.Text = "Название";
            this.ColumnHeader3.Width = 160;
            // 
            // ColumnHeader4
            // 
            this.ColumnHeader4.Text = "Начало";
            this.ColumnHeader4.Width = 120;
            // 
            // ColumnHeader5
            // 
            this.ColumnHeader5.Text = "Завершение";
            this.ColumnHeader5.Width = 120;
            // 
            // materialLabel3
            // 
            this.materialLabel3.AutoSize = true;
            this.materialLabel3.Depth = 0;
            this.materialLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialLabel3.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel3.Location = new System.Drawing.Point(3, 0);
            this.materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel3.Name = "materialLabel3";
            this.materialLabel3.Size = new System.Drawing.Size(548, 20);
            this.materialLabel3.TabIndex = 2;
            this.materialLabel3.Text = "Список смен";
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.ColumnCount = 2;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel11.Controls.Add(this.tableLayoutPanel12, 1, 1);
            this.tableLayoutPanel11.Controls.Add(this.listViewShiftCycle, 0, 1);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(3, 242);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 3;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(604, 234);
            this.tableLayoutPanel11.TabIndex = 1;
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.ColumnCount = 1;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.Controls.Add(this.buttonTemplateDelete, 0, 2);
            this.tableLayoutPanel12.Controls.Add(this.buttonTemplateEdit, 0, 1);
            this.tableLayoutPanel12.Controls.Add(this.buttonTemplateAdd, 0, 0);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(557, 23);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 5;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(44, 188);
            this.tableLayoutPanel12.TabIndex = 0;
            // 
            // buttonTemplateDelete
            // 
            this.buttonTemplateDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonTemplateDelete.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonTemplateDelete.Depth = 0;
            this.buttonTemplateDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTemplateDelete.Enabled = false;
            this.buttonTemplateDelete.HighEmphasis = true;
            this.buttonTemplateDelete.Icon = null;
            this.buttonTemplateDelete.Location = new System.Drawing.Point(4, 86);
            this.buttonTemplateDelete.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonTemplateDelete.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonTemplateDelete.Name = "buttonTemplateDelete";
            this.buttonTemplateDelete.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonTemplateDelete.Size = new System.Drawing.Size(36, 28);
            this.buttonTemplateDelete.TabIndex = 3;
            this.buttonTemplateDelete.Text = "❌";
            this.buttonTemplateDelete.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonTemplateDelete.UseAccentColor = false;
            this.buttonTemplateDelete.UseVisualStyleBackColor = true;
            this.buttonTemplateDelete.Click += new System.EventHandler(this.buttonTemplateDelete_Click);
            // 
            // buttonTemplateEdit
            // 
            this.buttonTemplateEdit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonTemplateEdit.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonTemplateEdit.Depth = 0;
            this.buttonTemplateEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTemplateEdit.Enabled = false;
            this.buttonTemplateEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonTemplateEdit.HighEmphasis = true;
            this.buttonTemplateEdit.Icon = null;
            this.buttonTemplateEdit.Location = new System.Drawing.Point(4, 46);
            this.buttonTemplateEdit.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonTemplateEdit.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonTemplateEdit.Name = "buttonTemplateEdit";
            this.buttonTemplateEdit.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonTemplateEdit.Size = new System.Drawing.Size(36, 28);
            this.buttonTemplateEdit.TabIndex = 2;
            this.buttonTemplateEdit.Text = "📝";
            this.buttonTemplateEdit.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonTemplateEdit.UseAccentColor = false;
            this.buttonTemplateEdit.UseVisualStyleBackColor = true;
            this.buttonTemplateEdit.Click += new System.EventHandler(this.buttonTemplateEdit_Click);
            // 
            // buttonTemplateAdd
            // 
            this.buttonTemplateAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonTemplateAdd.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonTemplateAdd.Depth = 0;
            this.buttonTemplateAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTemplateAdd.HighEmphasis = true;
            this.buttonTemplateAdd.Icon = null;
            this.buttonTemplateAdd.Location = new System.Drawing.Point(4, 6);
            this.buttonTemplateAdd.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonTemplateAdd.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonTemplateAdd.Name = "buttonTemplateAdd";
            this.buttonTemplateAdd.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonTemplateAdd.Size = new System.Drawing.Size(36, 28);
            this.buttonTemplateAdd.TabIndex = 0;
            this.buttonTemplateAdd.Text = "➕";
            this.buttonTemplateAdd.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonTemplateAdd.UseAccentColor = false;
            this.buttonTemplateAdd.UseVisualStyleBackColor = true;
            this.buttonTemplateAdd.Click += new System.EventHandler(this.buttonTemplateAdd_Click);
            // 
            // listViewShiftCycle
            // 
            this.listViewShiftCycle.AutoSizeTable = false;
            this.listViewShiftCycle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.listViewShiftCycle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewShiftCycle.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader21,
            this.columnHeader22,
            this.columnHeader23,
            this.columnHeader24});
            this.listViewShiftCycle.Depth = 0;
            this.listViewShiftCycle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewShiftCycle.FullRowSelect = true;
            this.listViewShiftCycle.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewShiftCycle.HideSelection = false;
            this.listViewShiftCycle.Location = new System.Drawing.Point(3, 23);
            this.listViewShiftCycle.MinimumSize = new System.Drawing.Size(200, 100);
            this.listViewShiftCycle.MouseLocation = new System.Drawing.Point(-1, -1);
            this.listViewShiftCycle.MouseState = MaterialSkin.MouseState.OUT;
            this.listViewShiftCycle.MultiSelect = false;
            this.listViewShiftCycle.Name = "listViewShiftCycle";
            this.listViewShiftCycle.OwnerDraw = true;
            this.listViewShiftCycle.Size = new System.Drawing.Size(548, 188);
            this.listViewShiftCycle.TabIndex = 1;
            this.listViewShiftCycle.UseCompatibleStateImageBehavior = false;
            this.listViewShiftCycle.View = System.Windows.Forms.View.Details;
            this.listViewShiftCycle.SelectedIndexChanged += new System.EventHandler(this.listViewShiftCycle_SelectedIndexChanged);
            this.listViewShiftCycle.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewShiftCycle_MouseDoubleClick);
            // 
            // columnHeader21
            // 
            this.columnHeader21.Text = "№";
            this.columnHeader21.Width = 30;
            // 
            // columnHeader22
            // 
            this.columnHeader22.Text = "Название";
            this.columnHeader22.Width = 200;
            // 
            // columnHeader23
            // 
            this.columnHeader23.Text = "Длина";
            this.columnHeader23.Width = 80;
            // 
            // columnHeader24
            // 
            this.columnHeader24.Text = "Шаблон";
            this.columnHeader24.Width = 400;
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.ColumnCount = 2;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel13.Controls.Add(this.tableLayoutPanel14, 1, 1);
            this.tableLayoutPanel13.Controls.Add(this.listViewShiftTemplate, 0, 1);
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(633, 3);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 3;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(604, 233);
            this.tableLayoutPanel13.TabIndex = 2;
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.ColumnCount = 1;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Controls.Add(this.buttonBrigadeDelete, 0, 2);
            this.tableLayoutPanel14.Controls.Add(this.buttonBrigadeEdit, 0, 1);
            this.tableLayoutPanel14.Controls.Add(this.buttonBrigadeAdd, 0, 0);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(557, 23);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 5;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(44, 187);
            this.tableLayoutPanel14.TabIndex = 0;
            // 
            // buttonBrigadeDelete
            // 
            this.buttonBrigadeDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonBrigadeDelete.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonBrigadeDelete.Depth = 0;
            this.buttonBrigadeDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonBrigadeDelete.Enabled = false;
            this.buttonBrigadeDelete.HighEmphasis = true;
            this.buttonBrigadeDelete.Icon = null;
            this.buttonBrigadeDelete.Location = new System.Drawing.Point(4, 86);
            this.buttonBrigadeDelete.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonBrigadeDelete.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonBrigadeDelete.Name = "buttonBrigadeDelete";
            this.buttonBrigadeDelete.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonBrigadeDelete.Size = new System.Drawing.Size(36, 28);
            this.buttonBrigadeDelete.TabIndex = 3;
            this.buttonBrigadeDelete.Text = "❌";
            this.buttonBrigadeDelete.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonBrigadeDelete.UseAccentColor = false;
            this.buttonBrigadeDelete.UseVisualStyleBackColor = true;
            this.buttonBrigadeDelete.Click += new System.EventHandler(this.buttonBrigadeDelete_Click);
            // 
            // buttonBrigadeEdit
            // 
            this.buttonBrigadeEdit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonBrigadeEdit.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonBrigadeEdit.Depth = 0;
            this.buttonBrigadeEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonBrigadeEdit.Enabled = false;
            this.buttonBrigadeEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonBrigadeEdit.HighEmphasis = true;
            this.buttonBrigadeEdit.Icon = null;
            this.buttonBrigadeEdit.Location = new System.Drawing.Point(4, 46);
            this.buttonBrigadeEdit.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonBrigadeEdit.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonBrigadeEdit.Name = "buttonBrigadeEdit";
            this.buttonBrigadeEdit.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonBrigadeEdit.Size = new System.Drawing.Size(36, 28);
            this.buttonBrigadeEdit.TabIndex = 2;
            this.buttonBrigadeEdit.Text = "📝";
            this.buttonBrigadeEdit.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonBrigadeEdit.UseAccentColor = false;
            this.buttonBrigadeEdit.UseVisualStyleBackColor = true;
            this.buttonBrigadeEdit.Click += new System.EventHandler(this.buttonBrigadeEdit_Click);
            // 
            // buttonBrigadeAdd
            // 
            this.buttonBrigadeAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonBrigadeAdd.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonBrigadeAdd.Depth = 0;
            this.buttonBrigadeAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonBrigadeAdd.HighEmphasis = true;
            this.buttonBrigadeAdd.Icon = null;
            this.buttonBrigadeAdd.Location = new System.Drawing.Point(4, 6);
            this.buttonBrigadeAdd.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonBrigadeAdd.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonBrigadeAdd.Name = "buttonBrigadeAdd";
            this.buttonBrigadeAdd.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonBrigadeAdd.Size = new System.Drawing.Size(36, 28);
            this.buttonBrigadeAdd.TabIndex = 0;
            this.buttonBrigadeAdd.Text = "➕";
            this.buttonBrigadeAdd.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonBrigadeAdd.UseAccentColor = false;
            this.buttonBrigadeAdd.UseVisualStyleBackColor = true;
            this.buttonBrigadeAdd.Click += new System.EventHandler(this.buttonBrigadeAdd_Click);
            // 
            // listViewShiftTemplate
            // 
            this.listViewShiftTemplate.AutoSizeTable = false;
            this.listViewShiftTemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.listViewShiftTemplate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewShiftTemplate.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader31,
            this.columnHeader32,
            this.columnHeader33,
            this.columnHeader34});
            this.listViewShiftTemplate.Depth = 0;
            this.listViewShiftTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewShiftTemplate.FullRowSelect = true;
            this.listViewShiftTemplate.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewShiftTemplate.HideSelection = false;
            this.listViewShiftTemplate.Location = new System.Drawing.Point(3, 23);
            this.listViewShiftTemplate.MinimumSize = new System.Drawing.Size(200, 100);
            this.listViewShiftTemplate.MouseLocation = new System.Drawing.Point(-1, -1);
            this.listViewShiftTemplate.MouseState = MaterialSkin.MouseState.OUT;
            this.listViewShiftTemplate.MultiSelect = false;
            this.listViewShiftTemplate.Name = "listViewShiftTemplate";
            this.listViewShiftTemplate.OwnerDraw = true;
            this.listViewShiftTemplate.Size = new System.Drawing.Size(548, 187);
            this.listViewShiftTemplate.TabIndex = 1;
            this.listViewShiftTemplate.UseCompatibleStateImageBehavior = false;
            this.listViewShiftTemplate.View = System.Windows.Forms.View.Details;
            this.listViewShiftTemplate.SelectedIndexChanged += new System.EventHandler(this.listViewShiftTemplate_SelectedIndexChanged);
            this.listViewShiftTemplate.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewShiftTemplate_MouseDoubleClick);
            // 
            // columnHeader31
            // 
            this.columnHeader31.Text = "№";
            this.columnHeader31.Width = 30;
            // 
            // columnHeader32
            // 
            this.columnHeader32.Text = "Название";
            this.columnHeader32.Width = 196;
            // 
            // columnHeader33
            // 
            this.columnHeader33.Text = "Шаблон";
            this.columnHeader33.Width = 200;
            // 
            // columnHeader34
            // 
            this.columnHeader34.Text = "Начальная дата";
            this.columnHeader34.Width = 120;
            // 
            // tabPage7
            // 
            this.tabPage7.ImageKey = "++++.png";
            this.tabPage7.Location = new System.Drawing.Point(4, 39);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(1240, 479);
            this.tabPage7.TabIndex = 9;
            this.tabPage7.Text = "Параметры";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "++++.png");
            this.imageList.Images.SetKeyName(1, "exchange-6407194.png");
            this.imageList.Images.SetKeyName(2, "free-icon-align-justify-3917034.png");
            this.imageList.Images.SetKeyName(3, "free-icon-browser-3914415.png");
            this.imageList.Images.SetKeyName(4, "free-icon-copy-alt-3914174.png");
            this.imageList.Images.SetKeyName(5, "free-icon-database-3914565.png");
            this.imageList.Images.SetKeyName(6, "free-icon-form-3914182.png");
            this.imageList.Images.SetKeyName(7, "free-icon-indent-3917045.png");
            this.imageList.Images.SetKeyName(8, "free-icon-shuffle-3917134.png");
            this.imageList.Images.SetKeyName(9, "free-icon-sign-in-3917365.png");
            this.imageList.Images.SetKeyName(10, "free-icon-sign-in-alt-5528136.png");
            this.imageList.Images.SetKeyName(11, "free-icon-sign-out-3917349.png");
            this.imageList.Images.SetKeyName(12, "free-icon-subtitles-3916608.png");
            this.imageList.Images.SetKeyName(13, "free-icon-user-3917688.png");
            this.imageList.Images.SetKeyName(14, "free-icon-user-add-3917698.png");
            this.imageList.Images.SetKeyName(15, "free-icon-user-delete-3914336.png");
            this.imageList.Images.SetKeyName(16, "free-icon-user-remove-3914320.png");
            // 
            // materialCheckbox1
            // 
            this.materialCheckbox1.Depth = 0;
            this.materialCheckbox1.Location = new System.Drawing.Point(0, 0);
            this.materialCheckbox1.Margin = new System.Windows.Forms.Padding(0);
            this.materialCheckbox1.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialCheckbox1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCheckbox1.Name = "materialCheckbox1";
            this.materialCheckbox1.ReadOnly = false;
            this.materialCheckbox1.Ripple = true;
            this.materialCheckbox1.Size = new System.Drawing.Size(104, 37);
            this.materialCheckbox1.TabIndex = 0;
            this.materialCheckbox1.Text = "materialCheckbox1";
            this.materialCheckbox1.UseVisualStyleBackColor = true;
            // 
            // materialCheckbox2
            // 
            this.materialCheckbox2.Depth = 0;
            this.materialCheckbox2.Location = new System.Drawing.Point(0, 0);
            this.materialCheckbox2.Margin = new System.Windows.Forms.Padding(0);
            this.materialCheckbox2.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialCheckbox2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCheckbox2.Name = "materialCheckbox2";
            this.materialCheckbox2.ReadOnly = false;
            this.materialCheckbox2.Ripple = true;
            this.materialCheckbox2.Size = new System.Drawing.Size(104, 37);
            this.materialCheckbox2.TabIndex = 0;
            this.materialCheckbox2.Text = "materialCheckbox2";
            this.materialCheckbox2.UseVisualStyleBackColor = true;
            // 
            // materialCheckbox3
            // 
            this.materialCheckbox3.Depth = 0;
            this.materialCheckbox3.Location = new System.Drawing.Point(0, 0);
            this.materialCheckbox3.Margin = new System.Windows.Forms.Padding(0);
            this.materialCheckbox3.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialCheckbox3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCheckbox3.Name = "materialCheckbox3";
            this.materialCheckbox3.ReadOnly = false;
            this.materialCheckbox3.Ripple = true;
            this.materialCheckbox3.Size = new System.Drawing.Size(104, 37);
            this.materialCheckbox3.TabIndex = 0;
            this.materialCheckbox3.Text = "materialCheckbox3";
            this.materialCheckbox3.UseVisualStyleBackColor = true;
            // 
            // materialCheckbox4
            // 
            this.materialCheckbox4.Depth = 0;
            this.materialCheckbox4.Location = new System.Drawing.Point(0, 0);
            this.materialCheckbox4.Margin = new System.Windows.Forms.Padding(0);
            this.materialCheckbox4.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialCheckbox4.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCheckbox4.Name = "materialCheckbox4";
            this.materialCheckbox4.ReadOnly = false;
            this.materialCheckbox4.Ripple = true;
            this.materialCheckbox4.Size = new System.Drawing.Size(104, 37);
            this.materialCheckbox4.TabIndex = 0;
            this.materialCheckbox4.Text = "materialCheckbox4";
            this.materialCheckbox4.UseVisualStyleBackColor = true;
            // 
            // materialCheckbox5
            // 
            this.materialCheckbox5.Depth = 0;
            this.materialCheckbox5.Location = new System.Drawing.Point(0, 0);
            this.materialCheckbox5.Margin = new System.Windows.Forms.Padding(0);
            this.materialCheckbox5.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialCheckbox5.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCheckbox5.Name = "materialCheckbox5";
            this.materialCheckbox5.ReadOnly = false;
            this.materialCheckbox5.Ripple = true;
            this.materialCheckbox5.Size = new System.Drawing.Size(104, 37);
            this.materialCheckbox5.TabIndex = 0;
            this.materialCheckbox5.Text = "materialCheckbox5";
            this.materialCheckbox5.UseVisualStyleBackColor = true;
            // 
            // materialCheckbox6
            // 
            this.materialCheckbox6.Depth = 0;
            this.materialCheckbox6.Location = new System.Drawing.Point(0, 0);
            this.materialCheckbox6.Margin = new System.Windows.Forms.Padding(0);
            this.materialCheckbox6.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialCheckbox6.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCheckbox6.Name = "materialCheckbox6";
            this.materialCheckbox6.ReadOnly = false;
            this.materialCheckbox6.Ripple = true;
            this.materialCheckbox6.Size = new System.Drawing.Size(104, 37);
            this.materialCheckbox6.TabIndex = 0;
            this.materialCheckbox6.Text = "materialCheckbox6";
            this.materialCheckbox6.UseVisualStyleBackColor = true;
            // 
            // materialCheckbox7
            // 
            this.materialCheckbox7.Depth = 0;
            this.materialCheckbox7.Location = new System.Drawing.Point(0, 0);
            this.materialCheckbox7.Margin = new System.Windows.Forms.Padding(0);
            this.materialCheckbox7.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialCheckbox7.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCheckbox7.Name = "materialCheckbox7";
            this.materialCheckbox7.ReadOnly = false;
            this.materialCheckbox7.Ripple = true;
            this.materialCheckbox7.Size = new System.Drawing.Size(104, 37);
            this.materialCheckbox7.TabIndex = 0;
            this.materialCheckbox7.Text = "materialCheckbox7";
            this.materialCheckbox7.UseVisualStyleBackColor = true;
            // 
            // materialCheckbox8
            // 
            this.materialCheckbox8.Depth = 0;
            this.materialCheckbox8.Location = new System.Drawing.Point(0, 0);
            this.materialCheckbox8.Margin = new System.Windows.Forms.Padding(0);
            this.materialCheckbox8.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialCheckbox8.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCheckbox8.Name = "materialCheckbox8";
            this.materialCheckbox8.ReadOnly = false;
            this.materialCheckbox8.Ripple = true;
            this.materialCheckbox8.Size = new System.Drawing.Size(104, 37);
            this.materialCheckbox8.TabIndex = 0;
            this.materialCheckbox8.Text = "materialCheckbox8";
            this.materialCheckbox8.UseVisualStyleBackColor = true;
            // 
            // materialCheckbox9
            // 
            this.materialCheckbox9.Depth = 0;
            this.materialCheckbox9.Location = new System.Drawing.Point(0, 0);
            this.materialCheckbox9.Margin = new System.Windows.Forms.Padding(0);
            this.materialCheckbox9.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialCheckbox9.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCheckbox9.Name = "materialCheckbox9";
            this.materialCheckbox9.ReadOnly = false;
            this.materialCheckbox9.Ripple = true;
            this.materialCheckbox9.Size = new System.Drawing.Size(104, 37);
            this.materialCheckbox9.TabIndex = 0;
            this.materialCheckbox9.Text = "materialCheckbox9";
            this.materialCheckbox9.ThreeState = true;
            this.materialCheckbox9.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1254, 613);
            this.Controls.Add(this.materialTabControl1);
            this.DrawerShowIconsWhenHidden = true;
            this.DrawerTabControl = this.materialTabControl1;
            this.DrawerUseColors = true;
            this.DrawerWidth = 320;
            this.FormStyle = MaterialSkin.Controls.MaterialForm.FormStyles.ActionBar_64;
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(3, 88, 3, 3);
            this.Text = "Производственный план";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.materialTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPlanning)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPlan)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel15.ResumeLayout(false);
            this.tableLayoutPanel15.PerformLayout();
            this.tableLayoutPanel16.ResumeLayout(false);
            this.tableLayoutPanel16.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanel12.PerformLayout();
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel14.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialTabControl materialTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private MaterialSkin.Controls.MaterialComboBox planComboBoxYear;
        private MaterialSkin.Controls.MaterialComboBox planComboBoxMonth;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private MaterialSkin.Controls.MaterialButton materialButton1;
        private DoubleBufferedDataGridView dataGridViewPlan;
        private ImageList imageList;
        private MaterialSkin.Controls.MaterialComboBox workPlanAreaComboBox;
        private MaterialSkin.Controls.MaterialComboBox workPlanEquipComboBox;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TabPage tabPage6;
        private TabPage tabPage7;
        private TableLayoutPanel tableLayoutPanel5;
        private TableLayoutPanel tableLayoutPanel6;
        private TableLayoutPanel tableLayoutPanel7;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialCheckbox materialCheckbox1;
        private MaterialSkin.Controls.MaterialCheckbox materialCheckbox2;
        private MaterialSkin.Controls.MaterialCheckbox materialCheckbox3;
        private MaterialSkin.Controls.MaterialCheckbox materialCheckbox4;
        private MaterialSkin.Controls.MaterialCheckbox materialCheckbox5;
        private MaterialSkin.Controls.MaterialCheckbox materialCheckbox6;
        private MaterialSkin.Controls.MaterialCheckbox materialCheckbox7;
        private MaterialSkin.Controls.MaterialCheckbox materialCheckbox8;
        private MaterialSkin.Controls.MaterialCheckbox materialCheckbox9;
        private MaterialSkin.Controls.MaterialListBox listBoxAreas;
        private DoubleBufferedDataGridView dataGridPlanning;
        private MaterialSkin.Controls.MaterialComboBox planComboBoxAreas;
        private MaterialSkin.Controls.MaterialButton materialButton2;
        private TabPage tabPage3;
        private TableLayoutPanel tableLayoutPanel8;
        private TableLayoutPanel tableLayoutPanel9;
        private TableLayoutPanel tableLayoutPanel10;
        private MaterialSkin.Controls.MaterialButton buttonShiftAdd;
        private MaterialSkin.Controls.MaterialListView listViewShiftsDef;
        private ColumnHeader ColumnHeader1;
        private ColumnHeader ColumnHeader2;
        private ColumnHeader ColumnHeader3;
        private ColumnHeader ColumnHeader4;
        private ColumnHeader ColumnHeader5;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
        private TableLayoutPanel tableLayoutPanel11;
        private TableLayoutPanel tableLayoutPanel12;
        private MaterialSkin.Controls.MaterialListView listViewShiftCycle;
        private ColumnHeader columnHeader21;
        private ColumnHeader columnHeader22;
        private ColumnHeader columnHeader23;
        private ColumnHeader columnHeader24;
        private MaterialSkin.Controls.MaterialButton buttonTemplateAdd;
        private TableLayoutPanel tableLayoutPanel13;
        private TableLayoutPanel tableLayoutPanel14;
        private MaterialSkin.Controls.MaterialListView listViewShiftTemplate;
        private ColumnHeader columnHeader31;
        private ColumnHeader columnHeader32;
        private ColumnHeader columnHeader33;
        private ColumnHeader columnHeader34;
        private MaterialSkin.Controls.MaterialButton buttonBrigadeAdd;
        private MaterialSkin.Controls.MaterialButton buttonShiftEdit;
        private MaterialSkin.Controls.MaterialButton buttonShiftDelete;
        private MaterialSkin.Controls.MaterialButton buttonTemplateDelete;
        private MaterialSkin.Controls.MaterialButton buttonTemplateEdit;
        private MaterialSkin.Controls.MaterialButton buttonBrigadeDelete;
        private MaterialSkin.Controls.MaterialButton buttonBrigadeEdit;
        private TableLayoutPanel tableLayoutPanel15;
        private TableLayoutPanel tableLayoutPanel16;
        private MaterialSkin.Controls.MaterialButton buttonAreaMoveDown;
        private MaterialSkin.Controls.MaterialButton buttonAreaMoveUp;
        private MaterialSkin.Controls.MaterialButton buttonAreaDelete;
        private MaterialSkin.Controls.MaterialButton buttonAreaEdit;
        private MaterialSkin.Controls.MaterialButton buttonAreaAdd;
        private MaterialSkin.Controls.MaterialButton buttonEquipMoveDown;
        private MaterialSkin.Controls.MaterialButton buttonEquipMoveUp;
        private MaterialSkin.Controls.MaterialButton buttonEquipDelete;
        private MaterialSkin.Controls.MaterialButton buttonEquipEdit;
        private MaterialSkin.Controls.MaterialButton buttonEquipAdd;
        private MaterialSkin.Controls.MaterialListView listViewEquips;
        private ColumnHeader columnHeader11;
        private ColumnHeader columnHeader12;
        private ColumnHeader columnHeader15;
        private ColumnHeader columnHeader16;
        private ColumnHeader columnHeader6;
    }
}

