using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Core.Logging;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// The ProjectStatusService is used to retrieve project stati from the db context.
    /// </summary>
    public class ProjectStatusService : LookupService<ProjectStatusDTO>, IProjectStatusService
    {
        private static readonly string COMPONENT_NAME = typeof(ProjectService).FullName;
        private readonly ILogger logger;

        /// <summary>
        /// Creates a new ProjectStatusService with the context and logger.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="logger">The logger.</param>
        public ProjectStatusService(EcaContext context, ILogger logger)
            : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(logger != null, "The logger must not be null.");
            this.logger = logger;
        }

        #region Get
        /// <summary>
        /// Returns a query to retrieve project status dtos.
        /// </summary>
        /// <returns>A query to get project status dtos.</returns>
        protected override IQueryable<ProjectStatusDTO> GetSelectDTOQuery()
        {
            var query = this.Context.ProjectStatuses.Select(x => new ProjectStatusDTO
            {
                Id = x.ProjectStatusId,
                Name = x.Status
            });
            return query;
        }
        #endregion
    }
}
