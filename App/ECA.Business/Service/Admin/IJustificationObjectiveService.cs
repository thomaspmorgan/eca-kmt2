using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System;
using System.Threading.Tasks;
namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// An IFocusService is capable of performing crud operations on Foci.
    /// </summary>
    public interface IJustificationObjectiveService
    {

        PagedQueryResults<JustificationObjectiveDTO> Get(QueryableOperator<JustificationObjectiveDTO> queryOperator);

        Task<PagedQueryResults<JustificationObjectiveDTO>> GetAsync(QueryableOperator<JustificationObjectiveDTO> queryOperator);

        /// <summary>
        /// Returns the focus with the given id or null if not found.
        /// </summary>
        /// <param name="id">The id of the focus.</param>
        /// <returns>The focus with the given id, or null if not found.</returns>
        JustificationObjectiveDTO GetJustificationObjectiveById(int id);

        /// <summary>
        /// Returns the focus with the given id or null if not found.
        /// </summary>
        /// <param name="id">The id of the focus.</param>
        /// <returns>The focus with the given id, or null if not found.</returns>
        Task<JustificationObjectiveDTO> GetJustificationObjectiveByIdAsync(int id);
    }
}
