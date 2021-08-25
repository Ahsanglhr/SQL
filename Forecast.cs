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
using System.Reflection;


namespace TimeReg
{
    public partial class Forecast : Form
    {
        Model model = null;
        private Color dirty = Color.LightGreen;
        private Color clean = Color.White;

        bool suspendAutoUpdate = false;
//        List<DBUtil.Employee> employeesList = null;
        Dictionary<int, DBUtil.Employee> employeesDict = null;
        DataGridViewComboBoxColumn projectsCombo = null;
        DataGridViewComboBoxColumn employeesCombo = null;
        List<DBUtil.ForecastLine> forecast = null;
        List<DBUtil.ForecastLine> deletedForecast = null;
        DBUtil.ForecastLine employeeTotal = null;

        int fixedColumns = 3;                       // Columns for Project, Employee and Task
        int indexOfProject = 0;
        int indexOfEmployee = 1;
        int indexOfTaskDescr = 2;

        int firstYear = 2017;
        int firstMonth = 1;

        int lastYear = 2018;
        int lastMonth = 12;

        int currentEmployeeID = -1;

        // Enable parsing of float numbers with local comma/point
        private CultureInfo culture = CultureInfo.CurrentCulture;    // As set in Control Panel - Region and Language
        private NumberStyles style = NumberStyles.AllowDecimalPoint; // Disallows the "thousands separator"

