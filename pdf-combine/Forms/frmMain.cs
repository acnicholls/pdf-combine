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
        private ctlCombine ctlCombine;
        private ctlSplit ctlSplit;
        private PdfFileList fileList;
        public frmMain()
        {
            ctlCombine = new ctlCombine();
            ctlSplit = new ctlSplit();
            fileList = new PdfFileList();
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void MainMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "tsmCombine":
                    {
                        ctlSplit.Hide();
                        ctlCombine.Show();
                        break;
                    }
                case "tsmSplit":
                    {
                        ctlCombine.Hide();
                        ctlSplit.Show();
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
