using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MobileDatingCMS.Startup))]
namespace MobileDatingCMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
