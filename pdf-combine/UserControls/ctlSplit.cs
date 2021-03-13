using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pdf_combine.UserControls
{
    public partial class ctlSplit : UserControl
    {
        private IList<int> pageList;
        public ctlSplit()
        {
            pageList = new List<int>();
            InitializeComponent();
        }


    }
}
