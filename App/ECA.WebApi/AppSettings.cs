﻿using Microsoft.Azure;
using System;
using System.Configuration;

namespace ECA.WebApi
{
    public static class AppSettings
    {
        private const string AD_PREFIX = "ad.";
        private const string AD_TENANT_ID = AD_PREFIX + "TenantId";
        private const string AD_CLIENT_ID = AD_PREFIX + "ClientId";
        private const string AD_CLIENT_SECRET = AD_PREFIX + "ClientSecret"; // App key
        private const string AD_AUDIENCE = AD_PREFIX + "Audience"; // App ID URI

        //private const string DB_PREFIX = "db.";
        //private const string DB_HOST = DB_PREFIX + "Host";
        //private const string DB_CATALOG = DB_PREFIX + "Catalog";
        //private const string DB_LOGIN = DB_PREFIX + "Login";
        //private const string DB_PASSWORD = DB_PREFIX + "Password";
        //private const string DB_MODEL = DB_PREFIX + "Model";

        private const string PDTracker_PREFIX = "pdtracker.";
        private const string PDTrackerVERSION = PDTracker_PREFIX + "Version";
        private const string PDTracker_OPEN_NET_MODE = PDTracker_PREFIX + "OpenNetMode";
        private const string PDTracker_REMOTE_SERVER = PDTracker_PREFIX + "RemoteServer";
        private const string PDTracker_REMOTE_SERVER_USERNAME = PDTracker_PREFIX + "RemoteServerUsername";
        private const string PDTracker_REMOTE_SERVER_PASSWORD = PDTracker_PREFIX + "RemoteServerPassword";

        private const string SMTP_PREFIX = "smtp.";
        private const string SMTP_HOST = SMTP_PREFIX + "Host";
        private const string SMTP_USERNAME = SMTP_PREFIX + "Username";
        private const string SMTP_PASSWORD = SMTP_PREFIX + "Password";
        private const string SMTP_PORT = SMTP_PREFIX + "Port";

        private const string COGITO_PREFIX = "cogito.";
        private const string COGITO_API_KEY = COGITO_PREFIX + "ApiKey";

        private const string SEARCH_PREFIX = "search";
        private const string SEARCH_API_KEY = SEARCH_PREFIX + ".ApiKey";
        private const string SEARCH_SERVICE_NAME_KEY = SEARCH_PREFIX + ".ServiceName";

        private const string DB_PREFIX = "db";
        private const string ECA_CONTEXT_KEY = DB_PREFIX + ".EcaContext";


        private static string GetAppSetting(string name)
        {
            // This will try to read from the cloud configuration file first
            // and then it will look in the web.config file. This allows us
            // to store development settings (i.e., development database) in
            // the web.config file and store productions setting in the
            // ServiceConfiguration.Cloud.cscfg file.
            var setting = ConfigurationManager.AppSettings[name];
            if (setting == null)
            {
                throw new Exception("Cannot find the application setting " + name);
            }
            return setting;
        }

        private static ConnectionStringSettings GetConnectionString(string name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name];
            if (connectionString == null)
            {
                throw new Exception("Cannot find the connection string setting " + name);
            }
            return connectionString;
        }

        public static string AdTenantId { get { return GetAppSetting(AD_TENANT_ID); } }
        public static string AdClientId { get { return GetAppSetting(AD_CLIENT_ID); } }
        public static string AdClientSecret { get { return GetAppSetting(AD_CLIENT_SECRET); } }
        public static string AdAudience { get { return GetAppSetting(AD_AUDIENCE); } }

        //public static string DbHost { get { return Setting(DB_HOST); } }
        //public static string DbCatalog { get { return Setting(DB_CATALOG); } }
        //public static string DbLogin { get { return Setting(DB_LOGIN); } }
        //public static string DbPassword { get { return Setting(DB_PASSWORD); } }
        //public static string DbModel { get { return Setting(DB_MODEL); } }

        public static string PdTrackerVersion { get { return GetAppSetting(PDTrackerVERSION); } }
        public static bool PdTrackerOpenNetMode { get { return bool.Parse(GetAppSetting(PDTracker_OPEN_NET_MODE)); } }
        public static string PdTrackerRemoteServer { get { return GetAppSetting(PDTracker_REMOTE_SERVER); } }
        public static string PdTrackerRemoteServerUsername { get { return GetAppSetting(PDTracker_REMOTE_SERVER_USERNAME); } }
        public static string PdTrackerRemoteServerPassword { get { return GetAppSetting(PDTracker_REMOTE_SERVER_PASSWORD); } }

        public static string SmtpHost { get { return GetAppSetting(SMTP_HOST); } }
        public static string SmtpUsername { get { return GetAppSetting(SMTP_USERNAME); } }
        public static string SmtpPassword { get { return GetAppSetting(SMTP_PASSWORD); } }
        public static string SmtpPort { get { return GetAppSetting(SMTP_PORT); } }

        public static string CogitoApiKey { get { return GetAppSetting(COGITO_API_KEY); } }

        public static string SearchApiKey { get { return GetAppSetting(SEARCH_API_KEY); } }

        public static string SearchServiceName { get { return GetAppSetting(SEARCH_SERVICE_NAME_KEY); } }

        public static ConnectionStringSettings EcaContextConnectionString { get { return GetConnectionString(ECA_CONTEXT_KEY); } }

        public static ConnectionStringSettings CamContextConnectionString { get { return GetConnectionString(ECA_CONTEXT_KEY); } }
    }
}