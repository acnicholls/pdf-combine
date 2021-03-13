using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace pdf_combine.Func
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
                return false;
            }
            return true;
        }
    }
}
