using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    public interface IIndexNotificationService
    {
        void Processed(DocumentType documentType, int totalDocumentsCount, int documentsProcessed);

        void Finished(DocumentType documentType);
    }
}
