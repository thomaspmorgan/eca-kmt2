﻿using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics.Contracts;

namespace ECA.Core.Settings
{
    public class AppSettings
    {
        #region AD Constants
        /// <summary>
        /// The active directory settings prefix.
        /// </summary>
        public const string AD_PREFIX = "ad.";

        /// <summary>
        /// The active directory tenant id.
        /// </summary>
        public const string AD_TENANT_ID = AD_PREFIX + "TenantId";

        /// <summary>
        /// The active directory client id.
        /// </summary>
        public const string AD_CLIENT_ID = AD_PREFIX + "ClientId";

        /// <summary>
        /// The active directory client secret.
        /// </summary>
        public const string AD_CLIENT_SECRET = AD_PREFIX + "ClientSecret"; // App key

        /// <summary>
        /// The active directory audience.
        /// </summary>
        public const string AD_AUDIENCE = AD_PREFIX + "Audience"; // App ID URI
        #endregion

        #region Search Constants
        /// <summary>
        /// The search configuration prefix.
        /// </summary>
        public const string SEARCH_PREFIX = "search.";

        /// <summary>
        /// The azure search api key.
        /// </summary>
        public const string SEARCH_API_KEY = SEARCH_PREFIX + "ApiKey";

        /// <summary>
        /// The azure search service name key.
        /// </summary>
        public const string SEARCH_SERVICE_NAME_KEY = SEARCH_PREFIX + "ServiceName";

        /// <summary>
        /// The azure search index name key.
        /// </summary>
        public const string SEARCH_INDEX_NAME_KEY = SEARCH_PREFIX + "IndexName";
        #endregion

        #region Database Constants
        /// <summary>
        /// The db configuration prefix.
        /// </summary>
        public const string DB_PREFIX = "db.";

        /// <summary>
        /// The eca context configuration key.
        /// </summary>
        public const string ECA_CONTEXT_KEY = DB_PREFIX + "EcaContext";

        /// <summary>
        /// The eca context configuration key.
        /// </summary>
        public const string CAM_CONTEXT_KEY = ECA_CONTEXT_KEY;
        #endregion

        private NameValueCollection appSettings;
        private ConnectionStringSettingsCollection connectionStrings;

        /// <summary>
        /// Creates a default instance using the Configuration Manager.
        /// </summary>
        public AppSettings() : this(ConfigurationManager.AppSettings, ConfigurationManager.ConnectionStrings) { }

        /// <summary>
        /// Creates a new instance using the given app settings and connection strings.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        /// <param name="connectionStrings">The connection strings.</param>
        public AppSettings(NameValueCollection appSettings, ConnectionStringSettingsCollection connectionStrings)
        {
            Contract.Requires(appSettings != null, "The app settings must not be null.");
            Contract.Requires(connectionStrings != null, "The connection strings must not be null.");
            this.appSettings = appSettings;
            this.connectionStrings = connectionStrings;
        }

        private string GetAppSetting(string name)
        {
            var setting = this.appSettings[name];
            if (setting == null)
            {
                throw new ConfigurationErrorsException(String.Format("Cannot find the application setting with key {0}.", name));
            }
            return setting;
        }

        private ConnectionStringSettings GetConnectionString(string name)
        {
            var connectionString = this.connectionStrings[name];
            if (connectionString == null)
            {
                throw new ConfigurationErrorsException(String.Format("Cannot find the connection string with key {0}.", name));
            }
            return connectionString;
        }
        #region App Settings
        /// <summary>
        /// Gets the azure search api key.
        /// </summary>
        public string SearchApiKey { get { return GetAppSetting(SEARCH_API_KEY); } }

        /// <summary>
        /// Gets the azure search service name.
        /// </summary>
        public string SearchServiceName { get { return GetAppSetting(SEARCH_SERVICE_NAME_KEY); } }

        /// <summary>
        /// Gets the azure search service index name.
        /// </summary>
        public string SearchIndexName { get { return GetAppSetting(SEARCH_INDEX_NAME_KEY); } }

        /// <summary>
        /// Gets the active directory client id.
        /// </summary>
        public string AdClientId { get { return GetAppSetting(AD_CLIENT_ID); } }

        /// <summary>
        /// Gets the active directory tenant id.
        /// </summary>
        public string AdTenantId { get { return GetAppSetting(AD_TENANT_ID); } }
        #endregion

        #region Connection Strings
        /// <summary>
        /// Gets the EcaContext connection string.
        /// </summary>
        public ConnectionStringSettings EcaContextConnectionString { get { return GetConnectionString(ECA_CONTEXT_KEY); } }

        /// <summary>
        /// Gets the Cam context connection string.
        /// </summary>
        public ConnectionStringSettings CamContextConnectionString { get { return GetConnectionString(ECA_CONTEXT_KEY); } }
        #endregion
    }
}