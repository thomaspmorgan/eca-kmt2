﻿using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics.Contracts;

namespace ECA.Core.Settings
{
    public class AppSettings
    {
        #region System
        /// <summary>
        /// The active directory settings prefix.
        /// </summary>
        public const string SYSTEM_PREFIX = "system.";

        /// <summary>
        /// The active directory tenant id.
        /// </summary>
        public const string SYSTEM_USER_ID_KEY = SYSTEM_PREFIX + "UserId";
        #endregion

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

        /// <summary>
        /// The azure search webjob queue name for receiving queue messages detailing updated entities.
        /// </summary>
        public const string SEARCH_INDEX_QUEUE_NAME_KEY = SEARCH_PREFIX + "IndexQueueName";
        #endregion

        #region Sevis Constants

        /// <summary>
        /// The sevis configuration prefix.
        /// </summary>
        public const string SEVIS_PREFIX = "sevis.";

        public const string SEVIS_UPLOAD_URI_KEY = SEVIS_PREFIX + "UploadUri";

        public const string SEVIS_DOWNLOAD_URI_KEY = SEVIS_PREFIX + "DownloadUri";

        public const string SEVIS_THUMBPRINT = SEVIS_PREFIX + "Thumbprint";

        /// <summary>
        /// The sevis site of activity address key.
        /// </summary>
        public const string SEVIS_SITE_OF_ACTIVITY_ADDRESS_DTO = SEVIS_PREFIX + "SiteOfActivityAddressDTO";

        /// <summary>
        /// The max create exchange visitor records per batch key.  This setting is used to adjust the number of 
        /// create exchange visitors are added to a sevis batch.
        /// </summary>
        public const string SEVIS_MAX_CREATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH = SEVIS_PREFIX + "MaxCreateExchangeVisitorRecordsPerBatch";

        /// <summary>
        /// The max update exchange visitor records per batch key.  This setting is used to adjust the number of 
        /// update exchange visitors are added to a sevis batch.
        /// </summary>
        public const string SEVIS_MAX_UPDATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH = SEVIS_PREFIX + "MaxUpdateExchangeVisitorRecordsPerBatch";

        /// <summary>
        /// The number of days to keep processed sevic batch records in days.
        /// </summary>
        public const string NUMBER_OF_DAYS_TO_KEEP_PROCESSED_SEVIS_BATCH_RECORDS = SEVIS_PREFIX + "NumberOfDaysToKeepProcessedSevisBatchRecords";

        /// <summary>
        /// The number of seconds to wait before trying to download a sevis batch from the sevis api again in seconds key.
        /// </summary>
        public const string DOWNLOAD_COOLDOWN_IN_SECONDS = SEVIS_PREFIX + "DownloadCooldownInSeconds";

        /// <summary>
        /// The number of seconds to wait before trying to upload a sevis batch to the sevis api again in seconds key.
        /// </summary>
        public const string UPLOAD_COOLDOWN_IN_SECONDS = SEVIS_PREFIX + "UploadCooldownInSeconds";

        /// <summary>
        /// The ds 2019 file azure blog storage container key.
        /// </summary>
        public const string SEVIS_DS2019_STORAGE_CONTAINER = SEVIS_PREFIX + "SevisDS2019StorageContainer";

        /// <summary>
        /// The ds 2019 file storage connection string key.
        /// </summary>
        public const string SEVIS_DS2019_STORAGE_CONNECTION_STRING_KEY = SEVIS_PREFIX + "SevisDS2019StorageConnection";

        /// <summary>
        /// The sevis comm cron schedule string key.
        /// </summary>
        public const string SEVIS_COMM_CRON_SCHEDULE_KEY = SEVIS_PREFIX + "SevisCommCronSchedule";

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

        #region Azure

        /// <summary>
        /// The azure settings prefix, used for anything azure related except web jobs dasbhoard and web jobs storage.
        /// </summary>
        public const string AZURE_PREFIX = "azure.";

        /// <summary>
        /// The azure application insights instrumentation settings key.
        /// 
        /// http://blogs.msdn.com/b/visualstudioalm/archive/2015/01/07/application-insights-support-for-multiple-environments-stamps-and-app-versions.aspx
        /// </summary>
        public const string APPLICATION_INSIGHTS_INSTRUMENTATION_SETTINGS_KEY = AZURE_PREFIX + "InstrumentationKey";

        /// <summary>
        /// The azure web jobs dashboard connection string key.
        /// </summary>
        public const string AZURE_WEB_JOBS_DASHBOARD_KEY = "AzureWebJobsDashboard";

        /// <summary>
        /// The azure web jobs storage connection string key.
        /// </summary>
        public const string AZURE_WEB_JOBS_STORAGE_KEY = "AzureWebJobsStorage";
        #endregion

        #region Session Constants

        /// <summary>
        /// The idle duration string key
        /// </summary>
        public const string IDLE_DURATION_IN_SECONDS = "session.IdleDurationInSeconds";

