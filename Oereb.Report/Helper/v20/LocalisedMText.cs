using System.Linq;

namespace Oereb.Report.Helper.v20
{
    public static class LocalisedMText
    {
        public static string GetStringFromArray(Service.DataContracts.Model.v20.LocalisedMText[] localisedMTexts, string language, string defaultValue = "-")
        {
            if (localisedMTexts.Any(x => x.Language.ToString() == language))
            {
                return localisedMTexts.First().Text;
            }

            return defaultValue;
        }
        public static string GetStringFromFirst2DArray(Service.DataContracts.Model.v20.GeneralInformation[] generalInformations, string language, string defaultValue = "-")
        {
            if (generalInformations.First().LocalisedText.Any(x => x.Language.ToString() == language))
            {
                return generalInformations.First().LocalisedText.First(x => x.Language.ToString() == language).Text;
            }

            return defaultValue;
        }
        public static string GetStringFromLast2DArray(Service.DataContracts.Model.v20.GeneralInformation[] generalInformations, string language, string defaultValue = "-")
        {
            if (generalInformations.Last().LocalisedText.Any(x => x.Language.ToString() == language))
            {
                return generalInformations.Last().LocalisedText.First(x => x.Language.ToString() == language).Text;
            }

            return defaultValue;
        }
    }
}
