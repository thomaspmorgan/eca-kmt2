using Microsoft.Azure;
using Microsoft.WindowsAzure;
using System;
using System.Configuration;

namespace ECA.WebJobs.Search
{
    public static class AppSettings
    {

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



        public static string SearchApiKey { get { return GetAppSetting(SEARCH_API_KEY); } }
        public static string SearchServiceName { get { return GetAppSetting(SEARCH_SERVICE_NAME_KEY); } }

        public static ConnectionStringSettings EcaContextConnectionString { get { return GetConnectionString(ECA_CONTEXT_KEY); } }
    }
}