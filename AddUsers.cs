using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TimeReg
{
    internal partial class AddUsers : Form
    {
        public AddUsers()
        {
            InitializeComponent();
            lblGuide.Text = "Read text-file with lines a'la': kthorsen; Kurt Thorsen; 34555; 7533\n\rExisting users will be skipped";
            this.Icon = TimeReg.Properties.Resources.clock;
        }

        List<DBUtil.Employee> employees;

        /// <summary>
        /// Read and parse a file into the grid - but NO SUBMIT here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFileOpen_Click(object sender, EventArgs e)
        {
            char sep = ';';
            char [] skip = {' ','\t'};
            int skipcount = 0;
            int syntaxOKcount = 0;

            ofDlg.AddExtension =true;
            ofDlg.Filter = "CSV (*.csv)|*.csv|Text Files (*.txt)|*.txt|All files (*.*)|*.*";
            if (ofDlg.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(ofDlg.FileName, System.Text.Encoding.GetEncoding("iso-8859-1")))
                {
                    string line;
                    employees = new List<DBUtil.Employee>();

                    while (sr.Peek() >= 0)
                    {
                        line = sr.ReadLine();
                        string [] tokens = line.Split(sep);
                        
                        if (tokens.Count() == 4)
                        {
                            if (tokens[0].Contains(' ') || tokens[0].Contains('@'))
                            {
                                MessageBox.Show("Using separator: '" + sep + "' I find spaces or @ in Netname: " + tokens[0],
                                "Illegal Netname");
                                return;
                            }

                            int  number;
                            bool numberOK = Int32.TryParse(tokens[2], out number);
                            int  dept;
                            bool deptOK = Int32.TryParse(tokens[3], out dept);

                            // Allow employee-numbers to be 0
                            if ( !numberOK || !deptOK || (number < 10000 && number > 0)|| dept > 10000)
                            {
                                MessageBox.Show("EmployeeNumber not legal number > 10000 or department not legal < 10000 in line:\r\n" +
                                    line, "Parsing error");
                                return;
                            }
                            
                            // The basics are fine - now test for dublets
                            syntaxOKcount++;
                            string netname = tokens[0].ToLower();
                            string fullname = tokens[1].TrimStart(skip);

                            bool dummy;
                            int curID = DBUtil.getEmployeeID(netname, out dummy);

                            // If we are inserting new users (NOT updating) && this user does not exist
                            // OR if we are updating existing && this user does exits 
                            // We will use the stuff (in INSERT or UPDATE)
                            if ((!chkUpdate.Checked && curID == 0) || (chkUpdate.Checked && curID != 0))
                                employees.Add(new DBUtil.Employee(netname, fullname, number, dept, false, -1));
                            else
                                skipcount++;
                        }
                        else
                        {
                            MessageBox.Show("Using separator: '"+sep+"' I find "+ tokens.Count() +" tokens in: "+line,
                            "Wrong no of tokens - Expecting 4");
                            return;
                        }
                    }
                }
                grid.DataSource = employees;
                grid.Refresh();
                chkUpdate.Enabled = false; // Don't allow user to change now!

                MessageBox.Show(skipcount.ToString() + " out of "+ syntaxOKcount.ToString()+ " ignored", "Status");
            }
        }

        /// <summary>
        /// Once the file is parsed and data looks good - insert into database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!chkUpdate.Checked)
                foreach (DBUtil.Employee employee in employees)
                    DBUtil.insertNewEmployee(employee); // Manually fix SuperUsers later
            else
                foreach (DBUtil.Employee employee in employees)
                    DBUtil.updateEmployee(employee);            // Manually fix SuperUsers later

            // Don't try again
            btnSubmit.Enabled = false;
        }
    }
}
