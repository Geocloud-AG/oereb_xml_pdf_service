using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Oereb.Service.DataContracts.Model.v10
{
    /// <summary>
    /// convert the parameters from the controllers into a valid format
    /// </summary>

    public class Options
    {
        public enum ResponseType
        {
            Mimetype,
            Specification,
            Url
        }

        public Settings.Format Format { get; set; }
        public Settings.Flavour Flavour { get; set; }
        public Settings.Language Language { get; set; }
        public bool Geometry { get; set; }
        public List<string> Topics { get; set; }
        public string Canton { get; set; }
        public bool WithImages { get; set; } = true;

        public ResponseType Response { get; set; }
        public bool AppendixAttached { get; set; } = false;
        public bool Details { get; set; } = false;

        public bool Cache { get; set; } = true;

        private List<Regex> _topicRegexes { get; set; }

        #region constructor

        public Options()
        {
            Format = Settings.Format.Xml;
            Flavour = Settings.Flavour.Reduced;
            Language = Settings.Language.De;
            Geometry = true;
            Topics = new List<string>() { Settings.TopicAll };
            Canton = string.Empty;
            Response = ResponseType.Specification;

            _topicRegexes = new List<Regex>();

            foreach (var canton in Settings.AvailableCantonsAndTopics)
            {
                foreach (var topic in canton.Value)
                {
                    _topicRegexes.Add(new Regex($"^ch.{canton.Key}.{topic}$"));
                }
            }
        }

        public Options(string format, string flavour, string language, string topics, bool geometry, string canton = "", string response = "Specification", bool appendixAttached = false, bool details = false, bool withImages = true, bool cache = true) : this()
        {
            Settings.Format formatParsed;

            Canton = canton;

            if (Enum.TryParse(format, true, out formatParsed))
            {
                Format = formatParsed;
            }
            else
            {
                throw new ExtException($"no valid format: {format}", this, 1);
            }

           if (!Settings.SupportedFormats.Contains(Format))
            {
                throw new ExtException($"no supported format: {format}", this , 2);
            }

            Settings.Flavour flavourParsed;

            if (Enum.TryParse(flavour, true, out flavourParsed))
            {
                Flavour = flavourParsed;
            }
            else
            {
                throw new ExtException($"no valid flavor: {flavour}", this, 3);
            }

            if (!Settings.SupportedFlavours.Contains(Flavour))
            {
                throw new ExtException($"no supported falvour: {flavour}", this, 4);
            }

            Settings.Language languageParsed;

            if (Enum.TryParse(language, true, out languageParsed))
            {
                Language = languageParsed;
            }
            else
            {
                throw new ExtException($"no valid language: {language}", this, 5);
            }

            if (!Settings.SupportedLanguages.Contains(Language))
            {
                throw new ExtException($"no supported language: {language}", this, 6);
            }

            Geometry = geometry;

            Topics = new List<string>();

            if (topics == Settings.TopicAll || String.IsNullOrEmpty(topics) || topics.Split(Settings.TopicSeparator).Contains(Settings.TopicAll))
            {
                Topics.Add(Settings.TopicAll);
            }
            else if (topics == Settings.TopicAllFederal || topics.Split(Settings.TopicSeparator).Contains(Settings.TopicAllFederal))
            {
                Topics.Add(Settings.TopicAllFederal);
            }
            else
            {
                var topicList = topics.Split(Settings.TopicSeparator);
                Topics.AddRange(topicList.Where(x=> IsTopicValid(x)).ToList());
            }

            if (!Topics.Any())
            {
                throw new ExtException($"no valid themes: {topics}", this, 7);
            }

            ResponseType responseParsed;

            if (Enum.TryParse(response, true, out responseParsed))
            {
                Response = responseParsed;
            }
            else
            {
                throw new ExtException($"no valid type of response: {response}", this, 9);
            }

            AppendixAttached = appendixAttached;
            Details = details;
            WithImages = withImages;

            if (!IsParameterCombinationValid())
            {
                throw new ExtException($"parameter combination is not valid: format: {format}, flavour: {flavour}", this, 8);
            }

            Cache = cache;
        }

        #endregion

        #region private

        private bool IsTopicValid(string topicname)
        {
            return _topicRegexes.Any(topicRegex => topicRegex.IsMatch(topicname));
        }

        /// <summary>
        /// check for combination see table in chapter 3.2.4.1
        /// </summary>
        /// <returns></returns>

        private bool IsParameterCombinationValid()
        {
            if (Format == Settings.Format.Pdf)
            {
                return (Flavour == Settings.Flavour.Reduced || Flavour == Settings.Flavour.Full || Flavour == Settings.Flavour.Signed);
            }

            if (Format == Settings.Format.Xml)
            {
                return (Flavour == Settings.Flavour.Reduced || Flavour == Settings.Flavour.Embeddable);
            }

            if (Format == Settings.Format.Html)
            {
                return (Flavour == Settings.Flavour.Reduced);
            }

            if (Format == Settings.Format.PdfA1a)
            {
                return (Flavour == Settings.Flavour.Reduced);
            }

            if (Format == Settings.Format.Json)
            {
                return (Flavour == Settings.Flavour.Reduced || Flavour == Settings.Flavour.Embeddable) && Response == ResponseType.Mimetype; //todo federal query does not support json at this time
            }

            return false;
        }

        #endregion

        #region public

        public override string ToString()
        {
            return $"format: {Format}, flavour: {Flavour}, response: {Response}, details: {Details}, geometry: {Geometry}, canton: {Canton}, appendixAttached: {AppendixAttached}, language: {Language}, withImages: {WithImages}, topics: {Topics.Aggregate((i, j)=> i + "," + j)}";
        }

        #endregion
    }
}
