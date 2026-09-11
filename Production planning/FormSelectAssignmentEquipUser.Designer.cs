namespace Production_planning
{
    partial class FormSelectAssignmentEquipUser
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
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxAssignment = new MaterialSkin.Controls.MaterialComboBox();
            this.comboBoxTargetShifts = new MaterialSkin.Controls.MaterialComboBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonAdd = new MaterialSkin.Controls.MaterialButton();
            this.buttonCancel = new MaterialSkin.Controls.MaterialButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelDate = new MaterialSkin.Controls.MaterialLabel();
            this.labelShift = new MaterialSkin.Controls.MaterialLabel();
            this.textBoxComment = new MaterialSkin.Controls.MaterialTextBox2();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxComment, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 88);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(714, 259);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel4.Controls.Add(this.comboBoxAssignment, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.comboBoxTargetShifts, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(13, 53);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(688, 54);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // comboBoxAssignment
            // 
            this.comboBoxAssignment.AutoResize = false;
            this.comboBoxAssignment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.comboBoxAssignment.Depth = 0;
            this.comboBoxAssignment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxAssignment.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxAssignment.DropDownHeight = 174;
            this.comboBoxAssignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAssignment.DropDownWidth = 121;
            this.comboBoxAssignment.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxAssignment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxAssignment.FormattingEnabled = true;
            this.comboBoxAssignment.Hint = "Рабочее место";
            this.comboBoxAssignment.IntegralHeight = false;
            this.comboBoxAssignment.ItemHeight = 43;
            this.comboBoxAssignment.Location = new System.Drawing.Point(3, 3);
            this.comboBoxAssignment.MaxDropDownItems = 4;
            this.comboBoxAssignment.MouseState = MaterialSkin.MouseState.OUT;
            this.comboBoxAssignment.Name = "comboBoxAssignment";
            this.comboBoxAssignment.Size = new System.Drawing.Size(475, 49);
            this.comboBoxAssignment.StartIndex = 0;
            this.comboBoxAssignment.TabIndex = 0;
            this.comboBoxAssignment.SelectedIndexChanged += new System.EventHandler(this.comboBoxAssignment_SelectedIndexChanged);
            // 
            // comboBoxTargetShifts
            // 
            this.comboBoxTargetShifts.AutoResize = false;
            this.comboBoxTargetShifts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.comboBoxTargetShifts.Depth = 0;
            this.comboBoxTargetShifts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxTargetShifts.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxTargetShifts.DropDownHeight = 174;
            this.comboBoxTargetShifts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTargetShifts.DropDownWidth = 121;
            this.comboBoxTargetShifts.Enabled = false;
            this.comboBoxTargetShifts.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.comboBoxTargetShifts.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxTargetShifts.FormattingEnabled = true;
            this.comboBoxTargetShifts.Hint = "Смена";
            this.comboBoxTargetShifts.IntegralHeight = false;
            this.comboBoxTargetShifts.ItemHeight = 43;
            this.comboBoxTargetShifts.Location = new System.Drawing.Point(484, 3);
            this.comboBoxTargetShifts.MaxDropDownItems = 4;
            this.comboBoxTargetShifts.MouseState = MaterialSkin.MouseState.OUT;
            this.comboBoxTargetShifts.Name = "comboBoxTargetShifts";
            this.comboBoxTargetShifts.Size = new System.Drawing.Size(201, 49);
            this.comboBoxTargetShifts.StartIndex = 0;
            this.comboBoxTargetShifts.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel3.Controls.Add(this.buttonAdd, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonCancel, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(13, 192);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(688, 64);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // buttonAdd
            // 
            this.buttonAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonAdd.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonAdd.Depth = 0;
            this.buttonAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAdd.HighEmphasis = true;
            this.buttonAdd.Icon = null;
            this.buttonAdd.Location = new System.Drawing.Point(332, 6);
            this.buttonAdd.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonAdd.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonAdd.Size = new System.Drawing.Size(232, 52);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "Сохранить";
            this.buttonAdd.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonAdd.UseAccentColor = false;
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.materialButton1_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonCancel.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.buttonCancel.Depth = 0;
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCancel.HighEmphasis = true;
            this.buttonCancel.Icon = null;
            this.buttonCancel.Location = new System.Drawing.Point(572, 6);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonCancel.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.NoAccentTextColor = System.Drawing.Color.Empty;
            this.buttonCancel.Size = new System.Drawing.Size(112, 52);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.buttonCancel.UseAccentColor = false;
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.materialButton2_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.labelDate, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelShift, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(13, 13);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(688, 34);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // labelDate
            // 
            this.labelDate.AutoSize = true;
            this.labelDate.Depth = 0;
            this.labelDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDate.Font = new System.Drawing.Font("Roboto Medium", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.labelDate.FontType = MaterialSkin.MaterialSkinManager.fontType.H6;
            this.labelDate.Location = new System.Drawing.Point(3, 0);
            this.labelDate.MouseState = MaterialSkin.MouseState.HOVER;
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(338, 34);
            this.labelDate.TabIndex = 0;
            this.labelDate.Text = "Дата";
            this.labelDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelShift
            // 
            this.labelShift.AutoSize = true;
            this.labelShift.Depth = 0;
            this.labelShift.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelShift.Font = new System.Drawing.Font("Roboto Medium", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.labelShift.FontType = MaterialSkin.MaterialSkinManager.fontType.H6;
            this.labelShift.Location = new System.Drawing.Point(347, 0);
            this.labelShift.MouseState = MaterialSkin.MouseState.HOVER;
            this.labelShift.Name = "labelShift";
            this.labelShift.Size = new System.Drawing.Size(338, 34);
            this.labelShift.TabIndex = 1;
            this.labelShift.Text = "Смена";
            this.labelShift.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxComment
            // 
            this.textBoxComment.AnimateReadOnly = false;
            this.textBoxComment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.textBoxComment.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.textBoxComment.Depth = 0;
            this.textBoxComment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxComment.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBoxComment.HideSelection = true;
            this.textBoxComment.Hint = "Комментарий";
            this.textBoxComment.LeadingIcon = null;
            this.textBoxComment.Location = new System.Drawing.Point(13, 113);
            this.textBoxComment.MaxLength = 32767;
            this.textBoxComment.MouseState = MaterialSkin.MouseState.OUT;
            this.textBoxComment.Name = "textBoxComment";
            this.textBoxComment.PasswordChar = '\0';
            this.textBoxComment.PrefixSuffixText = null;
            this.textBoxComment.ReadOnly = false;
            this.textBoxComment.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxComment.SelectedText = "";
            this.textBoxComment.SelectionLength = 0;
            this.textBoxComment.SelectionStart = 0;
            this.textBoxComment.ShortcutsEnabled = true;
            this.textBoxComment.Size = new System.Drawing.Size(688, 48);
            this.textBoxComment.TabIndex = 4;
            this.textBoxComment.TabStop = false;
            this.textBoxComment.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxComment.TrailingIcon = null;
            this.textBoxComment.UseSystemPasswordChar = false;
            // 
            // FormSelectAssignmentEquipUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 350);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormStyle = MaterialSkin.Controls.MaterialForm.FormStyles.ActionBar_64;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectAssignmentEquipUser";
            this.Padding = new System.Windows.Forms.Padding(3, 88, 3, 3);
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Новое назначение";
            this.Load += new System.EventHandler(this.FormSelectUserPosition_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private MaterialSkin.Controls.MaterialButton buttonAdd;
        private MaterialSkin.Controls.MaterialButton buttonCancel;
        private MaterialSkin.Controls.MaterialComboBox comboBoxAssignment;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private MaterialSkin.Controls.MaterialLabel labelDate;
        private MaterialSkin.Controls.MaterialLabel labelShift;
        private MaterialSkin.Controls.MaterialTextBox2 textBoxComment;
        private MaterialSkin.Controls.MaterialComboBox comboBoxTargetShifts;
    }
}