using ECA.Core.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace ECA.WebApi.Custom.Filters
{
    /// <summary>
    /// A Global WebAPI exception logger using an ILogger.
    /// </summary>
    public class LoggerExceptionHandler : ExceptionLogger
    {
        private ILogger logger;

        /// <summary>
        /// Creates a new LoggerExceptionHandler with the given ILogger.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public LoggerExceptionHandler(ILogger logger)
        {
            Debug.Assert(logger != null, "The logger must not be null.");
            this.logger = logger;
        }

        /// <summary>
        /// Overrides the Log method.
        /// </summary>
        /// <param name="context">The ExceptionLoggerContext instance.</param>
        public override void Log(ExceptionLoggerContext context)
        {
            base.Log(context);
            logger.Error(GetMessage(context));
        }

        private string GetMessage(ExceptionLoggerContext context)
        {
            var utcNow = DateTime.UtcNow;
            var localTime = utcNow.ToLocalTime();
            var timeZone = TimeZone.CurrentTimeZone;
            var sb = new StringBuilder();
            sb.AppendFormat("Exception occurred at {0} UTC ({1} {2})", utcNow.ToString(), utcNow.ToLocalTime().ToString(), timeZone.StandardName);
            sb.AppendLine(String.Empty);

            if (context.ExceptionContext.ActionContext != null)
            {
                var actionContext = context.ExceptionContext.ActionContext;
                sb.AppendFormat("Request Uri:  {0}", actionContext.Request.RequestUri.ToString());
                sb.AppendLine(String.Empty);

                sb.AppendFormat("User:  {0}", GetUser(context));
                sb.AppendLine(String.Empty);

                var actionName = actionContext.ActionDescriptor.ActionName;
                sb.AppendFormat("Controller Name:  {0}", actionContext.ControllerContext.ControllerDescriptor.ControllerName);
                sb.AppendLine(String.Empty);

                sb.AppendFormat("Action Name:  {0}", actionName);
                sb.AppendLine(String.Empty);

                var actionArguments = actionContext.ActionArguments;
                if (actionArguments != null && actionArguments.Count > 0)
                {
                    var actionArgumentsJson = Jsonify(actionArguments);
                    sb.AppendFormat("Action Arguments:  {0}", actionArgumentsJson);
                    sb.AppendLine(String.Empty);
                }
            }
            sb.Append(context.Exception.ToString());
            return sb.ToString();
        }

        private string Jsonify(Dictionary<string, object> actionArguments)
        {
            var json = JsonConvert.SerializeObject(actionArguments);
            var prettyJson = JValue.Parse(json).ToString(Formatting.Indented);
            return prettyJson;
        }

        private string GetUser(ExceptionLoggerContext context)
        {
            var user = HttpContext.Current.User;
            if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
            {
                return user.Identity.Name;
            }
            else
            {
                return "Anonymous";
            }
        }
    }
}