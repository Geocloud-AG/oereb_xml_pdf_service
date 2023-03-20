using System.Web.Http;

namespace Oereb.Service
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            Helper.v10.SettingsReader.ReadFromConfig();
            Helper.v20.SettingsReader.ReadFromConfig();
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        }
    }
}
