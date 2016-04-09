using ECA.WebApi.Custom.Filters;
using NLog;
using System;
using System.ComponentModel;
using System.Text;

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

        /// <summary>
        /// 
        /// </summary>
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
            GlobalDiagnosticsContext.Clear();
        }
    }
}