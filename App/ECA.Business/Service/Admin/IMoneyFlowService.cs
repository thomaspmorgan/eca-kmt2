using ECA.Business.Queries.Models.Admin;
using ECA.Business.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// A service to perform crud operations on moneyflows
    /// </summary>
    public interface IMoneyFlowService : ISaveable
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

        MoneyFlow Create(DraftMoneyFlow moneyFlow, User user);

        Task<MoneyFlow> CreateAsync(DraftMoneyFlow moneyFlow, User user);
        
        void Update(EcaMoneyFlow updatedMoneyFlow);

        /// <summary>
        /// Updates the project in the system with the given project.
        /// </summary>
        /// <param name="updatedProject">The updated project.</param>
        Task UpdateAsync(EcaMoneyFlow updatedMoneyFlow);

        MoneyFlow Copy(int moneyFlowId, User user);

        Task<MoneyFlow> CopyAsync(int moneyFlowId, User user);

        Task<MoneyFlow> GetMoneyFlowByIdAsync(int moneyFlowId);

        MoneyFlow GetMoneyFlowById(int moneyFlowId);

    }
}
