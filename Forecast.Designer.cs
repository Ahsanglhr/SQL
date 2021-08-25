namespace TimeReg
{
    partial class Forecast
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.grpFilters = new System.Windows.Forms.GroupBox();
            this.rdBtnEmployeesFirst = new System.Windows.Forms.RadioButton();
            this.rdBtnProjectsFirst = new System.Windows.Forms.RadioButton();
            this.lblProjects = new System.Windows.Forms.Label();
            this.cmbProjects = new System.Windows.Forms.ListBox();
            this.txtLastYear = new System.Windows.Forms.TextBox();
            this.lblLastYear = new System.Windows.Forms.Label();
            this.txtFirstYear = new System.Windows.Forms.TextBox();
            this.lblFirstYear = new System.Windows.Forms.Label();
            this.grid = new System.Windows.Forms.DataGridView();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.cntxtMnuDGV = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyCTRLCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteCTRLVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gridTotal = new System.Windows.Forms.DataGridView();
            this.grpFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.cntxtMnuDGV.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTotal)).BeginInit();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUpdate.Location = new System.Drawing.Point(1016, 65);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(133, 28);
            this.btnUpdate.TabIndex = 0;
            this.btnUpdate.Text = "Refresh Grid";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // grpFilters
            // 
            this.grpFilters.Controls.Add(this.rdBtnEmployeesFirst);
            this.grpFilters.Controls.Add(this.rdBtnProjectsFirst);
            this.grpFilters.Controls.Add(this.lblProjects);
            this.grpFilters.Controls.Add(this.cmbProjects);
            this.grpFilters.Controls.Add(this.txtLastYear);
            this.grpFilters.Controls.Add(this.lblLastYear);
            this.grpFilters.Controls.Add(this.txtFirstYear);
            this.grpFilters.Controls.Add(this.btnUpdate);
            this.grpFilters.Controls.Add(this.lblFirstYear);
            this.grpFilters.Location = new System.Drawing.Point(36, 31);
            this.grpFilters.Margin = new System.Windows.Forms.Padding(4);
            this.grpFilters.Name = "grpFilters";
            this.grpFilters.Padding = new System.Windows.Forms.Padding(4);
            this.grpFilters.Size = new System.Drawing.Size(1180, 126);
            this.grpFilters.TabIndex = 1;
            this.grpFilters.TabStop = false;
            this.grpFilters.Text = "Control";
            // 
            // rdBtnEmployeesFirst
            // 
            this.rdBtnEmployeesFirst.AutoSize = true;
            this.rdBtnEmployeesFirst.Location = new System.Drawing.Point(683, 68);
            this.rdBtnEmployeesFirst.Margin = new System.Windows.Forms.Padding(4);
            this.rdBtnEmployeesFirst.Name = "rdBtnEmployeesFirst";
            this.rdBtnEmployeesFirst.Size = new System.Drawing.Size(148, 21);
            this.rdBtnEmployeesFirst.TabIndex = 7;
            this.rdBtnEmployeesFirst.TabStop = true;
            this.rdBtnEmployeesFirst.Text = "Sort By Employees";
            this.rdBtnEmployeesFirst.UseVisualStyleBackColor = true;
            // 
            // rdBtnProjectsFirst
            // 
            this.rdBtnProjectsFirst.AutoSize = true;
            this.rdBtnProjectsFirst.Location = new System.Drawing.Point(683, 25);
            this.rdBtnProjectsFirst.Margin = new System.Windows.Forms.Padding(4);
            this.rdBtnProjectsFirst.Name = "rdBtnProjectsFirst";
            this.rdBtnProjectsFirst.Size = new System.Drawing.Size(130, 21);
            this.rdBtnProjectsFirst.TabIndex = 6;
            this.rdBtnProjectsFirst.TabStop = true;
            this.rdBtnProjectsFirst.Text = "Sort By Projects";
            this.rdBtnProjectsFirst.UseVisualStyleBackColor = true;
            // 
            // lblProjects
            // 
            this.lblProjects.AutoSize = true;
            this.lblProjects.Location = new System.Drawing.Point(285, 26);
            this.lblProjects.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProjects.Name = "lblProjects";
            this.lblProjects.Size = new System.Drawing.Size(63, 17);
            this.lblProjects.TabIndex = 5;
            this.lblProjects.Text = "Projects:";
            // 
            // cmbProjects
            // 
            this.cmbProjects.FormattingEnabled = true;
            this.cmbProjects.ItemHeight = 16;
            this.cmbProjects.Location = new System.Drawing.Point(361, 21);
            this.cmbProjects.Margin = new System.Windows.Forms.Padding(4);
            this.cmbProjects.Name = "cmbProjects";
            this.cmbProjects.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.cmbProjects.Size = new System.Drawing.Size(248, 84);
            this.cmbProjects.TabIndex = 4;
            // 
            // txtLastYear
            // 
            this.txtLastYear.Location = new System.Drawing.Point(113, 68);
            this.txtLastYear.Margin = new System.Windows.Forms.Padding(4);
            this.txtLastYear.Name = "txtLastYear";
            this.txtLastYear.Size = new System.Drawing.Size(83, 22);
            this.txtLastYear.TabIndex = 3;
            // 
            // lblLastYear
            // 
            this.lblLastYear.AutoSize = true;
            this.lblLastYear.Location = new System.Drawing.Point(31, 71);
            this.lblLastYear.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLastYear.Name = "lblLastYear";
            this.lblLastYear.Size = new System.Drawing.Size(73, 17);
            this.lblLastYear.TabIndex = 2;
            this.lblLastYear.Text = "Last Year:";
            // 
            // txtFirstYear
            // 
            this.txtFirstYear.Location = new System.Drawing.Point(113, 21);
            this.txtFirstYear.Margin = new System.Windows.Forms.Padding(4);
            this.txtFirstYear.Name = "txtFirstYear";
            this.txtFirstYear.Size = new System.Drawing.Size(83, 22);
            this.txtFirstYear.TabIndex = 1;
            // 
            // lblFirstYear
            // 
            this.lblFirstYear.AutoSize = true;
            this.lblFirstYear.Location = new System.Drawing.Point(27, 25);
            this.lblFirstYear.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFirstYear.Name = "lblFirstYear";
            this.lblFirstYear.Size = new System.Drawing.Size(73, 17);
            this.lblFirstYear.TabIndex = 0;
            this.lblFirstYear.Text = "First Year:";
            // 
            // grid
            // 
            this.grid.AllowUserToResizeRows = false;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Location = new System.Drawing.Point(36, 265);
            this.grid.Margin = new System.Windows.Forms.Padding(4);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(1181, 320);
            this.grid.TabIndex = 2;
            this.grid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellClick);
            this.grid.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellEnter);
            this.grid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellValueChanged);
            this.grid.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.grid_ColumnWidthChanged);
            this.grid.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.grid_DefaultValuesNeeded);
            this.grid.Scroll += new System.Windows.Forms.ScrollEventHandler(this.grid_Scroll);
            this.grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_KeyDown);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubmit.Location = new System.Drawing.Point(1116, 617);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(4);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(100, 28);
            this.btnSubmit.TabIndex = 3;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAs.Location = new System.Drawing.Point(863, 617);
            this.btnSaveAs.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(100, 28);
            this.btnSaveAs.TabIndex = 3;
            this.btnSaveAs.Text = "Save As...";
            this.btnSaveAs.UseVisualStyleBackColor = true;
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // cntxtMnuDGV
            // 
            this.cntxtMnuDGV.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cntxtMnuDGV.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyCTRLCToolStripMenuItem,
            this.pasteCTRLVToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.cntxtMnuDGV.Name = "cntxtMnuDGV";
            this.cntxtMnuDGV.Size = new System.Drawing.Size(212, 82);
            this.cntxtMnuDGV.Opening += new System.ComponentModel.CancelEventHandler(this.cntxtMnuDGV_Opening);
            // 
            // copyCTRLCToolStripMenuItem
            // 
            this.copyCTRLCToolStripMenuItem.Name = "copyCTRLCToolStripMenuItem";
            this.copyCTRLCToolStripMenuItem.Size = new System.Drawing.Size(211, 26);
            this.copyCTRLCToolStripMenuItem.Text = "Copy Selected";
            // 
            // pasteCTRLVToolStripMenuItem
            // 
            this.pasteCTRLVToolStripMenuItem.Name = "pasteCTRLVToolStripMenuItem";
            this.pasteCTRLVToolStripMenuItem.Size = new System.Drawing.Size(211, 26);
            this.pasteCTRLVToolStripMenuItem.Text = "Paste Selected";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(211, 26);
            this.deleteToolStripMenuItem.Text = "Delete Current Line";
            this.deleteToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.deleteToolStripMenuItem_Paint);
            // 
            // gridTotal
            // 
            this.gridTotal.AllowUserToAddRows = false;
            this.gridTotal.AllowUserToDeleteRows = false;
            this.gridTotal.AllowUserToResizeColumns = false;
            this.gridTotal.AllowUserToResizeRows = false;
            this.gridTotal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTotal.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridTotal.Enabled = false;
            this.gridTotal.Location = new System.Drawing.Point(36, 185);
            this.gridTotal.Margin = new System.Windows.Forms.Padding(4);
            this.gridTotal.MultiSelect = false;
            this.gridTotal.Name = "gridTotal";
            this.gridTotal.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlDark;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridTotal.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridTotal.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridTotal.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.gridTotal.Size = new System.Drawing.Size(1180, 63);
            this.gridTotal.TabIndex = 4;
            // 
            // Forecast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1295, 660);
            this.Controls.Add(this.gridTotal);
            this.Controls.Add(this.btnSaveAs);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.grpFilters);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Forecast";
            this.Text = "Forecast";
            this.Load += new System.EventHandler(this.Forecast_Load);
            this.ResizeEnd += new System.EventHandler(this.Forecast_ResizeEnd);
            this.grpFilters.ResumeLayout(false);
            this.grpFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.cntxtMnuDGV.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTotal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.GroupBox grpFilters;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.TextBox txtLastYear;
        private System.Windows.Forms.Label lblLastYear;
        private System.Windows.Forms.TextBox txtFirstYear;
        private System.Windows.Forms.Label lblFirstYear;
        private System.Windows.Forms.Button btnSaveAs;
        private System.Windows.Forms.ContextMenuStrip cntxtMnuDGV;
        private System.Windows.Forms.ToolStripMenuItem copyCTRLCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteCTRLVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.RadioButton rdBtnEmployeesFirst;
        private System.Windows.Forms.RadioButton rdBtnProjectsFirst;
        private System.Windows.Forms.Label lblProjects;
        private System.Windows.Forms.ListBox cmbProjects;
        private System.Windows.Forms.DataGridView gridTotal;
    }
}