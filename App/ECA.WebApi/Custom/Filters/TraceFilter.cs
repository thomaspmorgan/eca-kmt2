using ECA.Core.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace ECA.WebApi.Custom.Filters
{
    public class TraceFilter : ActionFilterAttribute
    {
        private ILogger logger;
        private Stopwatch stopwatch;

        public TraceFilter(ILogger logger)
        {
            Contract.Requires(logger != null, "The logger must not be null.");
            this.logger = logger;
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
            logger.TraceApi(controllerName + "Controller", stopwatch.Elapsed, actionName);
        }
    }
}