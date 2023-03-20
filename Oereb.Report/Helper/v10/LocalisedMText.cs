using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oereb.Report.Helper.v10
{
    public static class LocalisedMText
    {
        public static string GetStringFromArray(Service.DataContracts.Model.v10.LocalisedMText[] localisedMTexts, string language, string defaultValue = "-")
        {
            if (localisedMTexts.Any(x => x.Language.ToString() == language))
            {
                return localisedMTexts.First().Text;
            }

            return defaultValue;
        }
    }
}