        public Forecast()
        {
            this.Icon = TimeReg.Properties.Resources.clock;

            model = Model.getInstance(false);  

            this.WindowState = FormWindowState.Maximized;

            InitializeComponent();

            // Setup default start and end years
            txtFirstYear.Text = firstYear.ToString();
            txtLastYear.Text = lastYear.ToString();

            toolTip1.SetToolTip(txtFirstYear, "First full year included");
            toolTip1.SetToolTip(txtLastYear, "Last full year included");
            toolTip1.SetToolTip(btnUpdate, "Use this initially and to clear all unsaved edits");
            toolTip1.SetToolTip(btnSubmit, "Use this to submit all changes to the database");

            // Create the combo-box columns needed in the grid
            projectsCombo = new DataGridViewComboBoxColumn();
            projectsCombo.HeaderText = "Project";
            projectsCombo.Name = "cmbProjects";

            // Lampda is also practical for sorting
            List<DBUtil.ProjTupple> allProjects = DBUtil.getAllProjects(false);
            allProjects.Sort((first, second) => first.Name.CompareTo(second.Name));

            projectsCombo.DataSource = allProjects;
            projectsCombo.DisplayMember = "Name";
            projectsCombo.ValueMember = "ProjectID";
            projectsCombo.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing; // Needed to get the "dirty" background when we get there


            employeesCombo = new DataGridViewComboBoxColumn();
            employeesCombo.HeaderText = "Employee";
            employeesCombo.Name = "cmbEmployees";
            employeesCombo.DataSource = DBUtil.getEmployees(false);
            employeesCombo.DisplayMember = "FullName";
            employeesCombo.ValueMember = "EmployeeID";
            employeesCombo.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing; // Needed to get the "dirty" background when we get there

            // TODO: Do this in the model under init
            employeesDict = DBUtil.getEmployees(false).ToDictionary(x => x.EmployeeID, x => x);

            // Attach a right-click popup-menu to the grid (see menu-items in designer)
            grid.ContextMenuStrip = this.cntxtMnuDGV;
            //grid.AllowUserToAddRows = false;

            // Attach events for the above emnu-items
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            pasteCTRLVToolStripMenuItem.Click += pasteCTRLVToolStripMenuItem_Click;
            pasteCTRLVToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+V";
            copyCTRLCToolStripMenuItem.Click += copyCTRLCToolStripMenuItem_Click;
            copyCTRLCToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+C";

            deleteToolStripMenuItem.ToolTipText = "Deletes are also temporary until submitted";

            // Speed-up performance by less autosizing
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            grid.EnableHeadersVisualStyles = false;

            rdBtnEmployeesFirst.Checked = false;
            rdBtnProjectsFirst.Checked = true;

            List<DBUtil.ProjTupple> allProjectsWAll = new List<DBUtil.ProjTupple>(allProjects);
            allProjectsWAll.Insert(0, new DBUtil.ProjTupple(-1, "All Projects", -1, "", false, "", 0, "NON", "", "ALL"));
            cmbProjects.DataSource = allProjectsWAll;
            cmbProjects.DisplayMember = "Name";
            cmbProjects.ValueMember = "ProjectID";

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

        private void Forecast_Load(object sender, EventArgs e)
        {
            readFromDB();
        }


        /// <summary>
        /// Convert the multiple selected items to a strin of ProjectID's 
        /// </summary>
        /// <returns></returns>
        private string projects()
        {
            string projs = "";
            foreach (var item in cmbProjects.SelectedItems)
            {
                DBUtil.ProjTupple project = (DBUtil.ProjTupple)item;
                projs += project.ProjectID.ToString() + ",";
            }
            if (projs.Length > 0)
                projs = projs.Substring(0, projs.Length - 1);
            return projs;
        }


        /// <summary>
        /// Anytime that we want to refresh the grid with data from the database this is called
        /// </summary>
        private void readFromDB()
        {
            // We will fill in data programmatically - so skip events that help the user to do it manually
            suspendAutoUpdate = true;

            // Clear the content of the grid - if any
            grid.ColumnCount = 0;
            gridTotal.ColumnCount = 0;


            // Get a forecast - a list of ForecastLine's - each with a number of ForecastCellData in Months collection
            forecast = DBUtil.createForecast(firstYear * 100 + firstMonth, lastYear * 100 + lastMonth, projects(), rdBtnProjectsFirst.Checked);
            deletedForecast = new List<DBUtil.ForecastLine>();


            // Apparantly the grid always has a single column
            grid.Columns.Insert(indexOfProject, projectsCombo);
            grid.Columns.Insert(indexOfEmployee, employeesCombo);

            //gridTotal.Columns.Insert(indexOfProject, new DataGridViewColumn());
            //gridTotal.Columns.Insert(indexOfEmployee, new DataGridViewColumn());

            // Adding all the Months and The textDescription
            // Use static method here - the view may be empty!
            grid.ColumnCount += 1 + DBUtil.ForecastLine.noOfColumns(firstYear * 100 + firstMonth, lastYear * 100 + lastMonth);
            grid.Columns[indexOfTaskDescr].Name = "Task";
            grid.Columns[indexOfTaskDescr].SortMode = DataGridViewColumnSortMode.NotSortable;



            // Generate header names for the months and format the columns
            for (int col = fixedColumns; col < grid.ColumnCount; col++)
            {
                grid.Columns[col].Name = DBUtil.ForecastLine.indexToText(firstYear * 100 + firstMonth, col - fixedColumns);     //forecast[0].indexToText(col - fixedColumns);
               // grid.Columns[col].ValueType = typeof(float); // DON'T do this; inhibits intelligent parsing of floats
                grid.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                grid.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                grid.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            grid.RowCount = forecast.Count + 1;           // Include header
            grid.Columns[fixedColumns-1].Frozen = true;

            //gridTotal.Rows[0].Cells[indexOfProject].Value = "All Projects";
            //gridTotal.Rows[0].Cells[indexOfTaskDescr].Value = "Committed";

            // That was the formatting and Dropdowns.
            // Now for the forecast data.
            // Most cells in the model forecast are probably null values - to be treated as 0.0 Effort
            int rowCount = 0;
            foreach (DBUtil.ForecastLine line in forecast)
            {
                grid.Rows[rowCount].Cells[indexOfProject].Value = forecast[rowCount].ProjectID;  // We use the ID's but show the Text
                grid.Rows[rowCount].Cells[indexOfEmployee].Value = forecast[rowCount].EmployeeID;
                grid.Rows[rowCount].Cells[indexOfTaskDescr].Value = forecast[rowCount].TaskDescr;

                for (int col = 0; col < forecast[0].Columns; col++)
                    grid.Rows[rowCount].Cells[col + fixedColumns].Value = (forecast[rowCount].Months[col] == null) ? 0.0F : forecast[rowCount].Months[col].Effort;
                rowCount++;
            }


            // Now build the grid for the totals of the current Employee - MUST be done after init of the std grid.
            gridTotal.ColumnCount = grid.ColumnCount;

            gridTotal.Columns[indexOfProject].Name = "Project";
            gridTotal.Columns[indexOfEmployee].Name = "Employee";
            gridTotal.Columns[indexOfTaskDescr].Name = "Task";

            for (int col = fixedColumns; col < grid.ColumnCount; col++)
            {
                gridTotal.Columns[col].Name = grid.Columns[col].Name;
                gridTotal.Columns[col].AutoSizeMode = grid.Columns[col].AutoSizeMode;
                gridTotal.Columns[col].DefaultCellStyle.Alignment = grid.Columns[col].DefaultCellStyle.Alignment;
                gridTotal.Columns[col].SortMode = grid.Columns[col].SortMode;
            }

            gridTotal.RowCount = 1;
            gridTotal.Columns[fixedColumns - 1].Frozen = true;

            suspendAutoUpdate = false; // Now that the programmed update is done - start utilizing events

            btnSubmit.BackColor = clean;
        }

        /// <summary>
        /// Read data from the database and flatten it into the grid - (re)building it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtFirstYear.Text, out firstYear) && int.TryParse(txtLastYear.Text, out lastYear) && firstYear <= lastYear)
            {
                readFromDB();
            }
            else
                MessageBox.Show("Error parsing FirstYear or LastYear");
        }

        /// <summary>
        /// User wants to submit data
        /// Use the forecast data model to do the work
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //First delete the forecasts that are already "deleted" in the UI
            foreach (DBUtil.ForecastLine line in deletedForecast)
            {
                line.delete();
            }

            // Then update/insert/delete as needed
            foreach (DBUtil.ForecastLine line in forecast)
            {
                line.writeToDB();
            }

            DBUtil.updateDates(forecast[0].getHighestDate());
            
            btnSubmit.BackColor = clean;

            // Refresh the grid from the base
            readFromDB();
        }

