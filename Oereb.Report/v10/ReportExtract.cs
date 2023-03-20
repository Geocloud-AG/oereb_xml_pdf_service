using Oereb.Report.Helper;
using Oereb.Report.Helper.Exceptions;
using Oereb.Service.DataContracts.Model.v10;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace Oereb.Report.v10
{
    /// <summary>
    /// mapping class, from xml to report
    /// </summary>

    [DataObject]
    public class ReportExtract
    {
        public Service.DataContracts.Model.v10.Extract Extract { get; set; }
        public string Language { get; set; } = "de";

        public TocRegion Toc { get; set; }
        public List<TocAppendix> TocAppendixes { get; set; }
        public int AppendixCounter { get; set; } = 1;

        public bool ExtractComplete { get; set; } = true;

        public bool Attestation { get; set; }

        public bool UseWms { get; set; }

        public string Title {
            get
            {
                var ResourceManager = new System.Resources.ResourceManager("Oereb.Report.v10.ReportTitle", typeof(ReportTitle).Assembly);

                return !ExtractComplete
                    ? ResourceManager.GetString("PageTitleReduced")
                    : ResourceManager.GetString("PageTitle");
            }
        }

        public bool AttacheFiles { get; set; } = false;

        public List<BodyItem> ReportBodyItems { get; set; }
        public List<GlossaryItem> GlossaryItems { get; set; }

        public Image ImageTitle { get; set; }
        public GeoreferenceExtension GeoreferenceExtensionTitle { get; set; }
        public ImageExtension ImageRestrictionOnLandownership { get; set; }
        public List<ImageExtension> AdditionalLayers { get; set; }

        public string PlrCadastreAuthority
        {
            get
            {
                var office = Extract.PLRCadastreAuthority;
                return $"{Helper.v10.LocalisedText.GetStringFromArray(office.Name, Language)}, {office.Street} {office.Number}, {office.PostalCode} {office.City}";
            }
        }

        public int BodySectionCount => Extract.RealEstate.RestrictionOnLandownership != null ? Extract.RealEstate.RestrictionOnLandownership.GroupBy(x => x.Theme.Code + "|" +x.SubTheme ?? "").ToList().Count : 0;

        public int BodySectionFlag { get; set; } = -1;

        public ReportExtract()
        {
        }

        public ReportExtract(bool extractComplete, bool attacheFiles, bool attestation, bool useWms)
        {
            ExtractComplete = extractComplete;
            AttacheFiles = attacheFiles;
            Attestation = attestation;
            UseWms = useWms;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<BodyItem> GetBodyItems()
        {
            //path for the designer preview in visual studio
            //Extract = Xml<Service.DataContracts.Model.v10.Extract>.DeserializeFromFile(@"...");
            Ini();
            return ReportBodyItems;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public ReportExtract GetReportExtract()
        {
            //path for the designer preview in visual studio
            //Extract = Xml<Service.DataContracts.Model.v10.Extract>.DeserializeFromFile(@"...");
            Ini();
            return this;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<BodyItem> GetBodyItemsByExtract(ReportExtract reportExtract)
        {
            return reportExtract.ReportBodyItems;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public ReportExtract GetReportByExtract(ReportExtract reportExtract)
        {
            return reportExtract;
        }

        public void Ini()
        {
            AdditionalLayers = new List<ImageExtension>();

            //ini title image
            if (!UseWms && Extract.RealEstate.PlanForLandRegisterMainPage.Image == null) throw new ImageFromXmlException("The image embedded in XML under <Extract><RealEstate><PlanForLandRegisterMainPage><Image> is missing or cannot be read (vector formats are not yet supported).");
            var imageBg = UseWms ? Helper.v10.Wms.GetMap(Extract.RealEstate.PlanForLandRegisterMainPage.ReferenceWMS)  : PreProcessing.GetImageFromByteArray(Extract.RealEstate.PlanForLandRegisterMainPage.Image);
            var image = new Bitmap(imageBg.Width, imageBg.Height, PixelFormat.Format32bppArgb);

            image = PreProcessing.MergeTwoImages(image, imageBg);

            var georeferenceExtention = new GeoreferenceExtension() {};
            georeferenceExtention.Extent = null;
            georeferenceExtention.Seq = 0;
            georeferenceExtention.Transparency = 0;

            if (Extract.RealEstate.PlanForLandRegisterMainPage.ItemsElementName != null && Extract.RealEstate.PlanForLandRegisterMainPage.ItemsElementName.Length > 0)
            {
                georeferenceExtention = GetGeoreferenceExtension(Extract.RealEstate.PlanForLandRegisterMainPage.ItemsElementName, Extract.RealEstate.PlanForLandRegisterMainPage.Items);

                if (!String.IsNullOrEmpty(Extract.RealEstate.Limit))
                {
                    var offsetBorderTitle = Convert.ToInt32(ConfigurationManager.AppSettings["offsetBorderTitle"] ?? "0");

                    var parcelHighligthed = Helper.Geometry.RasterizeGeometryFromGml(
                        Extract.RealEstate.Limit,
                        new double[] { georeferenceExtention.Extent.Xmin, georeferenceExtention.Extent.Ymin, georeferenceExtention.Extent.Xmax, georeferenceExtention.Extent.Ymax },
                        image.Width,
                        image.Height,
                        offsetBorderTitle
                    );

                    image = PreProcessing.MergeTwoImages(image, parcelHighligthed);
                }
            }

            ImageTitle = image;

            //ini rol images and additionl layers

            var titleExtention = Extract.RealEstate.PlanForLandRegister.extensions?.Any.FirstOrDefault(x => x.LocalName == "MapExtension");

            if (titleExtention != null)
            {
                var titleExtentionElement = XElement.Parse(titleExtention.OuterXml);
            }

            var realEstateExtension = Extract.RealEstate.extensions?.Any.FirstOrDefault(x => x.LocalName == "RealEstateExtension");

            if (realEstateExtension != null && UseWms)
            {
                throw new Exception("additional layers are not supported with the wms option");
            }

            if (realEstateExtension != null && !UseWms)
            {
                var realEstateElement = XElement.Parse(realEstateExtension.OuterXml);

                foreach (var additionalLayerElement in realEstateElement.Elements("AdditionalLayer"))
                {
                    var item = GetImageExtension(additionalLayerElement);

                    if (item == null)
                    {
                        continue;
                    }

                    AdditionalLayers.Add(item);
                }
            }

            ImageRestrictionOnLandownership = new ImageExtension()
            {
                Image = UseWms ? Helper.v10.Wms.GetMap(Extract.RealEstate.PlanForLandRegister.ReferenceWMS) : PreProcessing.GetImageFromByteArray(Extract.RealEstate.PlanForLandRegister.Image),
                Extent = georeferenceExtention.Extent,
                Seq = System.Convert.ToInt32(Extract.RealEstate.PlanForLandRegister.layerIndex),
                Transparency = 1 - Extract.RealEstate.PlanForLandRegister.layerOpacity
            };

            if (Math.Abs(ImageRestrictionOnLandownership.Transparency) > 0.98)
            {
                //todo add warning to log, make no sense
                ImageRestrictionOnLandownership.Transparency = 0;
            }

            IniReportBody();
            IniReportGlossary();
            IniToc(Extract, Language);
        }

        #region General

        public DescriptionExtension GetDescriptionExtension(XElement extension)
        {
            var descriptionExtension = new DescriptionExtension();

            descriptionExtension.Seq = extension.Element("Seq") == null ? 5 : System.Convert.ToInt32(extension.Element("Seq").Value);
            descriptionExtension.Transparency = extension.Element("Transparency") == null ? 0.5 : System.Convert.ToDouble(extension.Element("Transparency").Value);

            return descriptionExtension;
        }

        public GeoreferenceExtension GetGeoreferenceExtension(ItemsChoiceType[] itemsElement, string[] items)
        {
            if (!itemsElement.Any() || !items.Any())
            {
                return null;
            }

            var georeferenceExtension = new GeoreferenceExtension();
            georeferenceExtension.Extent = new Extent();

            //<gml:pos>{x} {y}</gml:pos>

            Regex regex = new Regex("<gml:pos>(.*)</gml:pos>");

            if (itemsElement[0] == ItemsChoiceType.min_NS03)
            {
                georeferenceExtension.Extent.Crs = 21781;
            }
            else
            {
                georeferenceExtension.Extent.Crs = 2056;
            }

            var vmin = regex.Match(items[0]);

            if (!vmin.Success || vmin.Groups.Count != 2)
            {
                return null;
            }

            var valueMin = vmin.Groups[1].ToString();
            var valueMinParts = valueMin.Trim().Split(' ');

            var vmax = regex.Match(items[1]);

            if (!vmax.Success || vmax.Groups.Count != 2)
            {
                return null;
            }

            var valueMax = vmax.Groups[1].ToString();
            var valueMaxParts = valueMax.Trim().Split(' ');

            if (valueMinParts.Length != 2 || valueMaxParts.Length != 2)
            {
                return null;
            }

            double xmin, ymin, xmax, ymax;

            if (!(
                double.TryParse(valueMinParts[0], out xmin) && 
                double.TryParse(valueMinParts[1], out ymin) && 
                double.TryParse(valueMaxParts[0], out xmax) && 
                double.TryParse(valueMaxParts[1], out ymax)
                ))
            {
                return null;
            }

            georeferenceExtension.Extent.Xmin = xmin;
            georeferenceExtension.Extent.Ymin = ymin;
            georeferenceExtension.Extent.Xmax = xmax;
            georeferenceExtension.Extent.Ymax = ymax;

            return georeferenceExtension;
        }

        public GeoreferenceExtension GetGeoreferenceExtension(XElement extension)
        {
            if (extension.Element("Extent") == null ||
                extension.Element("Extent").Element("Xmin") == null || extension.Element("Extent").Element("Ymin") == null ||
                extension.Element("Extent").Element("Xmax") == null || extension.Element("Extent").Element("Ymax") == null)
            {
                return null;
            }

            var georeferenceExtension = new GeoreferenceExtension();

            georeferenceExtension.Extent = new Extent();
            georeferenceExtension.Extent.Xmin = System.Convert.ToDouble(extension.Element("Extent").Element("Xmin").Value);
            georeferenceExtension.Extent.Ymin = System.Convert.ToDouble(extension.Element("Extent").Element("Ymin").Value);
            georeferenceExtension.Extent.Xmax = System.Convert.ToDouble(extension.Element("Extent").Element("Xmax").Value);
            georeferenceExtension.Extent.Ymax = System.Convert.ToDouble(extension.Element("Extent").Element("Ymax").Value);
            georeferenceExtension.SetDescription(GetDescriptionExtension(extension));

            return georeferenceExtension;
        }

        public ImageExtension GetImageExtension(XElement extension)
        {
            if (extension.Element("Image") == null || extension.Element("Extent")== null || 
                extension.Element("Extent").Element("Xmin") == null || extension.Element("Extent").Element("Ymin") == null || 
                extension.Element("Extent").Element("Xmax") == null || extension.Element("Extent").Element("Ymax") == null)
            {
                return null;
            }

            var imageExtension = new ImageExtension();

            imageExtension.Image = Helper.HImage.Base64ToImage(extension.Element("Image").Value);
            imageExtension.SetGeoreference(GetGeoreferenceExtension(extension));
            imageExtension.Name = extension.Element("Topicname") == null ? "" : extension.Element("Topicname").Value;
            return imageExtension;
        }

        public static GeometryExtension GetGeometryExtension(XElement extension)
        {
            return new GeometryExtension
            {
                Type = extension.Element("Type") == null ? "Unknown" : extension.Element("Type").Value
            };
        }

        public class DescriptionExtension
        {
            public int Seq { get; set; }
            public double Transparency { get; set; }
        }

        public class GeoreferenceExtension : DescriptionExtension
        {
            public Extent Extent { get; set; }

            public void SetDescription(DescriptionExtension descriptionExtension)
            {
                Seq = descriptionExtension.Seq;
                Transparency = descriptionExtension.Transparency;
            }
        }

        public class ImageExtension : GeoreferenceExtension
        {
            public string Name { get; set; }
            public Image Image { get; set; }

            public void SetGeoreference(GeoreferenceExtension georeferenceExtension)
            {
                Extent = georeferenceExtension.Extent;
                Transparency = georeferenceExtension.Transparency;
                Seq = georeferenceExtension.Seq;
            }
        }

        public class GeometryExtension
        {
            public string Type { get; set; }
        }

        public class Extent
        {
            public double Xmin { get; set; }
            public double Ymin { get; set; }
            public double Xmax { get; set; }
            public double Ymax { get; set; }
            public int Crs { get; set; }
        }

        #endregion

        #region TOC

        public void IniToc(Service.DataContracts.Model.v10.Extract extract, string language )
        {
            Toc = new TocRegion();
            TocAppendixes = new List<TocAppendix>();
            Toc.Extract = extract;
            Toc.Language = language;

            Toc.GeneralInformation = Helper.v10.LocalisedMText.GetStringFromArray(Extract.GeneralInformation, Language);
            Toc.BaseData = Helper.v10.LocalisedMText.GetStringFromArray(Extract.BaseData, Language);

            if (Extract.ExclusionOfLiability != null)
            {
                Toc.ExclusionOfLiabilityTitle = Extract.ExclusionOfLiability
                    .Select(x => Helper.v10.LocalisedText.GetStringFromArray(x.Title, Language)).First();
                Toc.ExclusionOfLiabilityContent = Extract.ExclusionOfLiability
                    .Select(x => Helper.v10.LocalisedMText.GetStringFromArray(x.Content, Language)).First();
            }

            var groupedBodyItems = ReportBodyItems.GroupBy(x => x.Theme).ToList(); // Extract.RealEstate.RestrictionOnLandownership.GroupBy(x => x.Theme.Code).ToList();

            foreach (var bodyItem in groupedBodyItems)
            {
                var tocItem = new TocItem()
                {
                    Page = 0,
                    Label = bodyItem.First().Theme,
                    Appendixes = new List<TocAppendix>()
                };

                if (ExtractComplete)
                {
                    foreach (var item in bodyItem.ToList())
                    {
                        foreach (var legalProvision in item.LegalProvisions)
                        {
                            if (string.IsNullOrEmpty(legalProvision.Url))
                            {
                                continue; //an empty url is possible
                            }

                            var tocAppendix = new TocAppendix()
                            {
                                Key = legalProvision.Title,
                                Shortname = $"A{AppendixCounter}",
                                Description = legalProvision.Title,
                                FileDescription = legalProvision.Title,
                                Url = WebUtility.HtmlEncode(WebUtility.UrlDecode(legalProvision.Url))
                            };

                            if (TocAppendixes.Any(x => x.Url == tocAppendix.Url && x.Key == tocAppendix.Key))
                            {
                                tocAppendix = TocAppendixes.First(x => x.Url == tocAppendix.Url && x.Key == tocAppendix.Key);
                                tocItem.Appendixes.Add(tocAppendix);
                                continue;
                            }

                            var urlFile = tocAppendix.Url;
                            var directory = Path.Combine(Path.GetTempPath(), $"_TempFile_{Guid.NewGuid()}");

                            Directory.CreateDirectory(directory);

                            var filepath = Path.Combine(directory, "output.bin");
                            tocAppendix.Filename = filepath;

                            var result = Oereb.Report.Helper.Content.GetFromUrl(urlFile, filepath);

                            tocAppendix.ContentType = result.ContentType;
                            tocAppendix.State = result.Successful;

                            if (AttacheFiles && result.Successful)
                            {
                                // tocAppendix.Description += " (siehe Anhang)";
                            }
                            else if (AttacheFiles && !result.Successful)
                            {
                                // tocAppendix.Description += " (nicht anhängbar)";
                                throw new AttachmentRequestException(urlFile, "Attachment failed.");
                            }
                            else if (result.Successful)
                            {
                                try
                                {
                                    var files = Oereb.Report.Helper.Pdf.GetImagesFromPpdf(filepath);
                                    tocAppendix.Pages.AddRange(files);
                                }
                                catch (Exception ex)
                                {
                                    throw new ImageConversionException(filepath, "Cannot extract images from PDF. Attachment failed.");
                                }
                            }

                            tocItem.Appendixes.Add(tocAppendix);
                            TocAppendixes.Add(tocAppendix);
                            AppendixCounter++;
                        }
                    }
                }

                Toc.TocItems.Add(tocItem);
            }

            if (Extract.NotConcernedTheme != null)
            {
                foreach (var notConcernedTheme in Extract.NotConcernedTheme)
                {
                    var localisedText = new Service.DataContracts.Model.v10.LocalisedText[] { notConcernedTheme.Text };
                    var label = Helper.v10.LocalisedText.GetStringFromArray(localisedText, Language);
                    Toc.ThemeNotConcerned.Add(new TocItemTheme() { Label = label });
                }
            }

            if (Extract.ThemeWithoutData != null)
            {
                foreach (var themeWithoutData in Extract.ThemeWithoutData)
                {
                    var localisedText = new Service.DataContracts.Model.v10.LocalisedText[] { themeWithoutData.Text };
                    var label = Helper.v10.LocalisedText.GetStringFromArray(localisedText, Language);
                    Toc.ThemeWithoutData.Add(new TocItemTheme() { Label = label });
                }
            }
        }

        public class TocRegion
        {
            public Service.DataContracts.Model.v10.Extract Extract { get; set; }
            public string Language { get; set; }

            public List<TocItem> TocItems { get; set; }
            public List<TocItemTheme> ThemeNotConcerned { get; set; }
            public List<TocItemTheme> ThemeWithoutData { get; set; }

            public string GeneralInformation { get; set; }
            public string BaseData { get; set; }
            public string ExclusionOfLiabilityTitle { get; set; }
            public string ExclusionOfLiabilityContent { get; set; }

            public TocRegion()
            {
                ThemeWithoutData = new List<TocItemTheme>();
                ThemeNotConcerned = new List<TocItemTheme>();
                TocItems = new List<TocItem>();
            }
        }

        public class TocItem
        {
            public int Page { get; set; }
            public string Label { get; set; }
            public List<TocAppendix> Appendixes { get; set; }

            public TocItem()
            {
                Appendixes = new List<TocAppendix>();
            }
        }

        public class TocAppendix
        {
            public string Shortname { get; set; }
            public string Description { get; set; }
            public string Url { get; set; }
            public List<byte[]> Pages { get; set; }
            public string Filename { get; set; }
            public string Key { get; set; }
            public bool State { get; set; }
            public string ContentType { get; set; }
            public string FileDescription { get; set; }

            public TocAppendix()
            {
                Pages = new List<byte[]>();
            }
        }

        public class TocItemTheme
        {
            public string Label { get; set; }
        }

        #endregion

        #region report body

        public void IniReportBody()
        {
            ReportBodyItems = new List<BodyItem>();

            if (Extract.RealEstate.RestrictionOnLandownership != null)
            {
                var groupedBodyItems = Extract.RealEstate.RestrictionOnLandownership.GroupBy(x => x.Theme.Code + "|" + x.SubTheme ?? "").ToList();

                int section = -1;
                int appendixCounter = 1;

                foreach (var bodyItem in groupedBodyItems)
                {
                    section++;

                    if (!(BodySectionFlag == section || BodySectionFlag == -1))
                    {
                        continue;
                    }

                    var reportBodyItem = new BodyItem(Extract, bodyItem.ToList(), Language, ImageRestrictionOnLandownership, AdditionalLayers, UseWms, appendixCounter, ExtractComplete);
                    appendixCounter += reportBodyItem.Appendixes.Count;
                    ReportBodyItems.Add(reportBodyItem);
                }
            }
        }

        public class BodyItem
        {
            public byte[] FederalLogo { get; set; }
            public byte[] CantonalLogo { get; set; }
            public byte[] MunicipalityLogo { get; set; }
            public byte[] LogoPLRCadastre { get; set; }
            public string Theme { get; set; }
            public Image Image { get; set; }
            public List<LegendItem> LegendItems { get; set; }
            public List<LegendItem> LegendItemsNotInvolved
            {
                get
                {
                    var legendItemsNotInvolved = new List<LegendItem>();

                    foreach (var legendItem in LegendItems)
                    {
                        if (LegendItemsInvolved.Any(x => legendItem.TypeCode == x.TypeCode))
                        {
                            continue; //involved
                        }

                        if (legendItemsNotInvolved.Any(x => legendItem.TypeCode == x.TypeCode))
                        {
                            continue; //already exist
                        }
                        legendItemsNotInvolved.Add(legendItem);
                    }

                    return legendItemsNotInvolved;
                }
            }
            public List<LegendItemInvolved> LegendItemsInvolved { get; set; }
            public List<LegendAtWeb> LegendAtWeb { get; set; }
            public bool VisibleLegendAtWeb => LegendAtWeb.Any();
            public List<Document> LegalProvisions { get; set; }
            public List<Document> Documents { get; set; }
            public List<Document> MoreInformations { get; set; }
            public List<ResponsibleOffice> ResponsibleOffice { get; set; }
            public string ExtractIdentifier { get; set; }
            public DateTime CreationDate { get; set; }
            public List<TocAppendix> Appendixes { get; set; }

            public BodyItem(Service.DataContracts.Model.v10.Extract extract, List<Service.DataContracts.Model.v10.RestrictionOnLandownership> restrictionOnLandownership, string language, ImageExtension baselayer, List<ImageExtension> additionalLayers, bool useWms = false, int appendixCounter = 1, bool hasAppendixes = false)
            {
                #region initialization

                LegendItems = new List<LegendItem>();
                LegendItemsInvolved = new List<LegendItemInvolved>();
                LegendAtWeb = new List<LegendAtWeb>();

                LegalProvisions = new List<Document>();
                Documents = new List<Document>();
                MoreInformations = new List<Document>();
                //MoreInformations = new List<Document>()
                //{
                //    new Document() {Abbrevation = "", Level = 0, OfficialNumber = "-", OfficialTitle = "", Title = "", Url =""}
                //};

                ResponsibleOffice = new List<ResponsibleOffice>();
                Appendixes = new List<TocAppendix>();

                #endregion

                FederalLogo = GetImageAsBytes(extract.Item1);
                CantonalLogo = GetImageAsBytes(extract.Item2);
                MunicipalityLogo = GetImageAsBytes(extract.Item3);
                LogoPLRCadastre = GetImageAsBytes(extract.Item);
                ExtractIdentifier = extract.ExtractIdentifier;
                CreationDate = extract.CreationDate;

                var rolLayers = new List<ImageExtension>();
                rolLayers.Add(baselayer);

                if (restrictionOnLandownership != null && restrictionOnLandownership.Count > 0)
                {
                    Theme = string.IsNullOrEmpty(restrictionOnLandownership.First().SubTheme) ? restrictionOnLandownership.First().Theme.Text.Text : restrictionOnLandownership.First().Theme.Text.Text + ": " + restrictionOnLandownership.First().SubTheme;
                    if (!useWms && restrictionOnLandownership.First().Map.Image == null) throw new ImageFromXmlException("The image embedded in XML under <RestrictionOnLandownership><Map><Image> is missing or cannot be read (vector formats are not yet supported).");
                    var image = useWms ? Helper.v10.Wms.GetMap(restrictionOnLandownership.First().Map.ReferenceWMS) : PreProcessing.GetImageFromByteArray(restrictionOnLandownership.First().Map.Image); //take only with and height from image
                    Image = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);

                    rolLayers.AddRange(additionalLayers.Where(x => x.Name == Theme));
                }
                else
                {
                    Theme = "";
                }

                var guidDSs = new List<string>();

                if (restrictionOnLandownership != null)
                {
                    foreach (var restriction in restrictionOnLandownership)
                    {
                        if (restriction.TypeCode == null)
                        {
                            //Todo oereb ur typecode == null, this should not be, log warning
                            continue;
                        }

                        var guidDSPart = restriction.TypeCode.Split(':');
                        var guidDS = "";

                        if (guidDSPart.Length == 3)
                        {
                            guidDS = guidDSPart[0].Substring(0, 8);
                        }

                        if (guidDSs.Contains(guidDS))
                        {
                            continue;
                        }
                        else
                        {
                            guidDSs.Add(guidDS);
                        }

                        //var restrictionExtension = restriction.Map.extensions?.Any.FirstOrDefault(x => x.LocalName == "MapExtension");
                        //int seq = 5;
                        //double transparency = 0.5;

                        //if (restrictionExtension != null)
                        //{
                        //    var restrictionExtensionElement = XElement.Parse(restrictionExtension.OuterXml);

                        //    seq = restrictionExtensionElement.Element("Seq") == null ? 5 : System.Convert.ToInt32(restrictionExtensionElement.Element("Seq").Value);
                        //    transparency = restrictionExtensionElement.Element("Transparency") == null ? 0.5 : System.Convert.ToDouble(restrictionExtensionElement.Element("Transparency").Value);
                        //}

                        if (!useWms && restriction.Map.Image == null) throw new ImageFromXmlException("The image embedded in XML under <Restriction><Map><Image> is missing or cannot be read (vector formats are not yet supported).");
                        var imageExtension = new ImageExtension()
                        {
                            Image = useWms ? Helper.v10.Wms.GetMap(restriction.Map.ReferenceWMS) : PreProcessing.GetImageFromByteArray(restriction.Map.Image),
                            Seq = System.Convert.ToInt32(restriction.Map.layerIndex),
                            Transparency = 1 - restriction.Map.layerOpacity
                        };

                        if (Math.Abs(imageExtension.Transparency) > 0.98)
                        {
                            //todo add warning to log, make no sense
                            imageExtension.Transparency = 0;
                        }

                        rolLayers.Add(imageExtension);
                    }
                }

                var sortedRolLayers = rolLayers.OrderBy(x => x.Seq);
                var overview = sortedRolLayers.Select(x => $"{x.Seq}, {x.Name}");

                foreach (var rolLayer in sortedRolLayers)
                {
                    Image = PreProcessing.MergeTwoImages(Image, PreProcessing.SetImageOpacity(rolLayer.Image, (float)(1-rolLayer.Transparency)));
                }

                if (baselayer != null && baselayer.Extent != null)
                {
                    var offsetBorder = Convert.ToInt32(ConfigurationManager.AppSettings["offsetBorder"] ?? "0");

                    var parcelHighligthed = Helper.Geometry.RasterizeGeometryFromGml(
                        extract.RealEstate.Limit,
                        new double[] { baselayer.Extent.Xmin, baselayer.Extent.Ymin, baselayer.Extent.Xmax, baselayer.Extent.Ymax },
                        Image.Width,
                        Image.Height,
                        offsetBorder
                    );

                    Image = PreProcessing.MergeTwoImages(Image, parcelHighligthed);
                }

                var legalProvisions = new List<Document>();
                var documents = new List<Document>();
                var moreinformations = new List<Document>();

                if (restrictionOnLandownership != null)
                {
                    foreach (var restriction in restrictionOnLandownership)
                    {

                        #region  legenditems

                        if (restriction.Map.OtherLegend != null)
                        {
                            foreach (var legend in restriction.Map.OtherLegend)
                            {
                                LegendItems.Add(new LegendItem()
                                {
                                    Symbol = GetImageAsBytes(legend.Item),
                                    TypeCode = legend.TypeCode,
                                    Label = legend.LegendText.FirstOrDefault(x => x.Language.ToString() == language) != null ? legend.LegendText.First(x => x.Language.ToString() == language).Text : "-"
                                });
                            }
                        }

                        var legendItemCatched = LegendItems.FirstOrDefault(x => x.TypeCode == restriction.TypeCode);

                        if (legendItemCatched == null && restriction.Item != null)
                        {
                            legendItemCatched = new LegendItem()
                            {
                                Symbol = GetImageAsBytes(restriction.Item),
                                TypeCode = restriction.TypeCode,
                                Label = Helper.v10.LocalisedMText.GetStringFromArray(restriction.Information, "de")
                            };
                        }

                        var geometryExtension = restriction.Geometry.First().extensions != null ? restriction.Geometry.First().extensions.Any.FirstOrDefault(x => x.LocalName == "GeometryExtension") : null;
                        var type = "NoExtension";

                        if (geometryExtension != null)
                        {
                            var geometryExtentionElement = XElement.Parse(geometryExtension.OuterXml);
                            var geometryExtention = GetGeometryExtension(geometryExtentionElement);
                            type = geometryExtention.Type;
                        }
                        else if (restriction.extensions != null && restriction.extensions.Any != null)
                        {
                            var areaShare = restriction.extensions.Any.FirstOrDefault(x => x.LocalName == "AreaShare");
                            var lengthShare = restriction.extensions.Any.FirstOrDefault(x => x.LocalName == "LengthShare");

                            if (lengthShare != null) restriction.LengthShare = lengthShare.InnerText;
                            if (areaShare != null) restriction.AreaShare = areaShare.InnerText;
                        }

                        if (legendItemCatched != null)
                        {

                            var legendInvolved = new LegendItemInvolved()
                            {
                                Type = type,
                                Symbol = legendItemCatched.Symbol,
                                TypeCode = legendItemCatched.TypeCode,
                                Label = legendItemCatched.Label,
                                PartInPercentValue = restriction.PartInPercent
                            };

                            if ((type == "Polygon" || type == "NoExtension" || (type != "Line" && type != "Polyline")) && !string.IsNullOrEmpty(restriction.AreaShare))
                            {
                                legendInvolved.AreaValue = Convert.ToDouble(restriction.AreaShare);
                                legendInvolved.Type = "Polygon";
                            }
                            else if ((type == "Line" || type == "Polyline" || type == "NoExtension") && !string.IsNullOrEmpty(restriction.LengthShare))
                            {
                                legendInvolved.AreaValue = Convert.ToDouble(restriction.LengthShare);
                                legendInvolved.Type = "Line";
                            }

                            //distinct of legend, aggregate values

                            var useDistinct = ConfigurationManager.AppSettings["distinct"] == "true";
                            var markDistinct = ConfigurationManager.AppSettings["markDistinct"] == "true";

                            if (LegendItemsInvolved.Any(x => x.TypeCode == legendInvolved.TypeCode) && useDistinct)
                            {
                                if (!(type == "Polygon" || type == "NoExtension"))
                                {
                                    continue;
                                }

                                var legAggregation = LegendItemsInvolved.First(x => x.TypeCode == legendInvolved.TypeCode);
                                legAggregation.AreaValue += legendInvolved.AreaValue;
                                legAggregation.PartInPercentValue = Math.Round(legAggregation.AreaValue / System.Convert.ToDouble(extract.RealEstate.LandRegistryArea) * 100, 0);
                                legAggregation.Aggregate = markDistinct;
                            }
                            else
                            {
                                LegendItemsInvolved.Add(legendInvolved);
                            }
                        }

                        #endregion

                        #region legend at web

                        if (restriction.Map.LegendAtWeb != null && !string.IsNullOrEmpty(restriction.Map.LegendAtWeb.Value) && LegendAtWeb.All(x => x.Url != WebUtility.UrlDecode(restriction.Map.LegendAtWeb.Value)))
                        {
                            LegendAtWeb.Add(new LegendAtWeb() { Label = WebUtility.UrlDecode(restriction.Map.LegendAtWeb.Value), Url = WebUtility.UrlDecode(restriction.Map.LegendAtWeb.Value) });
                        }

                        #endregion

                        #region legalprovisions

                        if (restriction.LegalProvisions != null)
                        {
                            foreach (var document in restriction.LegalProvisions.Where(x => x.DocumentType == DocumentBaseDocumentType.LegalProvision).Select(x => (Oereb.Service.DataContracts.Model.v10.Document)x))
                            {
                                var documentItem = new Document()
                                {
                                    Title = Helper.v10.LocalisedText.GetStringFromArray(document.Title, language),
                                    Abbrevation = Helper.v10.LocalisedText.GetStringFromArray(document.Abbreviation, language),
                                    OfficialNumber = string.IsNullOrEmpty(document.OfficialNumber) ? "" : document.OfficialNumber + " ",
                                    OfficialTitle = Helper.v10.LocalisedText.GetStringFromArray(document.OfficialTitle, language),
                                    Url = WebUtility.HtmlEncode(WebUtility.UrlDecode(Helper.v10.LocalisedUri.GetStringFromArray(document.TextAtWeb, language))),
                                    Level = !String.IsNullOrEmpty(document.Municipality) ? 2 : document.CantonSpecified ? 1 : 0,
                                };

                                DocumentSetLevelAndSort(ref documentItem);

                                if (legalProvisions.Any(x => x.Id == documentItem.Id))
                                {
                                    continue;
                                }

                                legalProvisions.Add(documentItem);
                            }

                            if (!legalProvisions.Any())
                            {
                                legalProvisions.Add(new Document() { Abbrevation = "", Level = 0, OfficialNumber = "-", OfficialTitle = "", Title = "", Url = "" });
                            }
                        }

                        #endregion

                        #region documents

                        if (restriction.LegalProvisions != null)
                        {
                            foreach (var documentBase in restriction.LegalProvisions.Select(x => (Service.DataContracts.Model.v10.Document)x))
                            {
                                documents = CreateDocumentsFromDocumentBases(documentBase, language, documents);
                            }

                            if (!documents.Any())
                            {
                                documents.Add(new Document() { Abbrevation = "", Level = 0, OfficialNumber = "-", OfficialTitle = "", Title = "", Url = "" });
                            }
                        }

                        #endregion

                        #region more informations

                        if (restriction.LegalProvisions != null)
                        {
                            foreach (var document in restriction.LegalProvisions.Where(x => x.DocumentType == DocumentBaseDocumentType.Hint).Select(x => (Oereb.Service.DataContracts.Model.v10.Document)x))
                            {
                                var documentItem = new Document()
                                {
                                    Title = Helper.v10.LocalisedText.GetStringFromArray(document.Title, language),
                                    Abbrevation = Helper.v10.LocalisedText.GetStringFromArray(document.Abbreviation, language),
                                    OfficialNumber = string.IsNullOrEmpty(document.OfficialNumber) ? "" : document.OfficialNumber + " ",
                                    OfficialTitle = Helper.v10.LocalisedText.GetStringFromArray(document.OfficialTitle, language),
                                    Url = WebUtility.HtmlEncode(WebUtility.UrlDecode(Helper.v10.LocalisedUri.GetStringFromArray(document.TextAtWeb, language))),
                                    Level = !String.IsNullOrEmpty(document.Municipality) ? 2 : document.CantonSpecified ? 1 : 0,
                                };

                                DocumentSetLevelAndSort(ref documentItem);

                                if (moreinformations.Any(x => x.Id == documentItem.Id))
                                {
                                    continue;
                                }

                                moreinformations.Add(documentItem);
                            }
                        }

                        #endregion

                        #region responsible office

                        var responsibleOffice = new ResponsibleOffice()
                        {
                            Name = Helper.v10.LocalisedText.GetStringFromArray(restriction.ResponsibleOffice.Name, language),
                            Url = restriction.ResponsibleOffice.OfficeAtWeb == null ? "-" : WebUtility.HtmlEncode(WebUtility.UrlDecode(restriction.ResponsibleOffice.OfficeAtWeb.Value))
                        };

                        if (!ResponsibleOffice.Any(x => x.Id == responsibleOffice.Id))
                        {
                            ResponsibleOffice.Add(responsibleOffice);
                        }

                        #endregion
                    }
                }

                //with legal provision and laws this should not happen, but here would be the right place

                if (!moreinformations.Any())
                {
                    moreinformations.Add(new Document() { Abbrevation = "", Level = 0, OfficialNumber = "-", OfficialTitle = "", Title = "", Url = "" });
                }

                LegalProvisions.AddRange(legalProvisions.OrderBy(x => x.Sort));
                Documents.AddRange(documents.OrderBy(x => x.Sort));
                MoreInformations.AddRange(moreinformations.OrderBy(x => x.Sort));


                #region Anhaenge

                if (hasAppendixes)
                {
                    foreach (var documentItem in legalProvisions.OrderBy(x => x.Sort))
                    {
                        var tocAppendix = new TocAppendix()
                        {
                            Key = documentItem.Title,
                            Shortname = $"A{appendixCounter++}",
                            Description = documentItem.Title,
                            FileDescription = documentItem.Title,
                            Url = WebUtility.HtmlEncode(WebUtility.UrlDecode(documentItem.Url))
                        };

                        if (!Appendixes.Any(x => x.Url == tocAppendix.Url && x.Key == tocAppendix.Key))
                        {
                            Appendixes.Add(tocAppendix);
                        }
                    }
                }

                #endregion


            }

            protected List<Document> CreateDocumentsFromDocumentBases(Oereb.Service.DataContracts.Model.v10.Document document, string language, List<Document> documents)
            {
                if (document.DocumentType == DocumentBaseDocumentType.Law)
                {
                    var documentItem = new Document()
                    {
                        Title = Helper.v10.LocalisedText.GetStringFromArray(document.Title, language),
                        Abbrevation = Helper.v10.LocalisedText.GetStringFromArray(document.Abbreviation, language),
                        OfficialNumber = string.IsNullOrEmpty(document.OfficialNumber)
                            ? ""
                            : document.OfficialNumber + " ",
                        OfficialTitle = Helper.v10.LocalisedText.GetStringFromArray(document.OfficialTitle, language),
                        Url = WebUtility.HtmlEncode(WebUtility.UrlDecode(Helper.v10.LocalisedUri.GetStringFromArray(document.TextAtWeb, language))),
                        Level = !String.IsNullOrEmpty(document.Municipality) ? 2 : document.CantonSpecified ? 1 : 0
                    };

                    DocumentSetLevelAndSort(ref documentItem);

                    if (documents.All(x => x.Id != documentItem.Id))
                    {
                        documents.Add(documentItem);
                    }
                }

                if (document.Reference != null)
                {
                    foreach (var subdocument in document.Reference)
                    {
                        documents = CreateDocumentsFromDocumentBases(subdocument, language, documents);
                    }
                }
                return documents;
            }

            protected byte[] GetImageAsBytes(object data)
            {
                if (data is string)
                {
                    var url = HttpUtility.UrlDecode(data.ToString());
                    Uri uriResult;
                    bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                    if (result)
                    {
                        using (var webClient = new WebClient())
                        {
                            try
                            {
                                return webClient.DownloadData(uriResult);
                            }
                            catch (System.Net.WebException ex)
                            {
                                throw new ImageLoadingException(url, ex.Message);
                            }
                        }
                    }
                }

                return (byte[])data;
            }

            private void DocumentSetLevelAndSort(ref Document documentItem)
            {
                if (documentItem.Level == 0 && String.IsNullOrEmpty(documentItem.OfficialNumber))
                {
                    documentItem.Level = 3; // not a federal document, because there is no official number
                }

                var sort = "";

                if (string.IsNullOrEmpty(documentItem.OfficialNumber))
                {
                    sort = ";" + documentItem.Title;
                }
                else
                {
                    sort = documentItem.OfficialNumber + ";";
                }

                documentItem.Sort = $"{documentItem.Level};{sort}";
            }

        }

        #region helper classes report body

        public class LegendItem
        {
            public string TypeCode { get; set; }
            private byte[] symbol;

            public byte[] Symbol
            {
                get
                {
                    using (var ms = new MemoryStream(symbol))
                    {
                        Bitmap bmp;
                        try
                        {
                            bmp = new Bitmap(ms);
                        }
                        catch (Exception ex)
                        {
                            throw new ImageConversionException("LegendItem type " + TypeCode, "Trying to convert to bitmap failed. Only pixel images (PNG, TIFF, JPG, BMP, etc.) are allowed.");
                        }
                        var height = bmp.Height;
                        var width = bmp.Width;

                        if (height > width * 2)
                        {
                            width = height * 2;
                        }
                        else if (width > height * 2)
                        {
                            height = width / 2;
                        }
                        else
                        {
                            return symbol;
                        }

                        Bitmap result = new Bitmap(width, height);
                        try
                        {
                            using (Graphics g = Graphics.FromImage(result))
                            {
                                g.InterpolationMode = InterpolationMode.Bicubic;
                                g.DrawImage(bmp, (width - bmp.Width) / 2, (height - bmp.Height) / 2, bmp.Width, bmp.Height);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new ImageConversionException("LegendItem type " + TypeCode, "Trying to convert to Graphics after resize failed. " + ex.Message);
                        }

                        using (MemoryStream m = new MemoryStream())
                        {
                            result.Save(m, ImageFormat.Png);
                            return m.ToArray();
                        }
                    }
                }
                set { symbol = value; }
            }

            public string Label { get; set; }
        }

        public class LegendItemInvolved : LegendItem
        {
            public bool Aggregate { get; set; } = false;
            public string Type { get; set; }
            public double AreaValue { get; set; }
            public double PartInPercentValue { get; set; }

            public string Area {
                get
                {
                    var area = "";
                    
                    if (Type == "Polygon")
                    {
                        area = Math.Round(AreaValue, 0) + " m²";
                    }
                    else if (Type == "Line")
                    {
                        area = Math.Round(AreaValue, 0) + " m";
                    }

                    // return area + (Aggregate ? " *": "");
                    return area;
                }
            }

            public string PartInPercent {
                get
                {
                    string partInPercent = "";

                    if (Type == "Polygon")
                    {
                        if (Math.Abs(PartInPercentValue) < 1)
                        {
                            partInPercent = "< 1 %";
                        }
                        else
                        {
                            partInPercent = PartInPercentValue + " %"; 
                        }
                    }

                    // return partInPercent + (Aggregate ? " *" : "");
                    return partInPercent;
                }
            }
        }

        public class LegendAtWeb
        {
            public string Url { get; set; }
            public string Label { get; set; }
        }

        public class Document
        {
            public string Url { get; set; }
            public string Title { get; set; }
            public string OfficialTitle { get; set; }
            public string Abbrevation { get; set; }
            public string OfficialNumber { get; set; }
            public int Level { get; set; } //for sorting 0 federal | 1 canton | 2 municipality
            public string Sort { get; set; }

            public string Id => $"{Title}|{OfficialTitle}|{OfficialNumber}|{Url}";
        }

        public class ResponsibleOffice
        {
            public string Name { get; set; }
            public string Url { get; set; }

            public string Id => $"{Name}|{Url}";
        }

        #endregion //helper classes report body

        #endregion //report body

        #region report glossary

        public void IniReportGlossary()
        {
            GlossaryItems = new List<GlossaryItem>();

            if (Extract.Glossary != null)
            {
                GlossaryItems.AddRange(Extract.Glossary.Where(x => x.Title.Any(y => y.Language.ToString() == Language))
                    .Select(x => new GlossaryItem()
                    {
                        Abbreviation = Helper.v10.LocalisedText.GetStringFromArray(x.Title, Language),
                        Description = Helper.v10.LocalisedMText.GetStringFromArray(x.Content, Language),
                    }));
            }
        }

        public class GlossaryItem
        {
            public string Abbreviation { get; set; }
            public string Description { get; set; }
        }

        #endregion
    }
}
