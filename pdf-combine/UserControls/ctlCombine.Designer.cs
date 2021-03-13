
namespace pdf_combine.UserControls
{
    partial class ctlCombine
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbFileList = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCombine = new System.Windows.Forms.Button();
            this.btnOrderUp = new System.Windows.Forms.Button();
            this.btnOrderDown = new System.Windows.Forms.Button();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.ofDialog = new System.Windows.Forms.OpenFileDialog();
            this.sfDialog = new System.Windows.Forms.SaveFileDialog();
            this.btnRemove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbFileList
            // 
            this.lbFileList.FormattingEnabled = true;
            this.lbFileList.ItemHeight = 15;
            this.lbFileList.Location = new System.Drawing.Point(12, 78);
            this.lbFileList.Name = "lbFileList";
            this.lbFileList.Size = new System.Drawing.Size(228, 184);
            this.lbFileList.TabIndex = 5;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(13, 49);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(227, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add File(s)";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCombine
            // 
            this.btnCombine.Location = new System.Drawing.Point(13, 269);
            this.btnCombine.Name = "btnCombine";
            this.btnCombine.Size = new System.Drawing.Size(227, 23);
            this.btnCombine.TabIndex = 4;
            this.btnCombine.Text = "Combine!";
            this.btnCombine.UseVisualStyleBackColor = true;
            this.btnCombine.Click += new System.EventHandler(this.btnCombine_Click);
            // 
            // btnOrderUp
            // 
            this.btnOrderUp.Location = new System.Drawing.Point(247, 78);
            this.btnOrderUp.Name = "btnOrderUp";
            this.btnOrderUp.Size = new System.Drawing.Size(26, 60);
            this.btnOrderUp.TabIndex = 1;
            this.btnOrderUp.Text = "^ | |";
            this.btnOrderUp.UseVisualStyleBackColor = true;
            this.btnOrderUp.Click += new System.EventHandler(this.btnOrderUp_Click);
            // 
            // btnOrderDown
            // 
            this.btnOrderDown.Location = new System.Drawing.Point(247, 144);
            this.btnOrderDown.Name = "btnOrderDown";
            this.btnOrderDown.Size = new System.Drawing.Size(26, 60);
            this.btnOrderDown.TabIndex = 2;
            this.btnOrderDown.Text = "| | v";
            this.btnOrderDown.UseVisualStyleBackColor = true;
            this.btnOrderDown.Click += new System.EventHandler(this.btnOrderDown_Click);
            // 
            // lblInstructions
            // 
            this.lblInstructions.Location = new System.Drawing.Point(13, 13);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(260, 33);
            this.lblInstructions.TabIndex = 5;
            this.lblInstructions.Text = "Add all the files you want to combine, order the pages and click Combine!";
            // 
            // ofDialog
            // 
            this.ofDialog.Filter = "*.pdf|*.PDF";
            this.ofDialog.Multiselect = true;
            this.ofDialog.Title = "Add files to combine...";
            // 
            // sfDialog
            // 
            this.sfDialog.DefaultExt = "pdf";
            this.sfDialog.Title = "File to output combined PDF pages to...";
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(247, 211);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(26, 51);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "X";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // ctlCombine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.btnOrderDown);
            this.Controls.Add(this.btnOrderUp);
            this.Controls.Add(this.btnCombine);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lbFileList);
            this.Name = "ctlCombine";
            this.Size = new System.Drawing.Size(285, 301);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbFileList;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCombine;
        private System.Windows.Forms.Button btnOrderUp;
        private System.Windows.Forms.Button btnOrderDown;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.OpenFileDialog ofDialog;
        private System.Windows.Forms.SaveFileDialog sfDialog;
        private System.Windows.Forms.Button btnRemove;

    }
}