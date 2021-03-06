﻿using ECA.WebApi.Security;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;

namespace ECA.WebApi.Custom.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public class DebugWebApiUserHandler : DelegatingHandler
    {
        /// <summary>
        /// 
        /// </summary>
        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
#if DEBUG
            if (request.RequestUri.ToString().ToLower().Contains("api")
                && request.Headers != null
                && request.Headers.Referrer != null
                && request.Headers.Referrer.ToString().ToLower().Contains("http://localhost:5555/swagger/ui/index"))
            {
                var debugUser = new DebugWebApiUser();
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(debugUser.GetClaims(), "Bearer"));
                Thread.CurrentPrincipal = claimsPrincipal;
                HttpContext.Current.User = claimsPrincipal;
            }
#endif
            return base.SendAsync(request, cancellationToken);
        }
    }
}