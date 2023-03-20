using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oereb.Service.DataContracts;
using System.Collections.Generic;
using System.Linq;
using Oereb.Service.Tests.Helper;
using Oereb.Service.DataContracts.Model.v10;
using Oereb.Service.Helper.v10;

namespace Oereb.Service.Tests
{
    [TestClass]
    public class OptionsTest
    {
        public OptionsTest()
        {
            SettingsReader.ReadFromConfig();
        }

        [TestMethod]
        public void ValidParametersXmlReduced()
        {           
            var options = new Options("xml", "reduced", "de", "", false);

            Assert.IsNotNull(options);
            Assert.AreEqual(Settings.Flavour.Reduced, options.Flavour);
            Assert.AreEqual(Settings.Format.Xml, options.Format);
            Assert.AreEqual(Settings.Language.De, options.Language);
            Assert.AreEqual(1, options.Topics.Count);
            Assert.AreEqual("ALL", options.Topics.First());
        }

        [TestMethod]
        public void ValidParametersXmlEmbeddable()
        {
            var options = new Options("xml", "embeddable", "de", "", false);

            Assert.IsNotNull(options);
            Assert.AreEqual(Settings.Flavour.Embeddable, options.Flavour);
            Assert.AreEqual(Settings.Format.Xml, options.Format);
            Assert.AreEqual(Settings.Language.De, options.Language);
            Assert.AreEqual(1, options.Topics.Count);
            Assert.AreEqual("ALL", options.Topics.First());
        }

        [TestMethod]
        public void ValidParametersPdfReduced()
        {
            var options = new Options("pdf", "reduced", "de", "", false);

            Assert.IsNotNull(options);
            Assert.AreEqual(Settings.Flavour.Reduced, options.Flavour);
            Assert.AreEqual(Settings.Format.Pdf, options.Format);
            Assert.AreEqual(Settings.Language.De, options.Language);
            Assert.AreEqual(1, options.Topics.Count);
            Assert.AreEqual("ALL", options.Topics.First());
        }

        [TestMethod]
        public void ValidParametersPdfReducedAllTopics()
        {
            var options = new Options("pdf", "reduced", "de", "ALL", false);

            Assert.IsNotNull(options);
            Assert.AreEqual(Settings.Flavour.Reduced, options.Flavour);
            Assert.AreEqual(Settings.Format.Pdf, options.Format);
            Assert.AreEqual(Settings.Language.De, options.Language);
            Assert.AreEqual(1, options.Topics.Count);
            Assert.AreEqual("ALL", options.Topics.First());
        }

        [TestMethod]
        public void ValidParametersTopicFederal()
        {
            var options = new Options("pdf", "reduced", "de", "ALL_FEDERAL", false);

            Assert.IsNotNull(options);
            Assert.AreEqual(1, options.Topics.Count);
            Assert.AreEqual("ALL_FEDERAL", options.Topics.First());
        }

        [TestMethod]
        public void ValidParametersTopicMix()
        {
            var options = new Options("pdf", "reduced", "de", "ALL_FEDERAL,73", false); //if ALL or ALL_FEDERAL the other topics are cut away

            Assert.IsNotNull(options);
            Assert.AreEqual(1, options.Topics.Count);
            Assert.AreEqual("ALL_FEDERAL", options.Topics.First());
        }

        [TestMethod]
        public void ValidParametersTopicList()
        {
            var options = new Options("pdf", "reduced", "de", "ch.nw.73,ch.nw.88", false);

            Assert.IsNotNull(options);
            Assert.AreEqual(2, options.Topics.Count);
            Assert.AreEqual("ch.nw.73", options.Topics.First());
            Assert.AreEqual("ch.nw.88", options.Topics.Last());
        }

        [TestMethod]
        public void ValidParametersWrongFormat()
        {
            TestUtilities.AssertErrorcodeEqual<ExtException>(
               () => new Options("pdf2", "reduced", "de", "ALL", false),
               "Options.1"
           );
        }

        [TestMethod]
        public void ValidParametersNotSupportedFormat()
        {
            TestUtilities.AssertErrorcodeEqual<ExtException>(
               () => new Options("json", "reduced", "de", "ALL", false),
               "Options.2"
           );
        }

        [TestMethod]
        public void ValidParametersWrongFlavour()
        {
            TestUtilities.AssertErrorcodeEqual<ExtException>(
               () => new Options("pdf", "reduced2", "de", "ALL", false),
               "Options.3"
           );
        }

        [TestMethod]
        public void ValidParametersNotSupportedFlavour()
        {
            TestUtilities.AssertErrorcodeEqual<ExtException>(
               () => new Options("pdf", "signed", "de", "ALL", false),
               "Options.4"
           );
        }

        [TestMethod]
        public void ValidParametersWrongLanguage()
        {
            TestUtilities.AssertErrorcodeEqual<ExtException>(
               () => new Options("pdf", "reduced", "de2", "ALL", false),
               "Options.5"
           );
        }

        [TestMethod]
        public void ValidParametersNotSupportedLanguage()
        {
            TestUtilities.AssertErrorcodeEqual<ExtException>(
               () => new Options("pdf", "reduced", "it", "ALL", false),
               "Options.6"
           );
        }

        [TestMethod]
        public void ValidParametersWrongTopic()
        {
            TestUtilities.AssertErrorcodeEqual<ExtException>(
               () => new Options("pdf", "reduced", "de", "ALL2", false),
               "Options.7"
           );
        }

        [TestMethod]
        public void ValidParametersWrongTopicSchemaOk()
        {
            TestUtilities.AssertErrorcodeEqual<ExtException>(
               () => new Options("pdf", "reduced", "de", "ch.ag.73", false),
               "Options.7"
           );
        }

        [TestMethod]
        public void ValidParametersWrongCaseSensitive()
        {
            TestUtilities.AssertErrorcodeEqual<ExtException>(
               () => new Options("pdf", "reduced", "de", "ALL_Federal", false),
               "Options.7"
           );
        }

        [TestMethod]
        public void ValidParametersInvalidParameterCombination()
        {
            TestUtilities.AssertErrorcodeEqual<ExtException>(
               () => new Options("pdf", "embeddable", "de", "ALL_FEDERAL", false),
               "Options.8"
           );
        }
    }
}
