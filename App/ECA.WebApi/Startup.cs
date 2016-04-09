using System.Web.Http;
using Microsoft.Owin;
using Owin;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(ECA.WebApi.Startup))]

namespace ECA.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
        }

    }
}
