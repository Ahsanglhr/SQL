using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;


namespace TimeReg
{
    internal partial class ReportsDlg : Form
    {
        // SendMessage was used in an attempt to speed up the datagrid - but to no avail
//      [DllImport("user32.dll")]
//      private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
//      private const int WM_SETREDRAW = 11;

        private string fileName = null;
        private List<DBUtil.ProjectReport> projectReport = null;
        private List<DBUtil.PersonalReport> personalReport = null;
        private List<DBUtil.PersonWorkdays> daysAccounted = null;
        private List<DBUtil.ProjTupple> allProjs = null;
        private Model model = null;
        private DBUtil.Employee employee;
        private List<string> categories;

        #region init
        public ReportsDlg(Model model)
        {
            InitializeComponent();
            this.model = model;
            this.Icon = TimeReg.Properties.Resources.clock;
            toolTips.SetToolTip(lstUsers, "This is only relevant for a report on specific user - not totals");
            toolTips.SetToolTip(lblUser, "This is only relevant for a report on specific user - not totals");
            toolTips.SetToolTip(rdBtnTasksForMe, "Select any user in 'Basic Choices' group");
            toolTips.SetToolTip(rdBtnDaysAccounted, "How many weekdays are accounted for in any way - by person");
            toolTips.SetToolTip(rdBtnTotPerParentProjs, "In UI: Sorted by Parent (but with absense first)");
            toolTips.SetToolTip(rdBtnTotPerProj, "In UI: Sorted by Parent, then Child (but with absense first)");
            toolTips.SetToolTip(rdBtnTotalPerProjPerEmp, "In UI: Sorted by Employee, then Parent of project and finally Project");

            // If the user is GCC and not a manager they do not want each other to see anything
            // Welcome to 1970
            if (model.Catalog.Equals("GCC")  && !model.SuperUser)  
            {
                btnFileSave.Enabled = false; // Drop
                rdBtnDaysAccounted.Enabled = false;
                rdBtnTotalPerProjPerEmp.Enabled = false;
                rdBtnTotPerParentProjs.Enabled = false;
                rdBtnTotPerProj.Enabled = false;
            }

            // A convenient start date is start of current year
            dtFrom.Value = new DateTime(2018, 1, 1);

            // Double buffering improves performance
            // - but can make DGV slow in remote desktop
            if (!System.Windows.Forms.SystemInformation.TerminalServerSession)
            {
                Type dgvType = grid.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                    BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(grid, true, null);
            }
        }

        /// <summary>
        /// Called before Loading - after Constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportsDlg_Load(object sender, EventArgs e)
        {
            // Make sure that NONE are selected
            rdBtnTasksForMe.Checked = rdBtnTotalPerProjPerEmp.Checked = rdBtnTotPerParentProjs.Checked = false;
            rdBtnDaysAccounted.Checked  = rdBtnTotPerProj.Checked = false;
            rdBtnTotalPerProjPerEmp.TabStop = false;
            rdBtnTotPerProj.TabStop = false;
            rdBtnTasksForMe.TabStop = false;
            rdBtnTotPerParentProjs.TabStop = false;
            rdBtnDaysAccounted.TabStop = false;

            lstMyOnly.Enabled = rdBtnTasksForMe.Checked;

            // Make a list with all projects and a fake ALL in front
            allProjs = DBUtil.getAllProjects();
            allProjs.Insert(0, new DBUtil.ProjTupple(0, "All", 0, "", false,"", 0,"ALL","","All"));
            lstMyOnly.DataSource = allProjs;
            lstMyOnly.ValueMember = "Name";
            lstMyOnly.DisplayMember = "Name";
            lstMyOnly.DropDownStyle = ComboBoxStyle.DropDownList;
            lstMyOnly.MouseWheel += new MouseEventHandler(List_MouseWheel);
            lstMyOnly.Enabled = false;

            //Make a list of all users
            lstUsers.Enabled = rdBtnTasksForMe.Checked && model.SuperUser;
            List<DBUtil.Employee> allUsers = DBUtil.getEmployees(false);
            lstUsers.DataSource = allUsers;
            lstUsers.ValueMember = "NetName";
            lstUsers.DisplayMember = "FullName";
            lstUsers.SelectedValue = model.Me;
            lstUsers.MouseWheel += new MouseEventHandler(List_MouseWheel);
            lstUsers.Enabled = false;

            // Make a list of all departments - no selection = all
            lstDepts.DataSource = DBUtil.getDepartments();
            lstDepts.Enabled = false;
            chkUseDepts.Enabled = false;
            chkUseDepts.Checked = false;

            // Create a list with all the key from the category dictionary, with an "All" as the first
            categories = new List<string>();
            categories.Add("All");
            categories.AddRange(model.Categories.Values);
            cmbCategories.DataSource = categories;
            cmbCategories.SelectedIndex = 0;
        }
        #endregion

