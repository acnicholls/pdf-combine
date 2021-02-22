using System;
using System.Collections.Generic;
using System.Text;
using PdfSharp.Pdf;
using System.Linq;
using System.IO;
using System.Data;

namespace pdf_combine
{
    public class Combine
    {
        public static bool Start(List<PdfFileList.FileListRow> fileListRows, string outputFilename)
        {
            try
            {
                PdfDocument newDoc = new PdfDocument();
                foreach (var file in fileListRows.OrderBy(x => x.order))
                {
                    PdfDocument oldFile = new PdfDocument(Path.Combine(file.filePath, file.filename));
                    foreach (var page in oldFile.Pages)
                    {
                        newDoc.Pages.Add(page);
                    }
                }
                newDoc.Save(outputFilename);
            }
            catch(Exception x)
            {
                return false;
            }
            return true;
        }
    }
}
