using ECA.Business.Queries.Models.Programs;
using ECA.Business.Queries.Programs;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    public class ProgramDocumentService : DocumentService<EcaContext, ProgramDTO>
    {
        public ProgramDocumentService(EcaContext context, IIndexService indexService, IIndexNotificationService notificationService, int batchSize = 500) : base(context, indexService, notificationService, batchSize)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(indexService != null, "The index service must not be null.");
            Contract.Requires(notificationService != null, "The notification service must not be null.");
        }

        public override IQueryable<ProgramDTO> CreateGetDocumentsQuery()
        {
            return ProgramQueries.CreateGetPublishedProgramsQuery(this.Context).OrderBy(x => x.Id);
        }
    }
}