        #region file
        /// <summary>
        /// What filename are we to save in?
        /// Appears that the std. filesave dialog remembers the folder!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFileSave_Click(object sender, EventArgs e)
        {
            const string sep = ";";
            const string stStr = "=\"";
            const string endStr = "\"";

            saveReportDlg.AddExtension = true;
            saveReportDlg.Filter = "Comma Separated Values (*.csv)|*.csv|All files (*.*)|*.*";
            if (saveReportDlg.ShowDialog() == DialogResult.OK)
            {
                fileName = saveReportDlg.FileName;
                try
                {   // Unicode to support Danish chars!
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName,false,Encoding.Unicode))
                    {
                        file.WriteLine("sep=" + sep);
                        // Write a header 
                        // Project-names go like  ="2250" - with extra " around to assure that the 2250 stays text
                        if (rdBtnTotPerProj.Checked)
                        {
                            if (chkMonths.Checked)
                                exportAmmortizedTotals(false, file, sep, stStr, endStr);
                            else
                            {
                                file.WriteLine("Grouptag" + sep + "Project" + sep + "Manager" + sep + "ProjectNo" + sep + "PSONo" + sep + "Category" + sep + "TopLevel" + sep + "Hours" + sep + "Closed");
                                foreach (DBUtil.ProjectReport reportline in projectReport)
                                    file.WriteLine(
                                        stStr + reportline.Grouptag + endStr + sep +
                                        stStr + reportline.Project + endStr + sep +
                                        stStr + reportline.Manager + endStr + sep +
                                        stStr + reportline.ProjectNo + endStr + sep +
                                        stStr + reportline.PSONumber + endStr + sep + 
                                        stStr + reportline.Category + endStr + sep +
                                        stStr + ((reportline.ParentID <= 0) ? reportline.Project.ToString() : reportline.Parent.ToString()) + endStr + sep +
                                        reportline.Hours.ToString() + sep +
                                        DBUtil.projectsDict[reportline.ProjectID].Closed);
                            }
                        }
                        else if (rdBtnTotPerParentProjs.Checked)
                        {
                            if (chkMonths.Checked)
                                exportAmmortizedTotals(true, file, sep, stStr, endStr);
                            else
                            {
                                file.WriteLine("Grouptag" + sep + "Project" + sep + "Manager" + sep + "ProjectNo" + sep + "PSONo" + sep +"(Category)"+ sep + "Hours" + sep + "Closed");
                                foreach (DBUtil.ProjectReport reportline in projectReport)
                                    file.WriteLine(
                                        stStr + reportline.Grouptag + endStr + sep +
                                        stStr + reportline.Project + endStr + sep +
                                        stStr + reportline.Manager + endStr + sep +
                                        stStr + reportline.ProjectNo + endStr + sep +
                                        stStr + reportline.PSONumber + endStr + sep +
                                        stStr + reportline.Category + endStr + sep +
                                        reportline.Hours.ToString() + sep +
                                        DBUtil.projectsDict[reportline.ProjectID].Closed);
                            }
                        }
                        else if (rdBtnTotalPerProjPerEmp.Checked)
                        {
                            if (chkMonths.Checked)
                                exportAmmortizedEmployees(file, sep, stStr, endStr);
                            else
                            {
                                file.WriteLine("Employee" + sep + "Department" + sep + "Grouptag" + sep + "Project" + sep + "Manager" +sep + "ProjectNo" + sep + "PSONo" + sep + "Category" + sep + "TopLevel" + sep + "Hours" +sep +"Closed");
                                foreach (DBUtil.ProjectReport reportline in projectReport)
                                    file.WriteLine(reportline.Employee + sep +
                                        stStr + reportline.Department + endStr + sep +
                                        stStr + reportline.Grouptag + endStr + sep +
                                        stStr + reportline.Project + endStr + sep +
                                        stStr + reportline.Manager + endStr + sep +
                                        stStr + reportline.ProjectNo + endStr + sep +
                                        stStr + reportline.PSONumber + endStr + sep +
                                        stStr + reportline.Category + endStr + sep +
                                        stStr + ((reportline.ParentID <= 0) ? reportline.Project.ToString() : reportline.Parent.ToString()) + endStr + sep +
                                        reportline.Hours.ToString() + sep +
                                        stStr + DBUtil.projectsDict[reportline.ProjectID].Closed + endStr);
                            }
                        }
                        else if (rdBtnTasksForMe.Checked)
                        {
                            file.WriteLine("Date" + sep + "Project" + sep + "ProjectNo" + sep + "Hours" + sep + "Comment");
                            foreach (DBUtil.PersonalReport reportline in personalReport)
                                file.WriteLine(reportline.Date.ToShortDateString() + sep + stStr + reportline.Project + endStr + sep +
                                    stStr + reportline.ProjectNo + endStr + sep + +reportline.Hours + sep + stStr+reportline.Comment+endStr);
                        }
                        else if (rdBtnDaysAccounted.Checked)
                        {
                            file.WriteLine("Name" + sep + "Days" + sep + "Month" + sep + "Year");
                            foreach (DBUtil.PersonWorkdays reportline in daysAccounted)
                                file.WriteLine(reportline.Employee + sep + reportline.Days + sep + reportline.MonthNo + sep + +reportline.YearNo);
                        }
                    }
                }
                catch (System.IO.IOException exc)
                {
                    MessageBox.Show("Illegal Action Saving file: " + exc.Message, "File Save error");
                }
            }
        }
        #endregion

        /// <summary>
        /// Create an ammortized view by months for individual employees per project
        /// </summary>
        /// <param name="file"></param>
        /// <param name="sep"></param>
        /// <param name="stStr"></param>
        /// <param name="endStr"></param>
        private void exportAmmortizedEmployees(System.IO.StreamWriter file, string sep, string stStr, string endStr)
        {
            int startYear = dtFrom.Value.Year;
            int startMonth = dtFrom.Value.Month;
            int endYear = dtTo.Value.Year;
            int endMonth = dtTo.Value.Month;

            int months = (endYear - startYear) * 12 + endMonth - startMonth + 1;
            List<DBUtil.AggrProjectReport> report;

            report = DBUtil.getAmortizedEmployees(dtFrom.Value, dtTo.Value);

            int curYear = startYear;
            int curMonth = startMonth;

            file.Write("Employee" + sep + "Department" + sep + "Grouptag" + sep + "Project" + sep + "Manager" + sep + "ProjectNo" + sep + "PSONo" + sep + "Category" + sep + "TopLevel");
            for (int col = 0; col < months; col++)
            {
                file.Write(sep + curYear.ToString() + "-" + curMonth.ToString());
                incMonth(ref curYear, ref curMonth);
            }
            file.WriteLine(""); // End the header

            curYear = startYear;
            curMonth = startMonth;
            int curProject = 0;

            foreach (DBUtil.AggrProjectReport reportline in report)
            {
                if (curProject != reportline.ProjectID)
                {
                    if (!model.SuperUser && DBUtil.projectsDict[reportline.ProjectID].ParentID == -1)
                        continue;  // Next reportline

                    if (curProject != 0)
                        file.WriteLine(""); // End the previous row
                    // Next Project - write the left colums
                    curYear = startYear;
                    curMonth = startMonth;
                    curProject = reportline.ProjectID;
                    file.Write(stStr + reportline.Employee + endStr + sep +
                               stStr + reportline.Department + endStr + sep +
                               stStr + DBUtil.projectsDict[reportline.ProjectID].Grouptag + endStr + sep +
                               stStr + DBUtil.projectsDict[reportline.ProjectID].Name + endStr + sep +
                               stStr + DBUtil.projectsDict[reportline.ProjectID].Manager + endStr + sep +
                               stStr + DBUtil.projectsDict[reportline.ProjectID].ProjectNo + endStr + sep +
                               stStr + DBUtil.projectsDict[reportline.ProjectID].PSONumber + endStr + sep +
                               stStr + DBUtil.projectsDict[reportline.ProjectID].Category + endStr + sep +
                               stStr + ((DBUtil.projectsDict[reportline.ProjectID].ParentID <= 0) ? DBUtil.projectsDict[reportline.ProjectID].Name : DBUtil.projectsDict[reportline.ProjectID].ParentName) + endStr);
                }

                // Now write intermediate blanks
                while (curYear < reportline.Year || (curYear == reportline.Year && curMonth < reportline.Month))
                {
                    file.Write(sep + " ");
                    incMonth(ref curYear, ref curMonth);
                }

                // Finally - write the hours of this item - and prepare for next month
                file.Write(sep + ((reportline.Hours > 0) ? reportline.Hours.ToString() : " "));
                incMonth(ref curYear, ref curMonth);
            }
        }

        /// <summary>
        /// The SQL has returned an ammortized list - that is records like:
        /// ProjectID, Year, Month, Hours
        /// We need to write this into lines - extracting ProjectNo and Name from cache - and fill in the blanks
        /// Param "parent" is true when projects are summed at the parent
        /// </summary>
        private void exportAmmortizedTotals(bool parent, System.IO.StreamWriter file, string sep, string stStr, string endStr)
        {
            int startYear  = dtFrom.Value.Year;
            int startMonth = dtFrom.Value.Month;
            int endYear    = dtTo.Value.Year;
            int endMonth   = dtTo.Value.Month;

            int months = (endYear - startYear) * 12 + endMonth - startMonth + 1;

            List<DBUtil.AggrReport> report;
            if (parent)
            {
                // Wite the header as: ProjectName  ProjectNo PSONo 2015-1   2015-2  2015-3....
                file.Write("Grouptag" + sep + "Project" + sep + "Manager" + sep + "ProjectNo" + sep + "PSONo" + sep + "(Category)");
                report = DBUtil.getAmortizedParentTotals(dtFrom.Value, dtTo.Value, model.SuperUser, departments());
            }
            else
            {
                // Wite the header as: ProjectName  TopLevel ProjectNo PSONo 2015-1   2015-2  2015-3....
                file.Write("Grouptag" + sep + "Project" + sep + "Manager" + sep + "TopLevel" + sep + "ProjectNo" + sep + "PSONo" + sep + "Category");
                report = DBUtil.getAmortizedProjectTotals(dtFrom.Value, dtTo.Value, model.SuperUser, departments());
            }

            int curYear = startYear;
            int curMonth = startMonth;

            for (int col = 0; col < months; col++ )
            {
                file.Write(sep + curYear.ToString() + "-" + curMonth.ToString());
                incMonth(ref curYear, ref curMonth);
            }
            file.WriteLine(""); // End the header

            curYear  = startYear;
            curMonth = startMonth;
            int curProject = 0;

            foreach (DBUtil.AggrReport item in report)
            {
                if (curProject != item.Project)
                {
                    if (curProject != 0)
                        file.WriteLine(""); // End the previous row
                    // Next Project - write the left colums
                    curYear  = startYear;
                    curMonth = startMonth;
                    curProject = item.Project;
                    StringBuilder writeme = new StringBuilder();
                    writeme.Append(stStr + DBUtil.projectsDict[curProject].Grouptag + endStr + sep);
                    writeme.Append(stStr + DBUtil.projectsDict[curProject].Name + endStr + sep);
                    writeme.Append(stStr + DBUtil.projectsDict[curProject].Manager + endStr + sep);
                    if (!parent)  // Add a TopLevel column value if not summing on parents
                        writeme.Append(((DBUtil.projectsDict[curProject].ParentID <= 0) ? DBUtil.projectsDict[curProject].Name : DBUtil.projectsDict[curProject].ParentName) + sep);
                    writeme.Append(DBUtil.projectsDict[curProject].ProjectNo + sep);
                    writeme.Append(DBUtil.projectsDict[curProject].PSONumber + sep);
                    writeme.Append(stStr + DBUtil.projectsDict[curProject].Category + endStr);
                    file.Write(writeme);
                 }

                // Now write intermediate blanks
                while (curYear < item.Year ||  (curYear == item.Year && curMonth < item.Month))
                {
                    file.Write(sep + " ");
                    incMonth(ref curYear, ref curMonth);
                }

                // Finally - write the hours of this item - and prepare for next month
                file.Write(sep + ((item.Hours >0) ? item.Hours.ToString() : " "));
                incMonth(ref curYear, ref curMonth);
            }
        }

        /// <summary>
        /// Increment the Month and restart in next year if needed
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        private void incMonth(ref int year, ref int month)
        {
            if (++month > 12)
            {
                month = 1;
                year++;
            }
        }

        #region reports
        /// <summary>
        /// Update Personal Report. Called from several Event-Handlers
        /// </summary>
        private void updatePersonalReport()
        {
            Cursor saveCursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            bool dummy;
            int ID = DBUtil.getEmployeeID(employee.NetName, out dummy);

            List<DBUtil.PersonalReport> prepare = DBUtil.getPersonalReport(ID, dtFrom.Value.Date, dtTo.Value.Date);
            personalReport = new List<DBUtil.PersonalReport>();

            if (lstMyOnly.SelectedIndex == 0) // ALL Projects
                personalReport = prepare;
            else
                foreach (DBUtil.PersonalReport report in prepare)
                    if (report.Project.Equals(((DBUtil.ProjTupple)(lstMyOnly.SelectedItem)).Name))
                        personalReport.Add(report);

            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            grid.DataSource = personalReport;
            // This report is different from the others - all columns may be visible

            grid.Columns["Date"].FillWeight         = 40;
            grid.Columns["Project"].FillWeight      = 80;
            grid.Columns["ProjectNo"].FillWeight    = 40;
            grid.Columns["Hours"].FillWeight        = 30;
            grid.Columns["Hours"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns["Comment"].FillWeight      = 80;
            
            
 //           grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            Cursor = saveCursor;
        }


        /// <summary>
        /// Calculate the totals - accumulated on the Parents
        /// Called from several event-handlers.
        /// </summary>
        private void updateTotalsOnParentReport()
        {
            Cursor saveCursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            List<DBUtil.ProjectReport> prepare = DBUtil.getProjectTotals(dtFrom.Value.Date, dtTo.Value.Date, model.SuperUser, departments());
            projectReport = new List<DBUtil.ProjectReport>();

            DBUtil.ProjectReport curParent = null;
            int curParentID = 0;

            foreach (DBUtil.ProjectReport detailreport in prepare)
            {
                // Real parent have ParentID 0, while the special ones with "-1" are Parents with no children (Sickness, Vacation etc)
                if (detailreport.ParentID == 0 || detailreport.ParentID == -1)
                {
                    // A new parent - start a new condensed report based on the parent
                    projectReport.Add(detailreport);
                    curParent = detailreport;
                    curParentID = detailreport.ProjectID;   // the parentID is the PROJECT ID of the parent!
                }
                else
                {
                    if (detailreport.ParentID == curParentID)
                    // A child of a parent that had numbers itself - or number two or later parentless - add it to the latest parent 
                        curParent.Hours += detailreport.Hours;
                    else
                    {
                        // A "parentless child" - we only come here when the Parent Project is empty because the SQL skips it
                        // The fix is to recreate the parent - with the hours from the child
                        string parentname = allProjs.Find(x => x.ProjectID == detailreport.ProjectID).ParentName;
                        DBUtil.ProjectReport rep = new DBUtil.ProjectReport("All", parentname, detailreport.ProjectNo,
                            null, detailreport.Hours, detailreport.ParentID, 0, 0, detailreport.PSONumber, detailreport.Category, detailreport.Manager, detailreport.Grouptag);
                        projectReport.Add(rep);
                        curParent = rep;  // Act as parent for the next sibling
                        curParentID = detailreport.ParentID; // NB! This childs parent is the ID
                    }
                }
            }

            if (!cmbCategories.Text.ToLower().Equals("all"))
            {
                List<DBUtil.ProjectReport> reportTemp = new List<DBUtil.ProjectReport>();
                // Get e.g. "NPI" from "NPI - New Product Introduction"
                string key = model.Categories.FirstOrDefault(x => x.Value.Equals(cmbCategories.Text)).Key;   //Reverse lookup
                // Now filter based on this

                reportTemp = projectReport.FindAll(x => x.Category.Equals(key));
                projectReport = reportTemp;
            }

//          grid.SuspendLayout();
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            grid.DataSource = projectReport;
            grid.Columns["Employee"].Visible    = false;   
            grid.Columns["Project"].Visible     = true;
            grid.Columns["Project"].FillWeight  = 40;
            grid.Columns["Manager"].Visible     = true;
            grid.Columns["Manager"].FillWeight  = 20;
            grid.Columns["ProjectNo"].Visible   = true;
            grid.Columns["ProjectNo"].FillWeight = 20;
            grid.Columns["ProjectNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns["PSONumber"].Visible = true;
            grid.Columns["PSONumber"].FillWeight = 10;
            grid.Columns["PSONumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns["Parent"].Visible      = false;
            grid.Columns["Parent"].FillWeight   = 20;
            grid.Columns["Hours"].Visible       = true;
            grid.Columns["Hours"].FillWeight    = 15;
            grid.Columns["Hours"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns["ProjectID"].Visible   = false;  
            grid.Columns["ParentID"].Visible    = false;   
            grid.Columns["Department"].Visible  = false;
            grid.Columns["Category"].Visible    = true;
            grid.Columns["Category"].FillWeight = 10;
            grid.Columns["Grouptag"].Visible    = true;
            grid.Columns["Grouptag"].FillWeight = 15;
//            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
//          grid.ResumeLayout();
            Cursor = saveCursor;
        }



        /// <summary>
        /// Update the Sum per Project report. Called from several Event Handlers
        /// </summary>
        private void updateTotalsReport()
        {
            Cursor saveCursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            projectReport = DBUtil.getProjectTotals(dtFrom.Value.Date, dtTo.Value.Date, model.SuperUser, departments());

            // The report is more understandable if hours directly on Parents include the Parent name
            foreach (DBUtil.ProjectReport detailreport in projectReport)
            {
                if (detailreport.ParentID <= 0)   // This is a parent
                {
                    detailreport.Parent = detailreport.Project;
                }
            }


            if (!cmbCategories.Text.ToLower().Equals("all"))
            {
                List<DBUtil.ProjectReport> reportTemp = new List<DBUtil.ProjectReport>();
                // Get e.g. "NPI" from "NPI - New Product Introduction"
                string key = model.Categories.FirstOrDefault(x => x.Value.Equals(cmbCategories.Text)).Key;   //Reverse lookup
                // Now filter based on this
                reportTemp = projectReport.FindAll(x => x.Category.Equals(key));
                projectReport = reportTemp;
            }

//          grid.SuspendLayout();
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            grid.DataSource = projectReport;

            grid.Columns["Employee"].Visible    = false;
            grid.Columns["Project"].Visible     = true;
            grid.Columns["Project"].FillWeight  = 40;
            grid.Columns["Manager"].Visible     = true;
            grid.Columns["Manager"].FillWeight  = 20;
            grid.Columns["ProjectNo"].Visible   = true;
            grid.Columns["ProjectNo"].FillWeight = 20;
            grid.Columns["ProjectNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns["PSONumber"].Visible = true;
            grid.Columns["PSONumber"].FillWeight= 10;
            grid.Columns["PSONumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns["Parent"].Visible = true;
            grid.Columns["Parent"].FillWeight   = 20;
            grid.Columns["Hours"].Visible       = true;
            grid.Columns["Hours"].FillWeight    = 15;
            grid.Columns["Hours"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns["Category"].Visible    = true;
            grid.Columns["Category"].FillWeight = 10;
            grid.Columns["ProjectID"].Visible   = false;
            grid.Columns["ParentID"].Visible    = false;
            grid.Columns["Department"].Visible  = false;
            grid.Columns["Grouptag"].Visible    = true;
            grid.Columns["Grouptag"].FillWeight = 15;

//            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
//          grid.ResumeLayout();
            Cursor = saveCursor;
        }

        /// <summary>
        /// Update the Sum per Employee Per Project Report. Called from several event handlers
        /// </summary>
        private void updateTotalPerEmpsReport()
        {
            Cursor saveCursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            projectReport = DBUtil.getProjectTotalsPerEmployee(dtFrom.Value.Date, dtTo.Value.Date, model.SuperUser);

            // Filter out everything but the relevant category
            if (!cmbCategories.Text.ToLower().Equals("all"))
            {
                List<DBUtil.ProjectReport> reportTemp = new List<DBUtil.ProjectReport>();
                // Get e.g. "NPI" from "NPI - New Product Introduction"
                string key = model.Categories.FirstOrDefault(x => x.Value.Equals(cmbCategories.Text)).Key;   //Reverse lookup
                // Now filter based on this
                reportTemp = projectReport.FindAll(x => x.Category.Equals(key));
                projectReport = reportTemp;
            }

            
           // grid.SuspendLayout();
            //SendMessage(grid.Handle, WM_SETREDRAW, false, 0);
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            grid.DataSource = projectReport;

            grid.Columns["Employee"].Visible        = true;
            grid.Columns["Employee"].FillWeight     = 30;
            grid.Columns["Project"].Visible         = true;
            grid.Columns["Project"].FillWeight      = 40;
            grid.Columns["Manager"].Visible         = true;
            grid.Columns["Manager"].FillWeight      = 20;
            grid.Columns["ProjectNo"].Visible       = true;
            grid.Columns["ProjectNo"].FillWeight    = 20;
            grid.Columns["ProjectNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns["PSONumber"].Visible       = true;
            grid.Columns["PSONumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns["PSONumber"].FillWeight    = 15;
            grid.Columns["Parent"].Visible          = false;
            grid.Columns["Parent"].FillWeight       = 15;
            grid.Columns["Hours"].Visible           = true;
            grid.Columns["Hours"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns["Hours"].FillWeight        = 15;
            grid.Columns["ProjectID"].Visible       = false;
            grid.Columns["ParentID"].Visible        = false;
            grid.Columns["Department"].Visible      = true;
            grid.Columns["Department"].FillWeight   = 15;
            grid.Columns["Category"].Visible        = true;
            grid.Columns["Category"].FillWeight     = 10;
            grid.Columns["Grouptag"].Visible        = true;
            grid.Columns["Grouptag"].FillWeight     = 15;

//            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //SendMessage(grid.Handle, WM_SETREDRAW, true, 0);

            //grid.ResumeLayout();
            Cursor = saveCursor;
        }

        /// <summary>
        /// Update the numbers of days per month that people has accounted for
        /// </summary>
        private void updateDaysAccountedFor()
        {
            Cursor saveCursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            daysAccounted = DBUtil.getPersonWorkDays(dtFrom.Value.Date, dtTo.Value.Date, departments());
            //grid.SuspendLayout();

            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            grid.DataSource = daysAccounted;
            grid.Columns["Employee"].Visible =  true;
            grid.Columns["Employee"].FillWeight = 100;
            grid.Columns["Days"].Visible =      true;
            grid.Columns["MonthNo"].Visible =   true;
            grid.Columns["YearNo"].Visible =    true;

//            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //grid.ResumeLayout();
            Cursor = saveCursor;
        }
        #endregion

        #region helpers
        /// <summary>
        /// Called from the two Date-changed event-handlers
        /// </summary>
        private void reCalculate()
        {
            if (rdBtnTotPerProj.Checked)
                updateTotalsReport();
            else if (rdBtnTotalPerProjPerEmp.Checked)
                updateTotalPerEmpsReport();
            else if (rdBtnTasksForMe.Checked)
                updatePersonalReport();
            else if (rdBtnTotPerParentProjs.Checked)
                updateTotalsOnParentReport();
            else if (rdBtnDaysAccounted.Checked)
                updateDaysAccountedFor();

            chkMonths.Enabled = (rdBtnTotPerParentProjs.Checked || rdBtnTotPerProj.Checked || rdBtnTotalPerProjPerEmp.Checked);
        }
        #endregion
        /// <summary>
        /// Comma-separated string of values from ListBox - if Checkbox checked
        /// null otherwise
        /// </summary>
        /// <returns></returns>
        private string departments()
        {
            string dept = null;
            if (chkUseDepts.Checked)
            {
                dept = "";
                foreach (object item in lstDepts.SelectedItems)
                    dept += item.ToString() + ",";
                if (dept.Length > 0)
                    dept = dept.Substring(0, dept.Length - 1);
            }
            return dept;
        }

        #region eventhandlers

        /// <summary>
        /// Event-Handler. A new user has been selected for the personalized report
        /// GUI tightened so that this event only can happen when legal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            employee = (DBUtil.Employee)lstUsers.SelectedItem;
            reCalculate();
        }

        /// <summary>
        /// EventHandler. Called when a new project is selected as filter
        /// GUI tightened so that this event only can happen when legal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstMyOnly_SelectedIndexChanged(object sender, EventArgs e)
        {
            reCalculate();
        }

        /// <summary>
        /// Event-Handler. User just finished changing the "To" Date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtTo_CloseUp(object sender, EventArgs e)
        {
            reCalculate();
        }

        /// <summary>
        /// Event-Handler. User just finished changing the "From" Date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFrom_CloseUp(object sender, EventArgs e)
        {
            reCalculate();
        }

        /// <summary>
        /// COMMON Event-Handler. Assures that unvoluntary Mouse-wheel movements not affects list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void List_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        /// <summary>
        /// Eventhandler. Called when a Personal Report is requested
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdBtnTasksForMe_CheckedChanged(object sender, EventArgs e)
        {
            lstMyOnly.Enabled = rdBtnTasksForMe.Checked;
            lstUsers.Enabled = rdBtnTasksForMe.Checked && model.SuperUser;

            if (((RadioButton)sender).Checked)
            {
                lblCategories.Enabled = cmbCategories.Enabled = false;
                reCalculate();
            }
        }

        /// <summary>
        /// Event-Handler. Called when user has (de)selected the overall sum per project report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdBtnTotPerProj_CheckedChanged(object sender, EventArgs e)
        {
            lstDepts.Enabled = chkUseDepts.Enabled = rdBtnTotPerProj.Checked;

            if (((RadioButton)sender).Checked)
            {
                lblCategories.Enabled = cmbCategories.Enabled = true;
                reCalculate();
            }
        }

        /// <summary>
        /// Event-Handler. Called when user has (de)selected the Sum per employee per project report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdBtnTotalPerProjPerEmp_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                lblCategories.Enabled = cmbCategories.Enabled = true;
                reCalculate();
            }
        }

        /// <summary>
        /// Event-Handler for radio-button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdBtnTotPerParentProjs_CheckedChanged(object sender, EventArgs e)
        {
            lstDepts.Enabled = chkUseDepts.Enabled = rdBtnTotPerParentProjs.Checked;

            if (((RadioButton)sender).Checked)
            {
                lblCategories.Enabled = cmbCategories.Enabled = true;
                reCalculate();
            }
        }
        /// <summary>
        /// Event-Handler for radio-button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdBtnDaysAccounted_CheckedChanged(object sender, EventArgs e)
        {
            lstDepts.Enabled = chkUseDepts.Enabled = rdBtnDaysAccounted.Checked;

            if (((RadioButton)sender).Checked)
            {
                lblCategories.Enabled = cmbCategories.Enabled = false;
                reCalculate();
            }
        }

        private void chkUseDepts_CheckedChanged(object sender, EventArgs e)
        {
            reCalculate();
        }

        private void lstDepts_SelectedIndexChanged(object sender, EventArgs e)
        {
            reCalculate();
        }

        private void grid_SelectionChanged(object sender, EventArgs e)
        {
            float sum = 0;

            foreach (DataGridViewRow row in grid.SelectedRows)
            {
                if (rdBtnDaysAccounted.Checked)
                    sum += (int) row.Cells["Days"].Value;
                else
                    sum += (float) row.Cells["Hours"].Value;
            }

            txtSum.Text = sum.ToString();
        }

        private void cmbCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            reCalculate();
        }
        #endregion

    }
}
