﻿using PdfSharp.Pdf;
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
