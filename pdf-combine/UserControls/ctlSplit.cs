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
    /// <summary>
    /// this control handles the UI components for Splitting PDF files.
    /// </summary>
    public partial class ctlSplit : UserControl
    {
        private string inputFilename;
        private IList<int> pageList;
        private ErrorMessage errorMessage;
        private enum ErrorMessage
        {
            NoError = 0,
            MoreThanOneDash = 1,
            NotNumberDashNumber = 2,
            LeftNotLowerThanRight = 3,
            OutsideValidRange = 4,
            InvalidCharacters = 5
        }

        /// <summary>
        /// the main ctor
        /// </summary>
        public ctlSplit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// performs private variable initialization
        /// </summary>
        /// <param name="sender">the form</param>
        /// <param name="e">event arguments</param>
        private void ctlSplit_Load(object sender, EventArgs e)
        {
            inputFilename = "";
            pageList = new List<int>();
            errorMessage = ErrorMessage.NoError;
            this.txtFileName.Text = "";
            this.txtNumOfPages.Text = "";
            this.txtPageRange.Text = "";
            this.ofDialog = new OpenFileDialog();
        }

        /// <summary>
        /// determines the message to return to the user, based on the input error type
        /// </summary>
        /// <param name="inputError">the error</param>
        /// <returns>a string error message</returns>
        private string errorMessageString(ErrorMessage inputError)
        {
            switch(inputError)
            {
                case ErrorMessage.NoError:
                    {
                        return "No Error.";
                    }
                case ErrorMessage.MoreThanOneDash:
                    {
                        return "More than one dash present in input value.";
                    }
                case ErrorMessage.NotNumberDashNumber:
                    {
                        return "The input string is not in the expected format.";
                    }
                case ErrorMessage.LeftNotLowerThanRight:
                    {
                        return "The left number is not lower than the right.";
                    }
                case ErrorMessage.OutsideValidRange:
                    {
                        return "You have chosen a number greater than the number of pages in the selected file.";
                    }
                case ErrorMessage.InvalidCharacters:
                    {
                        return "You have entered invalid characters.";
                    }
                default:
                    {
                        return "unknown value";
                    }
            }
        }

        /// <summary>
        /// validates the input from the user when they press the Split Button.
        /// </summary>
        /// <param name="inputString">the requesteed pages to extract</param>
        /// <returns>an errorMessage enum value</returns>
        private ErrorMessage validateBtnSplitText(string inputString)
        {
            // make sure we only have expected characters
            //string validChars = @"1234567890\-,";
            string regExMatch = @"[^1234567890\-,]";
            if (Regex.IsMatch(inputString, regExMatch))
            {
                return ErrorMessage.InvalidCharacters;
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
                        return ErrorMessage.InvalidCharacters;
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
                            return ErrorMessage.NotNumberDashNumber;
                        }
                        var rangeEnds = page.Split("-");
                        if(Convert.ToInt32(rangeEnds[0]) > Convert.ToInt32(rangeEnds[1]))
                        {
                            return ErrorMessage.LeftNotLowerThanRight;
                        }
                        foreach(var end in rangeEnds)
                        {
                            if(Convert.ToInt32(end) < 1 || Convert.ToInt32(end) > Convert.ToInt32(this.txtNumOfPages.Text))
                            {
                                return ErrorMessage.OutsideValidRange;
                            }
                        }
                    }

                }
                else
                {
                    if(string.IsNullOrWhiteSpace(page))
                    {
                        return ErrorMessage.InvalidCharacters;
                    }
                    regExMatch = @"[^1234567890]";
                    if (Regex.IsMatch(page, regExMatch))
                    {
                        return ErrorMessage.InvalidCharacters;
                    }
                }
            }

            return ErrorMessage.NoError;
        }

        /// <summary>
        /// handles the event when the user presses the "Split" button
        /// validates the input text and starts the extraction process
        /// </summary>
        /// <param name="sender">the button</param>
        /// <param name="e">event arguments</param>
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
            
            if (validationReult == ErrorMessage.NoError)
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

        /// <summary>
        /// handles the event when the user presses the "Choose..." button
        /// opens the "File Open Dialog" and allows the user to choose a PDF file
        /// </summary>
        /// <param name="sender">the button</param>
        /// <param name="e">event arguments</param>
        private void btnChoose_Click(object sender, EventArgs e)
        {
            ofDialog.Title = "Choose the PDF to copy pages from";
            ofDialog.InitialDirectory = Path.GetFullPath(Environment.SpecialFolder.MyDocuments.ToString());
            var fileResult = ofDialog.ShowDialog();
            if (fileResult == DialogResult.OK)
            {
                inputFilename = ofDialog.FileName;
                PdfDocument pdfDoc = new PdfDocument();
                try
                {
                    pdfDoc = PdfReader.Open(inputFilename);
                    this.txtFileName.Text = Path.GetFileName(ofDialog.SafeFileName);
                    this.txtNumOfPages.Text = pdfDoc.PageCount.ToString();
                }
                catch(Exception x)
                {
                    MessageBox.Show(x.Message, "something went wrong...");
                    return;
                }

            }
        }

    }
}
