using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using Geocentrale.GDAL;
using OSGeo.OGR;

namespace Oereb.Report.Helper
{
    public static class Geometry
    {
        /// <summary>
        /// this is a compromise because we don't want attached a GDAL framework => more dependencies
        /// </summary>
        /// <param name="geometryGml"></param>
        /// <param name="extent"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>

        public static Image RasterizeGeometryFromGml(string geometryGml, double[] extent, int width, int height, int offset = 0)
        {
            var bitmap = new Bitmap(width, height);
            Graphics graphic = Graphics.FromImage(bitmap);

            graphic.Clear(Color.Transparent);

            if (String.IsNullOrEmpty(geometryGml))
            {
                return bitmap;
            }

            double conversionFactor = (extent[2] - extent[0]) / width; //scale is the same in both directions, meter per pixel

            //// If exterior and interior exist, drop interior
            //var exteriorRegex = new Regex(@"\<(\/)?gml:exterior\>");
            //if (exteriorRegex.Matches(geometryGml).Count > 0)
            //{
            //    var interiorRegex = new Regex(@"\<gml:interior\>(.|\n)*\<\/gml:interior\>");
            //    geometryGml = interiorRegex.Replace(geometryGml, "");
            //}

            geometryGml = BufferGeomtry(geometryGml, offset * conversionFactor * width / 2055);

            var gmlElement = RemoveAllNamespaces(geometryGml.Replace("gml:", ""));

            //var rings = gmlElement.XPathSelectElements($"/descendant::posList").ToList();
            var rings = gmlElement.XPathSelectElements($"/descendant::coordinates").ToList();

            if (!rings.Any())
            {
                return bitmap;
            }

            Color color = ColorTranslator.FromHtml("#99e60000"); // TODO: another config value candidate
            var pen = new Pen(color, (int)Math.Round(14 * width / 2055d, 0)); // 14

            foreach (var ring in rings)
            {
                var content = ring.Value.ToString();

                content = content.Replace(",", " "); //other GML version

                var coords = content.Split(' ').Where(x => !String.IsNullOrEmpty(x)).ToList();

                var points = new List<PointF>();

                for (int i = 0; i < coords.Count; i += 2)
                {
                    var point = new double[2];

                    point[0] = Convert.ToDouble(coords[i]);
                    point[1] = Convert.ToDouble(coords[i + 1]);

                    var pX = (float)((point[0] - extent[0]) / conversionFactor);
                    var pY = (float)((extent[3] - point[1]) / conversionFactor);

                    points.Add(new PointF(pX, pY));
                }

                graphic.DrawLines(pen, points.ToArray());
                graphic.Flush();
            }

            bitmap = (Bitmap)AddScalebarAndOrientation(bitmap, extent, width, height, 0.2, (int)Math.Round(width / 70d, 0));

            //bitmap.Save(@"c:\temp\testrasterize6.png", System.Drawing.Imaging.ImageFormat.Png);

            return bitmap;
        }


        public static Image RasterizeGeometryFromMultiSurfaceType(Service.DataContracts.Model.v20.MultiSurfaceType multiSurfaceType, double[] extent, int width, int height, int offset = 0)
        {
            var bitmap = new Bitmap(width, height);
            Graphics graphic = Graphics.FromImage(bitmap);

            graphic.Clear(Color.Transparent);

            if (multiSurfaceType?.surface == null)
            {
                return bitmap;
            }

            var surface = multiSurfaceType.surface;

            var srs = "2056";
            if (!string.IsNullOrEmpty(multiSurfaceType?.surface.epsg))
            {
                srs = multiSurfaceType?.surface.epsg; // Quickfix URI EPSG:3857
            }

            double conversionFactor = (extent[2] - extent[0]) / width; //scale is the same in both directions, meter per pixel

            // old surface = BufferGeomtry(surface, offset * conversionFactor);

            var rings = new List<Service.DataContracts.Model.v20.BoundaryType>();
            if (surface.exterior != null)
            {
                rings.AddRange(surface.exterior.ToList());
            }
            if (surface.interior != null)
            {
                rings.AddRange(surface.interior.ToList());
            }

            if (!rings.Any())
            {
                return bitmap;
            }

            Color color = ColorTranslator.FromHtml("#99e60000"); // TODO: another config value candidate
            var pen = new Pen(color, 14);

            foreach (var ring in rings)
            {
                var polyline = ring.polyline;

                var coords = new List<object>();
                coords.Add(polyline.coord); // CoordType
                coords.AddRange(polyline.Items); // ArcSegmentType, CoordType, LineSegmentType

                var points = new List<PointF>();

                foreach (var coord in coords)
                {
                    var point = new double[2];

                    if (coord is Service.DataContracts.Model.v20.CoordType)
                    {
                        var coordType = coord as Service.DataContracts.Model.v20.CoordType;

                        point[0] = coordType.c1;
                        point[1] = coordType.c2;

                        var pX = (float)((point[0] - extent[0]) / conversionFactor);
                        var pY = (float)((extent[3] - point[1]) / conversionFactor);

                        points.Add(new PointF(pX, pY));
                    }
                    else if (coord is Service.DataContracts.Model.v20.ArcSegmentType)
                    {
                        var arcSegmentType = coord as Service.DataContracts.Model.v20.ArcSegmentType;

                        point[0] = arcSegmentType.c1;
                        point[1] = arcSegmentType.c2;

                        var pX = (float)((point[0] - extent[0]) / conversionFactor);
                        var pY = (float)((extent[3] - point[1]) / conversionFactor);

                        points.Add(new PointF(pX, pY));
                    }
                    else if (coord is Service.DataContracts.Model.v20.LineSegmentType)
                    {
                        var lineSegmentType = coord as Service.DataContracts.Model.v20.LineSegmentType;

                        point[0] = lineSegmentType.c1;
                        point[1] = lineSegmentType.c2;

                        var pX = (float)((point[0] - extent[0]) / conversionFactor);
                        var pY = (float)((extent[3] - point[1]) / conversionFactor);

                        points.Add(new PointF(pX, pY));
                    }
                }

                graphic.DrawLines(pen, points.ToArray());
                graphic.Flush();
            }

            bitmap = (Bitmap)AddScalebarAndOrientation(bitmap, extent, width, height, 0.2, 30); //, srs);

            // bitmap.Save(@"c:\temp\testrasterize6.png", System.Drawing.Imaging.ImageFormat.Png);

            return bitmap;
        }


