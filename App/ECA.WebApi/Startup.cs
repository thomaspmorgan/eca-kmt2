using System;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Routing;
using Microsoft.Owin;
using Owin;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security.ActiveDirectory;
using ECA.WebApi.Common;

[assembly: OwinStartup(typeof(ECA.WebApi.Startup))]

namespace ECA.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            if (!AppSettings.PdTrackerOpenNetMode)
            {
                ConfigureAuth(app);
                //ConfigureActiveDirectory();
            }

            GlobalConfiguration.Configure(WebApiConfig.Register);
            //FilterConfig.RegisterGlobalFilters();
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

    }
}
