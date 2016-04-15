using ECA.Core.Settings;
using ECA.WebApi.Custom.Filters;
using ECA.WebApi.Custom.Handlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;

namespace ECA.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            var appSettings = new AppSettings();
            Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey = appSettings.AppInsightsInstrumentationKey;
            Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.TelemetryInitializers.Add(new KmtApiConfigInitializer());

            // Web API configuration and services

            // Enable cross-origin resource sharing.
            config.EnableCors(new EnableCorsAttribute("*", "*", "*", "Content-Disposition"));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.NullValueHandling = NullValueHandling.Ignore;

            config.Services.Add(typeof(IExceptionLogger), new LoggerExceptionHandler());

            config.Filters.Add(new ModelNotFoundExceptionFilter());
            config.Filters.Add(new UnknownStaticLookupExceptionFilter());
            config.Filters.Add(new ValidationExceptionFilter());
            config.Filters.Add(new DbEntityValidationExceptionFilter());
            config.Filters.Add(new EcaBusinessExceptionFilter());
            config.Filters.Add(new ConcurrencyExceptionFilter());

            //config.Services.Replace(typeof(System.Web.Http.Tracing.ITraceWriter), new NLogTraceWriter());
#if DEBUG
            config.MessageHandlers.Add(new DebugWebApiUserHandler());
#endif
        }
    }
}
