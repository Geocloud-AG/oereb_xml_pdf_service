using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Oereb.Report.Helper;
using Oereb.Service.DataContracts;
using Telerik.Reporting;
using Telerik.Reporting.Processing;
using System.IO;

namespace Oereb.Report.v10
{
    public class ReportBuilder
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// generate the report from xml
        /// </summary>
        /// <param name="xmlContent">valid oereb extract</param>
        /// <param name="format">format telerik export (pdf|docx) </param>
        /// <param name="complete">get the full pdf, incl. appendix</param>
        /// <param name="attachedFiles">true: attach the appendix as files in the pdf; false: convert the references pdf's to bitmap and include</param>
        /// <returns></returns>
        public static byte[] Generate(string xmlContent, string format, bool complete = true, bool attachedFiles = false, bool useWms = false)
        {
            var reportBody = new ReportBody();
            var reportGlossary = new ReportGlossary();
            var reportTitle = new ReportTitle();
            var reportToc = new ReportToc();
            var reportAppendix = new ReportAppendix();

            //var filename = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.xml");
            //File.WriteAllText(filename, xmlContent, Encoding.Unicode);
            //var source = XElement.Load(filename);

            if (xmlContent[0] == 65279)
            {
                xmlContent = xmlContent.Substring(1);
            }

            var source = XElement.Parse(xmlContent);

            var converted = PreProcessing.AssignCDataToGmlNamespace(source);
            var extract = Xml<Oereb.Service.DataContracts.Model.v10.Extract>.DeserializeFromXmlString(converted.ToString());

            var attestation = Convert.ToBoolean(ConfigurationManager.AppSettings["attestation"] ?? "false");

            var reportExtract = new ReportExtract(complete, attachedFiles, attestation, useWms);
            reportExtract.Extract = extract;
            //if (reportExtract.Extract.RealEstate.RestrictionOnLandownership == null) {
            //    reportExtract.Extract.RealEstate.RestrictionOnLandownership = new Service.DataContracts.Model.v10.RestrictionOnLandownership[0];
            //}
            reportExtract.Ini();

            //**************************************************************************************************************************************
            //report body

            var objectDataSourceBody = new Telerik.Reporting.ObjectDataSource();
            objectDataSourceBody.DataSource = typeof(ReportExtract);
            objectDataSourceBody.DataMember = "GetBodyItemsByExtract";
            objectDataSourceBody.Parameters.Add(new ObjectDataSourceParameter("reportExtract", typeof(ReportExtract), reportExtract));

            reportBody.DataSource = objectDataSourceBody;

            int currentpage = 3; //title and toc has only 1 page

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
            //report title, toc, glossary

            var objectDataSource = new Telerik.Reporting.ObjectDataSource();
            objectDataSource.DataSource = typeof(ReportExtract);
            objectDataSource.DataMember = "GetReportByExtract";
            objectDataSource.Parameters.Add(new ObjectDataSourceParameter("reportExtract", typeof(ReportExtract), reportExtract));

            reportGlossary.DataSource = objectDataSource;
            reportTitle.DataSource = objectDataSource;
            reportTitle.table1.DataSource = objectDataSource;
            reportTitle.table2.DataSource = objectDataSource;
            reportToc.DataSource = objectDataSource;
            reportAppendix.DataSource = objectDataSource;

            //todo multilanguage
            Helper.Report.AddBookmark(reportTitle, "Titelblatt");
            Helper.Report.AddBookmark(reportGlossary, "Glossar");
            Helper.Report.AddBookmark(reportToc, "Inhaltsverzeichnis");
            Helper.Report.AddBookmark(reportAppendix, "Anhang");
            Helper.Report.AddBookmark(reportBody, "Eigentumsbeschränkung");

            //**************************************************************************************************************************************
            //merge report and output

            var reportBook = new ReportBook();
            reportBook.Reports.Add(reportTitle);
            reportBook.Reports.Add(reportToc);
            reportBook.Reports.Add(reportBody);
            reportBook.Reports.Add(reportGlossary);

            if (reportExtract.ExtractComplete && !reportExtract.AttacheFiles)
            {
                reportBook.Reports.Add(reportAppendix);
            }

            var reportProcessor = new ReportProcessor();
            var instanceReportSource = new InstanceReportSource();

            instanceReportSource.ReportDocument = reportBook;
            var result = reportProcessor.RenderReport(format, instanceReportSource, null);

            if (reportExtract.ExtractComplete && reportExtract.AttacheFiles)
            {
                try
                {
                    var guid = Guid.NewGuid();
                    var pdfFile = Path.Combine(Path.GetTempPath(), $"{guid}.pdf");
                    var pdfFileAttached = Path.Combine(Path.GetTempPath(), $"{guid}_attached.pdf");

                    using (var fileStream = new System.IO.FileStream(pdfFile, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    {
                        fileStream.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                    }

                    var fileContainers = new List<FileContainer>();

                    foreach (var tocAppendix in reportExtract.TocAppendixes)
                    {
                        if (!tocAppendix.State)
                        {
                            continue; //todo what happen if this is not a pdf
                        }

                        fileContainers.Add( new FileContainer()
                        {
                            FilePath = tocAppendix.Filename,
                            Description = $"{tocAppendix.Shortname}_{tocAppendix.FileDescription}",
                            ContentType = tocAppendix.ContentType
                        });
                    }

                    Oereb.Report.Helper.Pdf.AddAttachments(pdfFile, pdfFileAttached, fileContainers);

                    return File.ReadAllBytes(pdfFileAttached);
                }
                catch (Exception ex)
                {
                    Log.Error($"error attache files, {ex.Message}");
                    throw ex;
                }
            }
            else
            {
                return result.DocumentBytes;
            }
        }

        public static byte[] GeneratePdf(string xmlContent, bool complete = true, bool attachedFiles = false, bool useWms = false)
        {
            string format = "pdf";
            return Generate(xmlContent, format, complete, attachedFiles, useWms);
        }

    }
}
