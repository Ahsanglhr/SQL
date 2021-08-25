using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeReg
{
    /// <summary>
    /// A nice and simple class.
    /// Basically transfers data from screen to database - except a lot of validation
    /// After being named it was changed to not only add new Projects but also to handle all updates
    /// </summary>
    internal partial class AddProject : Form
    {
        /// <summary>
        /// Constructor brings reference to AllProjects List, so that we can look for duplicates
        /// </summary>
        /// <param name="allProjects"></param>
        public AddProject(List<DBUtil.ProjTupple> allProjects)
        {
            InitializeComponent();
            Model model = Model.getInstance(false);
            cmbCategory.DataSource = new BindingSource(model.Categories,null);
            cmbCategory.DisplayMember = "Value";
            cmbCategory.ValueMember = "Key";
            this.Icon = TimeReg.Properties.Resources.clock;
            this.allProjects = allProjects;
            this.Text = "Edit Project Info";
            this.AcceptButton = btnOK;
        }

        private string  m_projectName;
        private int     m_psoNum;
        private string  m_projectNo;
        private string  m_category;
        private string  m_grouptag;
        private string  m_manager;

        public string   ProjectName 
        {   get {return m_projectName;}
            set { txtName.Text = value; }
        }

        public string Manager
        {
            // The value transferred is the Netname, but we show the full name.
            get { return m_manager; }
            set {
                List<DBUtil.Employee> managers = DBUtil.getEmployees(true);

                cmbManager.DataSource = new BindingSource(managers, null);
                cmbManager.DisplayMember = "FullName";
                cmbManager.ValueMember = "NetName";
                
                cmbManager.SelectedValue = (value != null) ? value : "Noname";
                m_manager = value;
            }
        }


        public string   ProjectNo 
        {   get { return m_projectNo;} 
            set { txtProjectNo.Text = value;}
        }
        
        public int      PSONo 
        {   get {return m_psoNum;}
            set { txtPSONo.Text = value.ToString(); } 
        }

        public string Category
        {   get {return m_category;}
            set 
            { 
                m_category = value;
                cmbCategory.SelectedValue = m_category;
            }
        }

        public string Grouptag  
        {
            get { return m_grouptag; }
            set { txtGrouptag.Text = value; }
        }


        public bool EnableCategory
        {
            get { return cmbCategory.Enabled; }
            set { cmbCategory.Enabled = value; }
        }

        public bool EnableProjectName
        {
            get { return txtName.Enabled;  }
            set { txtName.Enabled = value; }
        }

        public bool EnableManager
        {
            get { return cmbManager.Enabled; }
            set { cmbManager.Enabled = value; }
        }


        public bool EnableProjectNo
        {
            get { return txtProjectNo.Enabled; }
            set { txtProjectNo.Enabled = value; }
        }

        public bool EnablePSONo
        {
            get { return txtPSONo.Enabled; }
            set { txtPSONo.Enabled = value; }
        }

        public bool EnableGrouptag
        {
            get { return txtGrouptag.Enabled; }
            set { txtGrouptag.Enabled = value; }
        }

        List<DBUtil.ProjTupple> allProjects;

        /// <summary>
        /// The method that does it all
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            // Set up the bad result for all the early returns
            this.DialogResult = DialogResult.Cancel; 

            // Only change to the name string is to keep it with the database field size
            m_projectName = DBUtil.truncStr(txtName.Text);

            if (m_projectName.Length == 0)
            {
                MessageBox.Show("Name cannot be blank");
                return;
            }

            ulong uoraclenum; // Number version of the string used elsewhere

            // Blank is great for ProjectNo
            if (txtProjectNo.Text.Length == 0)
            {
                uoraclenum = 0; 
            }
            else if (txtProjectNo.Text.Length != 10) // If not blank there are demands!
            {
                MessageBox.Show("Oracle Project number MUST be 10 digits - starting with '00' -  or completely blank");
                return;
            }
            else if (!UInt64.TryParse(txtProjectNo.Text, out uoraclenum) || uoraclenum >= 100000000)
            {
                MessageBox.Show("Oracle Project number MUST be a 10 digit positive number-string - starting with '00'");
                return;
            }
            
            // OK - its good - we can use it as it is
            m_projectNo = txtProjectNo.Text;    

            // PSO number may be blank - or an integer. Default is 0
            m_psoNum = 0;
            if (txtPSONo.Text.Length != 0)
            {
                if (!Int32.TryParse(txtPSONo.Text, out m_psoNum) || m_psoNum < 0)
                {
                    MessageBox.Show("PSO Project Number MUST be an integer - use 0 or blank if not PSO");
                    return;
                }
            }

            // We now have all three attributes. Now assure that they are all unique
            // Note that a child project will inherit numbers from its parent
            foreach (DBUtil.ProjTupple project in allProjects)
            {
                if (txtName.Enabled && project.Name.Equals(ProjectName))
                {
                    MessageBox.Show("Sorry - This name is already in use");
                    return;
                }
                else if (txtPSONo.Enabled && m_psoNum > 0 && project.PSONumber == m_psoNum)
                {
                    MessageBox.Show("Sorry - This PSO Project Number is already in use");
                    return;
                }
                else if (txtProjectNo.Enabled && uoraclenum != 0 && project.ProjectNo.Equals(m_projectNo))
                {
                    MessageBox.Show("Sorry - This Oracle Project Number is already in use");
                    return;
                }
            }

            m_category = cmbCategory.SelectedValue.ToString();

            // Fetch the netname
            m_manager = (cmbManager.SelectedValue != null) ? cmbManager.SelectedValue.ToString() : null;

            m_grouptag = txtGrouptag.Text;

            // Finally - everything's OK
            this.DialogResult = DialogResult.OK;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
