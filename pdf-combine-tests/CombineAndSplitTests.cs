using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using pdf_combine.Func;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace pdf_combine_tests
{
    /// <summary>
    /// Integration tests for the Combine and Split business logic classes.
    /// Covers the bug where Split could not open the same PDF that Combine produced.
    /// </summary>
    public class CombineAndSplitTests : IDisposable
    {
        private readonly string _testDir;

        public CombineAndSplitTests()
        {
            // Each test class instance gets its own isolated temp directory
            _testDir = Path.Combine(Path.GetTempPath(), "pdf-combine-tests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testDir);

            // PdfSharp requires this on .NET Core/5+
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Ensure Serilog is initialised so Split.Start can log errors
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        public void Dispose()
        {
            try { Directory.Delete(_testDir, recursive: true); }
            catch { /* best-effort cleanup */ }
        }

        // ------------------------------------------------------------------ helpers

        /// <summary>Creates a minimal but valid PDF with the requested number of blank pages.</summary>
        private string CreateTestPdf(string filename, int pageCount)
        {
            var path = Path.Combine(_testDir, filename);
            using var doc = new PdfDocument();
            for (int i = 0; i < pageCount; i++)
            {
                doc.AddPage();
            }
            doc.Save(path);
            return path;
        }

        /// <summary>Returns the page count of a saved PDF file.</summary>
        private static int GetPageCount(string pdfPath)
        {
            using var doc = PdfReader.Open(pdfPath, PdfDocumentOpenMode.Import);
            return doc.PageCount;
        }

        /// <summary>Builds a list of FileListRows suitable for Combine.Start.</summary>
        private static List<pdf_combine.PdfFileList.FileListRow> MakeFileRows(params string[] filePaths)
        {
            var dataset = new pdf_combine.PdfFileList();
            int order = 0;
            foreach (var filePath in filePaths)
            {
                var row = dataset.FileList.NewFileListRow();
                row.filePath = Path.GetDirectoryName(filePath)!;
                row.filename = Path.GetFileName(filePath);
                row.order = order++;
                dataset.FileList.AddFileListRow(row);
            }
            dataset.AcceptChanges();
            return dataset.FileList.Cast<pdf_combine.PdfFileList.FileListRow>().ToList();
        }

        // ------------------------------------------------------------------ Combine tests

        [Fact]
        public void Combine_TwoPdfs_ProducesFileWithCorrectPageCount()
        {
            var source1 = CreateTestPdf("source1.pdf", pageCount: 2);
            var source2 = CreateTestPdf("source2.pdf", pageCount: 3);
            var output = Path.Combine(_testDir, "combined.pdf");

            var rows = MakeFileRows(source1, source2);
            bool result = Combine.Start(rows, output);

            Assert.True(result, "Combine.Start should return true on success.");
            Assert.True(File.Exists(output), "Combined PDF should exist on disk.");
            Assert.Equal(5, GetPageCount(output));
        }

        [Fact]
        public void Combine_AddsPageFromSameFile_ProducesDoubleLengthFile()
        {
            // "The join test should involve adding a page to the current file"
            var source = CreateTestPdf("original.pdf", pageCount: 3);
            var output = Path.Combine(_testDir, "doubled.pdf");

            // Combine the file with itself – simulates appending a page to the current file
            var rows = MakeFileRows(source, source);
            bool result = Combine.Start(rows, output);

            Assert.True(result);
            Assert.Equal(6, GetPageCount(output));
        }

        // ------------------------------------------------------------------ Split tests

        [Fact]
        public void Split_ExtractsRequestedPages_CreatesOutputFiles()
        {
            var source = CreateTestPdf("to-split.pdf", pageCount: 5);
            // Extract pages 1 and 3 (0-based indices: 0 and 2)
            var pages = new List<int> { 0, 2 };

            bool result = Split.Start(pages, source);

            Assert.True(result, "Split.Start should return true on success.");

            string outputDir = Path.Combine(_testDir, "to-split");
            Assert.True(Directory.Exists(outputDir));
            Assert.True(File.Exists(Path.Combine(outputDir, "Page1.pdf")));
            Assert.True(File.Exists(Path.Combine(outputDir, "Page3.pdf")));
        }

        [Fact]
        public void Split_ExtractsRandomPageFromMultiPagePdf()
        {
            // "The split should take a page from a random location in the file"
            var source = CreateTestPdf("randsplit.pdf", pageCount: 10);
            var rng = new Random(42);
            int randomPage = rng.Next(0, 10);   // 0-based index
            var pages = new List<int> { randomPage };

            bool result = Split.Start(pages, source);

            Assert.True(result);
            string outputDir = Path.Combine(_testDir, "randsplit");
            Assert.True(File.Exists(Path.Combine(outputDir, $"Page{randomPage + 1}.pdf")));
        }

        // ------------------------------------------------------------------ Roundtrip (the bug scenario)

        [Fact]
        public void Split_CanOpenPdfProducedByCombine()
        {
            // Regression test for: "BUG: split can't load the same pdf that combine can"
            //
            // Steps:
            //   1. Create two source PDFs
            //   2. Combine them into a new PDF using Combine.Start
            //   3. Split the combined PDF using Split.Start  ← this was failing
            //   4. Assert that Split succeeds and produced the expected files

            var source1 = CreateTestPdf("s1.pdf", pageCount: 2);
            var source2 = CreateTestPdf("s2.pdf", pageCount: 2);
            var combined = Path.Combine(_testDir, "combined_for_split.pdf");

            bool combineResult = Combine.Start(MakeFileRows(source1, source2), combined);
            Assert.True(combineResult, "Combine step must succeed before testing Split.");

            // Extract page 3 (0-based index 2), which is in the middle of the combined PDF
            var pages = new List<int> { 2 };
            bool splitResult = Split.Start(pages, combined);

            Assert.True(splitResult, "Split.Start should succeed on a PDF created by Combine.Start.");

            string outputDir = Path.Combine(_testDir, "combined_for_split");
            Assert.True(File.Exists(Path.Combine(outputDir, "Page3.pdf")));
        }

        [Fact]
        public void SplitThenRecombine_DuplicatingPages_RoundtripSucceeds()
        {
            // "Create a test that will split a document and then recombine it with parts,
            //  duplicating some of its pages."

            // 1. Create a source document with 4 pages
            var source = CreateTestPdf("roundtrip.pdf", pageCount: 4);

            // 2. Split it: extract pages 1, 2, 3, 4 (all, 0-based: 0-3)
            var allPages = new List<int> { 0, 1, 2, 3 };
            bool splitResult = Split.Start(allPages, source);
            Assert.True(splitResult, "Split step should succeed.");

            string splitDir = Path.Combine(_testDir, "roundtrip");
            string page1 = Path.Combine(splitDir, "Page1.pdf");
            string page2 = Path.Combine(splitDir, "Page2.pdf");
            string page3 = Path.Combine(splitDir, "Page3.pdf");
            string page4 = Path.Combine(splitDir, "Page4.pdf");

            Assert.True(File.Exists(page1));
            Assert.True(File.Exists(page2));
            Assert.True(File.Exists(page3));
            Assert.True(File.Exists(page4));

            // 3. Recombine with duplicates: page1, page3, page1, page4
            //    (page1 appears twice – "duplicating some of its pages")
            var output = Path.Combine(_testDir, "recombined.pdf");
            var rows = MakeFileRows(page1, page3, page1, page4);
            bool combineResult = Combine.Start(rows, output);
            Assert.True(combineResult, "Recombine step should succeed.");
            Assert.Equal(4, GetPageCount(output));
        }

        [Fact]
        public void Split_CanOpenSameFileThatCombineUsedAsInput()
        {
            // Regression: Combine opens input files; those same files must remain openable by Split
            var source = CreateTestPdf("shared.pdf", pageCount: 3);
            var combineOutput = Path.Combine(_testDir, "combine_out.pdf");

            // Combine uses 'source' as input
            bool combineResult = Combine.Start(MakeFileRows(source, source), combineOutput);
            Assert.True(combineResult);

            // Split must also be able to open 'source' (no lingering file lock from Combine)
            var pages = new List<int> { 1 };
            bool splitResult = Split.Start(pages, source);
            Assert.True(splitResult, "Split should open the same file that Combine used as input.");
        }
    }
}
