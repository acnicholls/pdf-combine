
namespace pdf_combine.UserControls
{
    partial class ctlSplit
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ofDialog = new System.Windows.Forms.OpenFileDialog();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.btnChoose = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.txtPageRange = new System.Windows.Forms.TextBox();
            this.lblPageList = new System.Windows.Forms.Label();
            this.lblExample = new System.Windows.Forms.Label();
            this.btnSplit = new System.Windows.Forms.Button();
            this.lblSplit = new System.Windows.Forms.Label();
            this.lblNumberOfPages = new System.Windows.Forms.Label();
            this.txtNumOfPages = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ofDialog
            // 
            this.ofDialog.Filter = "*.pdf|*.PDF";
            // 
            // lblInstructions
            // 
            this.lblInstructions.Location = new System.Drawing.Point(13, 13);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(260, 38);
            this.lblInstructions.TabIndex = 0;
            this.lblInstructions.Text = "Choose the file you want to extract pages from and then enter the list of pages t" +
    "o extract.";
            // 
            // btnChoose
            // 
            this.btnChoose.Location = new System.Drawing.Point(13, 49);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(227, 23);
            this.btnChoose.TabIndex = 1;
            this.btnChoose.Text = "Choose...";
            this.btnChoose.UseVisualStyleBackColor = true;
            this.btnChoose.Click += new System.EventHandler(this.btnChoose_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.txtFileName.Enabled = false;
            this.txtFileName.Location = new System.Drawing.Point(13, 79);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(227, 23);
            this.txtFileName.TabIndex = 2;
            // 
            // txtPageRange
            // 
            this.txtPageRange.Location = new System.Drawing.Point(13, 173);
            this.txtPageRange.Name = "txtPageRange";
            this.txtPageRange.Size = new System.Drawing.Size(227, 23);
            this.txtPageRange.TabIndex = 3;
            // 
            // lblPageList
            // 
            this.lblPageList.Location = new System.Drawing.Point(13, 137);
            this.lblPageList.Name = "lblPageList";
            this.lblPageList.Size = new System.Drawing.Size(260, 33);
            this.lblPageList.TabIndex = 4;
            this.lblPageList.Text = "Enter the pages to extract, dashes for ranges, commas to separate ranges, or sing" +
    "le pages.";
            // 
            // lblExample
            // 
            this.lblExample.AutoSize = true;
            this.lblExample.Location = new System.Drawing.Point(13, 200);
            this.lblExample.Name = "lblExample";
            this.lblExample.Size = new System.Drawing.Size(84, 15);
            this.lblExample.TabIndex = 5;
            this.lblExample.Text = "ex: 1,2,4,7-8,10";
            // 
            // btnSplit
            // 
            this.btnSplit.Location = new System.Drawing.Point(13, 269);
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(227, 23);
            this.btnSplit.TabIndex = 6;
            this.btnSplit.Text = "Split";
            this.btnSplit.UseVisualStyleBackColor = true;
            this.btnSplit.Click += new System.EventHandler(this.btnSplit_Click);
            // 
            // lblSplit
            // 
            this.lblSplit.Location = new System.Drawing.Point(13, 222);
            this.lblSplit.Name = "lblSplit";
            this.lblSplit.Size = new System.Drawing.Size(260, 39);
            this.lblSplit.TabIndex = 7;
            this.lblSplit.Text = "Your resulting split pages will be in a folder with the same name as the input fi" +
    "le, named Page#.";
            // 
            // lblNumberOfPages
            // 
            this.lblNumberOfPages.AutoSize = true;
            this.lblNumberOfPages.Location = new System.Drawing.Point(74, 111);
            this.lblNumberOfPages.Name = "lblNumberOfPages";
            this.lblNumberOfPages.Size = new System.Drawing.Size(105, 15);
            this.lblNumberOfPages.TabIndex = 8;
            this.lblNumberOfPages.Text = "Number of Pages: ";
            // 
            // txtNumOfPages
            // 
            this.txtNumOfPages.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.txtNumOfPages.Location = new System.Drawing.Point(185, 108);
            this.txtNumOfPages.Name = "txtNumOfPages";
            this.txtNumOfPages.Size = new System.Drawing.Size(55, 23);
            this.txtNumOfPages.TabIndex = 10;
            // 
            // ctlSplit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtNumOfPages);
            this.Controls.Add(this.lblNumberOfPages);
            this.Controls.Add(this.lblSplit);
            this.Controls.Add(this.btnSplit);
            this.Controls.Add(this.lblExample);
            this.Controls.Add(this.lblPageList);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.btnChoose);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.txtPageRange);
            this.Name = "ctlSplit";
            this.Size = new System.Drawing.Size(285, 301);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofDialog;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Button btnChoose;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.TextBox txtPageRange;
        private System.Windows.Forms.Label lblPageList;
        private System.Windows.Forms.Label lblExample;
        private System.Windows.Forms.Button btnSplit;
        private System.Windows.Forms.Label lblSplit;
        private System.Windows.Forms.Label lblNumberOfPages;
        private System.Windows.Forms.TextBox txtNumOfPages;
    }
}
