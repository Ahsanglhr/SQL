namespace TimeReg
{
    partial class MyProjects
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
            this.lstMyProjs = new System.Windows.Forms.ListBox();
            this.btnToMyProjs = new System.Windows.Forms.Button();
            this.btnFromMyProjs = new System.Windows.Forms.Button();
            this.lblHelp = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.rdAllProjects = new System.Windows.Forms.RadioButton();
            this.rdTxProjectsOnly = new System.Windows.Forms.RadioButton();
            this.rdNotTxProjects = new System.Windows.Forms.RadioButton();
            this.grpAllProjects = new System.Windows.Forms.GroupBox();
            this.grpMyProjects = new System.Windows.Forms.GroupBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblFilter = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.grpAllProjects.SuspendLayout();
            this.grpMyProjects.SuspendLayout();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid.Location = new System.Drawing.Point(23, 21);
            this.grid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(744, 527);
            this.grid.TabIndex = 0;
            this.grid.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grid_DataBindingComplete);
            // 
            // lstMyProjs
            // 
            this.lstMyProjs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstMyProjs.FormattingEnabled = true;
            this.lstMyProjs.ItemHeight = 16;
            this.lstMyProjs.Location = new System.Drawing.Point(24, 21);
            this.lstMyProjs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lstMyProjs.Name = "lstMyProjs";
            this.lstMyProjs.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstMyProjs.Size = new System.Drawing.Size(313, 516);
            this.lstMyProjs.TabIndex = 1;
            this.lstMyProjs.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lstMyProjs_MouseMove);
            // 
            // btnToMyProjs
            // 
            this.btnToMyProjs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToMyProjs.Image = global::TimeReg.Properties.Resources._1_018;
            this.btnToMyProjs.Location = new System.Drawing.Point(836, 134);
            this.btnToMyProjs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnToMyProjs.Name = "btnToMyProjs";
            this.btnToMyProjs.Size = new System.Drawing.Size(75, 62);
            this.btnToMyProjs.TabIndex = 2;
            this.btnToMyProjs.UseVisualStyleBackColor = true;
            this.btnToMyProjs.Click += new System.EventHandler(this.btnToMyProjs_Click);
            // 
            // btnFromMyProjs
            // 
            this.btnFromMyProjs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFromMyProjs.Image = global::TimeReg.Properties.Resources._1_012;
            this.btnFromMyProjs.Location = new System.Drawing.Point(836, 217);
            this.btnFromMyProjs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFromMyProjs.Name = "btnFromMyProjs";
            this.btnFromMyProjs.Size = new System.Drawing.Size(75, 62);
            this.btnFromMyProjs.TabIndex = 4;
            this.btnFromMyProjs.UseVisualStyleBackColor = true;
            this.btnFromMyProjs.Click += new System.EventHandler(this.btnFromMyProjs_Click);
            // 
            // lblHelp
            // 
            this.lblHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblHelp.AutoSize = true;
            this.lblHelp.Location = new System.Drawing.Point(24, 633);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Size = new System.Drawing.Size(71, 17);
            this.lblHelp.TabIndex = 7;
            this.lblHelp.Text = "label Help";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(1194, 643);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // rdAllProjects
            // 
            this.rdAllProjects.AutoSize = true;
            this.rdAllProjects.Location = new System.Drawing.Point(61, 548);
            this.rdAllProjects.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdAllProjects.Name = "rdAllProjects";
            this.rdAllProjects.Size = new System.Drawing.Size(44, 21);
            this.rdAllProjects.TabIndex = 9;
            this.rdAllProjects.TabStop = true;
            this.rdAllProjects.Text = "All";
            this.rdAllProjects.UseVisualStyleBackColor = true;
            this.rdAllProjects.CheckedChanged += new System.EventHandler(this.Projects_CheckedChanged);
            // 
            // rdTxProjectsOnly
            // 
            this.rdTxProjectsOnly.AutoSize = true;
            this.rdTxProjectsOnly.Location = new System.Drawing.Point(164, 548);
            this.rdTxProjectsOnly.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdTxProjectsOnly.Name = "rdTxProjectsOnly";
            this.rdTxProjectsOnly.Size = new System.Drawing.Size(102, 21);
            this.rdTxProjectsOnly.TabIndex = 10;
            this.rdTxProjectsOnly.TabStop = true;
            this.rdTxProjectsOnly.Text = "Transducer";
            this.rdTxProjectsOnly.UseVisualStyleBackColor = true;
            this.rdTxProjectsOnly.CheckedChanged += new System.EventHandler(this.Projects_CheckedChanged);
            // 
            // rdNotTxProjects
            // 
            this.rdNotTxProjects.AutoSize = true;
            this.rdNotTxProjects.Location = new System.Drawing.Point(320, 548);
            this.rdNotTxProjects.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdNotTxProjects.Name = "rdNotTxProjects";
            this.rdNotTxProjects.Size = new System.Drawing.Size(128, 21);
            this.rdNotTxProjects.TabIndex = 11;
            this.rdNotTxProjects.TabStop = true;
            this.rdNotTxProjects.Text = "Not Transducer";
            this.rdNotTxProjects.UseVisualStyleBackColor = true;
            this.rdNotTxProjects.CheckedChanged += new System.EventHandler(this.Projects_CheckedChanged);
            // 
            // grpAllProjects
            // 
            this.grpAllProjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAllProjects.AutoSize = true;
            this.grpAllProjects.Controls.Add(this.grid);
            this.grpAllProjects.Controls.Add(this.rdNotTxProjects);
            this.grpAllProjects.Controls.Add(this.rdTxProjectsOnly);
            this.grpAllProjects.Controls.Add(this.rdAllProjects);
            this.grpAllProjects.Location = new System.Drawing.Point(27, 31);
            this.grpAllProjects.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpAllProjects.Name = "grpAllProjects";
            this.grpAllProjects.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpAllProjects.Size = new System.Drawing.Size(788, 600);
            this.grpAllProjects.TabIndex = 12;
            this.grpAllProjects.TabStop = false;
            this.grpAllProjects.Text = "All Projects";
            // 
            // grpMyProjects
            // 
            this.grpMyProjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMyProjects.Controls.Add(this.lstMyProjs);
            this.grpMyProjects.Location = new System.Drawing.Point(931, 31);
            this.grpMyProjects.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpMyProjects.Name = "grpMyProjects";
            this.grpMyProjects.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpMyProjects.Size = new System.Drawing.Size(360, 600);
            this.grpMyProjects.TabIndex = 13;
            this.grpMyProjects.TabStop = false;
            this.grpMyProjects.Text = "My Projects";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(387, 7);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(292, 22);
            this.txtSearch.TabIndex = 14;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(321, 12);
            this.lblFilter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(43, 17);
            this.lblFilter.TabIndex = 15;
            this.lblFilter.Text = "Filter:";
            // 
            // MyProjects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(1326, 689);
            this.Controls.Add(this.lblFilter);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.grpMyProjects);
            this.Controls.Add(this.grpAllProjects);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lblHelp);
            this.Controls.Add(this.btnFromMyProjs);
            this.Controls.Add(this.btnToMyProjs);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MyProjects";
            this.Text = "MyProjects";
            this.Load += new System.EventHandler(this.MyProjects_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.grpAllProjects.ResumeLayout(false);
            this.grpAllProjects.PerformLayout();
            this.grpMyProjects.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.ListBox lstMyProjs;
        private System.Windows.Forms.Button btnToMyProjs;
        private System.Windows.Forms.Button btnFromMyProjs;
        private System.Windows.Forms.Label lblHelp;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.RadioButton rdAllProjects;
        private System.Windows.Forms.RadioButton rdTxProjectsOnly;
        private System.Windows.Forms.RadioButton rdNotTxProjects;
        private System.Windows.Forms.GroupBox grpAllProjects;
        private System.Windows.Forms.GroupBox grpMyProjects;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblFilter;
    }
}