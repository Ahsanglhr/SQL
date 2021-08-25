namespace TimeReg
{
    partial class AllProjects
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
            this.popupMenuAllProjs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToplvlMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addChildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeProjectNoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePSONoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeCategoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeGrouptagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChangePMToolstripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.assignMeAsManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeForEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblAllProjs = new System.Windows.Forms.Label();
            this.lblWarn = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.grid = new System.Windows.Forms.DataGridView();
            this.btnResetSort = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.chkClosed = new System.Windows.Forms.CheckBox();
            this.lblName = new System.Windows.Forms.Label();
            this.popupMenuAllProjs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // popupMenuAllProjs
            // 
            this.popupMenuAllProjs.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.popupMenuAllProjs.ImageScalingSize = new System.Drawing.Size(21, 21);
            this.popupMenuAllProjs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToplvlMenuItem,
            this.addChildToolStripMenuItem,
            this.toolStripSeparator1,
            this.renameToolStripMenuItem,
            this.changeProjectNoToolStripMenuItem,
            this.changePSONoToolStripMenuItem,
            this.changeCategoryToolStripMenuItem,
            this.changeGrouptagToolStripMenuItem,
            this.ChangePMToolstripMenuItem,
            this.toolStripSeparator2,
            this.assignMeAsManagerToolStripMenuItem,
            this.closeForEntryToolStripMenuItem});
            this.popupMenuAllProjs.Name = "popupMenuAllProjs";
            this.popupMenuAllProjs.Size = new System.Drawing.Size(251, 284);
            // 
            // addToplvlMenuItem
            // 
            this.addToplvlMenuItem.Name = "addToplvlMenuItem";
            this.addToplvlMenuItem.Size = new System.Drawing.Size(250, 24);
            this.addToplvlMenuItem.Text = "New Top level Project...";
            this.addToplvlMenuItem.Click += new System.EventHandler(this.addToplvlMenuItem_Click);
            this.addToplvlMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.addToplvlMenuItem_Paint);
            // 
            // addChildToolStripMenuItem
            // 
            this.addChildToolStripMenuItem.Name = "addChildToolStripMenuItem";
            this.addChildToolStripMenuItem.Size = new System.Drawing.Size(250, 24);
            this.addChildToolStripMenuItem.Text = "Add Child to this Project...";
            this.addChildToolStripMenuItem.Click += new System.EventHandler(this.addChildToolStripMenuItem_Click);
            this.addChildToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.addChildToolStripMenuItem_Paint);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(247, 6);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(250, 24);
            this.renameToolStripMenuItem.Text = "Change Name ...";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            this.renameToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.renameToolStripMenuItem_Paint);
            // 
            // changeProjectNoToolStripMenuItem
            // 
            this.changeProjectNoToolStripMenuItem.Name = "changeProjectNoToolStripMenuItem";
            this.changeProjectNoToolStripMenuItem.Size = new System.Drawing.Size(250, 24);
            this.changeProjectNoToolStripMenuItem.Text = "Change Project No...";
            this.changeProjectNoToolStripMenuItem.Click += new System.EventHandler(this.changeProjectNoToolStripMenuItem_Click);
            this.changeProjectNoToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.changeProjectNoToolStripMenuItem_Paint);
            // 
            // changePSONoToolStripMenuItem
            // 
            this.changePSONoToolStripMenuItem.Name = "changePSONoToolStripMenuItem";
            this.changePSONoToolStripMenuItem.Size = new System.Drawing.Size(250, 24);
            this.changePSONoToolStripMenuItem.Text = "Change PSO No...";
            this.changePSONoToolStripMenuItem.Click += new System.EventHandler(this.changePSONoToolStripMenuItem_Click);
            this.changePSONoToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.changePSONoToolStripMenuItem_Paint);
            // 
            // changeCategoryToolStripMenuItem
            // 
            this.changeCategoryToolStripMenuItem.Name = "changeCategoryToolStripMenuItem";
            this.changeCategoryToolStripMenuItem.Size = new System.Drawing.Size(250, 24);
            this.changeCategoryToolStripMenuItem.Text = "Change Category ...";
            this.changeCategoryToolStripMenuItem.Click += new System.EventHandler(this.changeCategoryToolStripMenuItem_Click);
            this.changeCategoryToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.changeCategoryToolStripMenuItem_Paint);
            // 
            // changeGrouptagToolStripMenuItem
            // 
            this.changeGrouptagToolStripMenuItem.Name = "changeGrouptagToolStripMenuItem";
            this.changeGrouptagToolStripMenuItem.Size = new System.Drawing.Size(250, 24);
            this.changeGrouptagToolStripMenuItem.Text = "Change Grouptag...";
            this.changeGrouptagToolStripMenuItem.Click += new System.EventHandler(this.changeGrouptagToolStripMenuItem_Click);
            this.changeGrouptagToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.changeGrouptagToolStripMenuItem_Paint);
            // 
            // ChangePMToolstripMenuItem
            // 
            this.ChangePMToolstripMenuItem.Name = "ChangePMToolstripMenuItem";
            this.ChangePMToolstripMenuItem.Size = new System.Drawing.Size(250, 24);
            this.ChangePMToolstripMenuItem.Text = "Change Project Manager...";
            this.ChangePMToolstripMenuItem.Click += new System.EventHandler(this.ChangePMToolstripMenuItem_Click);
            this.ChangePMToolstripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.ChangePMToolstripMenuItem_Paint);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(247, 6);
            // 
            // assignMeAsManagerToolStripMenuItem
            // 
            this.assignMeAsManagerToolStripMenuItem.Name = "assignMeAsManagerToolStripMenuItem";
            this.assignMeAsManagerToolStripMenuItem.Size = new System.Drawing.Size(250, 24);
            this.assignMeAsManagerToolStripMenuItem.Text = "Assign me as Manager...";
            this.assignMeAsManagerToolStripMenuItem.Click += new System.EventHandler(this.assignMeAsManagerToolStripMenuItem_Click);
            this.assignMeAsManagerToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.assignMeAsManagerToolStripMenuItem_Paint);
            // 
            // closeForEntryToolStripMenuItem
            // 
            this.closeForEntryToolStripMenuItem.Name = "closeForEntryToolStripMenuItem";
            this.closeForEntryToolStripMenuItem.Size = new System.Drawing.Size(250, 24);
            this.closeForEntryToolStripMenuItem.Text = "Close for entry...";
            this.closeForEntryToolStripMenuItem.Click += new System.EventHandler(this.closeForEntryToolStripMenuItem_Click);
            this.closeForEntryToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.closeForEntryToolStripMenuItem_Paint);
            // 
            // lblAllProjs
            // 
            this.lblAllProjs.AutoSize = true;
            this.lblAllProjs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAllProjs.Location = new System.Drawing.Point(57, 20);
            this.lblAllProjs.Name = "lblAllProjs";
            this.lblAllProjs.Size = new System.Drawing.Size(109, 25);
            this.lblAllProjs.TabIndex = 3;
            this.lblAllProjs.Text = "All Projects";
            // 
            // lblWarn
            // 
            this.lblWarn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblWarn.AutoSize = true;
            this.lblWarn.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarn.Location = new System.Drawing.Point(331, 453);
            this.lblWarn.Name = "lblWarn";
            this.lblWarn.Size = new System.Drawing.Size(85, 17);
            this.lblWarn.TabIndex = 4;
            this.lblWarn.Text = "lblWarning";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(944, 413);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToOrderColumns = true;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.ContextMenuStrip = this.popupMenuAllProjs;
            this.grid.Location = new System.Drawing.Point(37, 59);
            this.grid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grid.MultiSelect = false;
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(881, 377);
            this.grid.TabIndex = 6;
            this.grid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellClick);
            this.grid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grid_CellMouseDown);
            this.grid.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grid_DataBindingComplete);
            this.grid.Sorted += new System.EventHandler(this.grid_Sorted);
            // 
            // btnResetSort
            // 
            this.btnResetSort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetSort.Location = new System.Drawing.Point(944, 59);
            this.btnResetSort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnResetSort.Name = "btnResetSort";
            this.btnResetSort.Size = new System.Drawing.Size(75, 52);
            this.btnResetSort.TabIndex = 7;
            this.btnResetSort.Text = "Reset Sort";
            this.btnResetSort.UseVisualStyleBackColor = true;
            this.btnResetSort.Click += new System.EventHandler(this.btnResetSort_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(457, 23);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(261, 22);
            this.txtSearch.TabIndex = 8;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // chkClosed
            // 
            this.chkClosed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkClosed.AutoSize = true;
            this.chkClosed.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkClosed.Location = new System.Drawing.Point(941, 169);
            this.chkClosed.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkClosed.Name = "chkClosed";
            this.chkClosed.Size = new System.Drawing.Size(73, 21);
            this.chkClosed.TabIndex = 9;
            this.chkClosed.Text = "Closed";
            this.chkClosed.UseVisualStyleBackColor = true;
            this.chkClosed.CheckedChanged += new System.EventHandler(this.chkClosed_CheckedChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(399, 28);
            this.lblName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(43, 17);
            this.lblName.TabIndex = 10;
            this.lblName.Text = "Filter:";
            // 
            // AllProjects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(1040, 517);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.chkClosed);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnResetSort);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lblWarn);
            this.Controls.Add(this.lblAllProjs);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "AllProjects";
            this.Text = "AllProjects";
            this.Load += new System.EventHandler(this.AllProjects_Load);
            this.popupMenuAllProjs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAllProjs;
        private System.Windows.Forms.Label lblWarn;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip popupMenuAllProjs;
        private System.Windows.Forms.ToolStripMenuItem addChildToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeProjectNoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeForEntryToolStripMenuItem;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.Button btnResetSort;
        private System.Windows.Forms.ToolStripMenuItem changePSONoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToplvlMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ToolStripMenuItem changeCategoryToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkClosed;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ToolStripMenuItem assignMeAsManagerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeGrouptagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ChangePMToolstripMenuItem;
    }
}