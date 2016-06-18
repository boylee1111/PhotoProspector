using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PhotoProspector.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace PhotoProspector
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
