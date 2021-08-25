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
    internal partial class AllProjects : Form
    {
        private Model model;
        private int curID = -1;
        private SortableBindingList<DBUtil.ProjTupple> sortList = null;

        private List<DBUtil.ProjTupple> allProjects = null;

        // A dictionary of superusers based on netname
        Dictionary<string, DBUtil.Employee> managersDict = null;

        Color dirty = Color.LightGreen;
        Color clean = Color.White;
 
        /// <summary>
        /// Create the list with all projects
        /// </summary>
        /// <param name="model"></param>
        public AllProjects(TimeReg.Model model)
        {
            this.model = model;
            InitializeComponent();

            grid.ContextMenuStrip = popupMenuAllProjs;
        }

        /// <summary>
        /// Some stuff must be done here - not in the constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllProjects_Load(object sender, EventArgs e)
        {
            lblWarn.Text = "WARNING! - You are here as a superuser!\r\n" +
                "Projects are created when you have supplied a name.\r\n" +
                "Create projects based on WHAT - not WHO";

            toolTip1.SetToolTip(btnExit, "Exits Dialog. No OK/Submit needed");
            toolTip1.SetToolTip(chkClosed, "Show only closed or open");
            toolTip1.SetToolTip(txtSearch, "Ignores case. Hit if string is contained in any field.\r\n- except 'Category' which must match on all chars.");
            this.Icon = TimeReg.Properties.Resources.clock;

            // Make the dictionary of super-users - key is Netname (allows for null which employeeID doesn't)
            managersDict = DBUtil.getEmployees(true).ToDictionary(x => x.NetName, x => x);
            updateProjects();
        }

        /// <summary>
        /// Initially - and after sorting - we must bold all parent projects
        /// Here it becomes handy that we have a hidden column with ParentID
        /// </summary>
        private void markParents()
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle(grid.RowsDefaultCellStyle);
            style.Font = new System.Drawing.Font(grid.Font, FontStyle.Bold);
            
            foreach (DataGridViewRow row in grid.Rows)
            {
                int parent = (int)row.Cells["ParentID"].Value;

                if (parent <= 0)
                    row.DefaultCellStyle = style;
            }
        }

        /// <summary>
        /// Initially - and after creating new Projects or Closing them - recreate the list
        /// </summary>
        public void updateProjects()
        {
            // Find the current project - if any (there is none first time around)
            curID = -1;
            if (grid != null && grid.CurrentRow != null && grid.CurrentRow.DataBoundItem != null)
            {
                // Now we get the benefit of having the invisible rows - Project ID is nice and unique
                DBUtil.ProjTupple tupple = (DBUtil.ProjTupple)grid.CurrentRow.DataBoundItem;
                curID = tupple.ProjectID;
            }

            DBUtil.fillAllProjects();

            // Now we can select all closed (true) or all open (false) projects
            allProjects = DBUtil.getAllProjects(chkClosed.Checked);

            if (!txtSearch.Text.Equals(""))
            {
                string filter = txtSearch.Text.ToLower();
                List<DBUtil.ProjTupple> tempProjects = new List<DBUtil.ProjTupple>();
                tempProjects.AddRange(allProjects.FindAll(  x => x.Name.ToLower().Contains(filter) ||
                                                            x.ParentName.ToLower().Contains(filter) || 
                                                            x.PSONumber.ToString().ToLower().Contains(filter) ||
                                                            x.ProjectNo.ToLower().Contains(filter)  ||
                                                            x.Grouptag.ToLower().Contains(filter) ||
                                                            x.Manager.ToLower().Contains(filter) ||
                                                            x.Category.ToLower().Equals(filter)));
                allProjects = tempProjects;
            }

            sortList = new SortableBindingList<DBUtil.ProjTupple>(allProjects);

            grid.SuspendLayout();

            Color backcol = (chkClosed.Checked) ? Color.LightGray : Color.White;

            grid.DataSource = sortList;

            grid.Columns["ProjectID"].Visible = false;
            grid.Columns["ParentID"].Visible = false;
            grid.Columns["Name"].Visible = true;
            grid.Columns["Name"].SortMode = DataGridViewColumnSortMode.Automatic;
            grid.Columns["Name"].DefaultCellStyle.BackColor = backcol;
            grid.Columns["Name"].FillWeight = 40;
            grid.Columns["Manager"].Visible = !this.model.Catalog.Equals("UK"); // Do not edit in Royston
            grid.Columns["Manager"].SortMode = DataGridViewColumnSortMode.Automatic;
            grid.Columns["Manager"].DefaultCellStyle.BackColor = backcol;
            grid.Columns["Manager"].FillWeight = 20;
            grid.Columns["Closed"].Visible = false;
            grid.Columns["ParentName"].Visible = true;
            grid.Columns["ParentName"].SortMode = DataGridViewColumnSortMode.Automatic;
            grid.Columns["ParentName"].DefaultCellStyle.BackColor = backcol;
            grid.Columns["ParentName"].FillWeight = 20;
            grid.Columns["ProjectNo"].Visible = true;
            grid.Columns["ProjectNo"].SortMode = DataGridViewColumnSortMode.Automatic;
            grid.Columns["ProjectNo"].DefaultCellStyle.BackColor = backcol;
            grid.Columns["ProjectNo"].FillWeight = 20;
            grid.Columns["PSONumber"].Visible = true;
            grid.Columns["PSONumber"].SortMode = DataGridViewColumnSortMode.Automatic;
            grid.Columns["PSONumber"].DefaultCellStyle.BackColor = backcol;
            grid.Columns["PSONumber"].FillWeight = 15;
            grid.Columns["Category"].Visible = true;
            grid.Columns["Category"].SortMode = DataGridViewColumnSortMode.Automatic;
            grid.Columns["Category"].DefaultCellStyle.BackColor = backcol;
            grid.Columns["Category"].FillWeight = 10;
            grid.Columns["Grouptag"].Visible = model.Catalog.Equals("DK");
            grid.Columns["Grouptag"].SortMode = DataGridViewColumnSortMode.Automatic;
            grid.Columns["Grouptag"].FillWeight = 15;
            grid.Columns["Grouptag"].DefaultCellStyle.BackColor = backcol;

           // markParents();

            if (curID != -1)
            {
                int index = allProjects.FindIndex(x => (x.ProjectID == curID)); // We love lampda expressions!
                // If the project was closed just before it cannot be found now
                if (index != -1)
                    grid.CurrentCell = grid.Rows[index].Cells[2];  // Cell index must be visible
            }


            grid.ResumeLayout();
        }


        /// <summary>
        /// Add the child Project
        /// </summary>
        /// <param name="selItem"></param>
        private void addSublvlProject(DBUtil.ProjTupple selItem)
        {
            int parentID = selItem.ProjectID;
            string parentName = selItem.Name; 
            int grandpaID = selItem.ParentID;
            string parentNo = selItem.ProjectNo;
            int parentPsoNumber = selItem.PSONumber;
            string category = selItem.Category;
            string manager = managersDict.FirstOrDefault(x => x.Value.FullName.Equals(selItem.Manager)).Key; // null is possible

            if (grandpaID == 0)
            {
                AddProject dialog = new AddProject(allProjects);
                dialog.ProjectName = parentName;                            // Use parent Name
                dialog.EnableProjectName = true;                            // Allow it to change (dialog will insist)
                dialog.PSONo = parentPsoNumber;                             // Use parent PSO number
                dialog.EnablePSONo = false;                                 // .. and do not allow a change
                dialog.ProjectNo = parentNo;                                // Use parent project no
                dialog.EnableProjectNo = false;                             // .. and do not alllow a change
                dialog.Category = category;                                 // Use parent category
                dialog.EnableCategory = true;                               // A sub-project CAN have another category than the parent
                dialog.Grouptag = selItem.Grouptag;                         // We use the parent Grouptag
                dialog.EnableGrouptag = false;                              // .. and do not allow it to change (directly)
                dialog.Manager = (manager != null) ? manager : model.Me;    // If original selection was blank, set current user as default manager
                dialog.EnableManager = model.iMayDoThis("Projects");        // Allow administrator to change manager
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // There is no way the Manager cannot be OK here
                    DBUtil.insertIntoAllProjects(dialog.ProjectName, parentID, parentNo, parentPsoNumber, category, managersDict[dialog.Manager].EmployeeID);
                    updateProjects();
                }
            }
            else if (grandpaID > 0)
                MessageBox.Show("Sorry, only one level of parent allowed!", parentName+ " has a Parent");
            else
                MessageBox.Show("Sorry, no children of " + parentName + " allowed", "Illegal Parent");

        }

        /// <summary>
        /// To be used by several events
        /// </summary>
        private void addToplvlProject()
        {
            AddProject dlg = new AddProject(allProjects);
            dlg.EnablePSONo = true;                                         // Allow the user to set almost everything on new projects
            dlg.EnableCategory = true;
            dlg.EnableGrouptag = model.iMayDoThis("GroupTag");
            dlg.EnableManager = model.iMayDoThis("Projects");
            dlg.Manager = model.Me;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DBUtil.insertIntoAllProjects(dlg.ProjectName, 0, dlg.ProjectNo, dlg.PSONo, dlg.Category, managersDict[dlg.Manager].EmployeeID);
                updateProjects();
            }
        }

        #region Events

        /// <summary>
        /// This is called by the grid after sorting is done. All formatting was removed. 
        /// Redo the formatting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //grid.SuspendLayout();
            markParents();
            //grid.ResumeLayout();
        }

        /// <summary>
        /// Add a Child Project Menu Item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addChildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grid != null && grid.CurrentRow != null && grid.CurrentRow.DataBoundItem != null)
            {

                DBUtil.ProjTupple tupple = (DBUtil.ProjTupple)grid.CurrentRow.DataBoundItem;
                addSublvlProject(tupple);
            }
        }

        /// <summary>
        /// Rename the project. New name must be unique
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grid != null && grid.CurrentRow != null && grid.CurrentRow.DataBoundItem != null)
            {

                DBUtil.ProjTupple tupple = (DBUtil.ProjTupple)grid.CurrentRow.DataBoundItem;

                AddProject dialog = new AddProject(allProjects);
                dialog.ProjectName = tupple.Name;
                dialog.EnableProjectName = true;
                dialog.PSONo = tupple.PSONumber;
                dialog.EnablePSONo = false;
                dialog.ProjectNo = tupple.ProjectNo;
                dialog.EnableProjectNo = false;
                dialog.Category = tupple.Category;
                dialog.EnableCategory = false;
                dialog.Grouptag = tupple.Grouptag;
                dialog.EnableGrouptag = false;
                dialog.Manager = managersDict.FirstOrDefault(x => x.Value.FullName.Equals(tupple.Manager)).Key;
                dialog.EnableManager = false;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    DBUtil.renameProject(tupple.ProjectID, dialog.ProjectName, tupple.ProjectNo, tupple.PSONumber, tupple.Category, tupple.Grouptag);
                    updateProjects();
                }
            }
        }

        /// <summary>
        /// Called before showing right-click menu-item "Add Child".
        /// Used to enable this for only real parents
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addChildToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            if (chkClosed.Checked)
            {
                addChildToolStripMenuItem.Enabled = false;
                return;
            }

            if (grid != null && grid.CurrentRow != null && grid.CurrentRow.DataBoundItem != null)
            {
                DBUtil.ProjTupple tupple = (DBUtil.ProjTupple)grid.CurrentRow.DataBoundItem;
                addChildToolStripMenuItem.Enabled = (tupple.ParentID == 0 && (tupple.Manager.Equals("") || tupple.Manager.Equals(model.FullName) || model.iMayDoThis("Projects")));
            }
        }

        /// <summary>
        /// Add a completely new Project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addToplvlMenuItem_Click(object sender, EventArgs e)
        {
            addToplvlProject();
        }

        /// <summary>
        /// Update the ProjectNumber. Same number may be used by several Projects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeProjectNoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grid != null && grid.CurrentRow != null && grid.CurrentRow.DataBoundItem != null)
            {
                DBUtil.ProjTupple tupple = (DBUtil.ProjTupple)grid.CurrentRow.DataBoundItem;

                AddProject dialog = new AddProject(allProjects);
                dialog.ProjectName = tupple.Name;
                dialog.EnableProjectName = false;
                dialog.PSONo = tupple.PSONumber;
                dialog.EnablePSONo = false;
                dialog.ProjectNo = tupple.ProjectNo;
                dialog.EnableProjectNo = true;
                dialog.Category = tupple.Category;
                dialog.EnableCategory = false;
                dialog.Grouptag = tupple.Grouptag;
                dialog.EnableGrouptag = false;
                dialog.Manager = managersDict.FirstOrDefault (x => x.Value.FullName.Equals(tupple.Manager)).Key;
                dialog.EnableManager = false;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    DBUtil.renameProject(tupple.ProjectID, tupple.Name, dialog.ProjectNo, tupple.PSONumber, tupple.Category, tupple.Grouptag);

                    // Change the number of the children as well!
                    foreach (DBUtil.ProjTupple child in allProjects)
                        if (child.ParentID == tupple.ProjectID)
                            DBUtil.renameProject(child.ProjectID, child.Name, dialog.ProjectNo, tupple.PSONumber, tupple.Category, tupple.Grouptag);

                    updateProjects();
                }
            }
        }

        /// <summary>
        /// Method for adding a PSONumber to an existing project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changePSONoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grid != null && grid.CurrentRow != null && grid.CurrentRow.DataBoundItem != null)
            {
                DBUtil.ProjTupple tupple = (DBUtil.ProjTupple)grid.CurrentRow.DataBoundItem;

                AddProject dialog = new AddProject(allProjects);
                dialog.ProjectName = tupple.Name;
                dialog.EnableProjectName = false;
                dialog.PSONo = tupple.PSONumber;
                dialog.EnablePSONo = true;
                dialog.ProjectNo = tupple.ProjectNo;
                dialog.EnableProjectNo = false;
                dialog.Category = tupple.Category;
                dialog.EnableCategory = false;
                dialog.Grouptag = tupple.Grouptag;
                dialog.EnableGrouptag = false;
                dialog.Manager = managersDict.First(x => x.Value.FullName.Equals(tupple.Manager)).Key;
                dialog.EnableManager = false;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    DBUtil.renameProject(tupple.ProjectID, tupple.Name, tupple.ProjectNo, dialog.PSONo, tupple.Category, tupple.Grouptag);

                    // Change the number of the children as well!
                    foreach (DBUtil.ProjTupple child in allProjects)
                        if (child.ParentID == tupple.ProjectID)
                            DBUtil.renameProject(child.ProjectID, child.Name, tupple.ProjectNo, dialog.PSONo, tupple.Category, tupple.Grouptag);

                    updateProjects();
                }
            }
        }

        /// <summary>
        /// As above -  for Category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grid != null && grid.CurrentRow != null && grid.CurrentRow.DataBoundItem != null)
            {

                DBUtil.ProjTupple tupple = (DBUtil.ProjTupple)grid.CurrentRow.DataBoundItem;

                AddProject dialog = new AddProject(allProjects);
                dialog.ProjectName = tupple.Name;
                dialog.EnableProjectName = false;
                dialog.PSONo = tupple.PSONumber;
                dialog.EnablePSONo = false;
                dialog.ProjectNo = tupple.ProjectNo;
                dialog.EnableProjectNo = false;
                dialog.Category = tupple.Category;
                dialog.EnableCategory = true;
                dialog.Grouptag = tupple.Grouptag;
                dialog.EnableGrouptag = false;
                dialog.Manager = managersDict.FirstOrDefault(x => x.Value.FullName.Equals(tupple.Manager)).Key;
                dialog.EnableManager = false;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    DBUtil.renameProject(tupple.ProjectID, tupple.Name, tupple.ProjectNo, tupple.PSONumber, dialog.Category, tupple.Grouptag);

                    // Change the number of the children as well!
                    foreach (DBUtil.ProjTupple child in allProjects)
                        if (child.ParentID == tupple.ProjectID)
                            DBUtil.renameProject(child.ProjectID, child.Name, tupple.ProjectNo, dialog.PSONo, tupple.Category, tupple.Grouptag);

                    updateProjects();
                }
            }            
        }


        /// <summary>
        /// The (parent) project is closed. Close the children too.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeForEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("This will disallow further entries in this project and all it's children",
                "Close Project", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (res == DialogResult.OK)
            {
                if (grid != null && grid.CurrentRow != null && grid.CurrentRow.DataBoundItem != null)
                {

                    DBUtil.ProjTupple tupple = (DBUtil.ProjTupple)grid.CurrentRow.DataBoundItem;

                    DBUtil.closeProject(tupple);
                    updateProjects();
                }
            }
        }

        /// <summary>
        /// To grey or not to grey - Close Project
        /// This is only legal on parent projects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeForEntryToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            if (chkClosed.Checked)
            {
                closeForEntryToolStripMenuItem.Enabled = false;
                return;
            }

            if (grid != null && grid.CurrentRow != null && grid.CurrentRow.DataBoundItem != null)
            {
                DBUtil.ProjTupple tupple = (DBUtil.ProjTupple)grid.CurrentRow.DataBoundItem;
                closeForEntryToolStripMenuItem.Enabled = (tupple.Manager.Equals(model.FullName) || model.iMayDoThis("Projects"));
            }
        }

        /// <summary>
        /// To grey or not to grey - Change Project Number
        /// This is only legal on parent projects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeProjectNoToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            if (chkClosed.Checked)
            {
                changeProjectNoToolStripMenuItem.Enabled = false;
                return;
            }

            if (grid != null && grid.CurrentRow != null && grid.CurrentRow.DataBoundItem != null)
            {
                DBUtil.ProjTupple tupple = (DBUtil.ProjTupple)grid.CurrentRow.DataBoundItem;
                changeProjectNoToolStripMenuItem.Enabled = (tupple.ParentID == 0 && (tupple.Manager.Equals(model.FullName) || model.iMayDoThis("Projects")));
            }
        }

        /// <summary>
        /// As above - but now for PSO-Numbers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changePSONoToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            if (chkClosed.Checked)
            {
                changePSONoToolStripMenuItem.Enabled = false;
                return;
            }

            if (grid != null && grid.CurrentRow != null && grid.CurrentRow.DataBoundItem != null)
            {
                DBUtil.ProjTupple tupple = (DBUtil.ProjTupple)grid.CurrentRow.DataBoundItem;
                changePSONoToolStripMenuItem.Enabled = (tupple.ParentID == 0 && (tupple.Manager.Equals(model.FullName) || model.iMayDoThis("Projects")));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeCategoryToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            if (chkClosed.Checked)
            {
                changeCategoryToolStripMenuItem.Enabled = false;
                return;
            }

            if (grid != null && grid.CurrentRow != null && grid.CurrentRow.DataBoundItem != null)
            {
                DBUtil.ProjTupple tupple = (DBUtil.ProjTupple)grid.CurrentRow.DataBoundItem;
                changeCategoryToolStripMenuItem.Enabled = (tupple.Manager.Equals(model.FullName) || model.iMayDoThis("Projects"));
            }
        }

        /// <summary>
        /// Rename a project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void renameToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            if (chkClosed.Checked)
            {
                renameToolStripMenuItem.Enabled = false;
                return;
            }

            if (grid != null && grid.CurrentRow != null && grid.CurrentRow.DataBoundItem != null)
            {
                DBUtil.ProjTupple tupple = (DBUtil.ProjTupple)grid.CurrentRow.DataBoundItem;
                renameToolStripMenuItem.Enabled = (tupple.Manager.Equals(model.FullName) || model.iMayDoThis("Projects"));
            }
        }

        
        /// <summary>
        /// Add new Project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addToplvlMenuItem_Paint(object sender, PaintEventArgs e)
        {
            if (chkClosed.Checked)
            {
                addToplvlMenuItem.Enabled = false;
                return;
            }
        }

        /// <summary>
        /// We have a click in the grid.
        /// If it's not in a header and is a right-click we want to present a popup-menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                grid.Rows[e.RowIndex].Selected = true;
                grid.CurrentCell = grid[e.ColumnIndex, e.RowIndex];
            }
        }

        /// <summary>
        /// After sorting on columns, the user might want to get back to start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetSort_Click(object sender, EventArgs e)
        {
            SortableBindingList<DBUtil.ProjTupple> sortList = new SortableBindingList<DBUtil.ProjTupple>(allProjects);

            grid.DataSource = sortList;
        }


        /// <summary>
        /// Helper-event. Gets the information about a clicked cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // We only want to do something on a rowheader-click - but an event on that does not do the trick
            if (e.RowIndex != -1)
                return;

            curID = -1;
            if (grid != null && grid.CurrentRow != null && grid.CurrentRow.DataBoundItem != null)
            {
                // Now we get the benefit of having the invisible rows - Project ID is nice and unique
                DBUtil.ProjTupple tupple = (DBUtil.ProjTupple)grid.CurrentRow.DataBoundItem;
                curID = tupple.ProjectID;
            }
        }

        /// <summary>
        /// Point current and selected to the row that was selected before
        /// The trick is NOT the code - but to choose the right event to use it in
        /// - I had no luch using the BindingCompleted as the current (but not selected) index was overwritten "somewhere" afterwards.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_Sorted(object sender, EventArgs e)
        {
            for (int i = 0; i < grid.RowCount; i++)
            {
                DataGridViewRow row = grid.Rows[i];
                if ((int)row.Cells["ProjectID"].Value == curID)
                {
                    grid.CurrentCell = grid["Name", i];
                    break;
                }
            }
        }

        /// <summary>
        /// Start a filtering of the current projects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            updateProjects();
            txtSearch.BackColor =  (txtSearch.Text.Equals("")) ? clean : dirty;
        }


        /// <summary>
        /// Show the projects that are closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkClosed_CheckedChanged(object sender, EventArgs e)
        {
            updateProjects();
        }


        /// <summary>
        /// The user requests to be manager on this project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void assignMeAsManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grid != null && grid.CurrentRow != null && grid.CurrentRow.DataBoundItem != null)
            {
                // Now we get the benefit of having the invisible rows - Project ID is nice and unique
                DBUtil.ProjTupple tupple = (DBUtil.ProjTupple)grid.CurrentRow.DataBoundItem;

                if (MessageBox.Show("Are you the Project Manager of " + tupple.Name, "Assume Ownership", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // Set manager on this project and its children
                    DBUtil.setManager(tupple.ProjectID, model.MyID);

                    updateProjects();
                }
            }
        }

        /// <summary>
        /// Assign the current user as project manager for the selected parent and its children
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void assignMeAsManagerToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            if (chkClosed.Checked || this.model.Catalog.Equals("UK"))
            {
                assignMeAsManagerToolStripMenuItem.Enabled = false;
                return;
            }

            if (grid != null && grid.CurrentRow != null && grid.CurrentRow.DataBoundItem != null)
            {
                // Now we get the benefit of having the invisible rows - Project ID is nice and unique
                DBUtil.ProjTupple tupple = (DBUtil.ProjTupple)grid.CurrentRow.DataBoundItem;
                assignMeAsManagerToolStripMenuItem.Enabled = (tupple.Manager.Equals("") && tupple.ParentID == 0);
            }
        }

        /// <summary>
        /// User wants to change the GroupTag. Only allowed for a very few people
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeGrouptagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grid != null && grid.CurrentRow != null && grid.CurrentRow.DataBoundItem != null)
            {
                DBUtil.ProjTupple tupple = (DBUtil.ProjTupple)grid.CurrentRow.DataBoundItem;

                AddProject dialog = new AddProject(allProjects);
                dialog.ProjectName = tupple.Name;
                dialog.EnableProjectName = false;
                dialog.PSONo = tupple.PSONumber;
                dialog.EnablePSONo = false;
                dialog.ProjectNo = tupple.ProjectNo;
                dialog.EnableProjectNo = false;
                dialog.Category = tupple.Category;
                dialog.EnableCategory = false;
                dialog.Grouptag = tupple.Grouptag;
                dialog.EnableGrouptag = model.iMayDoThis("GroupTag");
                dialog.Manager = (tupple.Manager != null) ? managersDict.FirstOrDefault(x => x.Value.FullName.Equals(tupple.Manager)).Key : null;
                dialog.EnableManager = false;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    DBUtil.renameProject(tupple.ProjectID, tupple.Name, tupple.ProjectNo, tupple.PSONumber, tupple.Category, dialog.Grouptag);

                    // Change the tag of the children as well!
                    foreach (DBUtil.ProjTupple child in allProjects)
                        if (child.ParentID == tupple.ProjectID)
                            DBUtil.renameProject(child.ProjectID, child.Name, tupple.ProjectNo, tupple.PSONumber, tupple.Category, dialog.Grouptag);

                    updateProjects();
                }
            }
        }

        /// <summary>
        /// Called before showing "Change GroupTag" - Greyd out for most people
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeGrouptagToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            changeGrouptagToolStripMenuItem.Enabled = model.iMayDoThis("GroupTag");
        }

        /// <summary>
        /// A very few people are allowed to change an existing Project Manager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangePMToolstripMenuItem_Click(object sender, EventArgs e)
        {
            if (grid != null && grid.CurrentRow != null && grid.CurrentRow.DataBoundItem != null)
            {
                DBUtil.ProjTupple tupple = (DBUtil.ProjTupple)grid.CurrentRow.DataBoundItem;

                AddProject dialog = new AddProject(allProjects);
                dialog.ProjectName = tupple.Name;
                dialog.EnableProjectName = false;
                dialog.PSONo = tupple.PSONumber;
                dialog.EnablePSONo = false;
                dialog.ProjectNo = tupple.ProjectNo;
                dialog.EnableProjectNo = false;
                dialog.Category = tupple.Category;
                dialog.EnableCategory = false;
                dialog.Grouptag = tupple.Grouptag;
                dialog.EnableGrouptag = false; ;
                dialog.Manager = managersDict.FirstOrDefault(x => x.Value.FullName.Equals(tupple.Manager)).Key;
                dialog.EnableManager = model.iMayDoThis("Projects");


                if (dialog.ShowDialog() == DialogResult.OK && dialog.Manager != null)
                {
                    int newID = managersDict[dialog.Manager].EmployeeID;
                    // Set manager on this project and its children
                    DBUtil.setManager(tupple.ProjectID, newID);

                    updateProjects();
                }
            }
        }

        /// <summary>
        /// Very few people are allowed to change Project Manager - but more are allowed to take on a blank...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangePMToolstripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            ChangePMToolstripMenuItem.Enabled = model.iMayDoThis("Projects");
        }

        #endregion "Events"
    }
}
