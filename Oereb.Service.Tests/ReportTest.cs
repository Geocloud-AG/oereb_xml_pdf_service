using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oereb.Report;
using Oereb.Report.Helper;
using Oereb.Service.DataContracts;
using Telerik.Reporting;
using Telerik.Reporting.Processing;
using Geocentrale.Common;

namespace Oereb.Service.Tests
{
    [TestClass]
    public class ReportTest
    {
        [TestMethod]
        public void CreateReportV10()
        {
            //var file = Path.GetFullPath("../../Testfiles/CH607705073442_nw.xml");
            var file = Path.GetFullPath("../../Testfiles/v10_CH889434057724_nw_reduced.xml");

            try
            {
                var source = XElement.Load(file);
                var contentComplete = Oereb.Report.v10.ReportBuilder.GeneratePdf(source.ToString(), true, false);

                Assert.IsTrue(contentComplete.Length > 0);

                var pdfFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}_complete_embedded.pdf");

                using (var fileStream = new System.IO.FileStream(pdfFile, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    fileStream.Write(contentComplete, 0, contentComplete.Length);
                }

                var contentComplete2 = Oereb.Report.v10.ReportBuilder.GeneratePdf(source.ToString(), false, false);

                Assert.IsTrue(contentComplete2.Length > 0);

                var pdfFile2 = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}_reduced.pdf");

                using (var fileStream = new System.IO.FileStream(pdfFile2, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    fileStream.Write(contentComplete2, 0, contentComplete2.Length);
                }

                var contentComplete3 = Oereb.Report.v10.ReportBuilder.GeneratePdf(source.ToString(), true, true);

                Assert.IsTrue(contentComplete3.Length > 0);

                var pdfFile3 = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}_complete_attached.pdf");

                using (var fileStream = new System.IO.FileStream(pdfFile3, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    fileStream.Write(contentComplete3, 0, contentComplete3.Length);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        public void CreateReportV20()
        {
            //var file = Path.GetFullPath("../../Testfiles/CH607705073442_nw.xml");
            var file = Path.GetFullPath("../../Testfiles/v20_CH167807466221_ur_reduced.xml");

            try
            {
                var source = XElement.Load(file);                
                var contentComplete = Oereb.Report.v20.ReportBuilder.GeneratePdf(source.ToString());

                Assert.IsTrue(contentComplete.Length > 0);

                var pdfFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}_reduced.pdf");

                using (var fileStream = new System.IO.FileStream(pdfFile, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    fileStream.Write(contentComplete, 0, contentComplete.Length);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        public void AssignCData()
        {
            var file = Path.GetFullPath("../../Testfiles/CH710574347858_nw.xml");
            var source = XElement.Load(file);
            var converted = PreProcessing.AssignCDataToGmlNamespace(source);
            converted.Save(Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.xml"));
        }

        [TestMethod]
        public void ConvertPdfToByteArray()
        {
            var urlPdf = "https://www.gis-daten.ch/docs/gisdatenag/OEREB/BZR/BU_Bau-_und_Zonenreglement.pdf";
            var directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            Directory.CreateDirectory(directory);

            var filepath = Path.Combine(directory, "output.pdf");

            var check = Oereb.Report.Helper.Content.GetFromUrl(urlPdf, filepath);

            var fileArrays = Oereb.Report.Helper.Pdf.GetImagesFromPpdf(filepath);

            Assert.IsTrue(fileArrays.Count > 0);
        }

        [TestMethod]
        public void AttachFiletoPDF()
        {
            var urlPdf = "https://www.gis-daten.ch/docs/gisdatenag/OEREB/BZR/BU_Bau-_und_Zonenreglement.pdf";
            var directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            Directory.CreateDirectory(directory);

            var filepath = Path.Combine(directory, "output.pdf");

            var check = Oereb.Report.Helper.Content.GetFromUrl(urlPdf, filepath);

            var packages = new List<FileContainer>();

            packages.Add(new FileContainer()
            {
                Description = "Anhang 1",
                FilePath = filepath,
                ContentType = "application/pdf"
            });

            Oereb.Report.Helper.Pdf.AddAttachments(filepath, Path.Combine(directory, "output_attached.pdf"), packages);
        }

        [TestMethod]
        public void CreateReportExternV10()
        {
            //var file = Path.GetFullPath("../../Testfiles/_extern_ZH_CH607722082391.xml");
            //var file = Path.GetFullPath("../../Testfiles/_extern_ZH_CH499977299153.xml");
            var file = Path.GetFullPath("../../Testfiles/_extern_ZH_CH607722082391_notvalid.xml"); //some responsible offices are missing

            var source = XElement.Load(file).ToString();

            if (source.Contains("GetExtractByIdResponse"))
            {
                var document = XElement.Parse(source);
                source = document.InnerXML();
            }

            var contentComplete = Oereb.Report.v10.ReportBuilder.GeneratePdf(source, false, false);

            Assert.IsTrue(contentComplete.Length > 0);

            var pdfFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}_reduced.pdf");

            using (var fileStream = new System.IO.FileStream(pdfFile, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                fileStream.Write(contentComplete, 0, contentComplete.Length);
            }
        }

        [TestMethod]
        public void CreateReportExternV20()
        {
            //var file = Path.GetFullPath("../../Testfiles/_extern_ZH_CH607722082391.xml");
            //var file = Path.GetFullPath("../../Testfiles/_extern_ZH_CH499977299153.xml");
            //var file = Path.GetFullPath("../../Testfiles/_extern_ZH_CH607722082391_notvalid.xml"); //some responsible offices are missing
            var file = Path.GetFullPath("../../Testfiles/v20_CH167807466221_ur_reduced.xml"); 

            var source = XElement.Load(file).ToString();

            if (source.Contains("GetExtractByIdResponse"))
            {
                var document = XElement.Parse(source);
                source = document.InnerXML();
            }

            var contentComplete = Oereb.Report.v20.ReportBuilder.GeneratePdf(source);

            Assert.IsTrue(contentComplete.Length > 0);

            var pdfFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}_reduced.pdf");

            using (var fileStream = new System.IO.FileStream(pdfFile, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                fileStream.Write(contentComplete, 0, contentComplete.Length);
            }
        }
    }
}
