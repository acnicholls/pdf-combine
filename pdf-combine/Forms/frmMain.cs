using System;
using System.Windows.Forms;
using Serilog;

namespace pdf_combine.Forms
{
    /// <summary>
    /// this form is the main form of the application
    /// </summary>
    public partial class frmMain : Form
    {
        /// <summary>
        /// the main ctor
        /// </summary>
        public frmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// handles the load event
        /// </summary>
        /// <param name="sender">the form</param>
        /// <param name="e">event arguments</param>
        private void frmMain_Load(object sender, EventArgs e)
        {
            this.ctlSplitUi.Hide();
            this.ctlCombineUi.Show();
        }

        /// <summary>
        /// handles a user click on the main menu
        /// </summary>
        /// <param name="sender">the menu strip</param>
        /// <param name="e">event arguments</param>
        private void MainMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
			Log.Logger.Information("Starting MainMenu_ItemClicked...{0} was clicked.", e.ClickedItem.Text);
            switch (e.ClickedItem.Name)
            {
                case "tsmCombine":
                    {
                        this.ctlSplitUi.Hide();
                        this.ctlCombineUi.Show();
                        break;
                    }
                case "tsmSplit":
                    {
                        this.ctlCombineUi.Hide();
                        this.ctlSplitUi.Show();
                        break;
                    }
                case "tsmAbout":
                    {
                        var frmAbout = new frmAbout();
                        frmAbout.ShowDialog();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
			Log.Logger.Information("Completed MainMenu_ItemClicked.");
		}
    }
}
