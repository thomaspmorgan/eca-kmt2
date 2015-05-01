using ECA.WebApi.Custom.Filters;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;

namespace ECA.WebApi.Custom.LayoutRenders
{
    [NLog.LayoutRenderers.LayoutRenderer("webapiaction")]
    public class ActionParametersLayoutRenderer : NLog.LayoutRenderers.LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var controllerName = GlobalDiagnosticsContext.Get(LoggerExceptionHandler.CONTROLLER_CONTEXT_KEY);
            var actionName = GlobalDiagnosticsContext.Get(LoggerExceptionHandler.ACTION_CONTEXT_KEY);
            var actionArguments = GlobalDiagnosticsContext.Get(LoggerExceptionHandler.ACTION_ARGUMENTS_CONTEXT_KEY);
            builder.AppendLine();
            builder.AppendLine(String.Format(
                "Action Arguments for {0}Controller.{1} =>", 
                controllerName == null ? "null" : controllerName, 
                actionName == null ? "null" : actionName));
            builder.AppendLine(actionArguments == null ? "null" : actionArguments);
        }
    }
}