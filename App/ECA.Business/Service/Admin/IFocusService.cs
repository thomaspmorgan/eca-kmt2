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
    public interface IFocusService
    {
        /// <summary>
        /// Returns the foci currently in the system.
        /// </summary>
        /// <param name="queryOperator">The query oprator.</param>
        /// <returns>The foci in the system.</returns>
        PagedQueryResults<FocusDTO> GetFoci(QueryableOperator<FocusDTO> queryOperator);

        /// <summary>
        /// Returns the foci currently in the system.
        /// </summary>
        /// <param name="queryOperator">The query oprator.</param>
        /// <returns>The foci in the system.</returns>
        Task<PagedQueryResults<FocusDTO>> GetFociAsync(QueryableOperator<FocusDTO> queryOperator);

        /// <summary>
        /// Returns the focus with the given id or null if not found.
        /// </summary>
        /// <param name="id">The id of the focus.</param>
        /// <returns>The focus with the given id, or null if not found.</returns>
        FocusDTO GetFocusById(int id);

        /// <summary>
        /// Returns the focus with the given id or null if not found.
        /// </summary>
        /// <param name="id">The id of the focus.</param>
        /// <returns>The focus with the given id, or null if not found.</returns>
        Task<FocusDTO> GetFocusByIdAsync(int id);
    }
}
