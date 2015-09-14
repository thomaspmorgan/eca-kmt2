using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System.Threading.Tasks;

namespace ECA.Business.Service.Projects
{
    /// <summary>
    /// An IProjectStatusService performs crud operations for a project status.
    /// </summary>
    public interface IProgramStatusService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted program stati in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The program stati in the system.</returns>
        PagedQueryResults<ProgramStatusDTO> Get(QueryableOperator<ProgramStatusDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted program stati in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The program stati in the system.</returns>
        Task<PagedQueryResults<ProgramStatusDTO>> GetAsync(QueryableOperator<ProgramStatusDTO> queryOperator);
    }
}
