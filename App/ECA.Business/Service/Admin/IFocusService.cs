using System;
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
        ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Admin.FocusDTO> GetFoci(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Admin.FocusDTO> queryOperator);

        /// <summary>
        /// Returns the foci currently in the system.
        /// </summary>
        /// <param name="queryOperator">The query oprator.</param>
        /// <returns>The foci in the system.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Admin.FocusDTO>> GetFociAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Admin.FocusDTO> queryOperator);
    }
}
