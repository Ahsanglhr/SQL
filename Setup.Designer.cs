namespace TimeReg
{
    partial class Setup
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpDB = new System.Windows.Forms.GroupBox();
            this.rdBtnGottingen = new System.Windows.Forms.RadioButton();
            this.rdBtnRoyston = new System.Windows.Forms.RadioButton();
            this.rdBtnNaerum = new System.Windows.Forms.RadioButton();
            this.grpHolidays = new System.Windows.Forms.GroupBox();
            this.rdBtnDE = new System.Windows.Forms.RadioButton();
            this.rdBtnUS = new System.Windows.Forms.RadioButton();
            this.rdBtnUK = new System.Windows.Forms.RadioButton();
            this.rdBtnDK = new System.Windows.Forms.RadioButton();
            this.rdBtnGCC = new System.Windows.Forms.RadioButton();
            this.grpDB.SuspendLayout();
            this.grpHolidays.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(307, 367);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 30);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(205, 367);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // grpDB
            // 
            this.grpDB.Controls.Add(this.rdBtnGCC);
            this.grpDB.Controls.Add(this.rdBtnGottingen);
            this.grpDB.Controls.Add(this.rdBtnRoyston);
            this.grpDB.Controls.Add(this.rdBtnNaerum);
            this.grpDB.Location = new System.Drawing.Point(23, 32);
            this.grpDB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpDB.Name = "grpDB";
            this.grpDB.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpDB.Size = new System.Drawing.Size(148, 303);
            this.grpDB.TabIndex = 2;
            this.grpDB.TabStop = false;
            this.grpDB.Text = "Database";
            // 
            // rdBtnGottingen
            // 
            this.rdBtnGottingen.AutoSize = true;
            this.rdBtnGottingen.Location = new System.Drawing.Point(20, 183);
            this.rdBtnGottingen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdBtnGottingen.Name = "rdBtnGottingen";
            this.rdBtnGottingen.Size = new System.Drawing.Size(91, 21);
            this.rdBtnGottingen.TabIndex = 2;
            this.rdBtnGottingen.TabStop = true;
            this.rdBtnGottingen.Text = "Göttingen";
            this.rdBtnGottingen.UseVisualStyleBackColor = true;
            this.rdBtnGottingen.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // rdBtnRoyston
            // 
            this.rdBtnRoyston.AutoSize = true;
            this.rdBtnRoyston.Location = new System.Drawing.Point(20, 117);
            this.rdBtnRoyston.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdBtnRoyston.Name = "rdBtnRoyston";
            this.rdBtnRoyston.Size = new System.Drawing.Size(81, 21);
            this.rdBtnRoyston.TabIndex = 1;
            this.rdBtnRoyston.TabStop = true;
            this.rdBtnRoyston.Text = "Royston";
            this.rdBtnRoyston.UseVisualStyleBackColor = true;
            this.rdBtnRoyston.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // rdBtnNaerum
            // 
            this.rdBtnNaerum.AutoSize = true;
            this.rdBtnNaerum.Location = new System.Drawing.Point(20, 50);
            this.rdBtnNaerum.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdBtnNaerum.Name = "rdBtnNaerum";
            this.rdBtnNaerum.Size = new System.Drawing.Size(76, 21);
            this.rdBtnNaerum.TabIndex = 0;
            this.rdBtnNaerum.TabStop = true;
            this.rdBtnNaerum.Text = "Nærum";
            this.rdBtnNaerum.UseVisualStyleBackColor = true;
            this.rdBtnNaerum.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // grpHolidays
            // 
            this.grpHolidays.Controls.Add(this.rdBtnDE);
            this.grpHolidays.Controls.Add(this.rdBtnUS);
            this.grpHolidays.Controls.Add(this.rdBtnUK);
            this.grpHolidays.Controls.Add(this.rdBtnDK);
            this.grpHolidays.Location = new System.Drawing.Point(243, 32);
            this.grpHolidays.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpHolidays.Name = "grpHolidays";
            this.grpHolidays.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpHolidays.Size = new System.Drawing.Size(148, 303);
            this.grpHolidays.TabIndex = 3;
            this.grpHolidays.TabStop = false;
            this.grpHolidays.Text = "Holidays";
            // 
            // rdBtnDE
            // 
            this.rdBtnDE.AutoSize = true;
            this.rdBtnDE.Location = new System.Drawing.Point(21, 250);
            this.rdBtnDE.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdBtnDE.Name = "rdBtnDE";
            this.rdBtnDE.Size = new System.Drawing.Size(80, 21);
            this.rdBtnDE.TabIndex = 3;
            this.rdBtnDE.TabStop = true;
            this.rdBtnDE.Text = "German";
            this.rdBtnDE.UseVisualStyleBackColor = true;
            this.rdBtnDE.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // rdBtnUS
            // 
            this.rdBtnUS.AutoSize = true;
            this.rdBtnUS.Location = new System.Drawing.Point(21, 183);
            this.rdBtnUS.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdBtnUS.Name = "rdBtnUS";
            this.rdBtnUS.Size = new System.Drawing.Size(88, 21);
            this.rdBtnUS.TabIndex = 2;
            this.rdBtnUS.TabStop = true;
            this.rdBtnUS.Text = "American";
            this.rdBtnUS.UseVisualStyleBackColor = true;
            this.rdBtnUS.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // rdBtnUK
            // 
            this.rdBtnUK.AutoSize = true;
            this.rdBtnUK.Location = new System.Drawing.Point(21, 117);
            this.rdBtnUK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdBtnUK.Name = "rdBtnUK";
            this.rdBtnUK.Size = new System.Drawing.Size(75, 21);
            this.rdBtnUK.TabIndex = 1;
            this.rdBtnUK.TabStop = true;
            this.rdBtnUK.Text = "English";
            this.rdBtnUK.UseVisualStyleBackColor = true;
            this.rdBtnUK.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // rdBtnDK
            // 
            this.rdBtnDK.AutoSize = true;
            this.rdBtnDK.Location = new System.Drawing.Point(21, 50);
            this.rdBtnDK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdBtnDK.Name = "rdBtnDK";
            this.rdBtnDK.Size = new System.Drawing.Size(73, 21);
            this.rdBtnDK.TabIndex = 0;
            this.rdBtnDK.TabStop = true;
            this.rdBtnDK.Text = "Danish";
            this.rdBtnDK.UseVisualStyleBackColor = true;
            this.rdBtnDK.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // rdBtnGCC
            // 
            this.rdBtnGCC.AutoSize = true;
            this.rdBtnGCC.Location = new System.Drawing.Point(20, 250);
            this.rdBtnGCC.Name = "rdBtnGCC";
            this.rdBtnGCC.Size = new System.Drawing.Size(58, 21);
            this.rdBtnGCC.TabIndex = 3;
            this.rdBtnGCC.TabStop = true;
            this.rdBtnGCC.Text = "GCC";
            this.rdBtnGCC.UseVisualStyleBackColor = true;
            this.rdBtnGCC.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 425);
            this.Controls.Add(this.grpHolidays);
            this.Controls.Add(this.grpDB);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Setup";
            this.Text = "Setup";
            this.grpDB.ResumeLayout(false);
            this.grpDB.PerformLayout();
            this.grpHolidays.ResumeLayout(false);
            this.grpHolidays.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpDB;
        private System.Windows.Forms.RadioButton rdBtnRoyston;
        private System.Windows.Forms.RadioButton rdBtnNaerum;
        private System.Windows.Forms.GroupBox grpHolidays;
        private System.Windows.Forms.RadioButton rdBtnUK;
        private System.Windows.Forms.RadioButton rdBtnDK;
        private System.Windows.Forms.RadioButton rdBtnUS;
        private System.Windows.Forms.RadioButton rdBtnGottingen;
        private System.Windows.Forms.RadioButton rdBtnDE;
        private System.Windows.Forms.RadioButton rdBtnGCC;
    }
}