using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Microsoft.Extensions.Logging;

namespace pdf_combine.Func
{
    public class Split
    {
        public static bool Start(List<int> pageListRows, string inputFilename)
        {
            
            try
            {


                string basePath = Path.GetFullPath(inputFilename);
                string filePath = Path.GetFileName(inputFilename);
                var outputFolderName = Path.Combine(basePath, filePath);
                var oldFile = PdfReader.Open(Path.Combine(outputFolderName, inputFilename), PdfDocumentOpenMode.ReadOnly);
                foreach (var page in pageListRows)
                {
                    PdfDocument newPage = new PdfDocument();
                    newPage.AddPage(oldFile.Pages[page]);
                    newPage.Save(Path.Combine(outputFolderName, "Page" + (page + 1)));
                }
            }
            catch (Exception x)
            {
                return false;
            }
            return true;
        }
    }
}
