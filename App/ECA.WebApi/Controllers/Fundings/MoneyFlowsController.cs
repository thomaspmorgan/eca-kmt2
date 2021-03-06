﻿using CAM.Data;
using ECA.Business.Queries.Models.Fundings;
using ECA.Business.Service.Fundings;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.Data;
using ECA.WebApi.Models.Fundings;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Fundings
{
    /// <summary>
    /// Controller for managing moneyflows
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class MoneyFlowsController : ApiController
    {
        /// <summary>
        /// The default sorter of money flows.
        /// </summary>
        private static readonly ExpressionSorter<MoneyFlowDTO> DEFAULT_MONEY_FLOW_DTO_SORTER = new ExpressionSorter<MoneyFlowDTO>(x => x.TransactionDate, SortDirection.Descending);
        
        private readonly IMoneyFlowService moneyFlowService;
        private readonly IUserProvider userProvider;

        /// <summary>
        /// Creates a new money flows controller instance.
        /// </summary>
        /// <param name="moneyFlowService">The money flow service.</param>
        /// <param name="userProvider">The user provider.</param>
        public MoneyFlowsController(IMoneyFlowService moneyFlowService, IUserProvider userProvider)
        {
            Contract.Requires(moneyFlowService != null, "The money flow service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            this.userProvider = userProvider;
            this.moneyFlowService = moneyFlowService;
        }

        /// <summary>
        /// Returns the source or parent money dto for the money flow with the given id.
        /// </summary>
        /// <param name="id">The id of the money flow.</param>
        /// <returns>The source or parent money flow dto of the money flow with the given id.</returns>
        [ResponseType(typeof(SourceMoneyFlowDTO))]
        [Route("MoneyFlows/{id}/Source")]
        public async Task<IHttpActionResult> GetSourceMoneyFlowByIdAsync(int id)
        {
            var source = await moneyFlowService.GetSourceMoneyFlowDTOByIdAsync(id);
            if(source == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(source);
            }
        }

        #region Organization

        /// <summary>
        /// Returns the source money flows of the organization with the given id.  The source money flows will detail the original money flow amount
        /// and remaining money available to withdrawl.
        /// </summary>
        /// <param name="organizationId">The id of the organization to get source money flows for.</param>
        /// <returns>The source money flows.</returns>
        [ResponseType(typeof(List<SourceMoneyFlowDTO>))]
        [Route("Organizations/{organizationId:int}/MoneyFlows/Sources")]
        public async Task<IHttpActionResult> GetSourceMoneyFlowsByOrganizationIdAsync(int organizationId)
        {
            var results = await this.moneyFlowService.GetSourceMoneyFlowsByOrganizationIdAsync(organizationId);
            return Ok(results);
        }

        /// <summary>
        /// Returns the money flows for the organization with the given id.
        /// </summary>
        /// <param name="organizationId">The id of the organization.</param>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The money flows.</returns>
        [ResponseType(typeof(PagedQueryResults<MoneyFlowDTO>))]
        [Route("Organizations/{organizationId:int}/MoneyFlows")]
        public async Task<IHttpActionResult> GetMoneyFlowsByOrganizationIdAsync(int organizationId, [FromUri]PagingQueryBindingModel<MoneyFlowDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.moneyFlowService.GetMoneyFlowsByOrganizationIdAsync(organizationId, queryModel.ToQueryableOperator(DEFAULT_MONEY_FLOW_DTO_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Creates a new organization money flow.
        /// </summary>
        /// <param name="model">The new organization money flow.</param>
        /// <returns>An ok result.</returns>    
        [Route("Organization/MoneyFlows")]
        public Task<IHttpActionResult> PostCreateOrganizationMoneyFlowAsync([FromBody]AdditionalOrganizationMoneyFlowBindingModel model)
        {
            return DoCreateAsync(model);
        }

        /// <summary>
        /// Updates the given organization's money flow.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <param name="model">The updated money flow.</param>
        /// <returns>An Ok result.</returns>
        [Route("Organizations/{organizationId:int}/MoneyFlows")]
        public Task<IHttpActionResult> PutUpdateOrganizationMoneyFlowAsync([FromBody]UpdatedMoneyFlowBindingModel model, int organizationId)
        {
            return DoUpdateAsync(model, organizationId, MoneyFlowSourceRecipientType.Organization.Id);
        }

        /// <summary>
        /// Deletes the given offices's money flow.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <param name="id">The id of the money flow.</param>
        /// <returns>An Ok result.</returns>
        [Route("Organizations/{organizationId:int}/MoneyFlows/{id:int}")]
        public Task<IHttpActionResult> DeleteOrganizationMoneyFlowAsync(int id, int organizationId)
        {
            return DoDeleteAsync(id, organizationId, MoneyFlowSourceRecipientType.Organization.Id);
        }

        /// <summary>
        /// Returns the fiscal year summaries for the organization with the given id.
        /// </summary>
        /// <param name="organizationId">The id of the organization.</param>
        /// <returns>The fiscal year summaries by organization id.</returns>
        [Route("Organizations/{organizationId:int}/MoneyFlows/FiscalYears/Summary")]
        [ResponseType(typeof(List<FiscalYearSummaryDTO>))]
        public async Task<IHttpActionResult> GetFiscalYearSummarysByOrganizationIdAsync(int organizationId)
        {
            var results = await this.moneyFlowService.GetFiscalYearSummariesByOrganizationIdAsync(organizationId);
            return Ok(results);
        }

        #endregion

        #region Office
        /// <summary>
        /// Returns the source money flows of the office with the given id.  The source money flows will detail the original money flow amount
        /// and remaining money available to withdrawl.
        /// </summary>
        /// <param name="officeId">The id of the office to get source money flows for.</param>
        /// <returns>The source money flows.</returns>
        [ResponseType(typeof(List<SourceMoneyFlowDTO>))]
        [Route("Offices/{officeId:int}/MoneyFlows/Sources")]
        [ResourceAuthorize(Permission.VIEW_OFFICE_VALUE, ResourceType.OFFICE_VALUE, "officeId")]
        public async Task<IHttpActionResult> GetSourceMoneyFlowsByOfficeIdAsync(int officeId)
        {
            var results = await this.moneyFlowService.GetSourceMoneyFlowsByOfficeIdAsync(officeId);
            return Ok(results);
        }

        /// <summary>
        /// Returns the money for the office with the given id.
        /// </summary>
        /// <param name="officeId">The id of the office.</param>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The money flows.</returns>
        [ResponseType(typeof(PagedQueryResults<MoneyFlowDTO>))]
        [Route("Offices/{officeId:int}/MoneyFlows")]
        [ResourceAuthorize(Permission.VIEW_OFFICE_VALUE, ResourceType.OFFICE_VALUE, "officeId")]
        public async Task<IHttpActionResult> GetMoneyFlowsByOfficeIdAsync(int officeId, [FromUri]PagingQueryBindingModel<MoneyFlowDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.moneyFlowService.GetMoneyFlowsByOfficeIdAsync(officeId, queryModel.ToQueryableOperator(DEFAULT_MONEY_FLOW_DTO_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Creates a new office money flow.
        /// </summary>
        /// <param name="model">The new project money flow.</param>
        /// <returns>An ok result.</returns>    
        [Route("Office/MoneyFlows")]
        [ResourceAuthorize(Permission.EDIT_OFFICE_VALUE, ResourceType.OFFICE_VALUE, typeof(AdditionalOfficeMoneyFlowBindingModel), "officeId")]
        public Task<IHttpActionResult> PostCreateOfficeMoneyFlowAsync([FromBody]AdditionalOfficeMoneyFlowBindingModel model)
        {
            return DoCreateAsync(model);
        }

        /// <summary>
        /// Updates the given office's money flow.
        /// </summary>
        /// <param name="officeId">The office id.</param>
        /// <param name="model">The updated money flow.</param>
        /// <returns>An Ok result.</returns>
        [ResourceAuthorize(Permission.EDIT_OFFICE_VALUE, ResourceType.OFFICE_VALUE, "officeId")]
        [Route("Offices/{officeId:int}/MoneyFlows")]
        public Task<IHttpActionResult> PutUpdateOfficeMoneyFlowAsync([FromBody]UpdatedMoneyFlowBindingModel model, int officeId)
        {
            return DoUpdateAsync(model, officeId, MoneyFlowSourceRecipientType.Office.Id);
        }

        /// <summary>
        /// Deletes the given office's money flow.
        /// </summary>
        /// <param name="officeId">The office id.</param>
        /// <param name="id">The id of the money flow.</param>
        /// <returns>An Ok result.</returns>
        [ResourceAuthorize(Permission.EDIT_OFFICE_VALUE, ResourceType.OFFICE_VALUE, "officeId")]
        [Route("Offices/{officeId:int}/MoneyFlows/{id:int}")]
        public Task<IHttpActionResult> DeleteOfficeMoneyFlowAsync(int id, int officeId)
        {
            return DoDeleteAsync(id, officeId, MoneyFlowSourceRecipientType.Office.Id);
        }

        /// <summary>
        /// Returns the fiscal year summaries for the office with the given id.
        /// </summary>
        /// <param name="officeId">The id of the office.</param>
        /// <returns>The fiscal year summaries by office id.</returns>
        [Route("Offices/{officeId:int}/MoneyFlows/FiscalYears/Summary")]
        [ResponseType(typeof(List<FiscalYearSummaryDTO>))]
        [ResourceAuthorize(Permission.VIEW_OFFICE_VALUE, ResourceType.OFFICE_VALUE, "officeId")]
        public async Task<IHttpActionResult> GetFiscalYearSummarysByOfficeIdAsync(int officeId)
        {
            var results = await this.moneyFlowService.GetFiscalYearSummariesByOfficeIdAsync(officeId);
            return Ok(results);
        }

        #endregion

        #region Project

        /// <summary>
        /// Returns the source money flows of the project with the given id.  The source money flows will detail the original money flow amount
        /// and remaining money available to withdrawl.
        /// </summary>
        /// <param name="projectId">The id of the project to get source money flows for.</param>
        /// <returns>The source money flows.</returns>
        [ResponseType(typeof(List<SourceMoneyFlowDTO>))]
        [Route("Projects/{projectId:int}/MoneyFlows/Sources")]
        [ResourceAuthorize(Permission.VIEW_PROJECT_VALUE, ResourceType.PROJECT_VALUE, "projectId")]
        public async Task<IHttpActionResult> GetSourceMoneyFlowsByProjectIdAsync(int projectId)
        {
            var results = await this.moneyFlowService.GetSourceMoneyFlowsByProjectIdAsync(projectId);
            return Ok(results);
        }

        /// <summary>
        /// Returns the money for the project with the given id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The money flows.</returns>
        [ResponseType(typeof(PagedQueryResults<MoneyFlowDTO>))]
        [Route("Projects/{projectId:int}/MoneyFlows")]
        [ResourceAuthorize(Permission.VIEW_PROJECT_VALUE, ResourceType.PROJECT_VALUE, "projectId")]
        public async Task<IHttpActionResult> GetMoneyFlowsByProjectIdAsync(int projectId, [FromUri]PagingQueryBindingModel<MoneyFlowDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.moneyFlowService.GetMoneyFlowsByProjectIdAsync(projectId, queryModel.ToQueryableOperator(DEFAULT_MONEY_FLOW_DTO_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Creates a new project money flow.
        /// </summary>
        /// <param name="model">The new project money flow.</param>
        /// <returns>An ok result.</returns>    
        [Route("Project/MoneyFlows")]
        [ResourceAuthorize(Permission.EDIT_PROJECT_VALUE, ResourceType.PROJECT_VALUE, typeof(AdditionalProjectMoneyFlowBindingModel), "projectId")]
        public Task<IHttpActionResult> PostCreateProjectMoneyFlowAsync([FromBody]AdditionalProjectMoneyFlowBindingModel model)
        {
            return DoCreateAsync(model);
        }

        /// <summary>
        /// Updates the given project's money flow.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="model">The updated money flow.</param>
        /// <returns>An Ok result.</returns>
        [ResourceAuthorize(Permission.EDIT_PROJECT_VALUE, ResourceType.PROJECT_VALUE, "projectId")]
        [Route("Projects/{projectId:int}/MoneyFlows")]
        public Task<IHttpActionResult> PutUpdateProjectMoneyFlowAsync([FromBody]UpdatedMoneyFlowBindingModel model, int projectId)
        {
            return DoUpdateAsync(model, projectId, MoneyFlowSourceRecipientType.Project.Id);
        }

        /// <summary>
        /// Deletes the given project's money flow.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="id">The id of the money flow.</param>
        /// <returns>An Ok result.</returns>
        [ResourceAuthorize(Permission.EDIT_PROJECT_VALUE, ResourceType.PROJECT_VALUE, "projectId")]
        [Route("Projects/{projectId:int}/MoneyFlows/{id:int}")]
        public Task<IHttpActionResult> DeleteProjectMoneyFlowAsync(int id, int projectId)
        {
            return DoDeleteAsync(id, projectId, MoneyFlowSourceRecipientType.Project.Id);
        }

        /// <summary>
        /// Returns the fiscal year summaries for the project with the given id.
        /// </summary>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The fiscal year summaries by project id.</returns>
        [Route("Projects/{projectId:int}/MoneyFlows/FiscalYears/Summary")]
        [ResponseType(typeof(List<FiscalYearSummaryDTO>))]
        [ResourceAuthorize(Permission.VIEW_PROJECT_VALUE, ResourceType.PROJECT_VALUE, "projectId")]
        public async Task<IHttpActionResult> GetFiscalYearSummarysByProjectIdAsync(int projectId)
        {
            var results = await this.moneyFlowService.GetFiscalYearSummariesByProjectIdAsync(projectId);
            return Ok(results);
        }

        #endregion

        #region Program
        /// <summary>
        /// Returns the source money flows of the program with the given id.  The source money flows will detail the original money flow amount
        /// and remaining money available to withdrawl.
        /// </summary>
        /// <param name="programId">The id of the program to get source money flows for.</param>
        /// <returns>The source money flows.</returns>
        [ResponseType(typeof(List<SourceMoneyFlowDTO>))]
        [Route("Programs/{programId:int}/MoneyFlows/Sources")]
        [ResourceAuthorize(Permission.VIEW_PROGRAM_VALUE, ResourceType.PROGRAM_VALUE, "programId")]
        public async Task<IHttpActionResult> GetSourceMoneyFlowsByProgramIdAsync(int programId)
        {
            var results = await this.moneyFlowService.GetSourceMoneyFlowsByProgramIdAsync(programId);
            return Ok(results);
        }

        /// <summary>
        /// Returns the money flows for the program with the given id.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The money flows for the program.</returns>
        [ResponseType(typeof(PagedQueryResults<MoneyFlowDTO>))]
        [Route("Programs/{programId:int}/MoneyFlows")]
        [ResourceAuthorize(Permission.VIEW_PROGRAM_VALUE, ResourceType.PROGRAM_VALUE, "programId")]
        public async Task<IHttpActionResult> GetMoneyFlowsByProgramIdAsync(int programId, [FromUri]PagingQueryBindingModel<MoneyFlowDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.moneyFlowService.GetMoneyFlowsByProgramIdAsync(programId, queryModel.ToQueryableOperator(DEFAULT_MONEY_FLOW_DTO_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Updates the given program's money flow.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="model">The updated money flow.</param>
        /// <returns>An Ok result.</returns>
        [ResourceAuthorize(Permission.EDIT_PROGRAM_VALUE, ResourceType.PROGRAM_VALUE, "programId")]
        [Route("Programs/{programId:int}/MoneyFlows")]
        public Task<IHttpActionResult> PutUpdateProgramMoneyFlowAsync([FromBody]UpdatedMoneyFlowBindingModel model, int programId)
        {
            return DoUpdateAsync(model, programId, MoneyFlowSourceRecipientType.Program.Id);
        }

        /// <summary>
        /// Creates a new program money flow.
        /// </summary>
        /// <param name="model">The new program money flow.</param>
        /// <returns>An ok result.</returns>        
        [Route("Program/MoneyFlows")]
        [ResourceAuthorize(Permission.EDIT_PROGRAM_VALUE, ResourceType.PROGRAM_VALUE, typeof(AdditionalProgramMoneyFlowBindingModel), "programId")]
        public Task<IHttpActionResult> PostCreateProgramMoneyFlowAsync([FromBody]AdditionalProgramMoneyFlowBindingModel model)
        {
            return DoCreateAsync(model);
        }

        /// <summary>
        /// Deletes the given program's money flow.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="id">The id of the money flow.</param>
        /// <returns>An Ok result.</returns>
        [ResourceAuthorize(Permission.EDIT_PROGRAM_VALUE, ResourceType.PROGRAM_VALUE, "programId")]
        [Route("Programs/{programId:int}/MoneyFlows/{id:int}")]
        public Task<IHttpActionResult> DeleteProgramMoneyFlowAsync(int id, int programId)
        {
            return DoDeleteAsync(id, programId, MoneyFlowSourceRecipientType.Program.Id);
        }

        /// <summary>
        /// Returns the fiscal year summaries for the program with the given id.
        /// </summary>
        /// <param name="programId">The id of the program.</param>
        /// <returns>The fiscal year summaries by program id.</returns>
        [Route("Programs/{programId:int}/MoneyFlows/FiscalYears/Summary")]
        [ResponseType(typeof(List<FiscalYearSummaryDTO>))]
        [ResourceAuthorize(Permission.VIEW_PROGRAM_VALUE, ResourceType.PROGRAM_VALUE, "programId")]
        public async Task<IHttpActionResult> GetFiscalYearSummarysByProgramIdAsync(int programId)
        {
            var results = await this.moneyFlowService.GetFiscalYearSummariesByProgramIdAsync(programId);
            return Ok(results);
        }
        #endregion

        #region Person
        /// <summary>
        /// Returns the money flows for the program with the given id.
        /// </summary>
        /// <param name="personId">The person id.</param>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The money flows for the program.</returns>
        [ResponseType(typeof(PagedQueryResults<MoneyFlowDTO>))]
        [Route("People/{personId:int}/MoneyFlows")]
        public async Task<IHttpActionResult> GetMoneyFlowsByPersonIdAsync(int personId, [FromUri]PagingQueryBindingModel<MoneyFlowDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.moneyFlowService.GetMoneyFlowsByPersonIdAsync(personId, queryModel.ToQueryableOperator(DEFAULT_MONEY_FLOW_DTO_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Returns the fiscal year summaries for the person with the given id.
        /// </summary>
        /// <param name="personId">The id of the person.</param>
        /// <returns>The fiscal year summaries by person id.</returns>
        [Route("People/{personId:int}/MoneyFlows/FiscalYears/Summary")]
        [ResponseType(typeof(List<FiscalYearSummaryDTO>))]
        public async Task<IHttpActionResult> GetFiscalYearSummarysByPersonIdAsync(int personId)
        {
            var results = await this.moneyFlowService.GetFiscalYearSummariesByPersonIdAsync(personId);
            return Ok(results);
        }
        #endregion
        private async Task<IHttpActionResult> DoDeleteAsync(int moneyFlowId, int sourceEntityId, int sourceEntityTypeId)
        {
            var currentUser = this.userProvider.GetCurrentUser();
            var businessUser = this.userProvider.GetBusinessUser(currentUser);
            var model = new DeletedMoneyFlow(businessUser, moneyFlowId, sourceEntityId, sourceEntityTypeId);
            await this.moneyFlowService.DeleteAsync(model);
            await this.moneyFlowService.SaveChangesAsync();
            return Ok();
        }

        private async Task<IHttpActionResult> DoUpdateAsync(UpdatedMoneyFlowBindingModel model, int sourceEntityId, int sourceEntityTypeId)
        {
            if (ModelState.IsValid)
            {
                var currentUser = this.userProvider.GetCurrentUser();
                var businessUser = this.userProvider.GetBusinessUser(currentUser);
                await this.moneyFlowService.UpdateAsync(model.ToUpdatedMoneyFlow(businessUser, sourceEntityId, sourceEntityTypeId));
                await this.moneyFlowService.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        private async Task<IHttpActionResult> DoCreateAsync(AdditionalMoneyFlowBindingModel additionalMoneyFlow)
        {
            if (ModelState.IsValid)
            {
                var currentUser = this.userProvider.GetCurrentUser();
                var businessUser = this.userProvider.GetBusinessUser(currentUser);
                var instance = additionalMoneyFlow.ToAdditionalMoneyFlow(businessUser);
                var addedMoneyFlow = await this.moneyFlowService.CreateAsync(instance);
                await this.moneyFlowService.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}