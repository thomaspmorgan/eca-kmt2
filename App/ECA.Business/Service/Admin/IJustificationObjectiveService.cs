using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System;
using System.Threading.Tasks;
namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// An IJustificationObjectiveService is capable of performing crud operations on justification objectives.
    /// </summary>
    public interface IJustificationObjectiveService
    {
        /// <summary>
        /// Returns the justification objectives by the office.
        /// </summary>
        /// <param name="officeId">The office id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The justification objectives.</returns>
        PagedQueryResults<JustificationObjectiveDTO> GetJustificationObjectivesByOfficeId(int officeId, QueryableOperator<JustificationObjectiveDTO> queryOperator);

        /// <summary>
        /// Returns the justification objectives by the office.
        /// </summary>
        /// <param name="officeId">The office id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The justification objectives.</returns>
        Task<PagedQueryResults<JustificationObjectiveDTO>> GetJustificationObjectivesByOfficeIdAsync(int officeId, QueryableOperator<JustificationObjectiveDTO> queryOperator);

        /// <summary>
        /// Returns the justification objectives by the program.
        /// </summary>
        /// <param name="officeId">The program id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The justification objectives.</returns>
        PagedQueryResults<JustificationObjectiveDTO> GetJustificationObjectivesByProgramId(int programId, QueryableOperator<JustificationObjectiveDTO> queryOperator);

        /// <summary>
        /// Returns the justification objectives by the program.
        /// </summary>
        /// <param name="officeId">The program id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The justification objectives.</returns>
        Task<PagedQueryResults<JustificationObjectiveDTO>> GetJustificationObjectivesByProgramIdAsync(int programId, QueryableOperator<JustificationObjectiveDTO> queryOperator);
    }
}
