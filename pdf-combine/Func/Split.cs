using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace pdf_combine.Func
{
    public class Split
    {

        /// <summary>
        /// this method splits the given pdf into the listed page ranges.
        /// </summary>
        /// <param name="pageListRows">the page list to split out</param>
        /// <param name="inputFilename">the file to copy pages from</param>
        /// <returns>true if successful</returns>
        public static bool Start(IList<int> pageListRows, string inputFilename)
        {
            PdfDocument newPage = null;
            try
            {

                string basePath = Path.GetDirectoryName(inputFilename);
                string filePath = Path.GetFileNameWithoutExtension(inputFilename);
                DirectoryInfo outputDir = new DirectoryInfo(Path.Combine(basePath, filePath));
                if(!outputDir.Exists)
                {
                    outputDir.Create();
                }
                var oldFile = PdfReader.Open(inputFilename, PdfDocumentOpenMode.Import);

                foreach (var page in pageListRows)
                {
                    var newFileName = Path.Combine(outputDir.FullName, "Page" + (page + 1) + ".pdf");
                    newPage = new PdfDocument();
                    // reopen in import mode
                    newPage.AddPage(oldFile.Pages[page]);
                    newPage.Save(newFileName);
                    newPage.Close();
                }
            }
            catch (Exception x)
            {
                if (newPage != null)
                {
                    newPage.Close();
                }
                return false;
            }
            return true;
        }
    }
}
