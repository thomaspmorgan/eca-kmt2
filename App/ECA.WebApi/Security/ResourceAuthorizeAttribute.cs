using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ECA.WebApi.Security
{
    public class ResourceAuthorizeAttribute : AuthorizeAttribute
    {
        public string ArgumentName { get; set; }

        public object ResourceType { get; set; }

        public object PermissionName { get; set; }

        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //var programId = actionContext.ActionArguments[ArgumentName];
            //var user = HttpContext.Current.User;
            //var webApiUser = new WebApiUser(user);

            //return webApiUser.HasPermission(null, null);
            return true;

        }
    }
}