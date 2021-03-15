using System;
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
        private enum errorMessage
        {
            NoError = 0,
            MoreThanOneDash = 1,
            NotNumberDashNumber = 2,
            LeftNotLowerThanRight = 3,
            OutsideValidRange = 4,
            InvalidCharacters = 5
        }
        public ctlSplit()
        {
            inputFilename = "";
            pageList = new List<int>();
            InitializeComponent();
        }

        private string errorMessageString(errorMessage inputError)
        {
            switch(inputError)
            {
                case errorMessage.NoError:
                    {
                        return "No Error.";
                    }
                case errorMessage.MoreThanOneDash:
                    {
                        return "More than one dash present in input value.";
                    }
                case errorMessage.NotNumberDashNumber:
                    {
                        return "The input string is not in the expected format.";
                    }
                case errorMessage.LeftNotLowerThanRight:
                    {
                        return "The left number is not lower than the right.";
                    }
                case errorMessage.OutsideValidRange:
                    {
                        return "You have chosen a number greater than the number of pages in the selected file.";
                    }
                case errorMessage.InvalidCharacters:
                    {
                        return "You have entered invalid characters.";
                    }
                default:
                    {
                        return "unknown value";
                    }
            }
        }

        private errorMessage validateBtnSplitText(string inputString)
        {
            // make sure we only have expected characters
            //string validChars = @"1234567890\-,";
            string regExMatch = @"[^1234567890\-,]";
            if (Regex.IsMatch(inputString, regExMatch))
            {
                return errorMessage.InvalidCharacters;
            }
            // check for invalid characters in each range
            var pageRanges = inputString.Split(',');
            foreach (var page in pageRanges)
            {
                if (page.Length > 1)
                {
                    var onlyOneDash = true;
                    // only one dash per range
                    if (page.Count(c => c.ToString() == "-") > 1)
                    {
                        return errorMessage.InvalidCharacters;
                    }
                    else
                    {
                        onlyOneDash = true;
                    }
                    if (onlyOneDash)
                    {
                        // check if it's number-dash-number
                        var regexMatch = @"\d+\-\d+";
                        if (!Regex.IsMatch(page, regexMatch))
                        {
                            return errorMessage.NotNumberDashNumber;
                        }
                        var rangeEnds = page.Split("-");
                        if(Convert.ToInt32(rangeEnds[0]) > Convert.ToInt32(rangeEnds[1]))
                        {
                            return errorMessage.LeftNotLowerThanRight;
                        }
                        foreach(var end in rangeEnds)
                        {
                            if(Convert.ToInt32(end) < 1 || Convert.ToInt32(end) > Convert.ToInt32(this.txtNumOfPages.Text))
                            {
                                return errorMessage.OutsideValidRange;
                            }
                        }
                    }

                }
                else
                {
                    if(string.IsNullOrWhiteSpace(page))
                    {
                        return errorMessage.InvalidCharacters;
                    }
                    regExMatch = @"[^1234567890]";
                    if (Regex.IsMatch(page, regExMatch))
                    {
                        return errorMessage.InvalidCharacters;
                    }
                }
            }

            return errorMessage.NoError;
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(inputFilename))
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
            var validationReult = validateBtnSplitText(pageRange);
            
            if (validationReult == errorMessage.NoError)
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
                catch (Exception x)
                {
                    MessageBox.Show(x.Message, "Something went wrong...");
                }

            }
            else
            {
                MessageBox.Show(errorMessageString(validationReult), "invalid page list");
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
