using System.Linq;

namespace Oereb.Report.Helper.v20
{
    public static class LocalisedUri
    {
        public static string GetStringFromArray(Service.DataContracts.Model.v20.LocalisedUri[] localisedUri, string language, string defaultValue = "-")
        {
            if (localisedUri.Any(x => x.Language.ToString() == language))
            {
                return localisedUri.First().Text;
            }

            return defaultValue;
        }
    }
}
