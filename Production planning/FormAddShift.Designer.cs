namespace Production_planning
{
    partial class FormAddShift
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
            this.maskedTextBoxShiftEnd = new MaterialSkin.Controls.MaterialMaskedTextBox();
            this.textBoxShiftNum = new MaterialSkin.Controls.MaterialTextBox();
            this.textBoxShiftName = new MaterialSkin.Controls.MaterialTextBox();
            this.maskedTextBoxShiftStart = new MaterialSkin.Controls.MaterialMaskedTextBox();
            this.materialCheckbox1 = new MaterialSkin.Controls.MaterialCheckbox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.materialButton1 = new MaterialSkin.Controls.MaterialButton();
            this.materialButton2 = new MaterialSkin.Controls.MaterialButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
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
            this.tableLayoutPanel1.Controls.Add(this.materialCheckbox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 88);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(634, 218);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.63636F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.09091F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.63636F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.63636F));
            this.tableLayoutPanel2.Controls.Add(this.maskedTextBoxShiftEnd, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBoxShiftNum, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBoxShiftName, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.maskedTextBoxShiftStart, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(13, 46);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(608, 113);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // maskedTextBoxShiftEnd
            // 
            this.maskedTextBoxShiftEnd.AllowPromptAsInput = true;
            this.maskedTextBoxShiftEnd.AnimateReadOnly = false;
            this.maskedTextBoxShiftEnd.AsciiOnly = false;
            this.maskedTextBoxShiftEnd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.maskedTextBoxShiftEnd.BeepOnError = false;
            this.maskedTextBoxShiftEnd.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.IncludeLiterals;
            this.maskedTextBoxShiftEnd.Depth = 0;
            this.maskedTextBoxShiftEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBoxShiftEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.maskedTextBoxShiftEnd.HidePromptOnLeave = false;
            this.maskedTextBoxShiftEnd.HideSelection = true;
            this.maskedTextBoxShiftEnd.Hint = "  Конец";
            this.maskedTextBoxShiftEnd.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Default;
            this.maskedTextBoxShiftEnd.LeadingIcon = null;
            this.maskedTextBoxShiftEnd.Location = new System.Drawing.Point(526, 3);
            this.maskedTextBoxShiftEnd.Mask = "00:00";
            this.maskedTextBoxShiftEnd.MaxLength = 32767;
            this.maskedTextBoxShiftEnd.MouseState = MaterialSkin.MouseState.OUT;
            this.maskedTextBoxShiftEnd.Name = "maskedTextBoxShiftEnd";
            this.maskedTextBoxShiftEnd.PasswordChar = '\0';
            this.maskedTextBoxShiftEnd.PrefixSuffixText = null;
            this.maskedTextBoxShiftEnd.PromptChar = '_';
            this.maskedTextBoxShiftEnd.ReadOnly = false;
            this.maskedTextBoxShiftEnd.RejectInputOnFirstFailure = false;
            this.maskedTextBoxShiftEnd.ResetOnPrompt = true;
            this.maskedTextBoxShiftEnd.ResetOnSpace = true;
            this.maskedTextBoxShiftEnd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.maskedTextBoxShiftEnd.SelectedText = "";
            this.maskedTextBoxShiftEnd.SelectionLength = 0;
            this.maskedTextBoxShiftEnd.SelectionStart = 0;
            this.maskedTextBoxShiftEnd.ShortcutsEnabled = true;
            this.maskedTextBoxShiftEnd.Size = new System.Drawing.Size(79, 48);
            this.maskedTextBoxShiftEnd.SkipLiterals = true;
            this.maskedTextBoxShiftEnd.TabIndex = 3;
            this.maskedTextBoxShiftEnd.TabStop = false;
            this.maskedTextBoxShiftEnd.Text = "  :";
            this.maskedTextBoxShiftEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maskedTextBoxShiftEnd.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludeLiterals;
            this.maskedTextBoxShiftEnd.TrailingIcon = null;
            this.maskedTextBoxShiftEnd.UseSystemPasswordChar = false;
            this.maskedTextBoxShiftEnd.ValidatingType = null;
            this.maskedTextBoxShiftEnd.Leave += new System.EventHandler(this.maskedTextBoxShiftEnd_Leave);
            // 
            // textBoxShiftNum
            // 
            this.textBoxShiftNum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxShiftNum.AnimateReadOnly = false;
            this.textBoxShiftNum.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxShiftNum.Depth = 0;
            this.textBoxShiftNum.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBoxShiftNum.Hint = "Номер";
            this.textBoxShiftNum.LeadingIcon = null;
            this.textBoxShiftNum.Location = new System.Drawing.Point(3, 3);
            this.textBoxShiftNum.MaxLength = 2;
            this.textBoxShiftNum.MouseState = MaterialSkin.MouseState.OUT;
            this.textBoxShiftNum.Multiline = false;
            this.textBoxShiftNum.Name = "textBoxShiftNum";
            this.textBoxShiftNum.Size = new System.Drawing.Size(76, 50);
            this.textBoxShiftNum.TabIndex = 0;
            this.textBoxShiftNum.Text = "";
            this.textBoxShiftNum.TrailingIcon = null;
            this.textBoxShiftNum.TextChanged += new System.EventHandler(this.materialTextBox1_TextChanged);
            this.textBoxShiftNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.materialTextBox1_KeyPress);
            // 
            // textBoxShiftName
            // 
            this.textBoxShiftName.AnimateReadOnly = false;
            this.textBoxShiftName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxShiftName.Depth = 0;
            this.textBoxShiftName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxShiftName.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBoxShiftName.Hint = "Название";
            this.textBoxShiftName.LeadingIcon = null;
            this.textBoxShiftName.Location = new System.Drawing.Point(85, 3);
            this.textBoxShiftName.MaxLength = 50;
            this.textBoxShiftName.MouseState = MaterialSkin.MouseState.OUT;
            this.textBoxShiftName.Multiline = false;
            this.textBoxShiftName.Name = "textBoxShiftName";
            this.textBoxShiftName.Size = new System.Drawing.Size(353, 50);
            this.textBoxShiftName.TabIndex = 1;
            this.textBoxShiftName.Text = "";
            this.textBoxShiftName.TrailingIcon = null;
            // 
            // maskedTextBoxShiftStart
            // 
            this.maskedTextBoxShiftStart.AllowPromptAsInput = true;
            this.maskedTextBoxShiftStart.AnimateReadOnly = false;
            this.maskedTextBoxShiftStart.AsciiOnly = false;
            this.maskedTextBoxShiftStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.maskedTextBoxShiftStart.BeepOnError = false;
            this.maskedTextBoxShiftStart.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.IncludePrompt;
            this.maskedTextBoxShiftStart.Depth = 0;
            this.maskedTextBoxShiftStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBoxShiftStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.maskedTextBoxShiftStart.HidePromptOnLeave = false;
            this.maskedTextBoxShiftStart.HideSelection = true;
            this.maskedTextBoxShiftStart.Hint = "Начало";
            this.maskedTextBoxShiftStart.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Default;
            this.maskedTextBoxShiftStart.LeadingIcon = null;
            this.maskedTextBoxShiftStart.Location = new System.Drawing.Point(444, 3);
            this.maskedTextBoxShiftStart.Mask = "00:00";
            this.maskedTextBoxShiftStart.MaxLength = 32767;
            this.maskedTextBoxShiftStart.MouseState = MaterialSkin.MouseState.OUT;
            this.maskedTextBoxShiftStart.Name = "maskedTextBoxShiftStart";
            this.maskedTextBoxShiftStart.PasswordChar = '\0';
            this.maskedTextBoxShiftStart.PrefixSuffixText = null;
            this.maskedTextBoxShiftStart.PromptChar = '_';
            this.maskedTextBoxShiftStart.ReadOnly = false;
            this.maskedTextBoxShiftStart.RejectInputOnFirstFailure = false;
            this.maskedTextBoxShiftStart.ResetOnPrompt = true;
            this.maskedTextBoxShiftStart.ResetOnSpace = true;
            this.maskedTextBoxShiftStart.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.maskedTextBoxShiftStart.SelectedText = "";
            this.maskedTextBoxShiftStart.SelectionLength = 0;
            this.maskedTextBoxShiftStart.SelectionStart = 0;
            this.maskedTextBoxShiftStart.ShortcutsEnabled = true;
            this.maskedTextBoxShiftStart.Size = new System.Drawing.Size(76, 48);
            this.maskedTextBoxShiftStart.SkipLiterals = true;
            this.maskedTextBoxShiftStart.TabIndex = 2;
            this.maskedTextBoxShiftStart.TabStop = false;
            this.maskedTextBoxShiftStart.Text = "  :";
            this.maskedTextBoxShiftStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maskedTextBoxShiftStart.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludeLiterals;
            this.maskedTextBoxShiftStart.TrailingIcon = null;
            this.maskedTextBoxShiftStart.UseSystemPasswordChar = false;
            this.maskedTextBoxShiftStart.ValidatingType = null;
            this.maskedTextBoxShiftStart.Leave += new System.EventHandler(this.maskedTextBoxShiftStart_Leave);
            // 
            // materialCheckbox1
            // 
            this.materialCheckbox1.AutoSize = true;
            this.materialCheckbox1.Depth = 0;
            this.materialCheckbox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialCheckbox1.Location = new System.Drawing.Point(10, 0);
            this.materialCheckbox1.Margin = new System.Windows.Forms.Padding(0);
            this.materialCheckbox1.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialCheckbox1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCheckbox1.Name = "materialCheckbox1";
            this.materialCheckbox1.ReadOnly = false;
            this.materialCheckbox1.Ripple = true;
            this.materialCheckbox1.Size = new System.Drawing.Size(614, 43);
            this.materialCheckbox1.TabIndex = 1;
            this.materialCheckbox1.Text = "Выходной";
            this.materialCheckbox1.UseVisualStyleBackColor = true;
            this.materialCheckbox1.CheckedChanged += new System.EventHandler(this.materialCheckbox1_CheckedChanged);
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
            this.tableLayoutPanel3.Location = new System.Drawing.Point(13, 165);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(608, 50);
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
            this.materialButton1.Location = new System.Drawing.Point(372, 6);
            this.materialButton1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton1.Name = "materialButton1";
            this.materialButton1.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButton1.Size = new System.Drawing.Size(112, 38);
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
            this.materialButton2.Location = new System.Drawing.Point(492, 6);
            this.materialButton2.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialButton2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton2.Name = "materialButton2";
            this.materialButton2.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButton2.Size = new System.Drawing.Size(112, 38);
            this.materialButton2.TabIndex = 1;
            this.materialButton2.Text = "Отмена";
            this.materialButton2.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButton2.UseAccentColor = false;
            this.materialButton2.UseVisualStyleBackColor = true;
            this.materialButton2.Click += new System.EventHandler(this.materialButton2_Click);
            // 
            // FormAddShift
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 309);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormStyle = MaterialSkin.Controls.MaterialForm.FormStyles.ActionBar_64;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddShift";
            this.Padding = new System.Windows.Forms.Padding(3, 88, 3, 3);
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Смена";
            this.Load += new System.EventHandler(this.FormAddShift_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private MaterialSkin.Controls.MaterialTextBox textBoxShiftNum;
        private MaterialSkin.Controls.MaterialTextBox textBoxShiftName;
        private MaterialSkin.Controls.MaterialMaskedTextBox maskedTextBoxShiftEnd;
        private MaterialSkin.Controls.MaterialMaskedTextBox maskedTextBoxShiftStart;
        private MaterialSkin.Controls.MaterialCheckbox materialCheckbox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private MaterialSkin.Controls.MaterialButton materialButton1;
        private MaterialSkin.Controls.MaterialButton materialButton2;
    }
}