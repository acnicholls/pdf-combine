using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using pdf_combine.Func;

namespace pdf_combine.UserControls
{
    public partial class ctlCombine : UserControl
    {
        private PdfFileList fileList; 
        public ctlCombine()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            fileList = new PdfFileList();
            this.lbFileList.Items.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ofDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var result = ofDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                foreach (var file in ofDialog.FileNames)
                {
                    var fileInfo = new FileInfo(file);
                    PdfFileList.FileListRow row = fileList.FileList.NewFileListRow();
                    row.filePath = fileInfo.DirectoryName;
                    row.filename = fileInfo.Name;
                    row.order = fileList.FileList.Rows.Count;
                    fileList.FileList.AddFileListRow(row);
                }
                fileList.AcceptChanges();
                AssignListContent();
            }
        }

        private void btnCombine_Click(object sender, EventArgs e)
        {
            sfDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfDialog.Title = "Select where you want the output file and the name of the new file...";
            sfDialog.Filter = "PDF Files|*.pdf";
            var result = sfDialog.ShowDialog();
            if (result == DialogResult.OK)
            {

                List<PdfFileList.FileListRow> rows = new List<PdfFileList.FileListRow>();
                foreach (PdfFileList.FileListRow row in this.fileList.FileList.Rows)
                {
                    rows.Add(row);
                }
                try
                {
                    Combine.Start(rows, sfDialog.FileName);
                    MessageBox.Show(this, "Success");
                    this.fileList = new PdfFileList();
                    this.lbFileList.Items.Clear();
                }
                catch(Exception x)
                {
                    MessageBox.Show(this, x.Message, "Something went wrong...");
                }
            }
        }

        private void btnOrderUp_Click(object sender, EventArgs e)
        {
            var selectedRow = GetSelectedItem();
            var selectedIndex = this.lbFileList.SelectedIndex;
            if (selectedRow.order == 0)
            {
                MessageBox.Show(this, "You cannot move the top item up.");
                return;
            }
            ReorderItems(MovementDirection.Up, selectedRow);
            this.lbFileList.SelectedIndex = selectedIndex - 1;
        }

        private void btnOrderDown_Click(object sender, EventArgs e)
        {
            var selectedRow = GetSelectedItem();
            var selectedIndex = this.lbFileList.SelectedIndex;
            if (selectedRow.order == this.lbFileList.Items.Count - 1)
            {
                MessageBox.Show(this, "You cannot move the bottom item down.");
                return;
            }
            ReorderItems(MovementDirection.Down, selectedRow);
            this.lbFileList.SelectedIndex = selectedIndex + 1;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var selectedRow = GetSelectedItem();
            this.fileList.FileList.Rows.Remove(selectedRow);
            ReorderItems();
        }

        private PdfFileList.FileListRow GetSelectedItem()
        {
            var selectedRow = this.lbFileList.SelectedItem;
            return (PdfFileList.FileListRow)selectedRow;
        }

        private void ReorderItems()
        {
            var x = 0;
            foreach (var item in this.fileList.FileList.OrderBy(x=>x.order))
            {
                item.order = x;
                x++;
            }
            this.fileList.AcceptChanges();
            AssignListContent();
        }

        private enum MovementDirection
        {
            Up,
            Down
        }

        private void ReorderItems(MovementDirection direction, PdfFileList.FileListRow selectedRow)
        {
            var currentPosition = selectedRow.order;
            PdfFileList.FileListRow currentRow = this.fileList.FileList.FindByid(selectedRow.id);
            switch (direction)
            {
                case MovementDirection.Down:
                    {
                        PdfFileList.FileListRow nextRow = this.fileList.FileList.Where(r => r.order == currentPosition + 1).First();
                        nextRow.order = currentPosition;
                        currentRow.order = currentPosition + 1;
                        break;
                    }
                case MovementDirection.Up:
                    {
                        PdfFileList.FileListRow prevRow = this.fileList.FileList.Where(r => r.order == currentPosition - 1).First();
                        prevRow.order = currentPosition;
                        currentRow.order = currentPosition - 1;
                        break;
                    }
            }
            this.fileList.AcceptChanges();
            AssignListContent();
        }

        private void AssignListContent()
        {
            this.lbFileList.Sorted = false;
            var items = this.fileList.FileList.OrderBy(x => x.order).ToList();

            this.lbFileList.DataSource = items;
            this.lbFileList.DisplayMember = "filename";
            this.lbFileList.ValueMember = "order";

        }
    }
}
