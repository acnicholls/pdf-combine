﻿
namespace pdf_combine.Forms
{
    partial class frmMain
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
            this.ctlCombineUi = new pdf_combine.UserControls.ctlCombine();
            this.ctlSplitUi = new pdf_combine.UserControls.ctlSplit();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.tsmCombine = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSplit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmABout = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctlCombineUi
            // 
            this.ctlCombineUi.Location = new System.Drawing.Point(0, 25);
            this.ctlCombineUi.Name = "ctlCombineUi";
            this.ctlCombineUi.Size = new System.Drawing.Size(285, 301);
            this.ctlCombineUi.TabIndex = 1;
            // 
            // ctlSplitUi
            // 
            this.ctlSplitUi.Location = new System.Drawing.Point(0, 25);
            this.ctlSplitUi.Name = "ctlSplitUi";
            this.ctlSplitUi.Size = new System.Drawing.Size(285, 301);
            this.ctlSplitUi.TabIndex = 0;
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmCombine,
            this.tsmSplit,
            this.tsmABout});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(285, 24);
            this.MainMenu.TabIndex = 1;
            this.MainMenu.Text = "Main Menu";
            this.MainMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MainMenu_ItemClicked);
            // 
            // tsmCombine
            // 
            this.tsmCombine.Name = "tsmCombine";
            this.tsmCombine.Size = new System.Drawing.Size(68, 20);
            this.tsmCombine.Text = "Combine";
            // 
            // tsmSplit
            // 
            this.tsmSplit.Name = "tsmSplit";
            this.tsmSplit.Size = new System.Drawing.Size(42, 20);
            this.tsmSplit.Text = "Split";
            // 
            // tsmABout
            // 
            this.tsmABout.Name = "tsmABout";
            this.tsmABout.Size = new System.Drawing.Size(61, 20);
            this.tsmABout.Text = "About...";
            this.tsmABout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 335);
            this.Controls.Add(this.ctlCombineUi);
            this.Controls.Add(this.MainMenu);
            this.Controls.Add(this.ctlSplitUi);
            this.MainMenuStrip = this.MainMenu;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PDF Combiner";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UserControls.ctlCombine ctlCombineUi;
        private UserControls.ctlSplit ctlSplitUi;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmCombine;
        private System.Windows.Forms.ToolStripMenuItem tsmSplit;
        private System.Windows.Forms.ToolStripMenuItem tsmABout;
    }
}