using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PhotoProspector.App_Start;

namespace PhotoProspector
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DependencyConfig.RegisterComponents();
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
        }
    }
}
