namespace Production_planning
{
    partial class FormAddCycle
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxCycleName = new MaterialSkin.Controls.MaterialTextBox();
            this.listViewCycleItem = new MaterialSkin.Controls.MaterialListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelCycleLeght = new MaterialSkin.Controls.MaterialLabel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxShifts = new MaterialSkin.Controls.MaterialComboBox();
            this.buttonShiftAndDayAdd = new MaterialSkin.Controls.MaterialButton();
            this.buttonShiftAdd = new MaterialSkin.Controls.MaterialButton();
            this.buttonShiftLastDelete = new MaterialSkin.Controls.MaterialButton();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.materialButton1 = new MaterialSkin.Controls.MaterialButton();
            this.materialButton2 = new MaterialSkin.Controls.MaterialButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 81);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(732, 378);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.2068F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.7932F));
            this.tableLayoutPanel2.Controls.Add(this.textBoxCycleName, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.listViewCycleItem, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelCycleLeght, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(13, 12);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(706, 317);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // textBoxCycleName
            // 
            this.textBoxCycleName.AnimateReadOnly = false;
            this.textBoxCycleName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxCycleName.Depth = 0;
            this.textBoxCycleName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCycleName.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBoxCycleName.Hint = "Название";
            this.textBoxCycleName.LeadingIcon = null;
            this.textBoxCycleName.Location = new System.Drawing.Point(3, 3);
            this.textBoxCycleName.MaxLength = 50;
            this.textBoxCycleName.MouseState = MaterialSkin.MouseState.OUT;
            this.textBoxCycleName.Multiline = false;
            this.textBoxCycleName.Name = "textBoxCycleName";
            this.textBoxCycleName.Size = new System.Drawing.Size(412, 50);
            this.textBoxCycleName.TabIndex = 1;
            this.textBoxCycleName.Text = "";
            this.textBoxCycleName.TrailingIcon = null;
            // 
            // listViewCycleItem
            // 
            this.listViewCycleItem.AutoSizeTable = false;
            this.listViewCycleItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.listViewCycleItem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewCycleItem.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listViewCycleItem.Depth = 0;
            this.listViewCycleItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewCycleItem.FullRowSelect = true;
            this.listViewCycleItem.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewCycleItem.HideSelection = false;
            this.listViewCycleItem.Location = new System.Drawing.Point(3, 53);
            this.listViewCycleItem.MinimumSize = new System.Drawing.Size(200, 92);
            this.listViewCycleItem.MouseLocation = new System.Drawing.Point(-1, -1);
            this.listViewCycleItem.MouseState = MaterialSkin.MouseState.OUT;
            this.listViewCycleItem.MultiSelect = false;
            this.listViewCycleItem.Name = "listViewCycleItem";
            this.listViewCycleItem.OwnerDraw = true;
            this.listViewCycleItem.Size = new System.Drawing.Size(412, 261);
            this.listViewCycleItem.TabIndex = 2;
            this.listViewCycleItem.UseCompatibleStateImageBehavior = false;
            this.listViewCycleItem.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "№";
            this.columnHeader1.Width = 30;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "День";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Смена";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 80;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Название";
            this.columnHeader4.Width = 200;
            // 
            // labelCycleLeght
            // 
            this.labelCycleLeght.AutoSize = true;
            this.labelCycleLeght.Depth = 0;
            this.labelCycleLeght.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCycleLeght.Font = new System.Drawing.Font("Roboto", 34F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.labelCycleLeght.FontType = MaterialSkin.MaterialSkinManager.fontType.H4;
            this.labelCycleLeght.Location = new System.Drawing.Point(421, 0);
            this.labelCycleLeght.MouseState = MaterialSkin.MouseState.HOVER;
            this.labelCycleLeght.Name = "labelCycleLeght";
            this.labelCycleLeght.Size = new System.Drawing.Size(282, 50);
            this.labelCycleLeght.TabIndex = 3;
            this.labelCycleLeght.Text = "0";
            this.labelCycleLeght.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Controls.Add(this.comboBoxShifts, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.buttonShiftAndDayAdd, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.buttonShiftAdd, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.buttonShiftLastDelete, 0, 3);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(421, 53);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 5;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(282, 261);
            this.tableLayoutPanel4.TabIndex = 4;
            // 
            // comboBoxShifts
            // 
            this.comboBoxShifts.AutoResize = false;
            this.comboBoxShifts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.comboBoxShifts.Depth = 0;
            this.comboBoxShifts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxShifts.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxShifts.DropDownHeight = 174;
            this.comboBoxShifts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxShifts.DropDownWidth = 121;
            this.comboBoxShifts.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.comboBoxShifts.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxShifts.FormattingEnabled = true;
            this.comboBoxShifts.Hint = "Смена";
            this.comboBoxShifts.IntegralHeight = false;
            this.comboBoxShifts.ItemHeight = 43;
            this.comboBoxShifts.Location = new System.Drawing.Point(3, 3);
            this.comboBoxShifts.MaxDropDownItems = 4;
            this.comboBoxShifts.MouseState = MaterialSkin.MouseState.OUT;
            this.comboBoxShifts.Name = "comboBoxShifts";
            this.comboBoxShifts.Size = new System.Drawing.Size(276, 49);
            this.comboBoxShifts.StartIndex = 0;
            this.comboBoxShifts.TabIndex = 0;
            // 
            // buttonShiftAndDayAdd
            // 
            this.buttonShiftAndDayAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonShiftAndDayAdd.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonShiftAndDayAdd.Depth = 0;
            this.buttonShiftAndDayAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShiftAndDayAdd.HighEmphasis = true;
            this.buttonShiftAndDayAdd.Icon = null;
            this.buttonShiftAndDayAdd.Location = new System.Drawing.Point(4, 58);
            this.buttonShiftAndDayAdd.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonShiftAndDayAdd.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonShiftAndDayAdd.Name = "buttonShiftAndDayAdd";
            this.buttonShiftAndDayAdd.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonShiftAndDayAdd.Size = new System.Drawing.Size(274, 40);
            this.buttonShiftAndDayAdd.TabIndex = 1;
            this.buttonShiftAndDayAdd.Text = "Добавить смену в новый день";
            this.buttonShiftAndDayAdd.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonShiftAndDayAdd.UseAccentColor = false;
            this.buttonShiftAndDayAdd.UseVisualStyleBackColor = true;
            this.buttonShiftAndDayAdd.Click += new System.EventHandler(this.buttonShiftAndDayAdd_Click);
            // 
            // buttonShiftAdd
            // 
            this.buttonShiftAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonShiftAdd.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonShiftAdd.Depth = 0;
            this.buttonShiftAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShiftAdd.HighEmphasis = true;
            this.buttonShiftAdd.Icon = null;
            this.buttonShiftAdd.Location = new System.Drawing.Point(4, 110);
            this.buttonShiftAdd.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonShiftAdd.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonShiftAdd.Name = "buttonShiftAdd";
            this.buttonShiftAdd.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonShiftAdd.Size = new System.Drawing.Size(274, 40);
            this.buttonShiftAdd.TabIndex = 2;
            this.buttonShiftAdd.Text = "Добавить смену в текущий день";
            this.buttonShiftAdd.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonShiftAdd.UseAccentColor = false;
            this.buttonShiftAdd.UseVisualStyleBackColor = true;
            this.buttonShiftAdd.Click += new System.EventHandler(this.buttonShiftAdd_Click);
            // 
            // buttonShiftLastDelete
            // 
            this.buttonShiftLastDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonShiftLastDelete.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonShiftLastDelete.Depth = 0;
            this.buttonShiftLastDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShiftLastDelete.HighEmphasis = true;
            this.buttonShiftLastDelete.Icon = null;
            this.buttonShiftLastDelete.Location = new System.Drawing.Point(4, 162);
            this.buttonShiftLastDelete.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonShiftLastDelete.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonShiftLastDelete.Name = "buttonShiftLastDelete";
            this.buttonShiftLastDelete.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonShiftLastDelete.Size = new System.Drawing.Size(274, 40);
            this.buttonShiftLastDelete.TabIndex = 3;
            this.buttonShiftLastDelete.Text = "Удалить последнюю (выбранную?) смену";
            this.buttonShiftLastDelete.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonShiftLastDelete.UseAccentColor = false;
            this.buttonShiftLastDelete.UseVisualStyleBackColor = true;
            this.buttonShiftLastDelete.Click += new System.EventHandler(this.buttonShiftLastDelete_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel3.Controls.Add(this.materialButton1, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.materialButton2, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(13, 335);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(706, 40);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // materialButton1
            // 
            this.materialButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialButton1.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButton1.Depth = 0;
            this.materialButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialButton1.HighEmphasis = true;
            this.materialButton1.Icon = null;
            this.materialButton1.Location = new System.Drawing.Point(470, 6);
            this.materialButton1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton1.Name = "materialButton1";
            this.materialButton1.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButton1.Size = new System.Drawing.Size(112, 28);
            this.materialButton1.TabIndex = 0;
            this.materialButton1.Text = "Добавить";
            this.materialButton1.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButton1.UseAccentColor = false;
            this.materialButton1.UseVisualStyleBackColor = true;
            this.materialButton1.Click += new System.EventHandler(this.materialButton1_Click);
            // 
            // materialButton2
            // 
            this.materialButton2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialButton2.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButton2.Depth = 0;
            this.materialButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialButton2.HighEmphasis = true;
            this.materialButton2.Icon = null;
            this.materialButton2.Location = new System.Drawing.Point(590, 6);
            this.materialButton2.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialButton2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton2.Name = "materialButton2";
            this.materialButton2.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButton2.Size = new System.Drawing.Size(112, 28);
            this.materialButton2.TabIndex = 1;
            this.materialButton2.Text = "Отмена";
            this.materialButton2.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButton2.UseAccentColor = false;
            this.materialButton2.UseVisualStyleBackColor = true;
            this.materialButton2.Click += new System.EventHandler(this.materialButton2_Click);
            // 
            // FormAddCycle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 462);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormStyle = MaterialSkin.Controls.MaterialForm.FormStyles.ActionBar_64;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddCycle";
            this.Padding = new System.Windows.Forms.Padding(3, 81, 3, 3);
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Шаблоны смен";
            this.Load += new System.EventHandler(this.FormAddCycle_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private MaterialSkin.Controls.MaterialTextBox textBoxCycleName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private MaterialSkin.Controls.MaterialButton materialButton1;
        private MaterialSkin.Controls.MaterialButton materialButton2;
        private MaterialSkin.Controls.MaterialListView listViewCycleItem;
        private MaterialSkin.Controls.MaterialLabel labelCycleLeght;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private MaterialSkin.Controls.MaterialComboBox comboBoxShifts;
        private MaterialSkin.Controls.MaterialButton buttonShiftAndDayAdd;
        private MaterialSkin.Controls.MaterialButton buttonShiftAdd;
        private MaterialSkin.Controls.MaterialButton buttonShiftLastDelete;
    }
}