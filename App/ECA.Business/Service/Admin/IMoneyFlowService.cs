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

        /// <summary>
        /// Creates and returns money flow
        /// </summary>
        /// <param name="project">The MF to create</param>
        /// <returns>The MF that was created</returns>
        MoneyFlow Create(DraftMoneyFlow moneyFlow);

        /// <summary>
        /// Creates and returns money flow asyncronously
        /// </summary>
        /// <param name="project">The MF to create</param>
        /// <returns>The MF that was created</returns>
        Task<MoneyFlow> CreateAsync(DraftMoneyFlow moneyFlow);

        /// <summary>
        /// Updates the project in the system with the given project.
        /// </summary>
        /// <param name="updatedProject">The updated project.</param>
        void Update(EcaMoneyFlow updatedMoneyFlow);

        /// <summary>
        /// Updates the project in the system with the given project.
        /// </summary>
        /// <param name="updatedProject">The updated project.</param>
        Task UpdateAsync(EcaMoneyFlow updatedMoneyFlow);
    }
}
