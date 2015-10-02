using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
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
    /// A ProjectDocumentService is a DocumentService to retrieve ProjectDTOs from the EcaContext.
    /// </summary>
    public class ProjectDocumentService : DocumentService<EcaContext, ProjectDTO>
    {
        /// <summary>
        /// Initializes a new ProjectDocumentService.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="indexService">The index service.</param>
        /// <param name="notificationService">The notification service.</param>
        /// <param name="batchSize">The project dto batch size.</param>
        public ProjectDocumentService(EcaContext context, IIndexService indexService, IIndexNotificationService notificationService, int batchSize = 100)
            : base(context, indexService, notificationService, batchSize)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(indexService != null, "The index service must not be null.");
            Contract.Requires(notificationService != null, "The notification service must not be null.");
        }

        /// <summary>
        /// The query to get project dtos.
        /// </summary>
        /// <returns>The query to get project dtos.</returns>
        public override IQueryable<ProjectDTO> CreateGetDocumentsQuery()
        {
            return ProjectQueries.CreateGetProjectDTOQuery(this.Context).OrderBy(x => x.Id);
        }
    }
}
