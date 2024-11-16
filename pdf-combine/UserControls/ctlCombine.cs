using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using pdf_combine.Func;

namespace pdf_combine.UserControls
{
    /// <summary>
    /// this control handles the UI components for Combining PDF files.
    /// </summary>
    public partial class ctlCombine : UserControl
    {
        private PdfFileList fileList; 

        /// <summary>
        /// the main ctor
        /// </summary>
        public ctlCombine()
        {
            InitializeComponent();
        }

        /// <summary>
        /// performs private variable initialization
        /// </summary>
        /// <param name="sender">the form</param>
        /// <param name="e">event arguments</param>
        private void ctlCombine_Load(object sender, EventArgs e)
        {
            fileList = new PdfFileList();
            this.lbFileList.Items.Clear();
        }

        /// <summary>
        /// handles events when the user clicks "Add" button
        /// opens the "File Open Dialog"
        /// allows the user to add file(s) to the list
        /// </summary>
        /// <param name="sender">the button</param>
        /// <param name="e">event arguments</param>
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

        /// <summary>
        /// handles the event when the user clicks "Combine" button
        /// validates the list and starts the combine function
        /// </summary>
        /// <param name="sender">the button</param>
        /// <param name="e">event arguments</param>
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
					this.Enabled = false;
                    if(Combine.Start(rows, sfDialog.FileName))
					{
						MessageBox.Show(this, "Success");
					}

					this.Enabled = true;
                }
                catch(Exception x)
                {
                    MessageBox.Show(this, x.Message, "Something went wrong...");
					if(!this.Enabled)
					{
						this.Enabled = true;
					}
                }
            }
        }

        /// <summary>
        /// handles events when the user clicks the "Move Up" button
        /// moves file up one place in the file order
        /// </summary>
        /// <param name="sender">the button</param>
        /// <param name="e">event arguments</param>
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

        /// <summary>
        /// handles the event when the user clicks the "Move Down" button
        /// moves file down one place in the file order
        /// </summary>
        /// <param name="sender">the button</param>
        /// <param name="e">event arguments</param>
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

        /// <summary>
        /// returns the currently select item in the list
        /// </summary>
        /// <returns>the row in the data backing the listbox</returns>
        private PdfFileList.FileListRow GetSelectedItem()
        {
            var selectedRow = this.lbFileList.SelectedItem;
            return (PdfFileList.FileListRow)selectedRow;
        }

        /// <summary>
        /// resets the order of the items in the list
        /// </summary>
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

        /// <summary>
        /// describes the direction a file must travel in the list
        /// </summary>
        private enum MovementDirection
        {
            Up,
            Down
        }

        /// <summary>
        /// override for item ordering, used when moving an item up or down
        /// </summary>
        /// <param name="direction">the movement direction of the selected item</param>
        /// <param name="selectedRow">the corresponding row in the data to move</param>
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

        /// <summary>
        /// clears and re-assigns the listbox's content
        /// </summary>
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