        public static Image AddScalebarAndOrientation(Image image, double[] extent, int width, int height, double maxPercent, int offset)
        {
            Graphics graphic = Graphics.FromImage(image);
            graphic.SmoothingMode = SmoothingMode.AntiAlias;
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //todo replace staic image pathes
            var northArrow = Image.FromFile(Path.Combine(GetRootPath(), "Image/report_NorthArrow.png"));
            var scalebar = Image.FromFile(Path.Combine(GetRootPath(), "Image/report_Scalebar.png"));

            var distanceH = (extent[2] - extent[0]);
            var meterPerPixel = distanceH / width;

            var widthScaleBarMeter = Math.Round(distanceH * maxPercent / 10, 0) * 10;
            var widthScalebarPixel = (int)(widthScaleBarMeter / meterPerPixel);
            var heightScalebarPixel = (int)((double)widthScalebarPixel / scalebar.Width * scalebar.Height);
            var fontheight = (int)Math.Round(width / 58d, 0);
            if (fontheight == 0)
            {
                fontheight = 1;
            }

            var lrX = (int)0;
            var lrY = (int)height;

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            //graphic.DrawString("0", new Font("Arial", fontheight, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black,new PointF() {X = (float)lrX + offset, Y = (float)(lrY - offset)}, stringFormat);
            //graphic.DrawString($"{widthScaleBarMeter/2:0#}", new Font("Arial", fontheight, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, new PointF() { X = (float)(lrX + offset + widthScalebarPixel/2), Y = (float)(lrY - offset) }, stringFormat);
            //graphic.DrawString($"{widthScaleBarMeter:0#} m", new Font("Arial", fontheight, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, new PointF() { X = (float)(lrX + offset + widthScalebarPixel), Y = (float)(lrY - offset) }, stringFormat);

            var font = new Font("Arial", fontheight, FontStyle.Regular, GraphicsUnit.Pixel);
            AddText(graphic, "0", font, stringFormat, new PointF() { X = (float)lrX + offset, Y = (float)(lrY - offset) });
            AddText(graphic, $"{widthScaleBarMeter / 2:0#}", font, stringFormat, new PointF() { X = (float)(lrX + offset + widthScalebarPixel / 2), Y = (float)(lrY - offset) });
            AddText(graphic, $"{widthScaleBarMeter:0#} m", font, stringFormat, new PointF() { X = (float)(lrX + offset + widthScalebarPixel), Y = (float)(lrY - offset) });

            graphic.DrawImage(
                scalebar,
                new Rectangle(
                    lrX + offset,
                    lrY - offset - fontheight - heightScalebarPixel,
                    widthScalebarPixel,
                    heightScalebarPixel
                )
            );

            var widthNorthArrowPixel = (int)(widthScalebarPixel / 5);
            var heightNorthArrowPixel = (int)((double)widthNorthArrowPixel / (double)northArrow.Width * northArrow.Height);

            graphic.DrawImage(
                northArrow,
                new Rectangle
                (
                    lrX + offset + widthScalebarPixel / 2 - widthNorthArrowPixel / 2,
                    lrY - offset - fontheight - heightScalebarPixel - offset - heightNorthArrowPixel,
                    widthNorthArrowPixel,
                    heightNorthArrowPixel
                )
            );

            graphic.Flush();
            return image;
        }

        public static void AddText(Graphics graphic, string value, Font font, StringFormat stringFormat, PointF pointF)
        {
            var graphicsPath = new GraphicsPath();
            var outline = new Pen(Brushes.White, 8) { LineJoin = LineJoin.Round };

            using (Brush foreBrush = new SolidBrush(Color.Black))
            {
                graphicsPath.AddString(value, font.FontFamily, (int)font.Style, font.Size, pointF, stringFormat);
                graphic.DrawPath(outline, graphicsPath);
                graphic.FillPath(foreBrush, graphicsPath);
            }
        }

        public static XElement RemoveAllNamespaces(string xmlDocument)
        {
            return RemoveAllNamespaces(XElement.Parse(xmlDocument));
        }

        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                {
                    xElement.Add(attribute);
                }

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }

        private static string GetRootPath()
        {
            var assemblyPath = (new Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
            return new FileInfo(assemblyPath).Directory?.Parent?.FullName;
        }

        private static string BufferGeomtry(string geometryGml, double offset)
        {
            GdalConfiguration.ConfigureOgr();

            try
            {
                var geometry = Ogr.CreateGeometryFromGML(geometryGml);
                var buffer = geometry.Buffer(offset, 4);

                return buffer.ExportToGML();
            }
            catch (Exception ex)
            {
                return geometryGml;
            }
        }
    }
}
