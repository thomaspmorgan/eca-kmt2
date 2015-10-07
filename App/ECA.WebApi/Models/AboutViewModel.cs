using ECA.Core.Generation;
using ECA.Core.Settings;
using ECA.Data;
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
        public AboutViewModel(IStaticGeneratorValidator validator)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
            this.Version = fileVersion.FileVersion;
            var ecaErrors = ECA.Data.EcaDataValidator.ValidateAll(validator);
            var camErrors = CAM.Data.CamDataValidator.ValidateAll(validator);
            this.LookupErrors = new List<string>();
            this.LookupErrors.AddRange(ecaErrors);
            this.LookupErrors.AddRange(camErrors);
            this.LookupErrors = this.LookupErrors.OrderBy(x => x).ToList();

            var appSettings = new AppSettings();
            this.AzureSearchIndexName = appSettings.SearchIndexName;
            this.AzureSearchServiceName = appSettings.SearchServiceName;

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

        /// <summary>
        /// Gets the name of the azure search index.
        /// </summary>
        public string AzureSearchIndexName { get; private set; }

        /// <summary>
        /// Gets the name of the azure search service that is hosting indexes.
        /// </summary>
        public string AzureSearchServiceName { get; private set; }

        /// <summary>
        /// Gets the lookups in code that do not align with database values and vice versa.
        /// </summary>
        public List<string> LookupErrors { get; private set; }
    }
}