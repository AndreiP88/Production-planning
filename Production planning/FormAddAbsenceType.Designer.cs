namespace Production_planning
{
    partial class FormAddAbsenceType
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
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxName = new MaterialSkin.Controls.MaterialTextBox();
            this.textBoxDescription = new MaterialSkin.Controls.MaterialTextBox2();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.switchEndDateRequired = new MaterialSkin.Controls.MaterialSwitch();
            this.btnSelectColor = new MaterialSkin.Controls.MaterialButton();
            this.panelColorPreview = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.materialButtonSave = new MaterialSkin.Controls.MaterialButton();
            this.materialButton2 = new MaterialSkin.Controls.MaterialButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 88);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(734, 308);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel5, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBoxDescription, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(13, 13);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(708, 242);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Controls.Add(this.textBoxName, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(702, 54);
            this.tableLayoutPanel5.TabIndex = 3;
            // 
            // textBoxName
            // 
            this.textBoxName.AnimateReadOnly = false;
            this.textBoxName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxName.Depth = 0;
            this.textBoxName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxName.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBoxName.Hint = "Название";
            this.textBoxName.LeadingIcon = null;
            this.textBoxName.Location = new System.Drawing.Point(3, 3);
            this.textBoxName.MaxLength = 50;
            this.textBoxName.MouseState = MaterialSkin.MouseState.OUT;
            this.textBoxName.Multiline = false;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(696, 50);
            this.textBoxName.TabIndex = 1;
            this.textBoxName.Text = "";
            this.textBoxName.TrailingIcon = null;
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.AnimateReadOnly = false;
            this.textBoxDescription.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.textBoxDescription.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.textBoxDescription.Depth = 0;
            this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBoxDescription.HideSelection = true;
            this.textBoxDescription.Hint = "Примечание";
            this.textBoxDescription.LeadingIcon = null;
            this.textBoxDescription.Location = new System.Drawing.Point(3, 123);
            this.textBoxDescription.MaxLength = 32767;
            this.textBoxDescription.MouseState = MaterialSkin.MouseState.OUT;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.PasswordChar = '\0';
            this.textBoxDescription.PrefixSuffixText = null;
            this.textBoxDescription.ReadOnly = false;
            this.textBoxDescription.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxDescription.SelectedText = "";
            this.textBoxDescription.SelectionLength = 0;
            this.textBoxDescription.SelectionStart = 0;
            this.textBoxDescription.ShortcutsEnabled = true;
            this.textBoxDescription.Size = new System.Drawing.Size(702, 48);
            this.textBoxDescription.TabIndex = 4;
            this.textBoxDescription.TabStop = false;
            this.textBoxDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxDescription.TrailingIcon = null;
            this.textBoxDescription.UseSystemPasswordChar = false;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.switchEndDateRequired, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnSelectColor, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.panelColorPreview, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 63);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(702, 54);
            this.tableLayoutPanel4.TabIndex = 5;
            // 
            // switchEndDateRequired
            // 
            this.switchEndDateRequired.AutoSize = true;
            this.switchEndDateRequired.Depth = 0;
            this.switchEndDateRequired.Dock = System.Windows.Forms.DockStyle.Fill;
            this.switchEndDateRequired.Location = new System.Drawing.Point(190, 0);
            this.switchEndDateRequired.Margin = new System.Windows.Forms.Padding(0);
            this.switchEndDateRequired.MouseLocation = new System.Drawing.Point(-1, -1);
            this.switchEndDateRequired.MouseState = MaterialSkin.MouseState.HOVER;
            this.switchEndDateRequired.Name = "switchEndDateRequired";
            this.switchEndDateRequired.Ripple = true;
            this.switchEndDateRequired.Size = new System.Drawing.Size(512, 54);
            this.switchEndDateRequired.TabIndex = 2;
            this.switchEndDateRequired.Text = "  обязательно указание даты завершения";
            this.switchEndDateRequired.UseVisualStyleBackColor = true;
            // 
            // btnSelectColor
            // 
            this.btnSelectColor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSelectColor.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnSelectColor.Depth = 0;
            this.btnSelectColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectColor.HighEmphasis = true;
            this.btnSelectColor.Icon = null;
            this.btnSelectColor.Location = new System.Drawing.Point(59, 6);
            this.btnSelectColor.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnSelectColor.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSelectColor.Name = "btnSelectColor";
            this.btnSelectColor.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnSelectColor.Size = new System.Drawing.Size(112, 42);
            this.btnSelectColor.TabIndex = 3;
            this.btnSelectColor.Text = "Выбрать цвет";
            this.btnSelectColor.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnSelectColor.UseAccentColor = false;
            this.btnSelectColor.UseVisualStyleBackColor = true;
            this.btnSelectColor.Click += new System.EventHandler(this.btnSelectColor_Click);
            // 
            // panelColorPreview
            // 
            this.panelColorPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelColorPreview.Location = new System.Drawing.Point(3, 3);
            this.panelColorPreview.Name = "panelColorPreview";
            this.panelColorPreview.Size = new System.Drawing.Size(49, 48);
            this.panelColorPreview.TabIndex = 4;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel3.Controls.Add(this.materialButtonSave, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.materialButton2, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(13, 261);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(708, 44);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // materialButtonSave
            // 
            this.materialButtonSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialButtonSave.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButtonSave.Depth = 0;
            this.materialButtonSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.materialButtonSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialButtonSave.HighEmphasis = true;
            this.materialButtonSave.Icon = null;
            this.materialButtonSave.Location = new System.Drawing.Point(472, 6);
            this.materialButtonSave.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialButtonSave.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButtonSave.Name = "materialButtonSave";
            this.materialButtonSave.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButtonSave.Size = new System.Drawing.Size(112, 32);
            this.materialButtonSave.TabIndex = 0;
            this.materialButtonSave.Text = "Добавить";
            this.materialButtonSave.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButtonSave.UseAccentColor = false;
            this.materialButtonSave.UseVisualStyleBackColor = true;
            this.materialButtonSave.Click += new System.EventHandler(this.materialButtonSave_Click);
            // 
            // materialButton2
            // 
            this.materialButton2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialButton2.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButton2.Depth = 0;
            this.materialButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.materialButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialButton2.HighEmphasis = true;
            this.materialButton2.Icon = null;
            this.materialButton2.Location = new System.Drawing.Point(592, 6);
            this.materialButton2.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialButton2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton2.Name = "materialButton2";
            this.materialButton2.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButton2.Size = new System.Drawing.Size(112, 32);
            this.materialButton2.TabIndex = 1;
            this.materialButton2.Text = "Отмена";
            this.materialButton2.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButton2.UseAccentColor = false;
            this.materialButton2.UseVisualStyleBackColor = true;
            // 
            // FormAddAbsenceType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 399);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormStyle = MaterialSkin.Controls.MaterialForm.FormStyles.ActionBar_64;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddAbsenceType";
            this.Padding = new System.Windows.Forms.Padding(3, 88, 3, 3);
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Причина отсутствия";
            this.Load += new System.EventHandler(this.FormAddAbsenceType_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private MaterialSkin.Controls.MaterialTextBox textBoxName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private MaterialSkin.Controls.MaterialButton materialButtonSave;
        private MaterialSkin.Controls.MaterialButton materialButton2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private MaterialSkin.Controls.MaterialTextBox2 textBoxDescription;
        private MaterialSkin.Controls.MaterialSwitch switchEndDateRequired;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private MaterialSkin.Controls.MaterialButton btnSelectColor;
        private System.Windows.Forms.Panel panelColorPreview;
    }
}