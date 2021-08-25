using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;


/// What's with the icons?
/// There are two clock icons:
/// 1) The clock icon in the properties folder is used within the program - in the dialogs
///    It is also the one shown in the task bar.
/// 2) The clock icon in the main folder is used in the explorer - with the exe file
///
/// The following strange stuff happened due to the "Icon Cache" - cured by a reboot:
/// When copying a link to the desktop, (1) is used - IF (2) exists!
/// This happens only when I choose to see medium size or large size icons - not small ones as these use (2) - which I think is correct.
/// As far as I can see all sizes needed are included in (2) - while (1) only has one size (128x128, 32 bit).


namespace TimeReg
{
    internal partial class UserView : Form
    {
        // Reference to the single instance of the Model
        private Model model = null;

        private CultureInfo culture = CultureInfo.CurrentCulture;    // As set in Control Panel - Region and Language
        //private NumberStyles style = NumberStyles.AllowDecimalPoint; // Disallows the "thousands separator"

        private bool suspendCellValueChanged = false;               // Set this to jump out of CellValueChanged Event ASAP

        private Color dirty    = Color.LightGreen;
        private Color holliday = Color.LightGray;
        private Color workday  = Color.White;
        private Color readOnly = Color.LightGray;

        private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        // This is a reference to the table in the Model, through which GUI and DB communicate
        //private Model.CellData[][] localTable=null;
        private List<DBUtil.Week> localList = null;

        private static System.Windows.Forms.Timer exitTimer = new System.Windows.Forms.Timer();

        private static bool timeout = false;

        private DataGridViewCell cellEdit = null;

        Info missingDates = null;

        #region Init
        /// <summary>
        /// Constructor calls designer-code
        /// </summary>
        public UserView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Init the GUI AND the Model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserView_Load(object sender, EventArgs e)
        {
            this.Icon = TimeReg.Properties.Resources.clock;

            model = Model.getInstance(false);  // Also initializes Model  set to true to debug

            SetupGrid();

            this.Text = "Time-Registration - Current User: " + model.Me;

            // Use this instead og SelectionIndexChanged as that fired by itself!
            tlstrpCmbUsers.ComboBox.SelectionChangeCommitted += new EventHandler(ToolStripCombobox_selected);

            aliasToolStripMenuItem.ToolTipText = "Allow a superuser to impersonate someone else in order to get the data in\r\nNo unsubmitted data allowed!";
            useDebugDBToolStripMenuItem.ToolTipText = "Only for developers\r\nNo unsubmitted data allowed!";
            myProjectsToolStripMenuItem.ToolTipText = "Select the projects to input data on.\r\nNo unsubmitted data allowed!";
            allProjectsToolStripMenuItem.ToolTipText = "Allow a superuser to create new projects";
            toolTip1.SetToolTip(btnSubmit, "Save the current week to database");
            toolTip1.SetToolTip(btnReset, "Forget the entries made, and refetch from database");
            toolTip1.SetToolTip(btnNextWeek, "Go to next week (CTRL+RightArrow)");
            toolTip1.SetToolTip(btnPrevWeek, "Go to previous week (CTRL+LeftArrow)");

            compiledToolStripMenuItem.Text = "Compiled: "+ fileDate();
            this.compiledToolStripMenuItem.Click += new System.EventHandler(this.compiledToolStripMenuItem_Click);
            this.jirabugToolStripMenuItem.Click += new System.EventHandler(this.compiledToolStripMenuItem_Click);
            compiledToolStripMenuItem.ToolTipText = "Click to copy CompileDate and Version to ClipBoard";
            jirabugToolStripMenuItem.ToolTipText = "Click to start a Jira Bug";
            jiraEnhtoolStripMenuItem.ToolTipText = "Click to start Jira Enhancement";

            lblInfo.BackColor = Color.Red;
            model.CurrentMonday = Model.mondayThisWeek();

            databaseChanged();

            // Closedown after 8 hours to allow updates
            exitTimer.Tick += new EventHandler(OnTimedEvent);
            exitTimer.Interval = 8*60*60*1000;
            exitTimer.Enabled = true;
            exitTimer.Start();
        }

