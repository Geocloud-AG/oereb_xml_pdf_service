﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

namespace Oereb.Service.Helper
{
    public class XmlValidation
    {
        public List<string> Messages { get; set; }

        public XmlValidation()
        {
            Messages = new List<string>();
        }

        public bool Validate(string xmlContent, string schemapath)
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();

                settings.DtdProcessing = DtdProcessing.Parse;

                settings.Schemas.Add("http://schemas.geo.admin.ch/V_D/OeREB/1.0/ExtractData", $"{schemapath}ExtractData.xsd");
                settings.Schemas.Add("http://www.w3.org/2000/09/xmldsig#", $"{schemapath}xmldsig-core-schema_mod.xsd");
                settings.Schemas.Add("http://www.opengis.net/gml/3.2", $"{schemapath}gml.xsd");
                settings.Schemas.Add("http://www.w3.org/1999/xlink", $"{schemapath}xlink.xsd");

                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessIdentityConstraints;

                settings.ValidationType = ValidationType.Schema;

                XmlReader reader = XmlReader.Create(new StringReader(xmlContent), settings);
                XmlDocument document = new XmlDocument();
                document.Load(reader);

                ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);

                document.Validate(eventHandler);
            }
            catch (Exception ex)
            {
                Messages.Add(ex.Message);
                return false;
            }

            return true;
        }

        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    Messages.Add($"Error: {e.Message}");
                    break;
                case XmlSeverityType.Warning:
                    Messages.Add($"Warning: {e.Message}");
                    break;
            }
        }
    }
}