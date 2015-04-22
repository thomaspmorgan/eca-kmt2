using ECA.Core.Generation;
using ECA.Data;
using ECA.WebApi.Custom;
using ECA.WebApi.Custom.Filters;
using ECA.WebApi.Custom.Handlers;
using ECA.WebApi.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Tracing;

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

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.NullValueHandling = NullValueHandling.Ignore;

            var userProvider = config.DependencyResolver.GetService(typeof(IUserProvider)) as IUserProvider;
            Debug.Assert(userProvider != null, "The user provider must not be null.");

            config.Services.Add(typeof(IExceptionLogger), new LoggerExceptionHandler());

            config.Filters.Add(new ModelNotFoundExceptionFilter());
            config.Filters.Add(new UnknownStaticLookupExceptionFilter());
            config.Filters.Add(new ValidationExceptionFilter());
            config.Filters.Add(new DbEntityValidationExceptionFilter());

            //config.Services.Replace(typeof(System.Web.Http.Tracing.ITraceWriter), new NLogTraceWriter());
#if DEBUG
            config.MessageHandlers.Add(new DebugWebApiUserHandler());
#endif
        }
    }
}
