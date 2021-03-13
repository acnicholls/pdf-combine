using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using pdf_combine.Func;
using pdf_combine.UserControls;

namespace pdf_combine.Forms
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.ctlSplitUi.Hide();
            this.ctlCombineUi.Show();
        }

        private void MainMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
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
                default:
                    {
                        break;
                    }
            }
        }
    }
}
