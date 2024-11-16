using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Serilog;

namespace pdf_combine.Func
{
    public class Combine
    {
        /// <summary>
        /// this method combines all the listed pdf files into one file, in the order they appear in the list
        /// </summary>
        /// <param name="fileListRows">the information regarding the pdf files to combine</param>
        /// <param name="outputFilename">the name of the combined file</param>
        /// <returns></returns>
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
				Log.Logger.Error(x, "Something went wrong.");
                return false;
            }
            return true;
        }
    }
}
