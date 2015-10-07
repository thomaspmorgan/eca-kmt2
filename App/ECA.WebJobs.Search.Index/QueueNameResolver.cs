using ECA.Core.Settings;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.WebJobs.Search.Index
{
    public class QueueNameResolver : INameResolver
    {
        public string Resolve(string name)
        {
            var appSettings = new AppSettings();
            return appSettings.SearchDocumentQueueName;
        }
    }
}
