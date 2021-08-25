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

    internal partial class BulkEdit : Form
    {
        private Model model = null;

        public BulkEdit(Model model)
        {
            this.model = model;
            this.Icon = TimeReg.Properties.Resources.clock;
            InitializeComponent();
        }

        private void BulkEdit_Load(object sender, EventArgs e)
        {
            this.Text = "Add Vacation or Leave";
            toolTip.SetToolTip(dtFromDate, "First Day of Vacation or Leave");
            toolTip.SetToolTip(dtToDate, "Last Day of Vacation or Leave");
            toolTip.SetToolTip(txtComment, "Comment for all days");
            toolTip.SetToolTip(btnOK, "Submit Vacation/Leave");
        }

        /// <summary>
        /// Update Database from here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            DateTime date = dtFromDate.Value.Date;
         
            int projID = DBUtil.projectsDict.First(x => x.Value.Name.Equals("Vacation")).Key;

            string truncStr = DBUtil.truncComment(txtComment.Text);

            while (date <= dtToDate.Value.Date)
            {
                string name;
                if (!model.holidayDict.TryGetValue(date, out name) && date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    int mainID = DBUtil.getMainRowID(model.MyID, date, projID);
                    if (mainID == -1)
                        DBUtil.insertNewMainRow(model.MyID, date, projID, (float)8.0, truncStr);
                    else
                        DBUtil.updateExistingMainRow(mainID, (float)8.0, truncStr);
                }
                date = date.AddDays(1);
            }
        }
    }
}
