using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ECA.WebApi.Models
{
    /// <summary>
    /// The AboutViewModel returns information about the current web api instance.
    /// </summary>
    public class AboutViewModel
    {
        /// <summary>
        /// Creates and initializes a new instance of this model.
        /// </summary>
        public AboutViewModel()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
            this.Version = fileVersion.FileVersion;

            this.BuildConfiguration = "Release";
#if DEBUG
            this.BuildConfiguration = "Debug";
#endif
        }

        /// <summary>
        /// Gets or sets the current running assembly version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets the build configuration.
        /// </summary>
        public string BuildConfiguration { get; private set; }
    }
}