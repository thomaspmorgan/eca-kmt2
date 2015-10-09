using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ECA.Business.Search
{
    public class TextWriterIndexNotificationService : IIndexNotificationService
    {
        private Stopwatch processAllDocumentsStopwatch;
        private Stopwatch deleteDocumentsStopwatch;
        private Stopwatch updateDocumentStopwatch;

        public TextWriterIndexNotificationService()
        {
        }

        public void DeleteDocumentsFinished(string documentTypeName, List<object> ids)
        {
            deleteDocumentsStopwatch.Stop();
            var message = String.Format("Successfully deleted {0} documents with ids {1} in {2} time.", documentTypeName, String.Join(", ", ids), deleteDocumentsStopwatch.Elapsed);
            Console.WriteLine(message);
        }

        public void DeleteDocumentsStarted(string documentTypeName, List<object> ids)
        {
            deleteDocumentsStopwatch = Stopwatch.StartNew();
            var message = String.Format("Attempting to delete {0} documents with ids {1}.", documentTypeName, String.Join(", ", ids));
            Console.WriteLine(message);
        }

        public void ProcessAllDocumentsFinished(string documentType)
        {
            var message = String.Format("Finished processing {0} in {1} time.", documentType, processAllDocumentsStopwatch.Elapsed);
            Console.WriteLine(message);
        }

        public void ProcessedSomeOfAllDocuments(string documentType, int totalDocumentsCount, int documentsProcessed)
        {
            var message = String.Format("Processed {0} of {1} {2} documents.", documentsProcessed, totalDocumentsCount, documentType);
            Console.WriteLine(message);
        }

        public void StartedProcessingAllDocuments(string documentType)
        {
            processAllDocumentsStopwatch = Stopwatch.StartNew();
            var message = String.Format("Started processing {0} documents.", documentType);
            Console.WriteLine(message);
        }

        public void UpdateFinished(string documentTypeName, object id)
        {
            updateDocumentStopwatch.Stop();
            var message = String.Format("Succesfully updated the {0} document with id {1} in {2} time.", documentTypeName, id, updateDocumentStopwatch.Elapsed);
            Console.WriteLine(message);
        }

        public void UpdateStarted(string documentTypeName, object id)
        {
            updateDocumentStopwatch = Stopwatch.StartNew();
            var message = String.Format("Attempting to update the {0} document with id {1}.", documentTypeName, id);
            Console.WriteLine(message);
        }
    }
}