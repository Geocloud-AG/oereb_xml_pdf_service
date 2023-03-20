using Aspose.Words;
using Aspose.Words.Saving;
using DocumentFormat.OpenXml.Vml.Office;
using Oereb.Report.Helper;
using Oereb.Service.DataContracts;
using Oereb.Service.DataContracts.Model.v20;
using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Xml.Linq;
using Telerik.Reporting;
using Telerik.Reporting.Processing;

namespace Oereb.Report.v20
{
    public class ReportBuilder
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// generate the report from xml
        /// </summary>
        /// <param name="xmlContent">valid oereb extract</param>
        /// <param name="format">format telerik export (pdf|docx) </param>
        /// <returns></returns>
        public static byte[] Generate(string xmlContent, string format, bool useWms = false)
        {
            var reportBody = new ReportBody();
            var reportGlossary = new ReportGlossary();
            var reportTitle = new ReportTitle();
            var reportToc = new ReportToc();

            if (xmlContent[0] == 65279)
            {
                xmlContent = xmlContent.Substring(1);
            }

            var source = XElement.Parse(xmlContent);

            var converted = PreProcessing.AssignCDataToGmlNamespace(source);
            var extract = Xml<Extract>.DeserializeFromXmlString(converted.ToString());

            var attestation = Convert.ToBoolean(ConfigurationManager.AppSettings["attestation"] ?? "true");

            var reportExtract = new ReportExtract(attestation, useWms);
            reportExtract.Extract = extract;
            reportExtract.Ini();

            //**************************************************************************************************************************************
            //report title, toc, glossary

            var objectDataSource = new ObjectDataSource();
            objectDataSource.DataSource = typeof(ReportExtract);
            objectDataSource.DataMember = "GetReportByExtract";
            objectDataSource.Parameters.Add(new ObjectDataSourceParameter("reportExtract", typeof(ReportExtract), reportExtract));

            reportTitle.DataSource = objectDataSource;
            reportTitle.table1.DataSource = objectDataSource;
            reportTitle.table2.DataSource = objectDataSource;
            reportToc.DataSource = objectDataSource;
            reportGlossary.DataSource = objectDataSource;

            //**************************************************************************************************************************************
            //report body

            var objectDataSourceBody = new ObjectDataSource();
            objectDataSourceBody.DataSource = typeof(ReportExtract);
            objectDataSourceBody.DataMember = "GetBodyItemsByExtract";
            objectDataSourceBody.Parameters.Add(new ObjectDataSourceParameter("reportExtract", typeof(ReportExtract), reportExtract));

            reportBody.DataSource = objectDataSourceBody;

            var currentpage = Helper.Report.GetPageCountFromReport(reportTitle) + Helper.Report.GetPageCountFromReport(reportToc) + 1;

            for (int i = 0; i < reportExtract.BodySectionCount; i++)
            {
                reportExtract.BodySectionFlag = i;

                reportExtract.Toc.TocItems[i].Page = currentpage;
                reportExtract.IniReportBody();

                currentpage += Helper.Report.GetPageCountFromReport(reportBody);
            }

            reportExtract.BodySectionFlag = -1;
            reportExtract.IniReportBody();

            //**************************************************************************************************************************************
            //todo multilanguage
            Helper.Report.AddBookmark(reportTitle, "Titelblatt");
            Helper.Report.AddBookmark(reportToc, "Inhaltsverzeichnis");
            Helper.Report.AddBookmark(reportBody, "Eigentumsbeschränkung");
            Helper.Report.AddBookmark(reportGlossary, "Glossar");

            //**************************************************************************************************************************************
            //merge report and output

            var reportBook = new ReportBook();
            reportBook.Reports.Add(reportTitle);
            reportBook.Reports.Add(reportToc);
            reportBook.Reports.Add(reportBody);
            reportBook.Reports.Add(reportGlossary);

            var reportProcessor = new ReportProcessor();
            var instanceReportSource = new InstanceReportSource();

            instanceReportSource.ReportDocument = reportBook;
            var result = reportProcessor.RenderReport(format, instanceReportSource, null);

            return result.DocumentBytes;
        }

        public static byte[] GeneratePdf(string xmlContent, bool useWms = false)
        {
            /*
            string format = "pdf";
            return Generate(xmlContent, format, useWms);
            */

            var docContent = Generate(xmlContent, "docx", useWms);

            Stream streamDoc = new MemoryStream(docContent);

            var license = new Aspose.Words.License();
            license.SetLicense("License/Aspose.Words.lic");

            var doc = new Aspose.Words.Document(streamDoc);

            var options = new PdfSaveOptions
            {
                SaveFormat = SaveFormat.Pdf,
                TextCompression = PdfTextCompression.Flate,
                JpegQuality = 60,
                Compliance = PdfCompliance.PdfA1a
            };

            var streamPdf = new MemoryStream();
            doc.Save(streamPdf, options);
            streamPdf.Position = 0;

            return ByteArrayFromStream(streamPdf);
        }

        private static byte[] ByteArrayFromStream(Stream stream)
        {
            byte[] buffer = new byte[16 * 1024];

            using (var memoryStream = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    memoryStream.Write(buffer, 0, read);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
