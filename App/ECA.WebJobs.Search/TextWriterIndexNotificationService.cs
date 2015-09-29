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
        private readonly TextWriter textWriter;
        private Stopwatch stopwatch;
        public TextWriterIndexNotificationService(TextWriter textWriter)
        {
            Contract.Requires(textWriter != null, "The text writer must not be null.");
            this.textWriter = textWriter;
        }

        public void Finished(DocumentType documentType)
        {
            var message = String.Format("Finished processing {0} in {1} time.", documentType.Name, stopwatch.Elapsed);
            textWriter.WriteLine(message);
            Console.WriteLine(message);
        }

        public void Processed(DocumentType documentType, int totalDocumentsCount, int documentsProcessed)
        {
            var message = String.Format("Processed {0} of {1} documents of type {2}", documentsProcessed, totalDocumentsCount, documentType.Name);
            textWriter.WriteLine(message);
            Console.WriteLine(message);
        }

        public void Started(DocumentType documentType)
        {
            stopwatch = Stopwatch.StartNew();
            var message = String.Format("Started processing {0}", documentType.Name);
            Console.WriteLine(message);
            textWriter.WriteLine(message);
        }
    }
}