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
        public void Finished(string documentTypeName)
        {
            Console.WriteLine(String.Format("Finished processing {0}", documentTypeName));
        }

        public void Processed(string documentTypeName, int totalDocumentsCount, int documentsProcessed)
        {
            Console.WriteLine("Processed {0} of {1} documents of type {2}", documentsProcessed, totalDocumentsCount, documentTypeName);
        }

        public void Started(string documentTypeName)
        {
            Console.WriteLine(String.Format("Started processing {0}", documentTypeName));
        }
    }
}
