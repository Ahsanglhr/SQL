namespace TimeReg
{
    partial class ReportsDlg
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
            this.components = new System.ComponentModel.Container();
            this.saveReportDlg = new System.Windows.Forms.SaveFileDialog();
            this.grpQuery = new System.Windows.Forms.GroupBox();
            this.rdBtnDaysAccounted = new System.Windows.Forms.RadioButton();
            this.lstUsers = new System.Windows.Forms.ComboBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.lstMyOnly = new System.Windows.Forms.ComboBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.rdBtnTasksForMe = new System.Windows.Forms.RadioButton();
            this.rdBtnTotPerProj = new System.Windows.Forms.RadioButton();
            this.rdBtnTotPerParentProjs = new System.Windows.Forms.RadioButton();
            this.rdBtnTotalPerProjPerEmp = new System.Windows.Forms.RadioButton();
            this.btnFileSave = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.grpRange = new System.Windows.Forms.GroupBox();
            this.cmbCategories = new System.Windows.Forms.ComboBox();
            this.lblCategories = new System.Windows.Forms.Label();
            this.lblDepts = new System.Windows.Forms.Label();
            this.chkUseDepts = new System.Windows.Forms.CheckBox();
            this.lstDepts = new System.Windows.Forms.ListBox();
            this.lblLastDay = new System.Windows.Forms.Label();
            this.lblFirstDay = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.grid = new System.Windows.Forms.DataGridView();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.txtSum = new System.Windows.Forms.TextBox();
            this.lblSum = new System.Windows.Forms.Label();
            this.grpFile = new System.Windows.Forms.GroupBox();
            this.chkMonths = new System.Windows.Forms.CheckBox();
            this.grpQuery.SuspendLayout();
            this.grpRange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.grpFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpQuery
            // 
            this.grpQuery.Controls.Add(this.rdBtnDaysAccounted);
            this.grpQuery.Controls.Add(this.lstUsers);
            this.grpQuery.Controls.Add(this.lblUser);
            this.grpQuery.Controls.Add(this.lstMyOnly);
            this.grpQuery.Controls.Add(this.lblFilter);
            this.grpQuery.Controls.Add(this.rdBtnTasksForMe);
            this.grpQuery.Controls.Add(this.rdBtnTotPerProj);
            this.grpQuery.Controls.Add(this.rdBtnTotPerParentProjs);
            this.grpQuery.Controls.Add(this.rdBtnTotalPerProjPerEmp);
            this.grpQuery.Location = new System.Drawing.Point(280, 37);
            this.grpQuery.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpQuery.Name = "grpQuery";
            this.grpQuery.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpQuery.Size = new System.Drawing.Size(355, 359);
            this.grpQuery.TabIndex = 0;
            this.grpQuery.TabStop = false;
            this.grpQuery.Text = "Report Type";
            // 
            // rdBtnDaysAccounted
            // 
            this.rdBtnDaysAccounted.AutoSize = true;
            this.rdBtnDaysAccounted.Location = new System.Drawing.Point(29, 286);
            this.rdBtnDaysAccounted.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdBtnDaysAccounted.Name = "rdBtnDaysAccounted";
            this.rdBtnDaysAccounted.Size = new System.Drawing.Size(191, 21);
            this.rdBtnDaysAccounted.TabIndex = 4;
            this.rdBtnDaysAccounted.TabStop = true;
            this.rdBtnDaysAccounted.Text = "Weekdays Accounted For";
            this.rdBtnDaysAccounted.UseVisualStyleBackColor = true;
            this.rdBtnDaysAccounted.CheckedChanged += new System.EventHandler(this.rdBtnDaysAccounted_CheckedChanged);
            // 
            // lstUsers
            // 
            this.lstUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstUsers.FormattingEnabled = true;
            this.lstUsers.Location = new System.Drawing.Point(93, 208);
            this.lstUsers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.Size = new System.Drawing.Size(200, 24);
            this.lstUsers.TabIndex = 13;
            this.lstUsers.SelectedIndexChanged += new System.EventHandler(this.lstUsers_SelectedIndexChanged);
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(48, 212);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(42, 17);
            this.lblUser.TabIndex = 12;
            this.lblUser.Text = "User:";
            // 
            // lstMyOnly
            // 
            this.lstMyOnly.FormattingEnabled = true;
            this.lstMyOnly.ItemHeight = 16;
            this.lstMyOnly.Location = new System.Drawing.Point(93, 238);
            this.lstMyOnly.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lstMyOnly.Name = "lstMyOnly";
            this.lstMyOnly.Size = new System.Drawing.Size(200, 24);
            this.lstMyOnly.TabIndex = 5;
            this.lstMyOnly.SelectedIndexChanged += new System.EventHandler(this.lstMyOnly_SelectedIndexChanged);
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(48, 242);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(45, 17);
            this.lblFilter.TabIndex = 4;
            this.lblFilter.Text = "Only: ";
            // 
            // rdBtnTasksForMe
            // 
            this.rdBtnTasksForMe.AutoSize = true;
            this.rdBtnTasksForMe.Location = new System.Drawing.Point(29, 185);
            this.rdBtnTasksForMe.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdBtnTasksForMe.Name = "rdBtnTasksForMe";
            this.rdBtnTasksForMe.Size = new System.Drawing.Size(268, 21);
            this.rdBtnTasksForMe.TabIndex = 3;
            this.rdBtnTasksForMe.TabStop = true;
            this.rdBtnTasksForMe.Text = "User Entries with Comments - By Date";
            this.rdBtnTasksForMe.UseVisualStyleBackColor = true;
            this.rdBtnTasksForMe.CheckedChanged += new System.EventHandler(this.rdBtnTasksForMe_CheckedChanged);
            // 
            // rdBtnTotPerProj
            // 
            this.rdBtnTotPerProj.AutoSize = true;
            this.rdBtnTotPerProj.Location = new System.Drawing.Point(29, 84);
            this.rdBtnTotPerProj.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdBtnTotPerProj.Name = "rdBtnTotPerProj";
            this.rdBtnTotPerProj.Size = new System.Drawing.Size(226, 21);
            this.rdBtnTotPerProj.TabIndex = 1;
            this.rdBtnTotPerProj.TabStop = true;
            this.rdBtnTotPerProj.Text = "Totals for Parents and Children";
            this.rdBtnTotPerProj.UseVisualStyleBackColor = true;
            this.rdBtnTotPerProj.CheckedChanged += new System.EventHandler(this.rdBtnTotPerProj_CheckedChanged);
            // 
            // rdBtnTotPerParentProjs
            // 
            this.rdBtnTotPerParentProjs.AutoSize = true;
            this.rdBtnTotPerParentProjs.Location = new System.Drawing.Point(29, 134);
            this.rdBtnTotPerParentProjs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdBtnTotPerParentProjs.Name = "rdBtnTotPerParentProjs";
            this.rdBtnTotPerParentProjs.Size = new System.Drawing.Size(226, 21);
            this.rdBtnTotPerParentProjs.TabIndex = 2;
            this.rdBtnTotPerParentProjs.TabStop = true;
            this.rdBtnTotPerParentProjs.Text = "Totals Accumulated on Parents";
            this.rdBtnTotPerParentProjs.UseVisualStyleBackColor = true;
            this.rdBtnTotPerParentProjs.CheckedChanged += new System.EventHandler(this.rdBtnTotPerParentProjs_CheckedChanged);
            // 
            // rdBtnTotalPerProjPerEmp
            // 
            this.rdBtnTotalPerProjPerEmp.AutoSize = true;
            this.rdBtnTotalPerProjPerEmp.Location = new System.Drawing.Point(29, 33);
            this.rdBtnTotalPerProjPerEmp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdBtnTotalPerProjPerEmp.Name = "rdBtnTotalPerProjPerEmp";
            this.rdBtnTotalPerProjPerEmp.Size = new System.Drawing.Size(201, 21);
            this.rdBtnTotalPerProjPerEmp.TabIndex = 0;
            this.rdBtnTotalPerProjPerEmp.TabStop = true;
            this.rdBtnTotalPerProjPerEmp.Text = "All Projects - Per developer";
            this.rdBtnTotalPerProjPerEmp.UseVisualStyleBackColor = true;
            this.rdBtnTotalPerProjPerEmp.CheckedChanged += new System.EventHandler(this.rdBtnTotalPerProjPerEmp_CheckedChanged);
            // 
            // btnFileSave
            // 
            this.btnFileSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFileSave.Location = new System.Drawing.Point(11, 121);
            this.btnFileSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFileSave.Name = "btnFileSave";
            this.btnFileSave.Size = new System.Drawing.Size(100, 36);
            this.btnFileSave.TabIndex = 1;
            this.btnFileSave.Text = "Save As...";
            this.btnFileSave.UseVisualStyleBackColor = true;
            this.btnFileSave.Click += new System.EventHandler(this.btnFileSave_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(932, 737);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(99, 36);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // grpRange
            // 
            this.grpRange.Controls.Add(this.cmbCategories);
            this.grpRange.Controls.Add(this.lblCategories);
            this.grpRange.Controls.Add(this.lblDepts);
            this.grpRange.Controls.Add(this.chkUseDepts);
            this.grpRange.Controls.Add(this.lstDepts);
            this.grpRange.Controls.Add(this.lblLastDay);
            this.grpRange.Controls.Add(this.lblFirstDay);
            this.grpRange.Controls.Add(this.dtTo);
            this.grpRange.Controls.Add(this.dtFrom);
            this.grpRange.Location = new System.Drawing.Point(12, 37);
            this.grpRange.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpRange.Name = "grpRange";
            this.grpRange.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpRange.Size = new System.Drawing.Size(261, 359);
            this.grpRange.TabIndex = 8;
            this.grpRange.TabStop = false;
            this.grpRange.Text = "Filters";
            // 
            // cmbCategories
            // 
            this.cmbCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategories.FormattingEnabled = true;
            this.cmbCategories.Location = new System.Drawing.Point(97, 305);
            this.cmbCategories.Name = "cmbCategories";
            this.cmbCategories.Size = new System.Drawing.Size(141, 24);
            this.cmbCategories.TabIndex = 17;
            this.cmbCategories.SelectedIndexChanged += new System.EventHandler(this.cmbCategories_SelectedIndexChanged);
            // 
            // lblCategories
            // 
            this.lblCategories.AutoSize = true;
            this.lblCategories.Location = new System.Drawing.Point(22, 309);
            this.lblCategories.Name = "lblCategories";
            this.lblCategories.Size = new System.Drawing.Size(69, 17);
            this.lblCategories.TabIndex = 16;
            this.lblCategories.Text = "Category:";
            // 
            // lblDepts
            // 
            this.lblDepts.AutoSize = true;
            this.lblDepts.Location = new System.Drawing.Point(19, 231);
            this.lblDepts.MaximumSize = new System.Drawing.Size(100, 100);
            this.lblDepts.Name = "lblDepts";
            this.lblDepts.Size = new System.Drawing.Size(80, 51);
            this.lblDepts.TabIndex = 15;
            this.lblDepts.Text = "Filter by source department";
            // 
            // chkUseDepts
            // 
            this.chkUseDepts.AutoSize = true;
            this.chkUseDepts.Location = new System.Drawing.Point(19, 203);
            this.chkUseDepts.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkUseDepts.Name = "chkUseDepts";
            this.chkUseDepts.Size = new System.Drawing.Size(96, 21);
            this.chkUseDepts.TabIndex = 14;
            this.chkUseDepts.Text = "Use Depts";
            this.chkUseDepts.UseVisualStyleBackColor = true;
            this.chkUseDepts.CheckedChanged += new System.EventHandler(this.chkUseDepts_CheckedChanged);
            // 
            // lstDepts
            // 
            this.lstDepts.FormattingEnabled = true;
            this.lstDepts.ItemHeight = 16;
            this.lstDepts.Location = new System.Drawing.Point(121, 191);
            this.lstDepts.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lstDepts.Name = "lstDepts";
            this.lstDepts.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstDepts.Size = new System.Drawing.Size(116, 100);
            this.lstDepts.TabIndex = 13;
            this.lstDepts.SelectedIndexChanged += new System.EventHandler(this.lstDepts_SelectedIndexChanged);
            // 
            // lblLastDay
            // 
            this.lblLastDay.AutoSize = true;
            this.lblLastDay.Location = new System.Drawing.Point(19, 95);
            this.lblLastDay.Name = "lblLastDay";
            this.lblLastDay.Size = new System.Drawing.Size(68, 17);
            this.lblLastDay.TabIndex = 11;
            this.lblLastDay.Text = "Last Day:";
            // 
            // lblFirstDay
            // 
            this.lblFirstDay.AutoSize = true;
            this.lblFirstDay.Location = new System.Drawing.Point(19, 33);
            this.lblFirstDay.Name = "lblFirstDay";
            this.lblFirstDay.Size = new System.Drawing.Size(68, 17);
            this.lblFirstDay.TabIndex = 10;
            this.lblFirstDay.Text = "First Day:";
            // 
            // dtTo
            // 
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtTo.Location = new System.Drawing.Point(19, 126);
            this.dtTo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(218, 22);
            this.dtTo.TabIndex = 9;
            this.dtTo.CloseUp += new System.EventHandler(this.dtTo_CloseUp);
            // 
            // dtFrom
            // 
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtFrom.Location = new System.Drawing.Point(19, 57);
            this.dtFrom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(218, 22);
            this.dtFrom.TabIndex = 8;
            this.dtFrom.CloseUp += new System.EventHandler(this.dtFrom_CloseUp);
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToOrderColumns = true;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grid.Location = new System.Drawing.Point(12, 430);
            this.grid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(881, 343);
            this.grid.TabIndex = 9;
            this.grid.SelectionChanged += new System.EventHandler(this.grid_SelectionChanged);
            // 
            // txtSum
            // 
            this.txtSum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSum.Location = new System.Drawing.Point(952, 481);
            this.txtSum.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSum.Name = "txtSum";
            this.txtSum.ReadOnly = true;
            this.txtSum.Size = new System.Drawing.Size(77, 22);
            this.txtSum.TabIndex = 10;
            this.txtSum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblSum
            // 
            this.lblSum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSum.AutoSize = true;
            this.lblSum.Location = new System.Drawing.Point(961, 457);
            this.lblSum.Name = "lblSum";
            this.lblSum.Size = new System.Drawing.Size(67, 17);
            this.lblSum.TabIndex = 11;
            this.lblSum.Text = "Selected:";
            this.lblSum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grpFile
            // 
            this.grpFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFile.Controls.Add(this.chkMonths);
            this.grpFile.Controls.Add(this.btnFileSave);
            this.grpFile.Location = new System.Drawing.Point(911, 538);
            this.grpFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpFile.Name = "grpFile";
            this.grpFile.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpFile.Size = new System.Drawing.Size(120, 174);
            this.grpFile.TabIndex = 12;
            this.grpFile.TabStop = false;
            this.grpFile.Text = "Export";
            // 
            // chkMonths
            // 
            this.chkMonths.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkMonths.AutoSize = true;
            this.chkMonths.Location = new System.Drawing.Point(16, 63);
            this.chkMonths.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkMonths.Name = "chkMonths";
            this.chkMonths.Size = new System.Drawing.Size(96, 21);
            this.chkMonths.TabIndex = 2;
            this.chkMonths.Text = "By Months";
            this.chkMonths.UseVisualStyleBackColor = true;
            // 
            // ReportsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(1055, 791);
            this.Controls.Add(this.grpFile);
            this.Controls.Add(this.lblSum);
            this.Controls.Add(this.txtSum);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.grpRange);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.grpQuery);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ReportsDlg";
            this.Text = "Reports";
            this.Load += new System.EventHandler(this.ReportsDlg_Load);
            this.grpQuery.ResumeLayout(false);
            this.grpQuery.PerformLayout();
            this.grpRange.ResumeLayout(false);
            this.grpRange.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.grpFile.ResumeLayout(false);
            this.grpFile.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveReportDlg;
        private System.Windows.Forms.GroupBox grpQuery;
        private System.Windows.Forms.RadioButton rdBtnTasksForMe;
        private System.Windows.Forms.RadioButton rdBtnTotPerProj;
        private System.Windows.Forms.RadioButton rdBtnTotPerParentProjs;
        private System.Windows.Forms.RadioButton rdBtnTotalPerProjPerEmp;
        private System.Windows.Forms.Button btnFileSave;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.GroupBox grpRange;
        private System.Windows.Forms.Label lblLastDay;
        private System.Windows.Forms.Label lblFirstDay;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.ComboBox lstMyOnly;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox lstUsers;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.ToolTip toolTips;
        private System.Windows.Forms.ListBox lstDepts;
        private System.Windows.Forms.CheckBox chkUseDepts;
        private System.Windows.Forms.Label lblDepts;
        private System.Windows.Forms.TextBox txtSum;
        private System.Windows.Forms.Label lblSum;
        private System.Windows.Forms.GroupBox grpFile;
        private System.Windows.Forms.CheckBox chkMonths;
        private System.Windows.Forms.RadioButton rdBtnDaysAccounted;
        private System.Windows.Forms.ComboBox cmbCategories;
        private System.Windows.Forms.Label lblCategories;
    }
}