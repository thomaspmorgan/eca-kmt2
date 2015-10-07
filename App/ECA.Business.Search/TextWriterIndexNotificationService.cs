using System;
using System.Diagnostics;

namespace ECA.Business.Search
{
    public class TextWriterIndexNotificationService : IIndexNotificationService
    {
        private Stopwatch stopwatch;
        public TextWriterIndexNotificationService()
        {
        }

        public void DeleteFinished(string documentTypeName, object id)
        {
            var message = String.Format("Successfully deleted {0} document with id {1}.", documentTypeName, id);
            Console.WriteLine(message);
        }

        public void DeleteStarted(string documentTypeName, object id)
        {
            var message = String.Format("Attempting to delete {0} document with id {1}.", documentTypeName, id);
            Console.WriteLine(message);
        }

        public void ProcessAllDocumentsFinished(string documentType)
        {
            var message = String.Format("Finished processing {0} in {1} time.", documentType, stopwatch.Elapsed);
            Console.WriteLine();
            Console.WriteLine(message);
        }

        public void ProcessedSomeOfAllDocuments(string documentType, int totalDocumentsCount, int documentsProcessed)
        {
            var message = String.Format("Processed {0} of {1} {2} documents.", documentsProcessed, totalDocumentsCount, documentType);
            Console.Write("\r{0}   ", message);
        }

        public void StartedProcessingAllDocuments(string documentType)
        {
            stopwatch = Stopwatch.StartNew();
            var message = String.Format("Started processing {0} documents.", documentType);
            Console.WriteLine(message);
        }

        public void UpdateFinished(string documentTypeName, object id)
        {
            var message = String.Format("Succesfully updated the {0} document with id {1}.", documentTypeName, id);
            Console.WriteLine(message);
        }

        public void UpdateStarted(string documentTypeName, object id)
        {
            var message = String.Format("Attempting to update the {0} document with id {1}.", documentTypeName, id);
            Console.WriteLine(message);
        }
    }
}