using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.ApplicationInsights.DataContracts;
using ECA.WebApi.Controllers;
using System.Reflection;
using System.Diagnostics;
using Microsoft.ApplicationInsights.Channel;

namespace ECA.WebApi
{
    /// <summary>
    /// The application insights initializer.
    /// </summary>
    public class KmtApiConfigInitializer : ITelemetryInitializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="telemetry"></param>
        public void Initialize(ITelemetry telemetry)
        {
            //http://blogs.msdn.com/b/visualstudioalm/archive/2015/01/07/application-insights-support-for-multiple-environments-stamps-and-app-versions.aspx
            //https://social.msdn.microsoft.com/Forums/sqlserver/en-US/20e92616-c784-4081-bfe9-60a175caccf2/application-insights-application-version-property?forum=ApplicationInsights
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
            telemetry.Context.Component.Version = fileVersion.FileVersion;
        }
    }
}