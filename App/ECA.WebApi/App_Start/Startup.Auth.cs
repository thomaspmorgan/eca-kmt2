using System;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Routing;
using Microsoft.Owin;
using Owin;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security.ActiveDirectory;
using ECA.WebApi.Common;

namespace ECA.WebApi
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                {
                    TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = AppSettings.AdClientId
                    },
                    Tenant = AppSettings.AdTenantId,
                });
        }

        //public void ConfigureActiveDirectory()
        //{
        //    var ad = AdGraph.Instance;
        //    ad.CreateGroup(SecurityGroups.GlobalAdministrator, "Administer all users and data.");
        //    ad.CreateGroup(SecurityGroups.PostAdministrator, "Administer local users and data.");
        //    ad.CreateGroup(SecurityGroups.Approver, "Read, write, and approve for publication.");
        //    ad.CreateGroup(SecurityGroups.StandardUser, "Read and write application data.");
        //    ad.CreateGroup(SecurityGroups.Guest, "Read application data.");
        //}
    }
}