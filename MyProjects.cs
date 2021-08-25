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
    internal partial class MyProjects : Form
    {
        private Model model;
        private List<DBUtil.ProjTupple> myProjects = null;
        private List<DBUtil.ProjTupple> allProjects = null;

        Color dirty = Color.LightGreen;
        Color clean = Color.White;


        private int LastTipItem = -1;

        /// <summary>
        /// Via the Model and DBUtil - fill the lstboxes
        /// </summary>
        /// <param name="model"></param>
        public MyProjects(TimeReg.Model model)
        {
            this.model = model;
            InitializeComponent();
        }


        private void MyProjects_Load(object sender, EventArgs e)
        {
            rdAllProjects.Checked = true;

            lblHelp.Text = "Removing a Project from 'MyProjects' deletes NO data. You can always add it again to have it on your input sheet.\r\n" +
                "TIP: Sort on 'Category' in the left table.";

            lstMyProjs.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            lstMyProjs.DrawItem += new DrawItemEventHandler(this.DrawItemHandler);

            toolTip1.SetToolTip(btnExit, "No OK/Submit needed");
            toolTip1.SetToolTip(btnToMyProjs, "Add selected Project(s) to MyProjects (auto-submitted)");
            toolTip1.SetToolTip(btnFromMyProjs, "Remove selected Project(s) from MyProjects (auto-submitted)");
            toolTip1.SetToolTip(txtSearch, "Ignores case. Hit if string is contained in any field.\r\n- except 'Category' which must match on all chars.");
            this.Icon = TimeReg.Properties.Resources.clock;

            updateAllProjects();

            myProjects = DBUtil.myProjects(model.MyID);
            if (myProjects.Count == 0)
            {
                // Add Vacation and Sickness as defaults
                foreach (DBUtil.ProjTupple item in allProjects)
                    if (item.ParentID == -1)
                        DBUtil.insertIntoMyProjects(item.ProjectID, model.MyID);

            }

            updateMyProjects();
        }

        /// <summary>
        /// Update "My Projects"
        /// </summary>
        public void updateMyProjects()
        {
            myProjects = DBUtil.myProjects(model.MyID);
            lstMyProjs.DataSource = myProjects;
            lstMyProjs.DisplayMember = "Name";
            lstMyProjs.Refresh();
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
        /// Update all projects according to Radio-Buttons
        /// </summary>
        public void updateAllProjects()
        {
            allProjects = DBUtil.getAllProjects(false);

            //if (rdAllProjects.Checked)
            if (rdNotTxProjects.Checked)
                allProjects.RemoveAll(project => project.Name.IndexOf("754") == 0);

            if (rdTxProjectsOnly.Checked)
                allProjects.RemoveAll(project => project.Name.IndexOf("754") != 0 && project.ParentID != -1);

            if (!txtSearch.Text.Equals(""))
            {
                string filter = txtSearch.Text.ToLower();
                List<DBUtil.ProjTupple> tempProjects = new List<DBUtil.ProjTupple>();
                tempProjects.AddRange(allProjects.FindAll(x => x.Name.ToLower().Contains(filter) ||
                                                            x.ParentName.ToLower().Contains(filter) ||
                                                            x.PSONumber.ToString().ToLower().Contains(filter) ||
                                                            x.ProjectNo.ToLower().Contains(filter) ||
                                                            x.Category.ToLower().Equals(filter)));
                allProjects = tempProjects;
            }

            SortableBindingList<DBUtil.ProjTupple> sortList = new SortableBindingList<DBUtil.ProjTupple>(allProjects);

            grid.SuspendLayout();

            grid.DataSource = sortList;

            // The columns are ordered by the Public "getters" in the ProjTupple
            grid.Columns["ProjectID"].Visible = false;
            grid.Columns["ParentID"].Visible = false;
            grid.Columns["Name"].Visible = true;
            grid.Columns["Name"].FillWeight = 40;
            grid.Columns["Name"].SortMode = DataGridViewColumnSortMode.Automatic;
            grid.Columns["Manager"].Visible = true;
            grid.Columns["Manager"].FillWeight = 20;
            grid.Columns["Manager"].SortMode = DataGridViewColumnSortMode.Automatic;
            grid.Columns["Closed"].Visible = false;
            grid.Columns["ParentName"].Visible = true;
            grid.Columns["ParentName"].SortMode = DataGridViewColumnSortMode.Automatic;
            grid.Columns["ParentName"].FillWeight = 20;
            grid.Columns["ProjectNo"].Visible = true;
            grid.Columns["ProjectNo"].SortMode = DataGridViewColumnSortMode.Automatic;
            grid.Columns["ProjectNo"].FillWeight = 20;
            grid.Columns["PSONumber"].Visible = true;
            grid.Columns["PSONumber"].SortMode = DataGridViewColumnSortMode.Automatic;
            grid.Columns["PSONumber"].FillWeight = 15;
            grid.Columns["Category"].Visible = true;
            grid.Columns["Category"].SortMode = DataGridViewColumnSortMode.Automatic;
            grid.Columns["Category"].FillWeight = 10;
            grid.Columns["Grouptag"].Visible = false;

            //markParents();
            grid.ResumeLayout();
        }

        #region Events
        /// <summary>
        /// Add the selected projects to the Database "MyProjects"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnToMyProjs_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in grid.SelectedRows)
            {
                DBUtil.ProjTupple item = (DBUtil.ProjTupple ) row.DataBoundItem;
                if (!myProjects.Contains(item))
                    DBUtil.insertIntoMyProjects(item.ProjectID, model.MyID);
            }

            updateMyProjects();
        }

        /// <summary>
        /// Delete the selected project from "My Projects"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFromMyProjs_Click(object sender, EventArgs e)
        {
            foreach (DBUtil.ProjTupple item in lstMyProjs.SelectedItems)
                DBUtil.deleteFromMyProjects(item.ProjectID, model.MyID);

            updateMyProjects();
        }

        /// <summary>
        /// Ownerdraw for both listboxes - assuring that Parent-Projects are bolded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawItemHandler(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;

            e.DrawBackground();

            Font normal = e.Font;

            if (sender == grid)
            {
                Font font = (allProjects[e.Index].ParentID <= 0) ? new Font(normal.Name, normal.Size, FontStyle.Bold) : normal;
                e.Graphics.DrawString(allProjects[e.Index].Name, font, Brushes.Black, e.Bounds, StringFormat.GenericTypographic);
            }
            else
            {
                Font font = (myProjects[e.Index].ParentID <= 0) ? new Font(normal.Name, normal.Size, FontStyle.Bold) : normal;
                e.Graphics.DrawString(myProjects[e.Index].Name, font, Brushes.Black, e.Bounds, StringFormat.GenericTypographic);
            }
            e.DrawFocusRectangle();
        }

        private void Projects_CheckedChanged(object sender, EventArgs e)
        {
            updateAllProjects();
        }

        private void grid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            markParents();
        }


        private void lstMyProjs_MouseMove(object sender, MouseEventArgs e)
        {
            int itemIndex = -1;
            if (allProjects != null)
            {
                if (lstMyProjs.ItemHeight != 0)
                {
                    itemIndex = e.Y / lstMyProjs.ItemHeight;
                    itemIndex += lstMyProjs.TopIndex;
                }
                if ((itemIndex >= 0) && (itemIndex < lstMyProjs.Items.Count))
                {
                    if (itemIndex != LastTipItem)
                    {
                        toolTip1.SetToolTip(lstMyProjs, lstMyProjs.Items[itemIndex].ToString());
                        LastTipItem = itemIndex;
                    }
                }
                else
                {
                    toolTip1.Hide(lstMyProjs);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            updateAllProjects();
            txtSearch.BackColor = (txtSearch.Text.Equals("")) ? clean : dirty;
        }

        #endregion "Events"

    }
}
