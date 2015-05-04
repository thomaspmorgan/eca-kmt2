using ECA.WebApi.Custom.Filters;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// <summary>
    /// The ActionParametersLayoutRenderer will append to an NLog event the arguments
    /// that were bound to a web api action.
    /// </summary>
    [NLog.LayoutRenderers.LayoutRenderer("webapiaction")]
    public class ActionParametersLayoutRenderer : NLog.LayoutRenderers.LayoutRenderer
    {
        /// <summary>
        /// Gets or sets IsEnabled, if true, action arguments will be logged.
        /// </summary>
        [DefaultValue(false)]
        public bool IsEnabled { get; set; }

        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            if (IsEnabled)
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
            else
            {
                builder.AppendLine();
                builder.AppendLine("Action argument logging disabled.");
            }
        }
    }
}