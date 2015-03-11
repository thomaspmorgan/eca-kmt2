using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// A service to perform crud operations on moneyflows
    /// </summary>
    public interface IMoneyFlowService
    {
        /// <summary>
        /// Gets moneyflows by the project id
        /// </summary>
        /// <param name="projectId">The project id to find associated moneyflows</param>
        /// <param name="queryOperator"></param>
        /// <returns>List of moneyflows that are paged, sorted, and filtered</returns>
        PagedQueryResults<MoneyFlowDTO> GetMoneyFlowsByProjectId(int projectId, QueryableOperator<MoneyFlowDTO> queryOperator);

        /// <summary>
        /// Gets moneyflows by the project id asynchronously
        /// </summary>
        /// <param name="projectId">The project id to find associated moneyflows</param>
        /// <param name="queryOperator"></param>
        /// <returns>List of moneyflows that are paged, sorted, and filtered</returns>
        Task<PagedQueryResults<MoneyFlowDTO>> GetMoneyFlowsByProjectIdAsync(int projectId, QueryableOperator<MoneyFlowDTO> queryOperator);
    }
}