        /// <summary>
        /// Whenever the database is changed we must
        /// 1) Connect to the new database 
        /// 2) Refill the cached list of AllProjects in the Model
        /// 3) Refresh the local cache of MyProjects
        /// 4) Update the info field
        /// 5) Update the grid
        /// 6) Update the Reset/Submit buttons
        /// </summary>
        private void databaseChanged()
        {
            model.connect();    // Refreshes AllProjects and myID
            updateInfo();       // Refreshes MyProjects
            if (model.SuperUser)
            {                   // Refreshes Employees - now excl SuperUsers
                tlstrpCmbUsers.DropDownStyle = ComboBoxStyle.DropDownList;
                List<DBUtil.Employee> employees = DBUtil.getEmployees(false);

                // Unless I am allowed to take the alias of a superuser - remove superusers from the list
                if (!model.iMayDoThis("AliasSuper"))
                    employees.RemoveAll(x => x.SuperUser);

                tlstrpCmbUsers.ComboBox.DataSource = employees;
                tlstrpCmbUsers.ComboBox.DisplayMember = "FullName";
                tlstrpCmbUsers.ComboBox.ValueMember = "NetName";
                tlstrpCmbUsers.ComboBox.BindingContext = this.BindingContext;
            }
           // Refreshes data from main-table
            UpdateGridFromTable(model.CurrentMonday);
            updateButtons();
            grid.Focus();
        }

