using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using System.Threading.Tasks;
namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// A ProjectService is a service capable of performing crud on projects.
    /// </summary>
    public interface IProjectService
    {
        ECA.Data.Project Create(DraftProject project);

        /// <summary>
        /// Returns the sorted, filtered, and paged projects in the program with the given program id.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted projects.</returns>
        PagedQueryResults<SimpleProjectDTO> GetProjectsByProgramId(int programId, QueryableOperator<SimpleProjectDTO> queryOperator);

        /// <summary>
        /// Returns the sorted, filtered, and paged projects in the program with the given program id.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted projects.</returns>
        Task<PagedQueryResults<SimpleProjectDTO>> GetProjectsByProgramIdAsync(int programId, QueryableOperator<SimpleProjectDTO> queryOperator);

        /// <summary>
        /// Returns a project by id asynchronously
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<ProjectDTO> GetProjectByIdAsync(int projectId);

        /// <summary>
        /// Returns a project by id  
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        ProjectDTO GetProjectById(int projectId);
    }
}
