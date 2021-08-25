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
    public partial class InputDlg : Form
    {
        public InputDlg()
        {
            InitializeComponent();
            this.Icon = TimeReg.Properties.Resources.clock;
        }

        /// <summary>
        /// The Question in the main window of the Form
        /// </summary>
        public string Question 
        {
            set { lblQuestion.Text = value; }
            get { return lblQuestion.Text; }
        }

        /// <summary>
        /// The answer from the user
        /// </summary>
        public string Answer
        {
            get { return txtAnswer.Text; }
            set { txtAnswer.Text = value; }
        }

        /// <summary>
        /// The Caption of the form
        /// </summary>
        public string Titel
        {
            set { this.Text = value; }
        }
    }
}
