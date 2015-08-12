using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Fundings
{
    /// <summary>
    /// An IProjectStatusService performs crud operations for a project status.
    /// </summary>
    public interface IMoneyFlowStatusService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted MF stati in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The project stati in the system.</returns>
        PagedQueryResults<MoneyFlowStatusDTO> Get(QueryableOperator<MoneyFlowStatusDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted project stati in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The project stati in the system.</returns>
        Task<PagedQueryResults<MoneyFlowStatusDTO>> GetAsync(QueryableOperator<MoneyFlowStatusDTO> queryOperator);
    }

    public interface IMoneyFlowTypeService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted MF stati in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The project stati in the system.</returns>
        PagedQueryResults<MoneyFlowTypeDTO> Get(QueryableOperator<MoneyFlowTypeDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted project stati in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The project stati in the system.</returns>
        Task<PagedQueryResults<MoneyFlowTypeDTO>> GetAsync(QueryableOperator<MoneyFlowTypeDTO> queryOperator);
    }
    public interface IMoneyFlowSourceRecipientTypeService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted MF stati in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The project stati in the system.</returns>
        PagedQueryResults<MoneyFlowSourceRecipientTypeDTO> Get(QueryableOperator<MoneyFlowSourceRecipientTypeDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted project stati in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The project stati in the system.</returns>
        Task<PagedQueryResults<MoneyFlowSourceRecipientTypeDTO>> GetAsync(QueryableOperator<MoneyFlowSourceRecipientTypeDTO> queryOperator);
    }
}
