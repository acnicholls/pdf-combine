﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using pdf_combine.Func;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace pdf_combine.UserControls
{
    public partial class ctlSplit : UserControl
    {
        private string inputFilename;
        private IList<int> pageList;
        public ctlSplit()
        {
            inputFilename = "";
            pageList = new List<int>();
            InitializeComponent();
        }

        private bool validateBtnSplitText(string inputString)
        {
            // make sure we only have expected characters
            string validChars = @"1234567890\-,";
            string regExMatch = "/[^" + validChars + "]+/g";
            var match = Regex.Match(inputString, regExMatch);
            if (match.Success)
            {
                return false;
            }
            // check for invalid characters in each range
            var pageRanges = inputString.Split(',');
            foreach (var page in pageRanges)
            {
                // only one dash per range
                if (page.Count(c => c.ToString() == "-") > 1)
                {
                    return false;
                }
                // no commas allowed in a range
                if (page.Any(c => c.ToString() == ","))
                {
                    return false;
                }
            }
            return true;
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(inputFilename))
            {
                MessageBox.Show("You must choose a file to copy from.");
                return;
            }
            var pageRange = this.txtPageRange.Text.Trim();
            if (string.IsNullOrWhiteSpace(pageRange))
            {
                MessageBox.Show("You must choose pages to copy.");
                return;
            }
            if (validateBtnSplitText(pageRange))
            {
                var pageRanges = pageRange.Split(',');
                foreach (var pageRequest in pageRanges)
                {
                    if (pageRequest.Contains("-"))
                    {
                        var rangeEnds = pageRequest.Split('-');
                        for (int x = Convert.ToInt32(rangeEnds[0]); x <= Convert.ToInt32(rangeEnds[1]); x += 1)
                        {
                            pageList.Add(x - 1);
                        }
                    }
                    else
                    {
                        pageList.Add(Convert.ToInt32(pageRequest) - 1);
                    }
                }
                try
                {
                    Split.Start(pageList, inputFilename);
                    MessageBox.Show("Success");
                    this.txtFileName.Text = "";
                    this.txtNumOfPages.Text = "";
                    this.txtPageRange.Text = "";
                }
                catch(Exception x)
                {
                    MessageBox.Show(x.Message, "Something went wrong...");
                }

            }
            else
            {
                MessageBox.Show("You have entered invalid characters, please try again.");
                return;
            }
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            ofDialog.Title = "Choose the PDF to copy pages from";
            ofDialog.InitialDirectory = Path.GetFullPath(Environment.SpecialFolder.MyDocuments.ToString());
            var fileResult = ofDialog.ShowDialog();
            if (fileResult == DialogResult.OK)
            {
                inputFilename = ofDialog.FileName;
                this.txtFileName.Text = Path.GetFileName(ofDialog.SafeFileName);
                var pdfDoc = PdfReader.Open(ofDialog.FileName);
                this.txtNumOfPages.Text = pdfDoc.PageCount.ToString();

            }
        }
    }
}