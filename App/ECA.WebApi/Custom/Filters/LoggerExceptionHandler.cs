﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace ECA.WebApi.Custom.Filters
{
    /// <summary>
    /// A Global WebAPI exception logger using NLog.
    /// </summary>
    public class LoggerExceptionHandler : ExceptionLogger
    {
        public const string ACTION_ARGUMENTS_CONTEXT_KEY = "actionArguments";

        public const string CONTROLLER_CONTEXT_KEY = "controllerName";

        public const string ACTION_CONTEXT_KEY = "actionName";

        private readonly Logger logger = LogManager.GetCurrentClassLogger();


        public override void Log(ExceptionLoggerContext context)
        {
            AddActionArguments(context);
            AddControllerAndAction(context);
            logger.Log(LogLevel.Error, RequestToString(context.Request), context.Exception);
        }

        private static string RequestToString(HttpRequestMessage request)
        {   
            var message = new StringBuilder();
            if (request.Method != null)
                message.Append(request.Method);

            if (request.RequestUri != null)
                message.Append(" ").Append(request.RequestUri);

            return message.ToString();
        }

        private void AddActionArguments(ExceptionLoggerContext context)
        {
            if (context.ExceptionContext.ActionContext != null)
            {
                var actionContext = context.ExceptionContext.ActionContext;
                var actionArguments = actionContext.ActionArguments;
                if (actionArguments != null && actionArguments.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    var actionArgumentsJson = Jsonify(actionArguments);
                    GlobalDiagnosticsContext.Set(ACTION_ARGUMENTS_CONTEXT_KEY, actionArgumentsJson);
                }
            }
        }

        private void AddControllerAndAction(ExceptionLoggerContext context)
        {

            if (context.ExceptionContext.ActionContext != null)
            {
                var actionContext = context.ExceptionContext.ActionContext;
                var actionName = actionContext.ActionDescriptor.ActionName;
                var controllerName = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
                GlobalDiagnosticsContext.Set(CONTROLLER_CONTEXT_KEY, controllerName);
                GlobalDiagnosticsContext.Set(ACTION_CONTEXT_KEY, actionName);
            }
        }

        private string Jsonify(Dictionary<string, object> actionArguments)
        {
            var json = JsonConvert.SerializeObject(actionArguments);
            var prettyJson = JValue.Parse(json).ToString(Formatting.Indented);
            return prettyJson;
        }
    }
}