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
    /// <summary>
    /// A ProgramDocumentService is a DocumentService to retrieve ProgramDTOs from the EcaContext.
    /// </summary>
    public class ProgramDocumentService : DocumentService<EcaContext, ProgramDTO>
    {
        /// <summary>
        /// Initializes a new ProgramDocumentService.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="indexService">The index service.</param>
        /// <param name="notificationService">The notification service.</param>
        /// <param name="batchSize">The program dto batch size.</param>
        public ProgramDocumentService(EcaContext context, IIndexService indexService, IIndexNotificationService notificationService, int batchSize = 100)
            : base(context, indexService, notificationService, batchSize)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(indexService != null, "The index service must not be null.");
            Contract.Requires(notificationService != null, "The notification service must not be null.");
        }

        /// <summary>
        /// The query to get the program by id.
        /// </summary>
        /// <param name="id">The id of the program.</param>
        /// <returns>The query to get the program.</returns>
        public override IQueryable<ProgramDTO> CreateGetDocumentByIdQuery(object id)
        {
            return CreateGetDocumentsQuery().Where(x => x.Id == (int)id);
        }

        /// <summary>
        /// The query to get program dtos.
        /// </summary>
        /// <returns>The query to get program dtos.</returns>
        public override IQueryable<ProgramDTO> CreateGetDocumentsQuery()
        {
            return ProgramQueries.CreateGetPublishedProgramsQuery(this.Context).OrderBy(x => x.Id);
        }
    }
}
