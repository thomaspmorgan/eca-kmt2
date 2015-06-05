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
    public interface IFocusCategoryService
    {
        /// <summary>
        /// Returns the focus categories for the office with the given id.
        /// </summary>
        /// <param name="officeId">The office by id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The focus categories.</returns>
        PagedQueryResults<FocusCategoryDTO> GetFocusCategoriesByOfficeId(int officeId, QueryableOperator<FocusCategoryDTO> queryOperator);

        /// <summary>
        /// Returns the focus categories for the office with the given id.
        /// </summary>
        /// <param name="officeId">The office by id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The focus categories.</returns>
        Task<PagedQueryResults<FocusCategoryDTO>> GetFocusCategoriesByOfficeIdAsync(int officeId, QueryableOperator<FocusCategoryDTO> queryOperator);

        /// <summary>
        /// Returns the focus categories for the program with the given id.
        /// </summary>
        /// <param name="officeId">The program by id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The focus categories.</returns>
        PagedQueryResults<FocusCategoryDTO> GetFocusCategoriesByProgramId(int programId, QueryableOperator<FocusCategoryDTO> queryOperator);

        /// <summary>
        /// Returns the focus categories for the program with the given id.
        /// </summary>
        /// <param name="programId">The program by id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The focus categories.</returns>
        Task<PagedQueryResults<FocusCategoryDTO>> GetFocusCategoriesByProgramIdAsync(int programId, QueryableOperator<FocusCategoryDTO> queryOperator);
    }
}
