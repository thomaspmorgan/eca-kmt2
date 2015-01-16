﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ECA.WebApi.Common;

namespace ECA.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
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
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            // Add custom formatters at the beginning.
            //config.Formatters.Insert(0, new PlanCsvFormatter());
            //config.Formatters.Insert(0, new PlanJsonFormatter());
            //config.Formatters.Insert(0, new PlanHtmlFormatter());
            //config.Formatters.Insert(0, new PlanXmlFormatter());

            config.Filters.Add(new WebApiErrorHandler());
        }
    }
}
