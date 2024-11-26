using Oereb.Report.Helper;
using Oereb.Service.DataContracts.Model.v20;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;

namespace Oereb.Report.v20
{
    /// <summary>
    /// mapping class, from xml to report
    /// </summary>

    [DataObject]
    public class ReportExtract
    {
        public Extract Extract { get; set; }
        public string Language { get; set; } = "de";

        public TocRegion Toc { get; set; }
        public List<TocAppendix> TocAppendixes { get; set; }
        public int AppendixCounter { get; set; } = 1;

        /*
        public bool ExtractComplete { get; set; } = true;
        public bool AttacheFiles { get; set; } = false;
        */

        public bool Attestation { get; set; }

        public bool UseWms { get; set; }

        public string Title
        {
            get
            {
                var ResourceManager = new System.Resources.ResourceManager("Oereb.Report.v20.ReportTitle", typeof(ReportTitle).Assembly);

                return ResourceManager.GetString("PageTitle");
            }
        }

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

                return $"{PlrCadastreAuthorityName}<br />{PlrCadastreAuthorityStreet}<br />{PlrCadastreAuthorityCity}" + (string.IsNullOrEmpty(PlrCadastreAuthorityOfficeAtWeb) ? "" : $"<br /><a href='{PlrCadastreAuthorityOfficeAtWeb}' style='text-decoration: none; color: #4c8fba;'>{PlrCadastreAuthorityOfficeAtWeb}</a>");
            }
        }
        public string PlrCadastreAuthorityName
        {
            get
            {
                var office = Extract.PLRCadastreAuthority;

                return Helper.v20.LocalisedText.GetStringFromArray(office.Name, Language).Trim();
            }
        }
        public string PlrCadastreAuthorityStreet
        {
            get
            {
                var office = Extract.PLRCadastreAuthority;

                return (office.Street + " " + office.Number).Trim();
            }
        }
        public string PlrCadastreAuthorityCity
        {
            get
            {
                var office = Extract.PLRCadastreAuthority;

                return (office.PostalCode + " " + office.City).Trim();
            }
        }
        public string PlrCadastreAuthorityOfficeAtWeb
        {
            get
            {
                var office = Extract.PLRCadastreAuthority;

                return Helper.v20.LocalisedText.GetUriFromArray(office.OfficeAtWeb, Language).Trim();
            }
        }

        public int BodySectionCount => Extract.RealEstate.RestrictionOnLandownership?.GroupBy(x => x.Theme.Code + "|" + x.Theme.SubCode ?? "").ToList().Count ?? 0;

        public int BodySectionFlag { get; set; } = -1;

        public ReportExtract()
        {
        }

        // public ReportExtract(bool extractComplete, bool attacheFiles, bool attestation, bool useWms)
        public ReportExtract(bool attestation, bool useWms)
        {
            /*
            ExtractComplete = extractComplete;
            AttacheFiles = attacheFiles;
            */
            Attestation = attestation;
            UseWms = useWms;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<BodyItem> GetBodyItems()
        {
            //path for the designer preview in visual studio
            //Extract = Xml<Service.DataContracts.Model.v20.Extract>.DeserializeFromFile(@"...");
            Ini();
            return ReportBodyItems;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public ReportExtract GetReportExtract()
        {
            //path for the designer preview in visual studio
            //Extract = Xml<Service.DataContracts.Model.v20.Extract>.DeserializeFromFile(@"...");
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

            var imageBg = UseWms ? Helper.v20.Wms.GetMap(Extract.RealEstate.PlanForLandRegisterMainPage.ReferenceWMS.First(x => x.Language == LanguageCode.de).Text) : PreProcessing.GetImageFromByteArray(Extract.RealEstate.PlanForLandRegisterMainPage.Image.First(x => x.Language == LanguageCode.de).Blob);
            var image = new Bitmap(imageBg.Width, imageBg.Height, PixelFormat.Format32bppArgb);

            image = PreProcessing.MergeTwoImages(image, imageBg);

            var georeferenceExtention = new GeoreferenceExtension() { };
            georeferenceExtention.Extent = null;
            georeferenceExtention.Seq = 0;
            georeferenceExtention.Transparency = 0;

            if (Extract.RealEstate.PlanForLandRegisterMainPage.min != null && Extract.RealEstate.PlanForLandRegisterMainPage.max != null)
            {
                georeferenceExtention = GetGeoreferenceExtension(Extract.RealEstate.PlanForLandRegisterMainPage.min, Extract.RealEstate.PlanForLandRegisterMainPage.max);

                if (Extract.RealEstate.Limit != null)
                {
                    var offsetBorderTitle = Convert.ToInt32(ConfigurationManager.AppSettings["offsetBorderTitle"] ?? "0");

                    var parcelHighligthed = Helper.Geometry.RasterizeGeometryFromMultiSurfaceType(
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

            Extract.RealEstate.TypeName = Extract.RealEstate.Type?.Text.FirstOrDefault(x => x.Language == LanguageCode.de)?.Text;

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
                Image = UseWms ? Helper.v20.Wms.GetMap(Extract.RealEstate.PlanForLandRegister.ReferenceWMS.First(x => x.Language == LanguageCode.de).Text) : PreProcessing.GetImageFromByteArray(Extract.RealEstate.PlanForLandRegister.Image.First(x => x.Language == LanguageCode.de).Blob),
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

        public GeoreferenceExtension GetGeoreferenceExtension(CoordType min, CoordType max)
        {
            if (min == null || max == null)
            {
                return null;
            }

            var georeferenceExtension = new GeoreferenceExtension();
            georeferenceExtension.Extent = new Extent();
            georeferenceExtension.Extent.Crs = 2056;


            georeferenceExtension.Extent.Xmin = min.c1;
            georeferenceExtension.Extent.Ymin = min.c2;
            georeferenceExtension.Extent.Xmax = max.c1;
            georeferenceExtension.Extent.Ymax = max.c2;

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
            georeferenceExtension.Extent.Xmin = Convert.ToDouble(extension.Element("Extent").Element("Xmin").Value);
            georeferenceExtension.Extent.Ymin = Convert.ToDouble(extension.Element("Extent").Element("Ymin").Value);
            georeferenceExtension.Extent.Xmax = Convert.ToDouble(extension.Element("Extent").Element("Xmax").Value);
            georeferenceExtension.Extent.Ymax = Convert.ToDouble(extension.Element("Extent").Element("Ymax").Value);
            georeferenceExtension.SetDescription(GetDescriptionExtension(extension));

            return georeferenceExtension;
        }

        public ImageExtension GetImageExtension(XElement extension)
        {
            if (extension.Element("Image") == null || extension.Element("Extent") == null ||
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

        #region report TOC

        public void IniToc(Extract extract, string language)
        {
            Toc = new TocRegion();
            TocAppendixes = new List<TocAppendix>();
            Toc.Extract = extract;
            Toc.Language = language;

            Toc.GeneralInformation = Helper.v20.LocalisedMText.GetStringFromFirst2DArray(Extract.GeneralInformation, Language);
            Toc.BaseData = Helper.v20.LocalisedMText.GetStringFromLast2DArray(Extract.GeneralInformation, Language);

            Toc.ExclusionOfLiabilityTitle = Extract.Disclaimer.Select(x => Helper.v20.LocalisedText.GetStringFromArray(x.Title, Language)).First();
            Toc.ExclusionOfLiabilityContent = Extract.Disclaimer.Select(x => Helper.v20.LocalisedMText.GetStringFromArray(x.Content, Language)).First();

            var groupedBodyItems = ReportBodyItems.GroupBy(x => x.Theme + " " + x.ThemeLawstatusCode).ToList(); // Extract.RealEstate.RestrictionOnLandownership.GroupBy(x => x.Theme.Code).ToList();

            foreach (var bodyItem in groupedBodyItems)
            {
                var firstBodyItem = bodyItem.First();

                var tocItemLabel = firstBodyItem.Theme;
                if (firstBodyItem.ThemeLawstatusCode != LawstatusCode.inForce)
                {
                    tocItemLabel += $" ({firstBodyItem.ThemeLawstatus})";
                }

                var tocItem = new TocItem()
                {
                    Page = 0,
                    Label = tocItemLabel,
                    Appendixes = new List<TocAppendix>()
                };

                /*
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
                                Url = legalProvision.Url
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
                                tocAppendix.Description += "(siehe im Anhang)";
                            }
                            else if (AttacheFiles && !result.Successful)
                            {
                                tocAppendix.Description += "(nicht anhängbar)";
                            }
                            else if (result.Successful)
                            {
                                var files = Oereb.Report.Helper.Pdf.GetImagesFromPpdf(filepath);
                                tocAppendix.Pages.AddRange(files);
                            }

                            tocItem.Appendixes.Add(tocAppendix);
                            TocAppendixes.Add(tocAppendix);
                            AppendixCounter++;
                        }
                    }
                }
                */

                Toc.TocItems.Add(tocItem);
            }

            if (Extract.NotConcernedTheme != null)
            {
                foreach (var notConcernedTheme in Extract.NotConcernedTheme)
                {
                    var localisedText = new LocalisedText[] { notConcernedTheme.Text.First(x => x.Language.ToString() == language) };
                    var label = Helper.v20.LocalisedText.GetStringFromArray(localisedText, Language);

                    /*
                    if (notConcernedTheme.ThemeLawstatusCode != LawstatusCode.inForce)
                    {
                        label += $" ({notConcernedTheme.ThemeLawstatus})";
                    }
                    */

                    Toc.ThemeNotConcerned.Add(new TocItemTheme() { Label = label });
                }
            }

            if (Extract.ThemeWithoutData != null)
            {
                foreach (var themeWithoutData in Extract.ThemeWithoutData)
                {
                    var localisedText = new LocalisedText[] { themeWithoutData.Text.First(x => x.Language.ToString() == language) };
                    var label = Helper.v20.LocalisedText.GetStringFromArray(localisedText, Language);

                    /*
                    if (themeWithoutData.ThemeLawstatusCode != LawstatusCode.inForce)
                    {
                        label += $" ({notConcernedTheme.ThemeLawstatus})";
                    }
                    */

                    Toc.ThemeWithoutData.Add(new TocItemTheme() { Label = label });
                }
            }
        }

        public class TocRegion
        {
            public Extract Extract { get; set; }
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

            var groupedBodyItems = Extract.RealEstate.RestrictionOnLandownership?.GroupBy(x => x.Theme.Code + "|" + x.Theme.SubCode ?? "").ToList();
            if (groupedBodyItems != null)
            {
                int section = -1;
                int appendixCounter = 1;

                foreach (var bodyItem in groupedBodyItems)
                {
                    section++;

                    if (!(BodySectionFlag == section || BodySectionFlag == -1))
                    {
                        continue;
                    }

                    // var reportBodyItem = new BodyItem(Extract, bodyItem.ToList(), Language, ImageRestrictionOnLandownership, AdditionalLayers, UseWms, appendixCounter, ExtractComplete);
                    var reportBodyItem = new BodyItem(Extract, bodyItem.ToList(), Language, ImageRestrictionOnLandownership, AdditionalLayers, UseWms, appendixCounter);
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
            public string ThemeLawstatus { get; set; }
            public LawstatusCode ThemeLawstatusCode { get; set; }
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

            // public BodyItem(Extract extract, List<RestrictionOnLandownership> restrictionsOnLandownership, string language, ImageExtension baselayer, List<ImageExtension> additionalLayers, bool useWms = false, int appendixCounter = 1, bool hasAppendixes = false)
            public BodyItem(Extract extract, List<RestrictionOnLandownership> restrictionsOnLandownership, string language, ImageExtension baselayer, List<ImageExtension> additionalLayers, bool useWms = false, int appendixCounter = 1)
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

                FederalLogo = GetImageAsBytes(extract.FederalLogo);
                CantonalLogo = GetImageAsBytes(extract.CantonalLogo);
                MunicipalityLogo = GetImageAsBytes(extract.MunicipalityLogo);
                LogoPLRCadastre = GetImageAsBytes(extract.LogoPLRCadastre);
                ExtractIdentifier = extract.ExtractIdentifier;
                CreationDate = extract.CreationDate;

                Theme = restrictionsOnLandownership.First().Theme.Text.First(x => x.Language.ToString() == language).Text;
                ThemeLawstatus = restrictionsOnLandownership.First().Lawstatus.Text.First(x => x.Language.ToString() == language).Text;
                ThemeLawstatusCode = restrictionsOnLandownership.First().Lawstatus.Code;

                var image = useWms ? Helper.v20.Wms.GetMap(restrictionsOnLandownership.First().Map.ReferenceWMS.First(x => x.Language.ToString() == language).Text) : PreProcessing.GetImageFromByteArray(restrictionsOnLandownership.First().Map.Image.First(x => x.Language.ToString() == language).Blob); //take only with and height from image
                if (image == null)
                {
                    image = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
                }

                Image = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);


                var rolLayers = new List<ImageExtension>();

                rolLayers.Add(baselayer);
                rolLayers.AddRange(additionalLayers.Where(x => x.Name == Theme));

                var guidDSs = new List<string>();

                foreach (var restriction in restrictionsOnLandownership)
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

                    var imageExtension = new ImageExtension()
                    {
                        Image = useWms ? Helper.v20.Wms.GetMap(restriction.Map.ReferenceWMS.First(x => x.Language.ToString() == language).Text) : PreProcessing.GetImageFromByteArray(restriction.Map.Image.First(x => x.Language.ToString() == language).Blob),
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

                var sortedRolLayers = rolLayers.OrderBy(x => x.Seq);
                var overview = sortedRolLayers.Select(x => $"{x.Seq}, {x.Name}");

                foreach (var rolLayer in sortedRolLayers)
                {
                    Image = PreProcessing.MergeTwoImages(Image, PreProcessing.SetImageOpacity(rolLayer.Image, (float)(1 - rolLayer.Transparency)));
                }

                if (baselayer != null && baselayer.Extent != null)
                {
                    var offsetBorder = Convert.ToInt32(ConfigurationManager.AppSettings["offsetBorder"] ?? "0");

                    var parcelHighligthed = Helper.Geometry.RasterizeGeometryFromMultiSurfaceType(
                        extract.RealEstate.Limit,
                        new double[] { baselayer.Extent.Xmin, baselayer.Extent.Ymin, baselayer.Extent.Xmax, baselayer.Extent.Ymax },
                        image.Width,
                        image.Height,
                        offsetBorder
                    );

                    Image = PreProcessing.MergeTwoImages(Image, parcelHighligthed);
                }

                var legalProvisions = new List<Document>();
                var documents = new List<Document>();
                var moreinformations = new List<Document>();

                foreach (var restrictionOnLandownership in restrictionsOnLandownership)
                {

                    #region  legenditems

                    if (restrictionOnLandownership.Map.OtherLegend != null)
                    {
                        foreach (var legend in restrictionOnLandownership.Map.OtherLegend)
                        {
                            LegendItems.Add(new LegendItem()
                            {
                                Symbol = GetImageAsBytes(legend.Item),
                                TypeCode = legend.TypeCode,
                                Label = legend.LegendText.FirstOrDefault(x => x.Language.ToString() == language) != null ? legend.LegendText.First(x => x.Language.ToString() == language).Text : "-"
                            });
                        }
                    }

                    var legendItemCatched = LegendItems.FirstOrDefault(x => x.TypeCode == restrictionOnLandownership.TypeCode);
                    if (legendItemCatched == null && restrictionOnLandownership.Item != null)
                    {
                        legendItemCatched = new LegendItem()
                        {
                            Symbol = GetImageAsBytes(restrictionOnLandownership.Item),
                            TypeCode = restrictionOnLandownership.TypeCode,
                            Label = Helper.v20.LocalisedMText.GetStringFromArray(restrictionOnLandownership.LegendText, language)
                        };
                    }

                    var geometryExtension = restrictionOnLandownership.Geometry?.First().extensions != null ? restrictionOnLandownership.Geometry.First().extensions.Any.FirstOrDefault(x => x.LocalName == "GeometryExtension") : null;
                    var type = "NoExtension";

                    if (geometryExtension != null)
                    {
                        var geometryExtentionElement = XElement.Parse(geometryExtension.OuterXml);
                        var geometryExtention = GetGeometryExtension(geometryExtentionElement);
                        type = geometryExtention.Type;
                    }

                    if (legendItemCatched != null)
                    {

                        var legendInvolved = new LegendItemInvolved()
                        {
                            Type = type,
                            Symbol = legendItemCatched.Symbol,
                            TypeCode = legendItemCatched.TypeCode,
                            Label = legendItemCatched.Label,
                            PartInPercentValue = restrictionOnLandownership.PartInPercent
                        };

                        if ((type == "Polygon" || type == "NoExtension" || (type != "Line" && type != "Polyline")) && !string.IsNullOrEmpty(restrictionOnLandownership.AreaShare))
                        {
                            legendInvolved.AreaValue = Convert.ToDouble(restrictionOnLandownership.AreaShare);
                            legendInvolved.Type = "Polygon";
                        }
                        else if ((type == "Line" || type == "Polyline" || type == "NoExtension") && !string.IsNullOrEmpty(restrictionOnLandownership.LengthShare))
                        {
                            legendInvolved.AreaValue = Convert.ToDouble(restrictionOnLandownership.LengthShare);
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
                    // old
                    /*
                    if (restrictionOnLandownership.Map.LegendAtWeb != null && !string.IsNullOrEmpty(restrictionOnLandownership.Map.LegendAtWeb.Value) && !LegendAtWeb.Any(x => x.Url == restrictionOnLandownership.Map.LegendAtWeb.Value))
                    {
                        LegendAtWeb.Add(new LegendAtWeb() { Label = restrictionOnLandownership.Map.LegendAtWeb.Value, Url = restrictionOnLandownership.Map.LegendAtWeb.Value });
                    }
                    */
                    #endregion

                    #region legalprovisions

                    if (restrictionOnLandownership.LegalProvisions != null)
                    {
                        foreach (var document in restrictionOnLandownership.LegalProvisions.Where(x => x.Type.Code == DocumentTypeCode.LegalProvision))
                        {
                            var documentItem = new Document()
                            {
                                Title = Helper.v20.LocalisedText.GetStringFromArray(document.Title, language),
                                Abbrevation = Helper.v20.LocalisedText.GetStringFromArray(document.Abbreviation, language),
                                OfficialNumber = Helper.v20.LocalisedText.GetStringFromArray(document.OfficialNumber, language, ""),
                                // old OfficialTitle = Helper.v20.LocalisedText.GetStringFromArray(document.OfficialTitle, language),
                                Url = WebUtility.HtmlEncode(Helper.v20.LocalisedUri.GetStringFromArray(document.TextAtWeb, language)),
                                Level = int.Parse(document.Index),
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

                    if (restrictionOnLandownership.LegalProvisions != null)
                    {
                        foreach (var document in restrictionOnLandownership.LegalProvisions.Where(x => x.Type.Code == DocumentTypeCode.Law))
                        {
                            var documentItem = new Document()
                            {
                                Title = Helper.v20.LocalisedText.GetStringFromArray(document.Title, language),
                                Abbrevation = Helper.v20.LocalisedText.GetStringFromArray(document.Abbreviation, language),
                                OfficialNumber = Helper.v20.LocalisedText.GetStringFromArray(document.OfficialNumber, language, ""),
                                // old OfficialTitle = Helper.v20.LocalisedText.GetStringFromArray(document.OfficialTitle, language),
                                Url = WebUtility.HtmlEncode(Helper.v20.LocalisedUri.GetStringFromArray(document.TextAtWeb, language)),
                                Level = int.Parse(document.Index),
                            };

                            DocumentSetLevelAndSort(ref documentItem);

                            if (documents.Any(x => x.Id == documentItem.Id))
                            {
                                continue;
                            }

                            documents.Add(documentItem);
                        }

                        if (!documents.Any())
                        {
                            documents.Add(new Document() { Abbrevation = "", Level = 0, OfficialNumber = "-", OfficialTitle = "", Title = "", Url = "" });
                        }
                    }

                    #endregion

                    #region more informations

                    if (restrictionOnLandownership.LegalProvisions != null)
                    {
                        foreach (var document in restrictionOnLandownership.LegalProvisions.Where(x => x.Type.Code == DocumentTypeCode.Hint))
                        {
                            var documentItem = new Document()
                            {
                                Title = Helper.v20.LocalisedText.GetStringFromArray(document.Title, language),
                                Abbrevation = Helper.v20.LocalisedText.GetStringFromArray(document.Abbreviation, language),
                                OfficialNumber = Helper.v20.LocalisedText.GetStringFromArray(document.OfficialNumber, language, ""),
                                // old OfficialTitle = Helper.v20.LocalisedText.GetStringFromArray(document.OfficialTitle, language),
                                Url = WebUtility.HtmlEncode(Helper.v20.LocalisedUri.GetStringFromArray(document.TextAtWeb, language)),
                                Level = int.Parse(document.Index),
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
                        Name = Helper.v20.LocalisedText.GetStringFromArray(restrictionOnLandownership.ResponsibleOffice.Name, language),
                        Url = restrictionOnLandownership.ResponsibleOffice.OfficeAtWeb == null ? "-" : System.Web.HttpUtility.HtmlEncode(restrictionOnLandownership.ResponsibleOffice.OfficeAtWeb.First(x => x.Language.ToString() == language).Text)
                    };

                    if (!ResponsibleOffice.Any(x => x.Id == responsibleOffice.Id))
                    {
                        ResponsibleOffice.Add(responsibleOffice);
                    }

                    #endregion

                }

                //with legal provision and laws this should not happen, but here would be the right place

                if (!moreinformations.Any())
                {
                    moreinformations.Add(new Document() { Abbrevation = "", Level = 0, OfficialNumber = "-", OfficialTitle = "", Title = "", Url = "" });
                }

                legalProvisions.Sort((x, y) => OrderBySortProperty(x.Sort, y.Sort));
                documents.Sort((x, y) => OrderBySortProperty(x.Sort, y.Sort));
                moreinformations.Sort((x, y) => OrderBySortProperty(x.Sort, y.Sort));

                LegalProvisions.AddRange(legalProvisions);
                Documents.AddRange(documents);
                MoreInformations.AddRange(moreinformations);

                /*
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
                            Url = documentItem.Url
                        };

                        if (!Appendixes.Any(x => x.Url == tocAppendix.Url && x.Key == tocAppendix.Key))
                        {
                            Appendixes.Add(tocAppendix);
                        }
                    }
                }

                #endregion
                */
            }

            protected int OrderBySortProperty(string first, string second)
            {
                var firstParts = first.Split(';');
                var secondParts = second.Split(';');

                if (int.TryParse(firstParts[0], out int firstIndex) && int.TryParse(secondParts[0], out int secondIndex))
                {
                    var compared = firstIndex.CompareTo(secondIndex);
                    if (compared != 0)
                    {
                        return compared;
                    }
                }

                try
                {
                    return first.Substring(first.IndexOf(';')).CompareTo(first.Substring(second.IndexOf(";")));
                }
                catch
                {
                    return first.CompareTo(second);
                }
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
                            return webClient.DownloadData(uriResult);
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
            public byte[] Symbol { get; set; }
            public string Label { get; set; }
        }

        public class LegendItemInvolved : LegendItem
        {
            public bool Aggregate { get; set; } = false;
            public string Type { get; set; }
            public double AreaValue { get; set; }
            public double PartInPercentValue { get; set; }

            public string Area
            {
                get
                {
                    var area = "";

                    if (Type == "Polygon" || Type == "NoExtension")
                    {
                        area = Math.Round(AreaValue, 0) + " m²";
                    }
                    else if (Type == "Line")
                    {
                        area = Math.Round(AreaValue, 0) + " m";
                    }

                    return area + (Aggregate ? " *" : "");
                }
            }

            public string PartInPercent
            {
                get
                {
                    string partInPercent = "";

                    if (Type == "Polygon" || Type == "NoExtension")
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

                    return partInPercent + (Aggregate ? " *" : "");
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

            GlossaryItems.AddRange(Extract.Glossary.Where(x => x.Title.Any(y => y.Language.ToString() == Language)).Select(x => new GlossaryItem()
            {
                Abbreviation = Helper.v20.LocalisedText.GetStringFromArray(x.Title, Language),
                Description = Helper.v20.LocalisedMText.GetStringFromArray(x.Content, Language),
            }));
        }

        public class GlossaryItem
        {
            public string Abbreviation { get; set; }
            public string Description { get; set; }
        }

        #endregion
    }
}
