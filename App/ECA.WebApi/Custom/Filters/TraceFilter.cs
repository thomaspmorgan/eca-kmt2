using ECA.Core.Logging;
using ECA.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace ECA.WebApi.Custom.Filters
{
    public class TraceFilter : ActionFilterAttribute
    {
        private ILogger logger;
        private Stopwatch stopwatch;
        private IUserProvider userProvider;

        public TraceFilter(ILogger logger, IUserProvider userProvider)
        {
            Contract.Requires(logger != null, "The logger must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            this.logger = logger;
            this.userProvider = userProvider;
        }

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            stopwatch = Stopwatch.StartNew();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            stopwatch.Stop();

            var controllerName = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            var actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
            var user = HttpContext.Current.User;
            var userName = new AnonymousUser().GetUsername();
            if (user != null && user.Identity.IsAuthenticated)
            {
                var providedUser = userProvider.GetCurrentUser();
                Debug.Assert(user is ClaimsPrincipal, "The user should be a claims principal.");
                var claimsPrincipal = user as ClaimsPrincipal;
                var webApiUser = new WebApiUser(this.logger, claimsPrincipal);
                userName = webApiUser.GetUsername();
            }
            
            logger.TraceApi(String.Format("[{0}]:  {1}Controller", userName, controllerName), stopwatch.Elapsed, actionName);
        }
    }
}