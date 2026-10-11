using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;

namespace pdf_combine.Func
{
    public static class Split
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
                using var oldFile = PdfReader.Open(inputFilename, PdfDocumentOpenMode.Import);

                foreach (var page in pageListRows)
                {
                    var newFileName = Path.Combine(outputDir.FullName, "Page" + (page + 1) + ".pdf");
                    newPage = new PdfDocument();
                    newPage.AddPage(oldFile.Pages[page]);
                    newPage.Save(newFileName);
                    newPage.Close();
                    newPage = null;
                }
            }
            catch (Exception x)
            {
                Log.Logger.Error(x, "Something went wrong splitting file.");
                newPage?.Close();
                return false;
            }
            return true;
        }
    }
}
