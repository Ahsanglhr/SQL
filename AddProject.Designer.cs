namespace TimeReg
{
    partial class AddProject
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblManager = new System.Windows.Forms.Label();
            this.txtGrouptag = new System.Windows.Forms.TextBox();
            this.lblGrouptag = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.txtPSONo = new System.Windows.Forms.TextBox();
            this.txtProjectNo = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblPSONo = new System.Windows.Forms.Label();
            this.lbl2 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbManager = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbManager);
            this.groupBox1.Controls.Add(this.lblManager);
            this.groupBox1.Controls.Add(this.txtGrouptag);
            this.groupBox1.Controls.Add(this.lblGrouptag);
            this.groupBox1.Controls.Add(this.cmbCategory);
            this.groupBox1.Controls.Add(this.lblCategory);
            this.groupBox1.Controls.Add(this.txtPSONo);
            this.groupBox1.Controls.Add(this.txtProjectNo);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.lblPSONo);
            this.groupBox1.Controls.Add(this.lbl2);
            this.groupBox1.Controls.Add(this.lblName);
            this.groupBox1.Location = new System.Drawing.Point(30, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(518, 379);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Project Info";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // lblManager
            // 
            this.lblManager.AutoSize = true;
            this.lblManager.Location = new System.Drawing.Point(34, 302);
            this.lblManager.Name = "lblManager";
            this.lblManager.Size = new System.Drawing.Size(112, 17);
            this.lblManager.TabIndex = 10;
            this.lblManager.Text = "Project Manager";
            // 
            // txtGrouptag
            // 
            this.txtGrouptag.Location = new System.Drawing.Point(223, 244);
            this.txtGrouptag.Name = "txtGrouptag";
            this.txtGrouptag.Size = new System.Drawing.Size(193, 22);
            this.txtGrouptag.TabIndex = 9;
            // 
            // lblGrouptag
            // 
            this.lblGrouptag.AutoSize = true;
            this.lblGrouptag.Location = new System.Drawing.Point(34, 249);
            this.lblGrouptag.Name = "lblGrouptag";
            this.lblGrouptag.Size = new System.Drawing.Size(68, 17);
            this.lblGrouptag.TabIndex = 8;
            this.lblGrouptag.Text = "Grouptag";
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(223, 195);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(258, 24);
            this.cmbCategory.TabIndex = 7;
            this.cmbCategory.FlatStyle = System.Windows.Forms.FlatStyle.Popup;

            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(34, 201);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(65, 17);
            this.lblCategory.TabIndex = 6;
            this.lblCategory.Text = "Category";
            // 
            // txtPSONo
            // 
            this.txtPSONo.Location = new System.Drawing.Point(223, 148);
            this.txtPSONo.Name = "txtPSONo";
            this.txtPSONo.Size = new System.Drawing.Size(193, 22);
            this.txtPSONo.TabIndex = 5;
            // 
            // txtProjectNo
            // 
            this.txtProjectNo.Location = new System.Drawing.Point(223, 101);
            this.txtProjectNo.Name = "txtProjectNo";
            this.txtProjectNo.Size = new System.Drawing.Size(193, 22);
            this.txtProjectNo.TabIndex = 4;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(223, 54);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(193, 22);
            this.txtName.TabIndex = 3;
            // 
            // lblPSONo
            // 
            this.lblPSONo.AutoSize = true;
            this.lblPSONo.Location = new System.Drawing.Point(34, 153);
            this.lblPSONo.Name = "lblPSONo";
            this.lblPSONo.Size = new System.Drawing.Size(91, 17);
            this.lblPSONo.TabIndex = 2;
            this.lblPSONo.Text = "PSO Number";
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.Location = new System.Drawing.Point(34, 105);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(104, 17);
            this.lbl2.TabIndex = 1;
            this.lbl2.Text = "Oracle Number";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(34, 57);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(45, 17);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(471, 433);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Submit";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(382, 433);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cmbManager
            // 
            this.cmbManager.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbManager.FormattingEnabled = true;
            this.cmbManager.Location = new System.Drawing.Point(223, 293);
            this.cmbManager.Name = "cmbManager";
            this.cmbManager.Size = new System.Drawing.Size(193, 24);
            this.cmbManager.TabIndex = 11;
            this.cmbManager.FlatStyle = System.Windows.Forms.FlatStyle.Popup;

            // 
            // AddProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 495);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.Name = "AddProject";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtPSONo;
        private System.Windows.Forms.TextBox txtProjectNo;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblPSONo;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.TextBox txtGrouptag;
        private System.Windows.Forms.Label lblGrouptag;
        private System.Windows.Forms.Label lblManager;
        private System.Windows.Forms.ComboBox cmbManager;
    }
}