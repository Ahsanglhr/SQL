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
    internal partial class Setup : Form
    {
        Model model;

        //public int Database { get; set; }
        public Setup(Model model)
        {
            InitializeComponent();
            this.Icon = TimeReg.Properties.Resources.clock;

            this.model = model;

            // The actual database
            rdBtnRoyston.Checked = model.Catalog.Equals("UK");
            rdBtnGottingen.Checked = model.Catalog.Equals("DE");
            rdBtnNaerum.Checked = model.Catalog.Equals("DK");
            rdBtnGCC.Checked = model.Catalog.Equals("GCC");

            // Make Naerum the default
            if (!rdBtnRoyston.Checked && !rdBtnGottingen.Checked && ! rdBtnGCC.Checked)
                rdBtnNaerum.Checked = true;

            // The holiday scheme
            rdBtnUK.Checked = model.Holidays.Equals("UK");
            rdBtnUS.Checked = model.Holidays.Equals("US");
            rdBtnDK.Checked = model.Holidays.Equals("DK");
            rdBtnDE.Checked = model.Holidays.Equals("DE");

            if (!rdBtnUK.Checked && !rdBtnUS.Checked && !rdBtnDE.Checked)
                rdBtnDK.Checked = true;

            btnOK.Enabled = false;
        }

        #region Events
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (rdBtnNaerum.Checked)
                model.Catalog = "DK";
            else if (rdBtnRoyston.Checked)
                model.Catalog = "UK";
            else if (rdBtnGottingen.Checked)
                model.Catalog = "DE";
            else
                model.Catalog = "GCC";


            if (rdBtnUS.Checked)
                model.Holidays = "US";
            else if (rdBtnUK.Checked)
                model.Holidays = "UK";
            else if (rdBtnDK.Checked)
                model.Holidays = "DK";
            else
                model.Holidays = "DE";
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = true;
        }
        #endregion "Events"

    }
}
