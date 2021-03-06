﻿using ECA.Business.Models.Fundings;
using ECA.Business.Queries.Models.Fundings;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System.Collections.Generic;
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
        #region Fiscal Year Summaries

        /// <summary>
        /// Returns the fiscal year summaries for the project with the given id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns>The fiscal year summaries.</returns>
        List<FiscalYearSummaryDTO> GetFiscalYearSummariesByProjectId(int projectId);

        /// <summary>
        /// Returns the fiscal year summaries for the project with the given id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns>The fiscal year summaries.</returns>
        Task<List<FiscalYearSummaryDTO>> GetFiscalYearSummariesByProjectIdAsync(int projectId);

        /// <summary>
        /// Returns the fiscal year summaries for the program with the given id.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <returns>The fiscal year summaries.</returns>
        List<FiscalYearSummaryDTO> GetFiscalYearSummariesByProgramId(int programId);

        /// <summary>
        /// Returns the fiscal year summaries for the program with the given id.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <returns>The fiscal year summaries.</returns>
        Task<List<FiscalYearSummaryDTO>> GetFiscalYearSummariesByProgramIdAsync(int programId);

        /// <summary>
        /// Returns the fiscal year summaries for the office with the given id.
        /// </summary>
        /// <param name="officeId">The office id.</param>
        /// <returns>The fiscal year summaries.</returns>
        List<FiscalYearSummaryDTO> GetFiscalYearSummariesByOfficeId(int officeId);

        /// <summary>
        /// Returns the fiscal year summaries for the office with the given id.
        /// </summary>
        /// <param name="officeId">The office id.</param>
        /// <returns>The fiscal year summaries.</returns>
        Task<List<FiscalYearSummaryDTO>> GetFiscalYearSummariesByOfficeIdAsync(int officeId);

        /// <summary>
        /// Returns the fiscal year summaries for the organization with the given id.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <returns>The fiscal year summaries.</returns>
        List<FiscalYearSummaryDTO> GetFiscalYearSummariesByOrganizationId(int organizationId);

        /// <summary>
        /// Returns the fiscal year summaries for the organization with the given id.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <returns>The fiscal year summaries.</returns>
        Task<List<FiscalYearSummaryDTO>> GetFiscalYearSummariesByOrganizationIdAsync(int organizationId);

        /// <summary>
        /// Returns the fiscal year summaries for the person with the given id.
        /// </summary>
        /// <param name="personId">The person id.</param>
        /// <returns>The fiscal year summaries.</returns>
        List<FiscalYearSummaryDTO> GetFiscalYearSummariesByPersonId(int personId);

        /// <summary>
        /// Returns the fiscal year summaries for the person with the given id.
        /// </summary>
        /// <param name="personId">The person id.</param>
        /// <returns>The fiscal year summaries.</returns>
        Task<List<FiscalYearSummaryDTO>> GetFiscalYearSummariesByPersonIdAsync(int personId);

        #endregion

        #region Money Flows

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
        #endregion

        #region Crud
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
        #endregion

        #region Source Money Flows
        /// <summary>
        /// Returns the source money flows of the project with the given id.  The source money flows will detail the original money flow amount
        /// and remaining money available to withdrawl.
        /// </summary>
        /// <param name="projectId">The id of the project to get source money flows for.</param>
        /// <returns>The source money flows.</returns>
        List<SourceMoneyFlowDTO> GetSourceMoneyFlowsByProjectId(int projectId);

        /// <summary>
        /// Returns the source money flows of the project with the given id.  The source money flows will detail the original money flow amount
        /// and remaining money available to withdrawl.
        /// </summary>
        /// <param name="projectId">The id of the project to get source money flows for.</param>
        /// <returns>The source money flows.</returns>
        Task<List<SourceMoneyFlowDTO>> GetSourceMoneyFlowsByProjectIdAsync(int projectId);

        /// <summary>
        /// Returns the source money flows of the program with the given id.  The source money flows will detail the original money flow amount
        /// and remaining money available to withdrawl.
        /// </summary>
        /// <param name="programId">The id of the program to get source money flows for.</param>
        /// <returns>The source money flows.</returns>
        List<SourceMoneyFlowDTO> GetSourceMoneyFlowsByProgramId(int programId);

        /// <summary>
        /// Returns the source money flows of the program with the given id.  The source money flows will detail the original money flow amount
        /// and remaining money available to withdrawl.
        /// </summary>
        /// <param name="programId">The id of the program to get source money flows for.</param>
        /// <returns>The source money flows.</returns>
        Task<List<SourceMoneyFlowDTO>> GetSourceMoneyFlowsByProgramIdAsync(int programId);

        /// <summary>
        /// Returns the source money flows of the organization with the given id.  The source money flows will detail the original money flow amount
        /// and remaining money available to withdrawl.
        /// </summary>
        /// <param name="organizationId">The id of the organization to get source money flows for.</param>
        /// <returns>The source money flows.</returns>
        List<SourceMoneyFlowDTO> GetSourceMoneyFlowsByOrganizationId(int organizationId);

        /// <summary>
        /// Returns the source money flows of the organization with the given id.  The source money flows will detail the original money flow amount
        /// and remaining money available to withdrawl.
        /// </summary>
        /// <param name="organizationId">The id of the organization to get source money flows for.</param>
        /// <returns>The source money flows.</returns>
        Task<List<SourceMoneyFlowDTO>> GetSourceMoneyFlowsByOrganizationIdAsync(int organizationId);

        /// <summary>
        /// Returns the source money flows of the office with the given id.  The source money flows will detail the original money flow amount
        /// and remaining money available to withdrawl.
        /// </summary>
        /// <param name="officeId">The id of the office to get source money flows for.</param>
        /// <returns>The source money flows.</returns>
        List<SourceMoneyFlowDTO> GetSourceMoneyFlowsByOfficeId(int officeId);

        /// <summary>
        /// Returns the source money flows of the office with the given id.  The source money flows will detail the original money flow amount
        /// and remaining money available to withdrawl.
        /// </summary>
        /// <param name="officeId">The id of the office to get source money flows for.</param>
        /// <returns>The source money flows.</returns>
        Task<List<SourceMoneyFlowDTO>> GetSourceMoneyFlowsByOfficeIdAsync(int officeId);

        /// <summary>
        /// Returns the source money flow with the given id or null if not found.
        /// </summary>
        /// <param name="moneyFlowId">The money flow id.</param>
        /// <returns>The source money flow dto.</returns>
        SourceMoneyFlowDTO GetSourceMoneyFlowDTOById(int moneyFlowId);

        /// <summary>
        /// Returns the source money flow with the given id or null if not found.
        /// </summary>
        /// <param name="moneyFlowId">The money flow id.</param>
        /// <returns>The source money flow dto.</returns>
        Task<SourceMoneyFlowDTO> GetSourceMoneyFlowDTOByIdAsync(int moneyFlowId);
        #endregion
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<SourceMoneyFlowDTO> GetSourceMoneyFlowsByProjectId(int projectId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public Task<List<SourceMoneyFlowDTO>> GetSourceMoneyFlowsByProjectIdAsync(int projectId)
        {
            return Task.FromResult<List<SourceMoneyFlowDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public List<SourceMoneyFlowDTO> GetSourceMoneyFlowsByProgramId(int programId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public Task<List<SourceMoneyFlowDTO>> GetSourceMoneyFlowsByProgramIdAsync(int programId)
        {
            return Task.FromResult<List<SourceMoneyFlowDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public List<SourceMoneyFlowDTO> GetSourceMoneyFlowsByOrganizationId(int organizationId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public Task<List<SourceMoneyFlowDTO>> GetSourceMoneyFlowsByOrganizationIdAsync(int organizationId)
        {
            return Task.FromResult<List<SourceMoneyFlowDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="officeId"></param>
        /// <returns></returns>
        public List<SourceMoneyFlowDTO> GetSourceMoneyFlowsByOfficeId(int officeId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="officeId"></param>
        /// <returns></returns>
        public Task<List<SourceMoneyFlowDTO>> GetSourceMoneyFlowsByOfficeIdAsync(int officeId)
        {
            return Task.FromResult<List<SourceMoneyFlowDTO>>(null); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moneyFlowId"></param>
        /// <returns></returns>
        public SourceMoneyFlowDTO GetSourceMoneyFlowDTOById(int moneyFlowId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moneyFlowId"></param>
        /// <returns></returns>
        public Task<SourceMoneyFlowDTO> GetSourceMoneyFlowDTOByIdAsync(int moneyFlowId)
        {
            return Task.FromResult<SourceMoneyFlowDTO>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<FiscalYearSummaryDTO> GetFiscalYearSummariesByProjectId(int projectId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public Task<List<FiscalYearSummaryDTO>> GetFiscalYearSummariesByProjectIdAsync(int projectId)
        {
            return Task.FromResult<List<FiscalYearSummaryDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public List<FiscalYearSummaryDTO> GetFiscalYearSummariesByProgramId(int programId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public Task<List<FiscalYearSummaryDTO>> GetFiscalYearSummariesByProgramIdAsync(int programId)
        {
            return Task.FromResult<List<FiscalYearSummaryDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="officeId"></param>
        /// <returns></returns>
        public List<FiscalYearSummaryDTO> GetFiscalYearSummariesByOfficeId(int officeId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="officeId"></param>
        /// <returns></returns>
        public Task<List<FiscalYearSummaryDTO>> GetFiscalYearSummariesByOfficeIdAsync(int officeId)
        {
            return Task.FromResult<List<FiscalYearSummaryDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public List<FiscalYearSummaryDTO> GetFiscalYearSummariesByOrganizationId(int organizationId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public Task<List<FiscalYearSummaryDTO>> GetFiscalYearSummariesByOrganizationIdAsync(int organizationId)
        {
            return Task.FromResult<List<FiscalYearSummaryDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public List<FiscalYearSummaryDTO> GetFiscalYearSummariesByPersonId(int personId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public Task<List<FiscalYearSummaryDTO>> GetFiscalYearSummariesByPersonIdAsync(int personId)
        {
            return Task.FromResult<List<FiscalYearSummaryDTO>>(null);
        }
    }
}