        /// <summary>
        /// One-time Setup of the grid
        /// It will always have 8 columns - Projects + 7 weekdays
        /// Do not sort as this will confuse the mapping to the localTable
        /// </summary>
        private void SetupGrid()
        {
            grid.ColumnCount = 8;
            grid.Columns[0].Name = "Project";
            grid.Columns[1].Name = "Monday";
            grid.Columns[2].Name = "Tuesday";
            grid.Columns[3].Name = "Wednesday";
            grid.Columns[4].Name = "Thursday";
            grid.Columns[5].Name = "Friday";
            grid.Columns[6].Name = "Saturday";
            grid.Columns[7].Name = "Sunday";

            // Change the headers of weekends and Projects
            grid.EnableHeadersVisualStyles = false;
            grid.Columns["Project"].HeaderCell.Style.BackColor = Color.LightGray;
            grid.Columns["Saturday"].HeaderCell.Style.BackColor = Color.LightGray;
            grid.Columns["Sunday"].HeaderCell.Style.BackColor = Color.LightGray;

            foreach (DataGridViewColumn col in grid.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            grid.ShowCellToolTips = true;
        }
        #endregion "Init"

        #region Helpers
        /// <summary>
        /// Utility routine - called from several places
        /// Update the Submit & Reset buttons according to status
        /// </summary>
        private void updateButtons()
        {
            btnReset.Enabled = btnSubmit.Enabled = model.Dirty;
            btnSubmit.BackColor = (model.Dirty) ? dirty : btnReset.BackColor;
        }

        /// <summary>
        /// This method writes hints in the lower field for beginners
        /// </summary>
        private void updateInfo()
        {
            if (model.MyID == 0)
                lblInfo.Text = "Please goto Edit-Basic Setup and select the base you are registered in";
            else
            {
                List<DBUtil.ProjTupple> myProjects = DBUtil.myProjects(model.MyID);
                if (myProjects.Count == 0)
                    lblInfo.Text = "Please go to Edit-My Projects and select your relevant projects";

                // Put in Tips on new stuff here and update the end time as practical
                else if (DateTime.Today < new DateTime(2017, 5, 10))
                {
                    lblInfo.Text = "Tip: If your colleague cannot login he/she needs to use a LINK to this program!";
                    lblInfo.BackColor = this.BackColor;  // Make it neutral
                }
                else // Default
                {
                    lblInfo.Text = "";
                }
            }
        }


        /// <summary>
        /// Calculates an array of strings that represent the bottom summing line in the sheet
        /// </summary>
        /// <returns></returns>
        private string[] calcSumRow(bool sumRowExists)
        {
            float[] sum = new float[8];         // One sum per day (and total in index 0)
            string[] ssum = new string[8];      // One text per day (and one in index 0)
            int rowcount = grid.RowCount;
            if (sumRowExists)
                rowcount--;

            sum[0] = 0;                         // Clear the total

            // Run through the weekdays (skip the project column)
            for (int colInx = 1; colInx < grid.ColumnCount; colInx++)
            {
                sum[colInx] = 0;                // Clear the sum
                // Run through the rows (header does not count as a row) - and do NOT include the last row, as this is what we calculate
                for (int rowInx = 0; rowInx < rowcount; rowInx++)
                {
                    sum[colInx] += localList[rowInx][colInx-1].Hours; // Now using pre-calculated values
                }
                ssum[colInx] = sum[colInx].ToString();  // Write the week-sum

                sum[0] += sum[colInx];          // Add todays sum to the week-total
            }

            ssum[0] = "Sum: "+sum[0].ToString();

            return ssum;   
        }

        /// <summary>
        /// Move back to previous Week - based on event
        /// </summary>
        private void prevWeek()
        {
            if (model.Dirty)
                if (MessageBox.Show("Do you really want to lose what you typed in?", "Unsubmitted Data!", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

            // Store knowledge on current Project
            string currentProject = "";
            int currentRow = -1;
            if (grid.CurrentCell != null)
                currentRow = grid.CurrentCell.RowIndex;

            if (currentRow < localList.Count && currentRow >= 0)
                currentProject = localList[currentRow].ProjectName;

            // Move to previous week
            DateTime currentMonday = model.CurrentMonday.AddDays(-7);
            UpdateGridFromTable(currentMonday);
            updateButtons();
            grid.Focus();
            
            // Set current Project - respecting that number of projects may change
            for (currentRow = 0; currentRow < localList.Count; currentRow++)
            {
                if (localList[currentRow].ProjectName.Equals(currentProject))
                {
                    grid.CurrentCell = grid.Rows[currentRow].Cells["Friday"]; // Go to friday as we are going back
                }
            }
        }

        /// <summary>
        /// Go to next week - based on event
        /// </summary>
        private void nextWeek()
        {
            if (model.Dirty)
                if (MessageBox.Show("Do you really want to lose what you typed in?", "Unsubmitted Data!", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

            // Store knowledge on current Project
            string currentProject = "";
            int currentRow = -1;
            if (grid.CurrentCell != null)
                currentRow = grid.CurrentCell.RowIndex;

            if (currentRow < localList.Count && currentRow >= 0)
                currentProject = localList[currentRow].ProjectName;

            // Move to next week
            DateTime currentMonday = model.CurrentMonday.AddDays(7);
            UpdateGridFromTable(currentMonday);
            updateButtons();
            grid.Focus();

            // Set current Project - respecting that number of projects may change
            for (currentRow = 0; currentRow < localList.Count; currentRow++)
            {
                if (localList[currentRow].ProjectName.Equals(currentProject))
                {
                    grid.CurrentCell = grid.Rows[currentRow].Cells["Monday"]; // Go to monday as we are going forward
                }
            }
        }

        #endregion "Helpers"

        /// <summary>
        /// Assuming that the local
        /// </summary>
        /// <param name="monday"></param>
        private void UpdateGridFromTable(DateTime monday)
        {
            // We do not want to mess with the database if the user is not verified
            if (model.MyID == 0)
                return;

            // Avoid re-entrancy issues - set flag to "suspend" event
            suspendCellValueChanged = true;

            // Fill the local "shadow" via the model
            int weekno;
            localList = model.selectWeekOf(monday, out weekno);
            lblWeekNo.Text = "Week: " + weekno.ToString() + " (EU/ISO)";

            // Start a new week
            grid.Rows.Clear();

            int gridRowCount;

            // Setup the headers - even if there are no data, we need to see the dates to navigate
            // Use weekday-name in general - but holliday if it exists
            grid.Columns[0].HeaderText = "Project";

            DateTime day = monday;
            for (int i = 0; i < 7; i++)
            {
                string name;
                if (model.holidayDict.TryGetValue(day, out name))
                {
                    // Use name of holliday and Paint grey
                    grid.Columns[i + 1].HeaderCell.Style.BackColor = holliday;
                }
                else
                {
                    // Use weekday name and Paint normal - remembering weekends
                    if (i < 5)
                        grid.Columns[i + 1].HeaderCell.Style.BackColor = workday;
                    name = day.DayOfWeek.ToString();
                }

                grid.Columns[i + 1].HeaderText = name + "\n" + day.ToShortDateString();
                day = day.AddDays(1);
            }

            if (localList.Count() == 0)
            {
                grid.Columns[0].HeaderText = "No Data";
                suspendCellValueChanged = false;
                return;
            }

            Font bold = new Font(grid.DefaultCellStyle.Font.Name, grid.DefaultCellStyle.Font.Size, FontStyle.Bold);

            // Don't AutoSize when updating the grid
            DataGridViewAutoSizeColumnsMode AutoSizeColMode = grid.AutoSizeColumnsMode;
            DataGridViewAutoSizeRowsMode AutoSizeRowsMode = grid.AutoSizeRowsMode;

            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // Loop through the rows - each is a project
            foreach (DBUtil.Week projectWeek in localList)
            {
                if (projectWeek == null)
                    break;

                string[] row = new string[8];
                // Very first column is ProjectID - should be the same for all cells - pick any one
                int parentID = projectWeek[0].ParentID;

                // First Column is Project Name
                row[0] = projectWeek.ProjectName;

                // Go through the weekdays - setting Hours
                // Starting in Column 1 - leaving the Project Names in Column 0
                for (int dayInx = 1; dayInx < 8; dayInx++)
                {
                    float hours = projectWeek[dayInx - 1].Hours;   
                    row[dayInx] = (hours == 0.0) ? "" : hours.ToString();
                }

                // Now put it in the grid
                grid.Rows.Add(row);
                gridRowCount = grid.Rows.Count;

                // Mark the parents bold
                if (parentID <= 0)
                   // grid.Rows[grid.RowCount - 1].Cells[0].Style.BackColor = Color.Yellow;
                    grid.Rows[grid.RowCount - 1].Cells[0].Style.Font = bold;

                // Go through the weekdays - setting Tooltips to Comments
                // And make read-only if value is transferred to Oracle
                // .. or if the day is a non-working day and the project is absence
                for (int dayInx = 1; dayInx < 8; dayInx++)
                {
                    grid.Rows[gridRowCount - 1].Cells[dayInx].ToolTipText = projectWeek[dayInx - 1].Comment;
                    if (projectWeek[dayInx - 1].InOracle || (projectWeek[dayInx-1].ParentID < 0 &&  grid.Columns[dayInx].HeaderCell.Style.BackColor == holliday))
                    {
                        grid.Rows[gridRowCount - 1].Cells[dayInx].ReadOnly = true;
                        grid.Rows[gridRowCount - 1].Cells[dayInx].Style.BackColor = readOnly;
                    }
                }
            }

            // Calculate AND create a sum row
            grid.Rows.Add(calcSumRow(false));
            grid.Rows[grid.RowCount - 1].ReadOnly = true;
            grid.Rows[grid.RowCount - 1].DefaultCellStyle.BackColor = Color.LightGray;
            grid.Rows[grid.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.LightGray;
            grid.Rows[grid.RowCount - 1].DefaultCellStyle.SelectionForeColor = Color.Black;

            // Size first column according to the text in it
            grid.Columns["Project"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            // Finally - enable autosize again
            grid.AutoSizeRowsMode = AutoSizeRowsMode;
            grid.AutoSizeColumnsMode = AutoSizeColMode;
            // Select the first project, first day
            grid.Rows[0].Cells["Monday"].Selected = true;
            grid.Columns["Project"].ReadOnly = true;

            // allow for the event again
            suspendCellValueChanged = false;
        }


        #region Events
        /// <summary>
        /// Goto Next week. Assure that unsubmitted data is to be lost
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNextWeek_Click(object sender, EventArgs e)
        {
            nextWeek();
        }

        /// <summary>
        /// Goto Prev week. Assure that unsubmitted data is to be lost
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrevWeek_Click(object sender, EventArgs e)
        {
            prevWeek();
        }

        /// <summary>
        /// Update the "shadow" localTable right away - incl. the Changed flag.
        /// Paint the background to show the changed value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!suspendCellValueChanged && grid.CurrentCell != null)
            {
                suspendCellValueChanged = true;

                float val;
                int row = grid.CurrentCell.RowIndex;
                int col = grid.CurrentCell.ColumnIndex;

                // If user deletes entries and then moves with arrows we have a null value
                if (grid.CurrentCell.Value == null || ((string)grid.CurrentCell.Value).Length == 0)
                {
                    grid.CurrentCell.Value = "";
                    val = 0;
                }

                else if (grid.CurrentCell.Value.ToString().Contains(':'))
                {
                    // Treat as Hours:Minutes
                    string fullTxt = grid.CurrentCell.Value.ToString();
                    string[] tokens = fullTxt.Split(':');
                    if (tokens.Length != 2)
                    {
                        MessageBox.Show("When using ':' operator please supply <Hours>:<Minutes> and nothing else",
                            "Illegal Number-format");
                        grid.CurrentCell.Value = localList[row][col - 1].Hours.ToString();
                        suspendCellValueChanged = false;
                        return;
                    }
                    int hours, minutes;
                    if (!int.TryParse(tokens[0], out hours) || !int.TryParse(tokens[1], out minutes) || minutes >= 60)
                    {
                        MessageBox.Show("When using ':' operator, hours and minutes must be integers\r\n" +
                        " - and minutes must be less than 60",
                            "Illegal Number-format");
                        grid.CurrentCell.Value = localList[row][col - 1].Hours.ToString();
                        suspendCellValueChanged = false;
                        return;
                    }
                    val = (float)hours + (float)minutes / (float)60;
                }

                else
                {
                    // Allow people to use , or . as they please
                    string invStr = grid.CurrentCell.Value.ToString().Replace(',', '.');
                    if (!float.TryParse(invStr,NumberStyles.Any, CultureInfo.InvariantCulture, out val))
                    {
                        MessageBox.Show("Cannot parse: "+ grid.CurrentCell.Value.ToString(),
                            "Illegal Number-format");
                        grid.CurrentCell.Value = localList[row][col - 1].Hours.ToString();
                        suspendCellValueChanged = false;
                        return;
                    }
                }
                // If the user has fiddled around - and made no change in the end - bail out
                if (Math.Abs(localList[row][col - 1].Hours - val) < 0.01)
                {
                    suspendCellValueChanged = false;
                    return;
                }

                localList[row][col - 1].Hours = val;
                localList[row][col - 1].Changed = true;
                grid.CurrentCell.Style.BackColor = dirty;

                string [] ssum = calcSumRow(true);

                for (int colInx = 0; colInx < grid.ColumnCount; colInx++)
                    grid.Rows[grid.RowCount - 1].Cells[colInx].Value = ssum[colInx];

                model.Dirty = true;
                updateButtons();
                // Used to be outside this scope!
                suspendCellValueChanged = false;
            }
        }

        /// <summary>
        /// Filter out unwanted input in the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (grid.ContainsFocus)
            {
                if ((e.KeyChar <= '9' && e.KeyChar >= '0')
                    || e.KeyChar == ',' || e.KeyChar == '.' || e.KeyChar == '\b' || e.KeyChar ==':')
                    e.Handled = false;
                else
                    e.Handled = true;
            }
        }

        /// <summary>
        /// Submit! Write the localTable to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // The localTable is already updated - send it to the database
            model.updateDatabase();
            // Give cells "unchanged background" - except the readonly wich is the sums and those in oracle
            foreach (DataGridViewRow row in grid.Rows)
                if (!row.ReadOnly)
                    foreach (DataGridViewCell cell in row.Cells)
                        if (!cell.ReadOnly)
                            cell.Style.BackColor = Color.White;

            updateButtons();
        }

        /// <summary>
        /// This clears the table from any unsubmitted input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            UpdateGridFromTable(model.CurrentMonday);
            updateButtons();
        }

        /// <summary>
        /// Check for a right-click - and display an input-messagebox for a comment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Do not fiddle with the last row with the sums or with the first column or with data exported to Oracle
            if (e.Button == MouseButtons.Right && e.RowIndex < grid.RowCount-1 && e.RowIndex >= 0
                && e.ColumnIndex > 0 && !localList[e.RowIndex][e.ColumnIndex - 1].InOracle)
            {
                InputDlg dlg = new InputDlg();
                dlg.Question = "Please input a Comment";
                dlg.Answer = grid.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText;
                dlg.Titel = "Comment";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    grid.CurrentCell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    grid.CurrentCell.ToolTipText = dlg.Answer;
                    localList[e.RowIndex][e.ColumnIndex - 1].Comment = DBUtil.truncComment(dlg.Answer);
                    localList[e.RowIndex][e.ColumnIndex-1].Changed = true;
                    grid.CurrentCell.Style.BackColor = dirty;
                    model.Dirty = true;
                    updateButtons();
                }
            }
        }

        /// <summary>
        /// Dont allow the user to go in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.Left)
                {
                    prevWeek();
                    e.Handled = true;
                    return;
                }
                if (e.KeyCode == Keys.Right)
                {
                    nextWeek();
                    e.Handled = true;
                    return;
                }
            }

