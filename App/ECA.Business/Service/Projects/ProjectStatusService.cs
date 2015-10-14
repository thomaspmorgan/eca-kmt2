using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Lookup;
using ECA.Core.Service;
using ECA.Data;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Service.Projects
{
    /// <summary>
    /// The ProjectStatusService is used to retrieve project stati from the db context.
    /// </summary>
    public class ProjectStatusService : LookupService<ProjectStatusDTO>, IProjectStatusService
    {
        /// <summary>
        /// Creates a new ProjectStatusService with the context.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to operate against.</param>
        public ProjectStatusService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
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
