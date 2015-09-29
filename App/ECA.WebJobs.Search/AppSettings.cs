using Microsoft.Azure;
using Microsoft.WindowsAzure;
using System;

namespace ECA.WebJobs.Search
{
    public static class AppSettings
    {

        private const string SEARCH_PREFIX = "search";
        private const string SEARCH_API_KEY = SEARCH_PREFIX + ".ApiKey";
        private const string SEARCH_SERVICE_NAME_KEY = SEARCH_PREFIX + ".ServiceName";


        private static string Setting(string name)
        {
            // This will try to read from the cloud configuration file first
            // and then it will look in the web.config file. This allows us
            // to store development settings (i.e., development database) in
            // the web.config file and store productions setting in the
            // ServiceConfiguration.Cloud.cscfg file.
            var setting = CloudConfigurationManager.GetSetting(name);
            // var setting = ConfigurationManager.AppSettings[name];
            if (setting == null)
            {
                throw new Exception("Cannot find the application setting " + name);
            }
            return setting;
        }

        public static string SearchApiKey { get { return Setting(SEARCH_API_KEY); } }
        public static string SearchServiceName { get { return Setting(SEARCH_SERVICE_NAME_KEY); } }
    }
}