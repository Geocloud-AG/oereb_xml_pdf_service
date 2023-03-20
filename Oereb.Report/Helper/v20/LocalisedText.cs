using System.Linq;

namespace Oereb.Report.Helper.v20
{
    public static class LocalisedText
    {
        public static string GetStringFromArray(Service.DataContracts.Model.v20.LocalisedText[] localisedText, string language, string defaultValue = "-")
        {
            if (localisedText == null)
            {
                return defaultValue;
            }

            if (localisedText.Any(x=> x.Language.ToString() == language))
            {
                return localisedText.First().Text;
            }

            return defaultValue;
        }

        public static string GetUriFromArray(Service.DataContracts.Model.v20.LocalisedUri[] localisedUri, string language, string defaultValue = "-")
        {
            if (localisedUri == null)
            {
                return defaultValue;
            }

            if (localisedUri.Any(x => x.Language.ToString() == language))
            {
                return localisedUri.First().Text;
            }

            return defaultValue;
        }
    }
}
