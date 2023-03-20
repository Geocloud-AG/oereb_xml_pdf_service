using log4net;
using System;
using System.Reflection;
using System.Web;

namespace Oereb.Report.Helper.v20
{
    public class Wms
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static System.Drawing.Image GetMap(string imageUrl)
        {
            var decodedImageUrl = HttpUtility.UrlDecode(imageUrl);

            if (string.IsNullOrEmpty(decodedImageUrl))
            {
                Log.Error($"error request wms server: {decodedImageUrl} || {imageUrl} is not valid");
                return null;
            }

            System.Drawing.Image image;

            try
            {
                var webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(decodedImageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;

                using (var webResponse = webRequest.GetResponse())
                {
                    if (!(webResponse.ContentType.ToLower().StartsWith("image/png") || webResponse.ContentType.ToLower().StartsWith("image/jpeg") || webResponse.ContentType.ToLower().StartsWith("image/jpg")))
                    {
                        Log.Error($"error response wms server: {imageUrl}, mimetype {webResponse.ContentType}");
                        return null;
                    }
                    
                    using (var stream = webResponse.GetResponseStream())
                    {
                        image = System.Drawing.Image.FromStream(stream);
                    }
                }                               
            }
            catch (Exception ex)
            {
                Log.Error($"error request wms server: {imageUrl}, error {ex.Message}");
                return null;
            }

            return image;
        }
    }
}