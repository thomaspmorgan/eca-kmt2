﻿using CAM.Business.Model;
using CAM.Business.Queries.Models;
using CAM.Business.Service;
using CAM.Data;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Fundings;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Fundings;
using ECA.WebApi.Models.Projects;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Models.Security;
using ECA.WebApi.Security;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace ECA.WebApi.Controllers.Admin
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

        #region Project
        /// <summary>
        /// Returns the money for the project with the given id.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
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
            return DoUpdateAsync(model, projectId);
        }

        #endregion

        #region Program

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
        /// Updates the given project's money flow.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="model">The updated money flow.</param>
        /// <returns>An Ok result.</returns>
        [ResourceAuthorize(Permission.EDIT_PROGRAM_VALUE, ResourceType.PROGRAM_VALUE, "programId")]
        [Route("Programs/{programId:int}/MoneyFlows")]
        public Task<IHttpActionResult> PutUpdateProgramMoneyFlowAsync([FromBody]UpdatedMoneyFlowBindingModel model, int programId)
        {
            return DoUpdateAsync(model, programId);
        }

        /// <summary>
        /// Creates a new project money flow.
        /// </summary>
        /// <param name="model">The new program money flow.</param>
        /// <returns>An ok result.</returns>        
        [Route("Program/MoneyFlows")]
        [ResourceAuthorize(Permission.EDIT_PROJECT_VALUE, ResourceType.PROJECT_VALUE, typeof(AdditionalProgramMoneyFlowBindingModel), "programId")]
        public Task<IHttpActionResult> PostCreateProgramMoneyFlowAsync([FromBody]AdditionalProgramMoneyFlowBindingModel model)
        {
            return DoCreateAsync(model);
        }
        #endregion

        private async Task<IHttpActionResult> DoUpdateAsync(UpdatedMoneyFlowBindingModel model, int sourceEntityId)
        {
            if (ModelState.IsValid)
            {
                var currentUser = this.userProvider.GetCurrentUser();
                var businessUser = this.userProvider.GetBusinessUser(currentUser);
                await this.moneyFlowService.UpdateAsync(model.ToUpdatedMoneyFlow(businessUser, sourceEntityId));
                await this.moneyFlowService.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        private async Task<IHttpActionResult> DoCreateAsync<T>(AdditionalMoneyFlowBindingModel<T> additionalMoneyFlow) where T : class
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