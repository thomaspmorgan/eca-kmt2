using ECA.Business.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Test.Search
{
    public class TestIndexNotificationService : IIndexNotificationService
    {
        public void DeleteDocumentsFinished(string documentTypeName, List<object> id)
        {
            
        }

        public void DeleteDocumentsStarted(string documentTypeName, List<object> ids)
        {
            
        }

        public void ProcessAllDocumentsFinished(string documentTypeName)
        {
            
        }

        public void ProcessedSomeOfAllDocuments(string documentTypeName, int totalDocumentsCount, int documentsProcessed)
        {
            
        }

        public void StartedProcessingAllDocuments(string documentTypeName)
        {
            
        }

        public void UpdateFinished(string documentTypeName, object id)
        {
            
        }

        public void UpdateStarted(string documentTypeName, object id)
        {
            
        }
    }
}
