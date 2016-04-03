using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PhotoProspector.Startup))]
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
