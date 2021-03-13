
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.lblPageList = new System.Windows.Forms.Label();
            this.lblExample = new System.Windows.Forms.Label();
            this.btnSplit = new System.Windows.Forms.Button();
            this.lblSplit = new System.Windows.Forms.Label();
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
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(13, 79);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(227, 23);
            this.textBox1.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(13, 165);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(227, 23);
            this.textBox2.TabIndex = 3;
            // 
            // lblPageList
            // 
            this.lblPageList.Location = new System.Drawing.Point(13, 129);
            this.lblPageList.Name = "lblPageList";
            this.lblPageList.Size = new System.Drawing.Size(260, 33);
            this.lblPageList.TabIndex = 4;
            this.lblPageList.Text = "Enter the pages to extract, dashes for ranges, commas to separate ranges, or sing" +
    "le pages.";
            // 
            // lblExample
            // 
            this.lblExample.AutoSize = true;
            this.lblExample.Location = new System.Drawing.Point(13, 195);
            this.lblExample.Name = "lblExample";
            this.lblExample.Size = new System.Drawing.Size(93, 15);
            this.lblExample.TabIndex = 5;
            this.lblExample.Text = "ex: 1, 2.4, 7-8, 10";
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
            this.lblSplit.Location = new System.Drawing.Point(13, 210);
            this.lblSplit.Name = "lblSplit";
            this.lblSplit.Size = new System.Drawing.Size(260, 56);
            this.lblSplit.TabIndex = 7;
            this.lblSplit.Text = "Your resulting split pages will be ina folder with the same name as the input fil" +
    "e, named for their page.";
            // 
            // ctlSplit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblSplit);
            this.Controls.Add(this.btnSplit);
            this.Controls.Add(this.lblExample);
            this.Controls.Add(this.lblPageList);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnChoose);
            this.Controls.Add(this.lblInstructions);
            this.Name = "ctlSplit";
            this.Size = new System.Drawing.Size(285, 301);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofDialog;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Button btnChoose;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label lblPageList;
        private System.Windows.Forms.Label lblExample;
        private System.Windows.Forms.Button btnSplit;
        private System.Windows.Forms.Label lblSplit;
    }
}