            if (grid.ContainsFocus && e.KeyCode == Keys.Left && grid.CurrentCell.ColumnIndex ==1)           
            {
                e.Handled = true;
                return;
            }
        }

        /// <summary>
        /// Edit MyProjects. If we have no unsubmitted data - do a refresh
        /// (since 2015-01-16 user is not allowed to choose MyProjects with unsubmitted data)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myProjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form dlgProjs = new MyProjects(model);
            dlgProjs.ShowDialog();
            if (!model.Dirty)
                UpdateGridFromTable(model.CurrentMonday);

            updateInfo();
        }

        /// <summary>
        /// Only for Superusers (designated so in the database)
        /// Allows the SuperUser to add projects att top-level or child-level
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allProjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form dlgProjs = new AllProjects(model);
            dlgProjs.ShowDialog();
            if (!model.Dirty)
                UpdateGridFromTable(model.CurrentMonday);
        }

        /// <summary>
        /// The user has clicked on the ability to export. Show the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form dlg = new ReportsDlg(model);
            dlg.ShowDialog();
        }

        /// <summary>
        /// Take an alias if possible. Toggle back if Alias is on already
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aliasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (aliasToolStripMenuItem.Checked)
            {
                model.alias(""); // Revoke the alias
                this.Text = "Time-Registration - Current User: " + model.Me;
                UpdateGridFromTable(model.CurrentMonday);
                aliasToolStripMenuItem.Checked = false;
            }
        }

        /// <summary>
        /// The user has toggled the use of the debug-DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void useDebugDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            model.DebugDB = useDebugDBToolStripMenuItem.Checked = !useDebugDBToolStripMenuItem.Checked;
            databaseChanged();
        }

        /// <summary>
        /// Called before "painting" the menuitem for choosing an Alias
        /// The choice is only legal if we are originally superuser AND there are no unsubmitted data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aliasToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            aliasToolStripMenuItem.Enabled = model.StartupSuper && !model.Dirty;
            //tlstrpCmbUsers.Enabled = (!aliasToolStripMenuItem.Checked && aliasToolStripMenuItem.Enabled);
        }

        /// <summary>
        /// Called before "painting" the MenuItem on Debug DB.
        /// Set a checkmark if the Debug DB is in use - and make the selection possible for only those with the privilege
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void useDebugDBToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {

            this.useDebugDBToolStripMenuItem.Checked = model.DebugDB;
            this.useDebugDBToolStripMenuItem.Enabled = model.iMayDoThis("DebugDB") && !model.Dirty && !aliasToolStripMenuItem.Checked;
        }

        /// <summary>
        /// Super-User is openeing the "Import Users Dialog"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddUsers dlg = new AddUsers();
            dlg.ShowDialog();
        }

        private void basicInputFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Basic Input Form always shows a full week - and always starts in the current week. "+
                            "\r\nThe projects shown are those designated as 'My Projects' - merged with those from earlier "+
                            "that contains data. \n\rHours are simply inserted, and comments can be written via a right click. " +
                            "\n\rNote that Hours may be written as fractions or as Hours:Minutes"+
                            "\r\nThe database is updated when 'Submit' is pressed. Submit as often as you like."+ 
                            "\r\nEntries with 0 hours will be deleted."+
                            "\r\nThe bold projects are parents, and the normal ones are children. ",
                            "Basic Input Form");
        }

        private void projectConceptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Typically we start out with (Parent) Projects such as 'BilboHW' - with a project-number. " +
                            "\r\nAs these progress, we want to have 'Child Projects' - such as 'Bilbo-ESW'. " +
                            "\n\rThey get the same Project-Number as the parent, and can easily be summed. " +
                            "\n\rThe original Parent Project now becomes a 'Misc' container. ",
                            "Project Concept");
        }

        private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Reports are NOT only for Project-Managers. \n\rApart from the summaries " +
                            "it is also possible to create a personal report - including comments. " +
                            "\n\rThis may be practical when e.g. Vacation is calculated - see link to Confluence. ",
                            "Reports");
        }

        /// <summary>
        /// We are in the drop-down-list with users we can "Alias" - and one has been selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
