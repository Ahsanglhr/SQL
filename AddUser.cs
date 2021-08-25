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
    public partial class AddUser : Form
    {
        public AddUser()
        {
            InitializeComponent();
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            this.Icon = TimeReg.Properties.Resources.clock;

            toolTip1.SetToolTip(txtFullName, "The Full Name of the new Employee");
            toolTip1.SetToolTip(txtNetName, "If login is BKOGC\\abcd then this is abcd");
            toolTip1.SetToolTip(txtDeptNo, "Typically a 4-digit number");
            toolTip1.SetToolTip(txtEmployeeNo, "Typically a 5-digit number. Royston can use 0");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Parse the integers and check all parameters before adding the user to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            int empNo;
            int deptNo;

            if (txtFullName.Text.Length == 0)
            {
                MessageBox.Show("Employee Full Name must not be blank!", "Illegal Full Name");
                return;
            }


            if (txtNetName.Text.Length == 0 || txtNetName.Text.Contains(' '))
            {
                MessageBox.Show("Employee Netname must not be blank or contain blanks!", "Illegal Netname");
                return;
            }

            if (!int.TryParse(txtDeptNo.Text, out deptNo))
            {
                MessageBox.Show("Department Number is not a number!", "Illegal Department Number");
                return;
            }

            if (deptNo > 10000)
            {
                MessageBox.Show("Department Number must not be greater than 10000!", "Illegal Department Number");
                return;
            }

            if (!int.TryParse(txtEmployeeNo.Text, out empNo))
            {
                MessageBox.Show("Employee Number is not a number!", "Illegal Employee Number");
                return;
            }

            if (empNo < 10000 && empNo != 0)
            {
                MessageBox.Show("Employee Number must not be less than 10000 (except 0)!", "Illegal Employee Number");
                return;
            }


            bool dummy;
            int curID = DBUtil.getEmployeeID(txtNetName.Text, out dummy);

            if (curID != 0)
            {
                MessageBox.Show("Someone with this netname already exists!", "Illegal Netname");
                return;
            }

            DBUtil.Employee employee = new DBUtil.Employee(txtNetName.Text.ToLower(), txtFullName.Text, empNo, deptNo, chkSuper.Checked, -1);

            // Finally: The actual submit!
            DBUtil.insertNewEmployee(employee); 

            // Check
            curID = DBUtil.getEmployeeID(txtNetName.Text, out dummy);

            if (curID != 0)
            {
                MessageBox.Show("New employee added - You may check Report or Alias!", "Success");
                Close();
            }
            else
                MessageBox.Show("For some reason the insert of the new Employee went wrong", "Error");

            return;
        }
    }
}
