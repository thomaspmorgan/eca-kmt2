﻿using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Projects;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;

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
        /// The query to get the project by id.
        /// </summary>
        /// <param name="id">The id of the project.</param>
        /// <returns>The query to get the project.</returns>
        public override IQueryable<ProjectDTO> CreateGetDocumentByIdQuery(object id)
        {
            return CreateGetDocumentsQuery().Where(x => x.Id == (int)id);
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
