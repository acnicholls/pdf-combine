using System;
using System.Collections.Generic;
using System.Text;
using PdfSharp.Pdf;
using System.Linq;
using System.IO;
using System.Data;
using PdfSharp.Pdf.IO;

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
                    var oldFile = PdfReader.Open(Path.Combine(file.filePath, file.filename), PdfDocumentOpenMode.Import);
                    foreach (var page in oldFile.Pages)
                    {
                        newDoc.Pages.Add(page);
                    }
                }
                newDoc.Save(outputFilename);
            }
            catch(Exception x)
            {
                throw x;
            }
            return true;
        }
    }
}
