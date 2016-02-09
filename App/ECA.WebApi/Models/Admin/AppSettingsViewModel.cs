using ECA.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// App settings view model
    /// </summary>
    public class AppSettingsViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AppSettingsViewModel()
        {
            var appSettings = new AppSettings();
            this.IdleDuration = appSettings.IdleDuration;
            this.IdleTimeout = appSettings.IdleTimeout;
        }   

        /// <summary>
        /// Gets or sets the idle duration
        /// </summary>
        public string IdleDuration { get; set; }

        /// <summary>
        /// Gets or sets the idle timeout
        /// </summary>
        public string IdleTimeout { get; set; }
    }
}