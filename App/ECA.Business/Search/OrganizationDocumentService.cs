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
    /// A OrganizationDocumentService is a DocumentService to retrieve OrganizationDTOs from the EcaContext.
    /// </summary>
    public class OrganizationDocumentService : DocumentService<EcaContext, OrganizationDTO>
    {
        /// <summary>
        /// Initializes a new OrganizationDocumentService.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="indexService">The index service.</param>
        /// <param name="notificationService">The notification service.</param>
        /// <param name="batchSize">The organization dto batch size.</param>
        public OrganizationDocumentService(EcaContext context, IIndexService indexService, IIndexNotificationService notificationService, int batchSize = 100)
            : base(context, indexService, notificationService, batchSize)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(indexService != null, "The index service must not be null.");
            Contract.Requires(notificationService != null, "The notification service must not be null.");
        }

        /// <summary>
        /// Returns a query to get an Organization DTO by id.
        /// </summary>
        /// <param name="id">The id of the organization.</param>
        /// <returns>The query.</returns>
        public override IQueryable<OrganizationDTO> CreateGetDocumentByIdQuery(object id)
        {
            Contract.Requires(id.GetType() == typeof(int), "The id type must be an int.");
            return OrganizationQueries.CreateGetOrganizationDTOByOrganizationIdQuery(this.Context, (int)id);
        }

        /// <summary>
        /// Returns a query to get organization dtos.
        /// </summary>
        /// <returns>The query.</returns>
        public override IQueryable<OrganizationDTO> CreateGetDocumentsQuery()
        {
            return OrganizationQueries.CreateGetOrganizationDTOsQuery(this.Context);
        }
    }
}
