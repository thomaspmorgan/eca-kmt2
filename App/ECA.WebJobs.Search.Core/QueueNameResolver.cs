using ECA.Core.Settings;
using Microsoft.Azure.WebJobs;
using System.Diagnostics.Contracts;

namespace ECA.WebJobs.Search.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class QueueNameResolver : INameResolver
    {
        private AppSettings appSettings;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appSettings"></param>
        public QueueNameResolver(AppSettings appSettings)
        {
            Contract.Requires(appSettings != null, "The app settings must not be null.");
            this.appSettings = appSettings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string Resolve(string name)
        {
            return appSettings.SearchDocumentQueueName;
        }
    }
}