        /// <summary>
        /// A cell has been changed. Skip the rest if the flag tells us it is part of our own code.
        /// If continuing and the cell is in the Months section we must update or create a single entry.
        /// If the user has changed any of the "fixed" columns it affects all cells in the line
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!suspendAutoUpdate)
            {
                suspendAutoUpdate = true;

                int row = e.RowIndex;
                int col = e.ColumnIndex;

                // We can go below current last line if needed - create a line in the "model" behind
                if (row >= forecast.Count)
                    createNewLine();

                // Columns are treated differently from each other
                if (col >= fixedColumns)
                {
                    // We are in the months section - read the float
                    float val;
                    string input = grid.Rows[row].Cells[col].Value.ToString();
                    if (!float.TryParse(input, style, culture, out val))
                    {
                        MessageBox.Show("Please use the number-format setup via\n\r'Region and Language' in Control Panel",
                            "Illegal Number-format");
                        val = 0.0f;

                        grid.Rows[row].Cells[col].Value = "0";
                    }

                    // Are we dealing with an existing cell or a new one?
                    // TODO - consider moving these cases into ForecastLine and simply "set" the effort
                    if (forecast[row].Months[col - fixedColumns] != null)
                    {
                        // An existing cell
                        forecast[row].Months[col - fixedColumns].Effort = val;
                    }
                    else
                    {
                        // A new cell means TaskID = -1 
                        // we need to put in CalMonth
                        forecast[row].createNewCell(col - fixedColumns, val);
                    }

                }
                else // One of the three first columns are changed - this will affect all cells
                {
                    if (col == indexOfProject)
                    {
                        // We set the ID - but we see the text due to the combo-box nature of this column
                        int projectID;
                        bool ok = int.TryParse(grid.Rows[row].Cells[col].Value.ToString(), out projectID);
                        forecast[row].ProjectID = projectID;
                    }
                    else if (col == indexOfEmployee)
                    {
                        // We set the ID - but we see the text due to the combo-box nature of this column
                        int employeeID;
                        bool ok = int.TryParse(grid.Rows[row].Cells[col].Value.ToString(), out employeeID);
                        forecast[row].EmployeeID = employeeID;

                    }
                    else if (col == indexOfTaskDescr)
                    {
                        // Simple text
                        forecast[row].TaskDescr = grid.Rows[row].Cells[col].Value.ToString();
                    }

                    // All cells are updated with the contents from the leftmost columns
                    forecast[row].updateCellsWithLineInfo();

                }

                // Mark the dirty cell (but not the month-ones affected by changes to the leftmost columns
                grid.Rows[row].Cells[col].Style.BackColor = dirty;
                btnSubmit.BackColor = dirty;

                suspendAutoUpdate = false;
            }
        }

        /// <summary>
        /// The GUI creates an empty dummy line if the user moves downwards. When the user starts editing we need to fill relevant data into it.
        /// The default values are copying the bottom line Project and Employee
        /// </summary>
        private void createNewLine()
        {
            suspendAutoUpdate = true;

            int projectID;
            int employeeID;
            string taskDescr = "Task Description";

            // Use latest entry if there is one
            if (forecast.Count > 0)
            {
                projectID = forecast[forecast.Count - 1].ProjectID;
                employeeID = forecast[forecast.Count - 1].EmployeeID;
            }
            else //- or take a legal one from the base
            {
                projectID = DBUtil.projectsDict.First(x => x.Value.Name.Equals("Vacation")).Key;
                employeeID = employeesDict.ElementAt(0).Key;
            }

            // Now apply the "best defaults" into the grid
            grid.Rows[grid.RowCount - 1].Cells[indexOfProject].Value = projectID;  // We use the ID's but show the Text
            grid.Rows[grid.RowCount - 1].Cells[indexOfEmployee].Value = employeeID;
            grid.Rows[grid.RowCount - 1].Cells[indexOfTaskDescr].Value = taskDescr;

            // Add a line to the model behind it all
            forecast.Add(new DBUtil.ForecastLine(firstYear * 100 + firstMonth, lastYear * 100 + lastMonth, projectID, employeeID, taskDescr));
            btnSubmit.BackColor = dirty;

            suspendAutoUpdate = false; ;
        }

        /// <summary>
        /// Event from the UI when the user went into the bottom empty line
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            createNewLine();
        }

        /// <summary>
        /// Save the content of the Grid in a CSV-file for excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            const string sep = ";";
            const string stStr = "=\"";
            const string endStr = "\"";

            System.Windows.Forms.SaveFileDialog saveDlg = new SaveFileDialog();
            string fileName;

            saveDlg.AddExtension = true;
            saveDlg.Filter = "Comma Separated Values (*.csv)|*.csv|All files (*.*)|*.*";
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                fileName = saveDlg.FileName;
                try
                {   // Unicode to support Danish chars!
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, false, Encoding.Unicode))
                    {
                        file.WriteLine("sep=" + sep);

                        // Write a header 
                        file.Write(stStr + "Grouptag" + endStr + sep);
                        foreach (DataGridViewColumn col in grid.Columns)
                            file.Write(stStr + col.HeaderText + endStr + sep);

                        file.WriteLine();  // newline

                        // Now the data
                        // Use ProjectsDict and employeesDict for names. Easy to extend later with Manager, OracleID etc
                        foreach (DataGridViewRow row in grid.Rows)
                        {
                            int gridCol = 0; int id;


                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                string val;
                                if (gridCol == indexOfProject)
                                {
                                    val = (cell != null && cell.Value != null && int.TryParse(cell.Value.ToString(), out id)) ? DBUtil.projectsDict[id].Grouptag : "";
                                    file.Write(stStr + val + endStr + sep);
                                    val = (cell != null && cell.Value != null && int.TryParse(cell.Value.ToString(), out id)) ? DBUtil.projectsDict[id].Name : "";
                                    file.Write(stStr + val + endStr + sep);
                                }
                                else if (gridCol == indexOfEmployee)
                                {
                                    val = (cell != null && cell.Value != null && int.TryParse(cell.Value.ToString(), out id)) ? employeesDict[id].FullName : "";
                                    file.Write(stStr + val + endStr + sep);
                                }
                                else
                                {
                                    val = (cell != null && cell.Value != null) ? cell.Value.ToString() : "";
                                    file.Write(val.ToString() + sep);
                                }
                                gridCol++;
                            }
                            file.WriteLine();
                        }
                    }
                }
                catch (System.IO.IOException exc)
                {
                    MessageBox.Show("Illegal Action Saving file: " + exc.Message, "File Save error");
                }
            }
        }

        /// <summary>
        /// The Clipboard Text is interpreted as coming from excel - also works when it comes from the grid itself
        /// </summary>
        /// <param name="inputStr"></param>
        private void pasteText(string inputStr)
        {
            // Where do we start?
            int row         = grid.CurrentCell.RowIndex;
            int firstCol    = grid.CurrentCell.ColumnIndex;

            // Columns are split with \t and rows with \r\n
            string[] rowDelim = new string[] { "\r\n" };        // An array is expected - we use only one element
            string[] colDelim = new string[] { "\t" };

            try
            {
                // Split up in lines first
                string[] lines = inputStr.Split(rowDelim, StringSplitOptions.RemoveEmptyEntries);


                foreach (string line in lines)
                {
                    // Every line starts at the same column as this is a "square-paste"
                    int col = firstCol;
                    // Now split up in columns
                    string[] fields = line.Split(colDelim, StringSplitOptions.None);

                    // We don't want to start if it brings us over the edge
                    if (fields.Length + col > grid.ColumnCount)
                    {
                        MessageBox.Show("Sorry - the paste will extend past last column");
                        return;
                    }

                    // However - If needed - add a row to the UI
                    if (lines.Length + row > grid.RowCount)
                    {
                        if (firstCol != indexOfProject || firstCol + fields.Length < indexOfTaskDescr)
                        {
                            MessageBox.Show("Sorry - pasting below current last line only legal if it includes Project & Employee");
                            return;
                        }
                        // ToDo: Manually add empty forecast
                        grid.Rows.Add();
                    }

                    // Now for the actual paste
                    foreach (string field in fields)
                    {
                        try
                        {
                            if (col == indexOfProject)
                                grid.Rows[row].Cells[col].Value = DBUtil.projectsDict.First(x => x.Value.Name.Equals(field)).Key; // May throw an exception
                            else if (col == indexOfEmployee)
                                grid.Rows[row].Cells[col].Value = employeesDict.First(x => x.Value.FullName.Equals(field)).Key;
                            else
                                grid.Rows[row].Cells[col].Value = (field.Equals("")) ? "0" : field;
                        }
                        catch (InvalidOperationException)
                        {
                            MessageBox.Show("Field: '"+field.ToString()+"' was not found in the database", "Non-matching Project or Employee");
                            return;
                        }

                        col++;
                    }
                    row++;
                }

                btnSubmit.BackColor = dirty;

            }
            catch (InvalidOperationException e)
            {
                MessageBox.Show("Sorry - cannot interpret data from clipboard\r\n"+e.Message, "Illegal Format");
            }
        }



        /// <summary>
        /// KeyDown Event inside the grid
        /// Handle:
        ///     CTRL-V Means Paste input from Clipboard - expecting Excel data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                if (Clipboard.ContainsText())
                {
                    string oneStr = Clipboard.GetText();
                    pasteText(oneStr);                   
                }

                e.Handled = true;
            }
        }

        private void cntxtMnuDGV_Opening(object sender, CancelEventArgs e)
        {
            pasteCTRLVToolStripMenuItem.Enabled = Clipboard.ContainsText();
        }
        
        /// <summary>
        /// Quickest way to do a Copy to clipboard is to amulate CTRL-C entered!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void copyCTRLCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendKeys.Send("^C");
        }


        /// <summary>
        /// The past action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pasteCTRLVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                string oneStr = Clipboard.GetText();
                pasteText(oneStr);
            }
        }

        /// <summary>
        /// Things only happen at the SUBMIT - so when a line is "deleted" it is kept in another list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int row = grid.CurrentCell.RowIndex;
            //int col = grid.CurrentCell.ColumnIndex;

            // Clear contents and remove from collection
            deletedForecast.Add(forecast[row]);
            forecast.RemoveAt(row);

            grid.Rows.RemoveAt(row);
            btnSubmit.BackColor = dirty;
        }

        private void deleteToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            deleteToolStripMenuItem.Enabled = grid.CurrentRow.Selected;
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        /// <summary>
        /// When the user moved to another cell we want to display the committed sums of the current employee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (suspendAutoUpdate)
                return;

            int row = e.RowIndex;
            int employeeID = (int)grid.Rows[row].Cells[indexOfEmployee].Value;

            if (employeeID != currentEmployeeID)
            {
                currentEmployeeID = employeeID;

                employeeTotal = DBUtil.createTotalsForEmployee(firstYear * 100 + firstMonth, lastYear * 100 + lastMonth, employeeID);
                //gridTotal.Rows[0].Cells[indexOfEmployee].Value = employeesDict[employeeID].FullName;

                // Of the fixed columns, 2 have fixed text. The third is the Employeename - lookup in the dictionary
                gridTotal.Rows[0].Cells[indexOfProject].Value = "Committed";  // We use the ID's but show the Text
                gridTotal.Rows[0].Cells[indexOfEmployee].Value = employeesDict[employeeID].FullName;
                gridTotal.Rows[0].Cells[indexOfTaskDescr].Value = employeeTotal.TaskDescr;

                // Data into the months
                for (int col = 0; col < employeeTotal.Columns; col++)
                    gridTotal.Rows[0].Cells[col + fixedColumns].Value = (employeeTotal.Months[col] == null) ? 0.0F : employeeTotal.Months[col].Effort;
            }

        }

        /// <summary>
        /// When the user resizes the main grid columns - the sum-grid should follow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (suspendAutoUpdate)
                return;

            for (int col = 0; col < employeeTotal.Columns + fixedColumns; col++)
                gridTotal.Columns[col].Width = grid.Columns[col].Width;
        }

        /// <summary>
        /// When the user scrolls or resizes the main grid - the sumline should follow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_Scroll(object sender, ScrollEventArgs e)
        {
            gridTotal.HorizontalScrollingOffset = grid.HorizontalScrollingOffset;
        }

        private void Forecast_ResizeEnd(object sender, EventArgs e)
        {
            gridTotal.HorizontalScrollingOffset = grid.HorizontalScrollingOffset;
        }
    }
}