        /// <summary>
        /// The idle timout string key
        /// </summary>
        public const string IDLE_TIMEOUT_IN_SECONDS = "session.IdleTimeoutInSeconds";
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
        /// Gets the name of the azure queue that will hold messages detailing updated entities that should be indexed for searching.
        /// </summary>
        public string AppInsightsInstrumentationKey { get { return GetAppSetting(APPLICATION_INSIGHTS_INSTRUMENTATION_SETTINGS_KEY); } }

        /// <summary>
        /// Gets the name of the azure queue that will hold messages detailing updated entities that should be indexed for searching.
        /// </summary>
        public string SearchDocumentQueueName { get { return GetAppSetting(SEARCH_INDEX_QUEUE_NAME_KEY); } }

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
        /// Gets the azure SEVIS Upload Uri.
        /// </summary>
        public string SevisUploadUri { get { return GetAppSetting(SEVIS_UPLOAD_URI_KEY); } }

        /// <summary>
        /// Gets the azure SEVIS Download Uri.
        /// </summary>
        public string SevisDownloadUri { get { return GetAppSetting(SEVIS_DOWNLOAD_URI_KEY); } }

        /// <summary>
        /// Gets the azure SEVIS Client Certificate Thumbprint
        /// </summary>
        public string SevisThumbprint { get { return GetAppSetting(SEVIS_THUMBPRINT); } }

        /// <summary>
        /// Gets the azure SEVIS site of activity.
        /// </summary>
        public string SevisSiteOfActivityAddressDTO { get { return GetAppSetting(SEVIS_SITE_OF_ACTIVITY_ADDRESS_DTO); } }

        /// <summary>
        /// Gets the maximum number of create exchange visitors records to add to a batch.
        /// </summary>
        public string MaxCreateExchangeVisitorRecordsPerBatch { get { return GetAppSetting(SEVIS_MAX_CREATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH); } }

        /// <summary>
        /// Gets the maximum number of update exchange visitors records to add to a batch.
        /// </summary>
        public string MaxUpdateExchangeVisitorRecordsPerBatch { get { return GetAppSetting(SEVIS_MAX_UPDATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH); } }

        /// <summary>
        /// Gets the number of days to keep processed sevis batch records.
        /// </summary>
        public string NumberOfDaysToKeepProcessedSevisBatchRecords { get { return GetAppSetting(NUMBER_OF_DAYS_TO_KEEP_PROCESSED_SEVIS_BATCH_RECORDS); } }

        /// <summary>
        /// Gets the number of seconds to wait before requesting a sevis batch to the api again.
        /// </summary>
        public string SevisDownloadCooldownInSeconds { get { return GetAppSetting(DOWNLOAD_COOLDOWN_IN_SECONDS); } }

        /// <summary>
        /// Gets the number of seconds to wait before sending a sevis batch to the api again.
        /// </summary>
        public string SevisUploadCooldownInSeconds { get { return GetAppSetting(UPLOAD_COOLDOWN_IN_SECONDS); } }

        /// <summary>
        /// Gets the sevis ds 2019 file storage container.
        /// </summary>
        public string DS2019FileStorageContainer { get { return GetAppSetting(SEVIS_DS2019_STORAGE_CONTAINER); } }

        /// <summary>
        /// Gets the cron schedule string from the sevis comm webjob.
        /// </summary>
        public string SevisCommCronSchedule { get { return GetAppSetting(SEVIS_COMM_CRON_SCHEDULE_KEY); } }

        /// <summary>
        /// Gets the active directory client id.
        /// </summary>
        public string AdClientId { get { return GetAppSetting(AD_CLIENT_ID); } }

        /// <summary>
        /// Gets the active directory tenant id.
        /// </summary>
        public string AdTenantId { get { return GetAppSetting(AD_TENANT_ID); } }

        /// <summary>
        /// Gets the id of the system user.  Useful for webjobs, system activities, etc.
        /// </summary>
        public string SystemUserId { get { return GetAppSetting(SYSTEM_USER_ID_KEY); } }

        public string IdleDurationInSeconds { get { return GetAppSetting(IDLE_DURATION_IN_SECONDS); } }

        public string IdleTimeoutInSeconds { get { return GetAppSetting(IDLE_TIMEOUT_IN_SECONDS); } }

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

        /// <summary>
        /// Gets the azure web jobs storage connection string key.
        /// </summary>
        public ConnectionStringSettings AzureWebJobsStorageConnectionString { get { return GetConnectionString(AZURE_WEB_JOBS_STORAGE_KEY); } }

        /// <summary>
        /// Gets the azure web jobs dashboard connection string key.
        /// </summary>
        public ConnectionStringSettings AzureWebJobsDashboardConnectionString { get { return GetConnectionString(AZURE_WEB_JOBS_DASHBOARD_KEY); } }

        /// <summary>
        /// Gets the connection string for the ds 2019 file storage.
        /// </summary>
        public ConnectionStringSettings DS2019FileStorageConnectionString { get { return GetConnectionString(SEVIS_DS2019_STORAGE_CONNECTION_STRING_KEY); } }
        #endregion
    }
}
