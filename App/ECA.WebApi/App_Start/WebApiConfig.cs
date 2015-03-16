using ECA.Core.Logging;
using ECA.WebApi.Custom.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;

namespace ECA.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            UnityConfig.RegisterComponents();

            // Enable cross-origin resource sharing.
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Remove the XML formatter and configure the JSON formatter.
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.NullValueHandling = NullValueHandling.Ignore;

            // Add custom formatters at the beginning.
            //config.Formatters.Insert(0, new PlanCsvFormatter());
            //config.Formatters.Insert(0, new PlanJsonFormatter());
            //config.Formatters.Insert(0, new PlanHtmlFormatter());
            //config.Formatters.Insert(0, new PlanXmlFormatter());

            var logger = config.DependencyResolver.GetService(typeof(ILogger)) as ILogger;
            Debug.Assert(logger != null, "The logger must not be null.");
            config.Services.Add(typeof(IExceptionLogger), new LoggerExceptionHandler(logger));

            config.Filters.Add(new ModelNotFoundExceptionFilter());
            config.Filters.Add(new UnknownStaticLookupExceptionFilter());
            config.Filters.Add(new ValidationExceptionFilter());
            config.Filters.Add(new DbEntityValidationExceptionFilter());

            config.Filters.Add(new ECA.WebApi.Custom.Filters.TraceFilter(logger));

        }
    }
}
