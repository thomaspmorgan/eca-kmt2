using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Projects
{
    /// <summary>
    /// An IProjectStatusService performs crud operations for a project status.
    /// </summary>
    public interface IProjectStatusService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted project stati in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The project stati in the system.</returns>
        PagedQueryResults<ProjectStatusDTO> Get(QueryableOperator<ProjectStatusDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted project stati in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The project stati in the system.</returns>
        Task<PagedQueryResults<ProjectStatusDTO>> GetAsync(QueryableOperator<ProjectStatusDTO> queryOperator);
    }
}
