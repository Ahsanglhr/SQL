namespace TimeReg
{
    partial class UserView
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
            this.grid = new System.Windows.Forms.DataGridView();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnPrevWeek = new System.Windows.Forms.Button();
            this.btnNextWeek = new System.Windows.Forms.Button();
            this.cellDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnReset = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importUsersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.myProjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allProjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forecastsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.basicSetupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bulkEditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.aliasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tlstrpCmbUsers = new System.Windows.Forms.ToolStripComboBox();
            this.useDebugDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForEmptyDaysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpInConfluenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.basicInputFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectConceptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jirabugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jiraEnhtoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compiledToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblWeekNo = new System.Windows.Forms.Label();
            this.btnHome = new System.Windows.Forms.Button();
            this.emptyContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellDataBindingSource)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Location = new System.Drawing.Point(107, 105);
            this.grid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grid.MultiSelect = false;
            this.grid.Name = "grid";
            this.grid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.grid.RowTemplate.Height = 24;
            this.grid.Size = new System.Drawing.Size(1020, 400);
            this.grid.TabIndex = 0;
            this.grid.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grid_CellMouseClick);
            this.grid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellValueChanged);
            this.grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.grid_EditingControlShowing);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubmit.Location = new System.Drawing.Point(1027, 511);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(101, 50);
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnPrevWeek
            // 
            this.btnPrevWeek.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnPrevWeek.Image = global::TimeReg.Properties.Resources._1_012;
            this.btnPrevWeek.Location = new System.Drawing.Point(13, 178);
            this.btnPrevWeek.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPrevWeek.Name = "btnPrevWeek";
            this.btnPrevWeek.Size = new System.Drawing.Size(75, 73);
            this.btnPrevWeek.TabIndex = 4;
            this.btnPrevWeek.UseVisualStyleBackColor = true;
            this.btnPrevWeek.Click += new System.EventHandler(this.btnPrevWeek_Click);
            // 
            // btnNextWeek
            // 
            this.btnNextWeek.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnNextWeek.Image = global::TimeReg.Properties.Resources._1_018;
            this.btnNextWeek.Location = new System.Drawing.Point(1151, 178);
            this.btnNextWeek.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnNextWeek.Name = "btnNextWeek";
            this.btnNextWeek.Size = new System.Drawing.Size(75, 73);
            this.btnNextWeek.TabIndex = 3;
            this.btnNextWeek.UseVisualStyleBackColor = true;
            this.btnNextWeek.Click += new System.EventHandler(this.btnNextWeek_Click);
            // 
            // cellDataBindingSource
            // 
            this.cellDataBindingSource.DataSource = typeof(TimeReg.DBUtil.CellData);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(880, 511);
            this.btnReset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(101, 50);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(21, 21);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1249, 28);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reportToolStripMenuItem,
            this.addUserToolStripMenuItem,
            this.importUsersToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // reportToolStripMenuItem
            // 
            this.reportToolStripMenuItem.Name = "reportToolStripMenuItem";
            this.reportToolStripMenuItem.ShortcutKeyDisplayString = "Alt+R";
            this.reportToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.R)));
            this.reportToolStripMenuItem.Size = new System.Drawing.Size(232, 26);
            this.reportToolStripMenuItem.Text = "Create Report...";
            this.reportToolStripMenuItem.Click += new System.EventHandler(this.reportToolStripMenuItem_Click);
            // 
            // addUserToolStripMenuItem
            // 
            this.addUserToolStripMenuItem.Name = "addUserToolStripMenuItem";
            this.addUserToolStripMenuItem.Size = new System.Drawing.Size(232, 26);
            this.addUserToolStripMenuItem.Text = "Add User ...";
            this.addUserToolStripMenuItem.Click += new System.EventHandler(this.addUserToolStripMenuItem_Click);
            this.addUserToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.addUserToolStripMenuItem_Paint);
            // 
            // importUsersToolStripMenuItem
            // 
            this.importUsersToolStripMenuItem.Name = "importUsersToolStripMenuItem";
            this.importUsersToolStripMenuItem.Size = new System.Drawing.Size(232, 26);
            this.importUsersToolStripMenuItem.Text = "Import Users ...";
            this.importUsersToolStripMenuItem.Click += new System.EventHandler(this.importUsersToolStripMenuItem_Click);
            this.importUsersToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.importUsersToolStripMenuItem_Paint);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.myProjectsToolStripMenuItem,
            this.allProjectsToolStripMenuItem,
            this.forecastsToolStripMenuItem,
            this.toolStripSeparator2,
            this.basicSetupToolStripMenuItem,
            this.bulkEditToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(47, 24);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // myProjectsToolStripMenuItem
            // 
            this.myProjectsToolStripMenuItem.Name = "myProjectsToolStripMenuItem";
            this.myProjectsToolStripMenuItem.ShortcutKeyDisplayString = "Alt+M";
            this.myProjectsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.M)));
            this.myProjectsToolStripMenuItem.Size = new System.Drawing.Size(266, 26);
            this.myProjectsToolStripMenuItem.Text = "My Projects...";
            this.myProjectsToolStripMenuItem.Click += new System.EventHandler(this.myProjectsToolStripMenuItem_Click);
            this.myProjectsToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.myProjectsToolStripMenuItem_Paint);
            // 
            // allProjectsToolStripMenuItem
            // 
            this.allProjectsToolStripMenuItem.Name = "allProjectsToolStripMenuItem";
            this.allProjectsToolStripMenuItem.Size = new System.Drawing.Size(266, 26);
            this.allProjectsToolStripMenuItem.Text = "Projects (SuperUsers Only)...";
            this.allProjectsToolStripMenuItem.Click += new System.EventHandler(this.allProjectsToolStripMenuItem_Click);
            this.allProjectsToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.allProjectsToolStripMenuItem_Paint);
            // 
            // forecastsToolStripMenuItem
            // 
            this.forecastsToolStripMenuItem.Name = "forecastsToolStripMenuItem";
            this.forecastsToolStripMenuItem.Size = new System.Drawing.Size(266, 26);
            this.forecastsToolStripMenuItem.Text = "Forecasts...";
            this.forecastsToolStripMenuItem.Click += new System.EventHandler(this.forecastsToolStripMenuItem_Click);
            this.forecastsToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.forecastsToolStripMenuItem_Paint);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(263, 6);
            // 
            // basicSetupToolStripMenuItem
            // 
            this.basicSetupToolStripMenuItem.Name = "basicSetupToolStripMenuItem";
            this.basicSetupToolStripMenuItem.Size = new System.Drawing.Size(266, 26);
            this.basicSetupToolStripMenuItem.Text = "Basic Setup...";
            this.basicSetupToolStripMenuItem.Click += new System.EventHandler(this.basicSetupToolStripMenuItem_Click);
            this.basicSetupToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.basicSetupToolStripMenuItem_Paint);
            // 
            // bulkEditToolStripMenuItem
            // 
            this.bulkEditToolStripMenuItem.Name = "bulkEditToolStripMenuItem";
            this.bulkEditToolStripMenuItem.ShortcutKeyDisplayString = "Alt+V";
            this.bulkEditToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.V)));
            this.bulkEditToolStripMenuItem.Size = new System.Drawing.Size(266, 26);
            this.bulkEditToolStripMenuItem.Text = "Vacation/Leave ...";
            this.bulkEditToolStripMenuItem.Click += new System.EventHandler(this.bulkEditToolStripMenuItem_Click);
            this.bulkEditToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.bulkEditToolStripMenuItem_Paint);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.aliasToolStripMenuItem,
            this.useDebugDBToolStripMenuItem,
            this.checkForEmptyDaysToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(56, 24);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(281, 6);
            // 
            // aliasToolStripMenuItem
            // 
            this.aliasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlstrpCmbUsers});
            this.aliasToolStripMenuItem.Name = "aliasToolStripMenuItem";
            this.aliasToolStripMenuItem.Size = new System.Drawing.Size(284, 26);
            this.aliasToolStripMenuItem.Text = "Alias (SuperUsers Only) ...";
            this.aliasToolStripMenuItem.Click += new System.EventHandler(this.aliasToolStripMenuItem_Click);
            this.aliasToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.aliasToolStripMenuItem_Paint);
            // 
            // tlstrpCmbUsers
            // 
            this.tlstrpCmbUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tlstrpCmbUsers.Name = "tlstrpCmbUsers";
            this.tlstrpCmbUsers.Size = new System.Drawing.Size(250, 28);
            // 
            // useDebugDBToolStripMenuItem
            // 
            this.useDebugDBToolStripMenuItem.Name = "useDebugDBToolStripMenuItem";
            this.useDebugDBToolStripMenuItem.Size = new System.Drawing.Size(284, 26);
            this.useDebugDBToolStripMenuItem.Text = "Use Debug DB";
            this.useDebugDBToolStripMenuItem.Click += new System.EventHandler(this.useDebugDBToolStripMenuItem_Click);
            this.useDebugDBToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.useDebugDBToolStripMenuItem_Paint);
            // 
            // checkForEmptyDaysToolStripMenuItem
            // 
            this.checkForEmptyDaysToolStripMenuItem.Name = "checkForEmptyDaysToolStripMenuItem";
            this.checkForEmptyDaysToolStripMenuItem.ShortcutKeyDisplayString = "Alt+C";
            this.checkForEmptyDaysToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.C)));
            this.checkForEmptyDaysToolStripMenuItem.Size = new System.Drawing.Size(284, 26);
            this.checkForEmptyDaysToolStripMenuItem.Text = "Check for Empty Days...";
            this.checkForEmptyDaysToolStripMenuItem.Click += new System.EventHandler(this.checkForEmptyDaysToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpInConfluenceToolStripMenuItem,
            this.basicInputFormToolStripMenuItem,
            this.projectConceptToolStripMenuItem,
            this.reportsToolStripMenuItem,
            this.jirabugToolStripMenuItem,
            this.jiraEnhtoolStripMenuItem,
            this.compiledToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // helpInConfluenceToolStripMenuItem
            // 
            this.helpInConfluenceToolStripMenuItem.Name = "helpInConfluenceToolStripMenuItem";
            this.helpInConfluenceToolStripMenuItem.ShortcutKeyDisplayString = "Alt+H";
            this.helpInConfluenceToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.H)));
            this.helpInConfluenceToolStripMenuItem.Size = new System.Drawing.Size(268, 26);
            this.helpInConfluenceToolStripMenuItem.Text = "Help in Confluence...";
            this.helpInConfluenceToolStripMenuItem.Click += new System.EventHandler(this.helpInConfluenceToolStripMenuItem_Click);
            // 
            // basicInputFormToolStripMenuItem
            // 
            this.basicInputFormToolStripMenuItem.Name = "basicInputFormToolStripMenuItem";
            this.basicInputFormToolStripMenuItem.Size = new System.Drawing.Size(268, 26);
            this.basicInputFormToolStripMenuItem.Text = "Basic Input Form...";
            this.basicInputFormToolStripMenuItem.Click += new System.EventHandler(this.basicInputFormToolStripMenuItem_Click);
            // 
            // projectConceptToolStripMenuItem
            // 
            this.projectConceptToolStripMenuItem.Name = "projectConceptToolStripMenuItem";
            this.projectConceptToolStripMenuItem.Size = new System.Drawing.Size(268, 26);
            this.projectConceptToolStripMenuItem.Text = "Project Concept...";
            this.projectConceptToolStripMenuItem.Click += new System.EventHandler(this.projectConceptToolStripMenuItem_Click);
            // 
            // reportsToolStripMenuItem
            // 
            this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            this.reportsToolStripMenuItem.Size = new System.Drawing.Size(268, 26);
            this.reportsToolStripMenuItem.Text = "Reports...";
            this.reportsToolStripMenuItem.Click += new System.EventHandler(this.reportsToolStripMenuItem_Click);
            // 
            // jirabugToolStripMenuItem
            // 
            this.jirabugToolStripMenuItem.Name = "jirabugToolStripMenuItem";
            this.jirabugToolStripMenuItem.Size = new System.Drawing.Size(268, 26);
            this.jirabugToolStripMenuItem.Text = "Make Jira Bug...";
            this.jirabugToolStripMenuItem.Click += new System.EventHandler(this.jiraToolStripMenuItem_Click);
            // 
            // jiraEnhtoolStripMenuItem
            // 
            this.jiraEnhtoolStripMenuItem.Name = "jiraEnhtoolStripMenuItem";
            this.jiraEnhtoolStripMenuItem.Size = new System.Drawing.Size(268, 26);
            this.jiraEnhtoolStripMenuItem.Text = "Make Jira Enhancement...";
            this.jiraEnhtoolStripMenuItem.Click += new System.EventHandler(this.jiraEnhtoolStripMenuItem_Click);
            // 
            // compiledToolStripMenuItem
            // 
            this.compiledToolStripMenuItem.Name = "compiledToolStripMenuItem";
            this.compiledToolStripMenuItem.Size = new System.Drawing.Size(268, 26);
            this.compiledToolStripMenuItem.Text = "Compiled:";
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.Location = new System.Drawing.Point(107, 522);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(44, 25);
            this.lblInfo.TabIndex = 7;
            this.lblInfo.Text = "Info";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWeekNo
            // 
            this.lblWeekNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWeekNo.AutoSize = true;
            this.lblWeekNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWeekNo.Location = new System.Drawing.Point(489, 62);
            this.lblWeekNo.Name = "lblWeekNo";
            this.lblWeekNo.Size = new System.Drawing.Size(95, 32);
            this.lblWeekNo.TabIndex = 8;
            this.lblWeekNo.Text = "Week:";
            // 
            // btnHome
            // 
            this.btnHome.Location = new System.Drawing.Point(396, 62);
            this.btnHome.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(77, 38);
            this.btnHome.TabIndex = 9;
            this.btnHome.Text = "Home";
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // emptyContextMenuStrip
            // 
            this.emptyContextMenuStrip.ImageScalingSize = new System.Drawing.Size(21, 21);
            this.emptyContextMenuStrip.Name = "emptyContextMenuStrip";
            this.emptyContextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // UserView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1249, 567);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.lblWeekNo);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnPrevWeek);
            this.Controls.Add(this.btnNextWeek);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UserView";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Time Registration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserView_FormClosing);
            this.Load += new System.EventHandler(this.UserView_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UserView_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.UserView_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellDataBindingSource)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource cellDataBindingSource;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnNextWeek;
        private System.Windows.Forms.Button btnPrevWeek;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem myProjectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allProjectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem aliasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useDebugDBToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem importUsersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem basicInputFormToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem projectConceptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox tlstrpCmbUsers;
        private System.Windows.Forms.ToolStripMenuItem jirabugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compiledToolStripMenuItem;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblWeekNo;
        private System.Windows.Forms.ToolStripMenuItem jiraEnhtoolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem basicSetupToolStripMenuItem;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.ToolStripMenuItem addUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bulkEditToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpInConfluenceToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip emptyContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem checkForEmptyDaysToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forecastsToolStripMenuItem;
    }
}

