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
    public partial class Info : Form
    {
        public Info(string headline, string bread)
        {
            InitializeComponent();

            lblHeader.Text = headline;
            this.Text = "Missing Dates"; 
            txtBread.Text = bread;
            txtBread.Select(0, 0);
            Size size = TextRenderer.MeasureText(txtBread.Text, txtBread.Font);
            txtBread.Width = size.Width;
            txtBread.Height = size.Height+10; // needed to add a little to avoid cutting last line!
        }

        private void ModeLessInfo_Load(object sender, EventArgs e)
        {
            this.Icon = TimeReg.Properties.Resources.clock;
        }

    }
}
