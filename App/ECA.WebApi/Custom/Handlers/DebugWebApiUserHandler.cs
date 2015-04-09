using ECA.Core.Logging;
using ECA.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace ECA.WebApi.Custom.Handlers
{
    public class DebugWebApiUserHandler : DelegatingHandler
    {
        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
#if DEBUG
            if (request.RequestUri.ToString().ToLower().Contains("api")
                && request.Headers != null
                && request.Headers.Referrer != null
                && request.Headers.Referrer.ToString().ToLower().Contains("http://localhost:5555/swagger/ui/index"))
            {
                var logger = (ILogger)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ILogger));
                var debugUser = new DebugWebApiUser(logger);
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(debugUser.GetClaims(), "Bearer"));
                Thread.CurrentPrincipal = claimsPrincipal;
                HttpContext.Current.User = claimsPrincipal;
            }
#endif
            return base.SendAsync(request, cancellationToken);
        }
    }
}