using ECA.Business.Models.Fundings;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System;

namespace ECA.Business.Service.Fundings
{
    /// <summary>
    /// A service to perform crud operations on moneyflows
    /// </summary>
    [ContractClass(typeof(MoneyFlowServiceContract))]
    public interface IMoneyFlowService : ISaveable
    {
        /// <summary>
        /// Returns the money flows for the project with the given id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The project's money flows.</returns>
        PagedQueryResults<MoneyFlowDTO> GetMoneyFlowsByProjectId(int projectId, QueryableOperator<MoneyFlowDTO> queryOperator);

        /// <summary>
        /// Returns the money flows for the project with the given id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The project's money flows.</returns>
        Task<PagedQueryResults<MoneyFlowDTO>> GetMoneyFlowsByProjectIdAsync(int projectId, QueryableOperator<MoneyFlowDTO> queryOperator);

        /// <summary>
        /// Returns the money flows for the program with the given id.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The programs's money flows.</returns>
        PagedQueryResults<MoneyFlowDTO> GetMoneyFlowsByProgramId(int programId, QueryableOperator<MoneyFlowDTO> queryOperator);

        /// <summary>
        /// Returns the money flows for the program with the given id.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The programs's money flows.</returns>
        Task<PagedQueryResults<MoneyFlowDTO>> GetMoneyFlowsByProgramIdAsync(int programId, QueryableOperator<MoneyFlowDTO> queryOperator);

        /// <summary>
        /// Returns the money flows for the organization with the given id.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The organization's money flows.</returns>
        PagedQueryResults<MoneyFlowDTO> GetMoneyFlowsByOrganizationId(int organizationId, QueryableOperator<MoneyFlowDTO> queryOperator);

        /// <summary>
        /// Returns the money flows for the organization with the given id.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The organization's money flows.</returns>
        Task<PagedQueryResults<MoneyFlowDTO>> GetMoneyFlowsByOrganizationIdAsync(int organizationId, QueryableOperator<MoneyFlowDTO> queryOperator);

        /// <summary>
        /// Returns the money flows for the office with the given id.
        /// </summary>
        /// <param name="officeId">The office id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The office's money flows.</returns>
        PagedQueryResults<MoneyFlowDTO> GetMoneyFlowsByOfficeId(int officeId, QueryableOperator<MoneyFlowDTO> queryOperator);

        /// <summary>
        /// Returns the money flows for the office with the given id.
        /// </summary>
        /// <param name="officeId">The office id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The office's money flows.</returns>
        Task<PagedQueryResults<MoneyFlowDTO>> GetMoneyFlowsByOfficeIdAsync(int officeId, QueryableOperator<MoneyFlowDTO> queryOperator);

        /// <summary>
        /// Adds the given money flow object to the ECA System.
        /// </summary>
        /// <param name="moneyFlow">The new money flow.</param>
        /// <returns>The ECA.Data money flow entity.</returns>
        MoneyFlow Create(AdditionalMoneyFlow moneyFlow);

        /// <summary>
        /// Adds the given money flow object to the ECA System.
        /// </summary>
        /// <param name="moneyFlow">The new money flow.</param>
        /// <returns>The ECA.Data money flow entity.</returns>
        Task<MoneyFlow> CreateAsync(AdditionalMoneyFlow moneyFlow);

        /// <summary>
        /// Updates the system's money flow entry with the given updated money flow.
        /// </summary>
        /// <param name="updatedMoneyFlow">The updated money flow.</param>
        void Update(UpdatedMoneyFlow updatedMoneyFlow);

        /// <summary>
        /// Updates the system's money flow entry with the given updated money flow.
        /// </summary>
        /// <param name="updatedMoneyFlow">The updated money flow.</param>
        /// <returns>The task.</returns>
        Task UpdateAsync(UpdatedMoneyFlow updatedMoneyFlow);

