namespace Production_planning
{
    partial class FormAddBrigade
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
            this.textBoxBrigadeName = new MaterialSkin.Controls.MaterialTextBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxCycles = new MaterialSkin.Controls.MaterialComboBox();
            this.textBoxDate = new MaterialSkin.Controls.MaterialMaskedTextBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.buttonCalendar = new MaterialSkin.Controls.MaterialButton();
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(624, 165);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.textBoxBrigadeName, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(13, 12);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(598, 104);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // textBoxBrigadeName
            // 
            this.textBoxBrigadeName.AnimateReadOnly = false;
            this.textBoxBrigadeName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxBrigadeName.Depth = 0;
            this.textBoxBrigadeName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxBrigadeName.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBoxBrigadeName.Hint = "Название";
            this.textBoxBrigadeName.LeadingIcon = null;
            this.textBoxBrigadeName.Location = new System.Drawing.Point(3, 3);
            this.textBoxBrigadeName.MaxLength = 50;
            this.textBoxBrigadeName.MouseState = MaterialSkin.MouseState.OUT;
            this.textBoxBrigadeName.Multiline = false;
            this.textBoxBrigadeName.Name = "textBoxBrigadeName";
            this.textBoxBrigadeName.Size = new System.Drawing.Size(592, 50);
            this.textBoxBrigadeName.TabIndex = 1;
            this.textBoxBrigadeName.Text = "";
            this.textBoxBrigadeName.TrailingIcon = null;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel4.Controls.Add(this.comboBoxCycles, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.textBoxDate, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.dateTimePicker1, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.buttonCalendar, 3, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 53);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(592, 48);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // comboBoxCycles
            // 
            this.comboBoxCycles.AutoResize = false;
            this.comboBoxCycles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.comboBoxCycles.Depth = 0;
            this.comboBoxCycles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxCycles.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxCycles.DropDownHeight = 174;
            this.comboBoxCycles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCycles.DropDownWidth = 121;
            this.comboBoxCycles.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxCycles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxCycles.FormattingEnabled = true;
            this.comboBoxCycles.Hint = "Шаблон";
            this.comboBoxCycles.IntegralHeight = false;
            this.comboBoxCycles.ItemHeight = 43;
            this.comboBoxCycles.Location = new System.Drawing.Point(3, 3);
            this.comboBoxCycles.MaxDropDownItems = 4;
            this.comboBoxCycles.MouseState = MaterialSkin.MouseState.OUT;
            this.comboBoxCycles.Name = "comboBoxCycles";
            this.comboBoxCycles.Size = new System.Drawing.Size(399, 49);
            this.comboBoxCycles.StartIndex = 0;
            this.comboBoxCycles.TabIndex = 0;
            // 
            // textBoxDate
            // 
            this.textBoxDate.AllowPromptAsInput = true;
            this.textBoxDate.AnimateReadOnly = false;
            this.textBoxDate.AsciiOnly = false;
            this.textBoxDate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.textBoxDate.BeepOnError = false;
            this.textBoxDate.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.IncludeLiterals;
            this.textBoxDate.Depth = 0;
            this.textBoxDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxDate.HidePromptOnLeave = false;
            this.textBoxDate.HideSelection = true;
            this.textBoxDate.Hint = "Дата начала";
            this.textBoxDate.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Default;
            this.textBoxDate.LeadingIcon = null;
            this.textBoxDate.Location = new System.Drawing.Point(408, 3);
            this.textBoxDate.Mask = "00\\.00\\.0000";
            this.textBoxDate.MaxLength = 32767;
            this.textBoxDate.MouseState = MaterialSkin.MouseState.OUT;
            this.textBoxDate.Name = "textBoxDate";
            this.textBoxDate.PasswordChar = '\0';
            this.textBoxDate.PrefixSuffixText = null;
            this.textBoxDate.PromptChar = '_';
            this.textBoxDate.ReadOnly = false;
            this.textBoxDate.RejectInputOnFirstFailure = false;
            this.textBoxDate.ResetOnPrompt = true;
            this.textBoxDate.ResetOnSpace = true;
            this.textBoxDate.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxDate.SelectedText = "";
            this.textBoxDate.SelectionLength = 0;
            this.textBoxDate.SelectionStart = 0;
            this.textBoxDate.ShortcutsEnabled = true;
            this.textBoxDate.Size = new System.Drawing.Size(129, 48);
            this.textBoxDate.SkipLiterals = true;
            this.textBoxDate.TabIndex = 3;
            this.textBoxDate.TabStop = false;
            this.textBoxDate.Text = "  .  .";
            this.textBoxDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxDate.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludeLiterals;
            this.textBoxDate.TrailingIcon = null;
            this.textBoxDate.UseSystemPasswordChar = false;
            this.textBoxDate.ValidatingType = null;
            this.textBoxDate.Leave += new System.EventHandler(this.textBoxDate_Leave);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.dateTimePicker1.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(543, 3);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(0, 21);
            this.dateTimePicker1.TabIndex = 2;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // buttonCalendar
            // 
            this.buttonCalendar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonCalendar.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonCalendar.Depth = 0;
            this.buttonCalendar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCalendar.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCalendar.HighEmphasis = true;
            this.buttonCalendar.Icon = null;
            this.buttonCalendar.Location = new System.Drawing.Point(544, 6);
            this.buttonCalendar.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonCalendar.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonCalendar.Name = "buttonCalendar";
            this.buttonCalendar.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonCalendar.Size = new System.Drawing.Size(44, 36);
            this.buttonCalendar.TabIndex = 4;
            this.buttonCalendar.Text = "🗓️";
            this.buttonCalendar.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonCalendar.UseAccentColor = false;
            this.buttonCalendar.UseVisualStyleBackColor = true;
            this.buttonCalendar.Click += new System.EventHandler(this.buttonCalendar_Click);
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
            this.tableLayoutPanel3.Location = new System.Drawing.Point(13, 122);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(598, 40);
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
            this.materialButton1.Location = new System.Drawing.Point(362, 6);
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
            this.materialButton2.Location = new System.Drawing.Point(482, 6);
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
            // FormAddBrigade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 249);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormStyle = MaterialSkin.Controls.MaterialForm.FormStyles.ActionBar_64;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddBrigade";
            this.Padding = new System.Windows.Forms.Padding(3, 81, 3, 3);
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "График";
            this.Load += new System.EventHandler(this.FormAddCycle_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private MaterialSkin.Controls.MaterialTextBox textBoxBrigadeName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private MaterialSkin.Controls.MaterialButton materialButton1;
        private MaterialSkin.Controls.MaterialButton materialButton2;
        private MaterialSkin.Controls.MaterialComboBox comboBoxCycles;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private MaterialSkin.Controls.MaterialMaskedTextBox textBoxDate;
        private MaterialSkin.Controls.MaterialButton buttonCalendar;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
    }
}