/*
        private void tlstrpCmbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (useDebugDBToolStripMenuItem.Checked)
                return;

            string title = "Time-Registration - Current User: " + model.Me + " alias " + tlstrpCmbUsers.ComboBox.SelectedValue; //dlg.Answer;
            if (model.alias(tlstrpCmbUsers.ComboBox.SelectedValue.ToString()))
            {
                UpdateGridFromTable(model.CurrentMonday);
                updateButtons();
                aliasToolStripMenuItem.Checked = true;
                this.Text = title;
            }
        }
*/
        /// <summary>
        /// Simply get the compile time and show it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void compiledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("Version: " + Application.ProductVersion + "\r\n"+ "Compiled: " + fileDate()); //DateTime.Now.ToShortDateString());
        }

        /// <summary>
        /// Start a Jira bug with build-info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jiraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://Jira/secure/CreateIssueDetails!init.jspa?pid=12762&issuetype=1&environment=Build: " + Application.ProductVersion + " Compiled: " + fileDate());
        }

        /// <summary>
        /// Start a Jira enhancement with Build-info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jiraEnhtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://Jira/secure/CreateIssueDetails!init.jspa?pid=12762&issuetype=4&environment=Build: " + Application.ProductVersion + " Compiled: " + fileDate());
        }

        /// <summary>
        /// Don't let user change My Projects if there are unsubmitted data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myProjectsToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            this.myProjectsToolStripMenuItem.Enabled =  !model.Dirty;
        }

        /// <summary>
        /// Open the Setup dialog - and if changes happen, change database (and holiday list)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void basicSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setup dlg = new Setup(model);
            if (dlg.ShowDialog() == DialogResult.OK)
                databaseChanged();
        }

        /// <summary>
        /// Don't change database unless we are "back to basics"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void basicSetupToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            this.basicSetupToolStripMenuItem.Enabled = (!model.Dirty && !aliasToolStripMenuItem.Checked && !useDebugDBToolStripMenuItem.Checked);
        }

        /// <summary>
        /// This is used instead of the usual "SelectedIndexChanged" as that fired "by itself"!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripCombobox_selected(object sender, EventArgs e)
        {
            string title = "Time-Registration - Current User: " + model.Me + " alias " + tlstrpCmbUsers.ComboBox.SelectedValue; //dlg.Answer;
            if (model.alias(tlstrpCmbUsers.ComboBox.SelectedValue.ToString()))
            {
                UpdateGridFromTable(model.CurrentMonday);
                updateButtons();
                aliasToolStripMenuItem.Checked = true;
                this.Text = title;
            }
        }

        /// <summary>
        /// Close down the application!
        /// Needed to allow for updates of the exe
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void OnTimedEvent(Object source, EventArgs e)
        {
            timeout = true;
            Application.Exit();
        }


        /// <summary>
        /// Help the user not to bail out with unsubmitted data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (model.Dirty && !timeout)
                if (MessageBox.Show("Do you really want to lose what you typed in?", "Unsubmitted Data!", MessageBoxButtons.YesNo) == DialogResult.No)
                    e.Cancel = true;
        }


        private void addUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form dlgAddUser = new AddUser();
            dlgAddUser.ShowDialog();            
        }

        private void bulkEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BulkEdit dlg = new BulkEdit(model);

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                UpdateGridFromTable(model.CurrentMonday);
                grid.Focus();
            }
        }

        private void bulkEditToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            bulkEditToolStripMenuItem.Enabled = !model.Dirty;
        }

        private void helpInConfluenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://confluence/display/INST/How+To+Register+Time");
        }

        /// <summary>
        /// Avoid the default right-click menu and allow for comments in any mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control.GetType() == typeof(DataGridViewTextBoxEditingControl))
            {
                DataGridViewTextBoxEditingControl box = e.Control as DataGridViewTextBoxEditingControl;
                box.ContextMenuStrip = emptyContextMenuStrip;                   // Strange - but it works...
                box.MouseUp -= new MouseEventHandler(this.MouseClickInEdit);    // Remove to assure not firing twice if more edits
                box.MouseUp += new MouseEventHandler(this.MouseClickInEdit);    // Add my handler
                if (sender.GetType() == typeof(DataGridView))
                {
                    cellEdit = grid.CurrentCell;                                // keep for use as HitTest is a pain...
                }
            }
        }

        /// <summary>
        /// Mouse-Event handler for text-box control inside gridView- active when in edit mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseClickInEdit(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right && cellEdit != null)
            {
                InputDlg dlg = new InputDlg();
                dlg.Question = "Please input a Comment";

                dlg.Answer = cellEdit.ToolTipText;
                dlg.Titel = "Comment";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    cellEdit.ToolTipText = dlg.Answer;
                    localList[cellEdit.RowIndex][cellEdit.ColumnIndex - 1].Comment = DBUtil.truncComment(dlg.Answer);
                    localList[cellEdit.RowIndex][cellEdit.ColumnIndex - 1].Changed = true;
                    cellEdit.Style.BackColor = dirty;
                    model.Dirty = true;
                    updateButtons();
                }
            }
        }


        /// <summary>
        /// Go back to this week
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHome_Click(object sender, EventArgs e)
        {
            if (model.Dirty)
                if (MessageBox.Show("Do you really want to lose what you typed in?", "Unsubmitted Data!", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

            model.CurrentMonday = Model.mondayThisWeek();
            UpdateGridFromTable(model.CurrentMonday);
            updateButtons();
            grid.Focus();
        }

        private string fileDate()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(assembly.Location);
            return fileInfo.LastWriteTime.ToShortDateString();
        }

        /// <summary>
        /// This calls the database function that gives a list of all empty non-weekend, non-holliday days
        /// Finally it creates a modeless dialog box, so that the user can keep it open while fixing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkForEmptyDaysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // If there is already an open dialog - close it
            if (missingDates != null && missingDates.CanFocus)
                missingDates.Close();

            int span = 150;
            DateTime now = DateTime.Now;
            List<DateTime> dates = DBUtil.dates(model.MyID, now.AddDays(-span) , now.AddDays(-1), model.holidayDict.Keys.ToList());


            if (dates.Count == 0)
                MessageBox.Show("You are Good! - no missing dates", "Success!");
            else
            {
                string datesStr = "";

                int max = 20;
                int lines = (dates.Count > max) ? max : dates.Count;

                for (int i = 0; i < lines; i++ )
                {
                    datesStr += dates[i].ToLongDateString() + " (" + dates[i].DayOfWeek.ToString() + ")";
                    if (i < lines-1)
                        datesStr += "\r\n";
                }

                missingDates = new Info("First up to "+ max + " missing dates in the latest "+span.ToString()+" days", datesStr);
                missingDates.Show();   // Modeless!
            }
        }

        /// <summary>
        /// Forecasts is a planning tool for superusers only
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void forecastsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form dlg = new Forecast();
            dlg.ShowDialog();
        }


        /// <summary>
        /// The choice of opening the "All Projects" dialog is only enabled for superusers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allProjectsToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            allProjectsToolStripMenuItem.Enabled = model.SuperUser;
        }

        /// <summary>
        /// The choice of opening the "Add User" dialog is only enabled for superusers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addUserToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            addUserToolStripMenuItem.Enabled = model.SuperUser;
        }

        /// <summary>
        /// The choice of opening the "Import Users" dialog is only enabled for super-superusers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importUsersToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            this.importUsersToolStripMenuItem.Enabled = model.iMayDoThis("ImportUsers");
        }


        /// <summary>
        /// The choice of opening the "Forecasts" dialog is only enabled for superusers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void forecastsToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            forecastsToolStripMenuItem.Enabled = model.SuperUser;
        }

    }
        #endregion "Events"
}
