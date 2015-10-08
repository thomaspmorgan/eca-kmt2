using ECA.Core.Settings;
using Microsoft.Azure.WebJobs;
using System.Diagnostics.Contracts;

namespace ECA.WebJobs.Search.Core
{
    public class QueueNameResolver : INameResolver
    {
        private AppSettings appSettings;

        public QueueNameResolver(AppSettings appSettings)
        {
            Contract.Requires(appSettings != null, "The app settings must not be null.");
            this.appSettings = appSettings;
        }

        public string Resolve(string name)
        {
            return appSettings.SearchDocumentQueueName;
        }
    }
}
