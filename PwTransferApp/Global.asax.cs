using System.Data.Entity;
using System.Web.Http;
using Common.DbContext;
using log4net.Config;
using PwTransferApp.ContainerConfig;
using PwTransferApp.Models.Identity;

namespace PwTransferApp
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            Container.Init();

            Database.SetInitializer(new DataContextInitializer());
            Database.SetInitializer(new ApplicationDbContextInitializer());
            XmlConfigurator.Configure();
        }
    }
}
