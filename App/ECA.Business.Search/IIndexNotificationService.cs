using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    public interface IIndexNotificationService
    {
        void Started(string documentTypeName);

        void Processed(string documentTypeName, int totalDocumentsToProcess, int documentsProcessed);

        void Finished(string documentTypeName);
    }
}
