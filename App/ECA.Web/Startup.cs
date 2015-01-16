using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ECA.Web.Startup))]
namespace ECA.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
