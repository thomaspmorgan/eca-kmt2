using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
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
    /// A OfficeDocumentService is a DocumentService to retrieve OfficeDTOs from the EcaContext.
    /// </summary>
    public class OfficeDocumentService : DocumentService<EcaContext, OfficeDTO>
    {
        /// <summary>
        /// Initializes a new OfficeDocumentService.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="indexService">The index service.</param>
        /// <param name="notificationService">The notification service.</param>
        /// <param name="batchSize">The office dto batch size.</param>
        public OfficeDocumentService(EcaContext context, IIndexService indexService, IIndexNotificationService notificationService, int batchSize = 100)
            : base(context, indexService, notificationService, batchSize)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(indexService != null, "The index service must not be null.");
            Contract.Requires(notificationService != null, "The notification service must not be null.");
        }

        /// <summary>
        /// The query to get the office by id.
        /// </summary>
        /// <param name="id">The id of the office.</param>
        /// <returns>The query to get the office.</returns>
        public override IQueryable<OfficeDTO> CreateGetDocumentByIdQuery(object id)
        {
            Contract.Requires(id.GetType() == typeof(int), "The id must be integer.");
            return CreateGetDocumentsQuery().Where(x => x.Id == (int)id);
        }

        /// <summary>
        /// The query to get office dtos.
        /// </summary>
        /// <returns>The query to get office dtos.</returns>
        public override IQueryable<OfficeDTO> CreateGetDocumentsQuery()
        {
            return OfficeQueries.CreateGetOfficesQuery(this.Context).OrderBy(x => x.Id);
        }
    }
}
