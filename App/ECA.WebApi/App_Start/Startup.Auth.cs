using System;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Routing;
using Microsoft.Owin;
using Owin;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security.ActiveDirectory;
using ECA.Core.Settings;

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
        /// <param name="app"></param>
        public void ConfigureAuth(IAppBuilder app)
        {
            var appSettings = new AppSettings();
            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                {
                    TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = appSettings.AdClientId
                    },
                    Tenant = appSettings.AdTenantId,
                });
        }
    }
}