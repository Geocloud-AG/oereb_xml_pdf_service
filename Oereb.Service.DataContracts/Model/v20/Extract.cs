﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Oereb.Service.DataContracts.Model.v20
{
    // 
    // This source code was auto-generated by xsd, Version=4.8.3928.0.
    // 

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.geo.admin.ch/V_D/OeREB/2.0/Extract")]
    [System.Xml.Serialization.XmlRootAttribute("GetExtractByIdResponse", Namespace = "http://schemas.geo.admin.ch/V_D/OeREB/2.0/Extract", IsNullable = false)]
    public partial class GetExtractByIdResponseType
    {
        private Extract extractField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.geo.admin.ch/V_D/OeREB/2.0/ExtractData")]
        public Extract Extract
        {
            get
            {
                return this.extractField;
            }
            set
            {
                this.extractField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.geo.admin.ch/V_D/OeREB/2.0/Extract")]
    [System.Xml.Serialization.XmlRootAttribute("GetEGRIDResponse", Namespace = "http://schemas.geo.admin.ch/V_D/OeREB/2.0/Extract", IsNullable = false)]
    public partial class GetEGRIDResponseType
    {

        private string[] egridField;

        private string[] numberField;

        private string[] identDNField;

        private RealEstateType[] typeField;

        private MultiSurfaceType[] limitField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("egrid", DataType = "token")]
        public string[] egrid
        {
            get
            {
                return this.egridField;
            }
            set
            {
                this.egridField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("number", DataType = "token")]
        public string[] number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("identDN", DataType = "token")]
        public string[] identDN
        {
            get
            {
                return this.identDNField;
            }
            set
            {
                this.identDNField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("type")]
        public RealEstateType[] type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("limit")]
        public MultiSurfaceType[] limit
        {
            get
            {
                return this.limitField;
            }
            set
            {
                this.limitField = value;
            }
        }
    }

    /** EXTENTED */
    public partial class GetEGRIDResponseTypeExtended : GetEGRIDResponseType
    {
        private string[] shapeField;
        private string[] kindOfField;

        [System.Xml.Serialization.XmlElementAttribute("shape", DataType = "token")]
        public string[] shape
        {
            get
            {
                return this.shapeField;
            }
            set
            {
                this.shapeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("kindOf", DataType = "token")]
        public string[] kindOf
        {
            get
            {
                return this.kindOfField;
            }
            set
            {
                this.kindOfField = value;
            }
        }
    }
    /** END EXTENTED */

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.geo.admin.ch/V_D/OeREB/2.0/Extract")]
    [System.Xml.Serialization.XmlRootAttribute("GetCapabilitiesResponse", Namespace = "http://schemas.geo.admin.ch/V_D/OeREB/2.0/Extract", IsNullable = false)]
    public partial class GetCapabilitiesResponseType
    {

        private Theme[] topicField;

        private string[] municipalityField;

        private string[] flavourField;

        private string[] languageField;

        private string[] crsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("topic")]
        public Theme[] topic
        {
            get
            {
                return this.topicField;
            }
            set
            {
                this.topicField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("municipality", DataType = "integer")]
        public string[] municipality
        {
            get
            {
                return this.municipalityField;
            }
            set
            {
                this.municipalityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("flavour", DataType = "token")]
        public string[] flavour
        {
            get
            {
                return this.flavourField;
            }
            set
            {
                this.flavourField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("language", DataType = "token")]
        public string[] language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("crs", DataType = "token")]
        public string[] crs
        {
            get
            {
                return this.crsField;
            }
            set
            {
                this.crsField = value;
            }
        }
    }
}