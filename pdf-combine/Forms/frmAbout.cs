using System;
using System.Windows.Forms;

namespace pdf_combine.Forms
{
    /// <summary>
    /// this form displays information about the program
    /// </summary>
    public partial class frmAbout : Form
    {
        /// <summary>
        /// main form ctor
        /// </summary>
        public frmAbout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// handles the close button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