        /// <summary>
        /// Deletes the money from the system.
        /// </summary>
        /// <param name="deletedMoneyFlow">The money flow to delete.</param>
        void Delete(DeletedMoneyFlow deletedMoneyFlow);

        /// <summary>
        /// Deletes the money from the system.
        /// </summary>
        /// <param name="deletedMoneyFlow">The money flow to delete.</param>
        Task DeleteAsync(DeletedMoneyFlow deletedMoneyFlow);

        /// <summary>
        /// Returns the money flows for the person with the given id.
        /// </summary>
        /// <param name="personId">The office id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The person's money flows.</returns>
        PagedQueryResults<MoneyFlowDTO> GetMoneyFlowsByPersonId(int personId, QueryableOperator<MoneyFlowDTO> queryOperator);

        /// <summary>
        /// Returns the money flows for the person with the given id.
        /// </summary>
        /// <param name="personId">The person id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The person's money flows.</returns>
        Task<PagedQueryResults<MoneyFlowDTO>> GetMoneyFlowsByPersonIdAsync(int personId, QueryableOperator<MoneyFlowDTO> queryOperator);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IMoneyFlowService))]
    public abstract class MoneyFlowServiceContract : IMoneyFlowService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<MoneyFlowDTO> GetMoneyFlowsByProjectId(int projectId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<PagedQueryResults<MoneyFlowDTO>> GetMoneyFlowsByProjectIdAsync(int projectId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<PagedQueryResults<MoneyFlowDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<MoneyFlowDTO> GetMoneyFlowsByProgramId(int programId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<PagedQueryResults<MoneyFlowDTO>> GetMoneyFlowsByProgramIdAsync(int programId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<PagedQueryResults<MoneyFlowDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moneyFlow"></param>
        /// <returns></returns>
        public MoneyFlow Create(AdditionalMoneyFlow moneyFlow)
        {
            Contract.Requires(moneyFlow != null, "The money flow must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moneyFlow"></param>
        /// <returns></returns>
        public Task<MoneyFlow> CreateAsync(AdditionalMoneyFlow moneyFlow)
        {
            Contract.Requires(moneyFlow != null, "The money flow must not be null.");
            return Task.FromResult<MoneyFlow>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedMoneyFlow"></param>
        public void Update(UpdatedMoneyFlow updatedMoneyFlow)
        {
            Contract.Requires(updatedMoneyFlow != null, "The updated money flow must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedMoneyFlow"></param>
        /// <returns></returns>
        public Task UpdateAsync(UpdatedMoneyFlow updatedMoneyFlow)
        {
            Contract.Requires(updatedMoneyFlow != null, "The updated money flow must not be null.");
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult<int>(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deletedMoneyFlow"></param>
        public void Delete(DeletedMoneyFlow deletedMoneyFlow)
        {
            Contract.Requires(deletedMoneyFlow != null, "The deletedMoneyFlow must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deletedMoneyFlow"></param>
        /// <returns></returns>
        public Task DeleteAsync(DeletedMoneyFlow deletedMoneyFlow)
        {
            Contract.Requires(deletedMoneyFlow != null, "The deletedMoneyFlow must not be null.");
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<MoneyFlowDTO> GetMoneyFlowsByOrganizationId(int organizationId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<PagedQueryResults<MoneyFlowDTO>> GetMoneyFlowsByOrganizationIdAsync(int organizationId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<PagedQueryResults<MoneyFlowDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="officeId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<MoneyFlowDTO> GetMoneyFlowsByOfficeId(int officeId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="officeId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<PagedQueryResults<MoneyFlowDTO>> GetMoneyFlowsByOfficeIdAsync(int officeId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<PagedQueryResults<MoneyFlowDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<MoneyFlowDTO> GetMoneyFlowsByPersonId(int personId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<PagedQueryResults<MoneyFlowDTO>> GetMoneyFlowsByPersonIdAsync(int personId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<PagedQueryResults<MoneyFlowDTO>>(null);
        }
    }
}
