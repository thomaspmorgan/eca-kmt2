﻿using ECA.Business.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Test.Search
{
    public class TestIndexNotificationService : IIndexNotificationService
    {
        public void Finished(DocumentType documentType)
        {
            Console.WriteLine(String.Format("Finished processing {0}", documentType.Name));
        }

        public void Processed(DocumentType documentType, int totalDocumentsCount, int documentsProcessed)
        {
            Console.WriteLine("Processed {0} of {1} documents of type {2}", documentsProcessed, totalDocumentsCount, documentType.Name);
        }

        public void Started(DocumentType documentType)
        {
            Console.WriteLine(String.Format("Started processing {0}", documentType.Name));
        }
    }
}
