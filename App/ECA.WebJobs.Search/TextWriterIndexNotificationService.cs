using ECA.Business.Search;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Web;

namespace ECA.WebJobs.Search
{
    public class TextWriterIndexNotificationService : IIndexNotificationService
    {
        private Stopwatch stopwatch;
        public TextWriterIndexNotificationService()
        {
        }

        public void Finished(string documentType)
        {
            var message = String.Format("Finished processing {0} in {1} time.", documentType, stopwatch.Elapsed);
            Console.WriteLine();
            Console.WriteLine(message);
        }

        public void Processed(string documentType, int totalDocumentsCount, int documentsProcessed)
        {
            var message = String.Format("Processed {0} of {1} {2} documents.", documentsProcessed, totalDocumentsCount, documentType);
            Console.Write("\r{0}   ", message);
        }

        public void Started(string documentType)
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