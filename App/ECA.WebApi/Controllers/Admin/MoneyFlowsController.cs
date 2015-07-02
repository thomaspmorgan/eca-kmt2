﻿using CAM.Business.Model;
using CAM.Business.Queries.Models;
using CAM.Business.Service;
using CAM.Data;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
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
        /// The default sorter
        /// </summary>
        private static readonly ExpressionSorter<MoneyFlowDTO> DEFAULT_MONEY_FLOW_DTO_SORTER = new ExpressionSorter<MoneyFlowDTO>(x => x.TransactionDate, SortDirection.Descending);
        
        private IMoneyFlowService moneyFlowService;
        private IResourceAuthorizationHandler authorizationHandler;
        private IUserProvider userProvider;
        private IResourceService resourceService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="moneyFlowService">The moneyflow service</param>
        public MoneyFlowsController(IMoneyFlowService moneyFlowService)
        {
            Contract.Requires(moneyFlowService != null, "The money flow service must not be null.");
            this.moneyFlowService = moneyFlowService;
        }
        public MoneyFlowsController(IMoneyFlowService moneyFlowService, 
            IResourceAuthorizationHandler authorizationHandler, IUserProvider userProvider, 
            IResourceService resourceService)
        {
            Contract.Requires(moneyFlowService != null, "The money flow service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            Contract.Requires(authorizationHandler != null, "The authorization handler must not be null.");
            Contract.Requires(resourceService != null, "The resource service must not be null.");

            this.moneyFlowService = moneyFlowService;
            this.resourceService = resourceService;
            this.authorizationHandler = authorizationHandler;
            this.userProvider = userProvider;
        }
        [ResponseType(typeof(MoneyFlowDTO))]
        public async Task<IHttpActionResult> GetMoneyFlowByIdAsync(int id)
        {
            var moneyFlow = await this.moneyFlowService.GetMoneyFlowByIdAsync(id);
            if (moneyFlow != null)
            {
                return Ok(moneyFlow);
            }
            else
            {
                return NotFound();
            }
        }
        /// <summary>
        /// Gets moneyflows by the project id
        /// </summary>
        /// <param name="projectId">The project id to query for associated moneyflows</param>
        /// <param name="queryModel">The page, sort, and filter info</param>
        /// <returns>Returns a list of moneyflows that are paged, filtered, and sorted</returns>
        [ResponseType(typeof(PagedQueryResults<MoneyFlowDTO>))]
        [Route("Projects/{projectId:int}/MoneyFlows")]
        public async Task<IHttpActionResult> GetMoneyFlowsByProjectId(int projectId, [FromUri]PagingQueryBindingModel<MoneyFlowDTO> queryModel)
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

        [ResponseType(typeof(MoneyFlowDTO))]
        public async Task<IHttpActionResult> PostMoneyFlowAsync(int moneyFlowId)  // copy money flow
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var moneyFlow = await this.moneyFlowService.GetMoneyFlowByIdAsync(moneyFlowId);

                var ecaMoneyFlow = new EcaMoneyFlow(moneyFlow);
                var newMoneyFlow = await moneyFlowService.CreateAsync(ecaMoneyFlow, businessUser);

                await moneyFlowService.SaveChangesAsync();
                var moneyFlowDTO = await moneyFlowService.GetMoneyFlowByIdAsync(newMoneyFlow.MoneyFlowId);
                return Ok(moneyFlowDTO);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        
        [ResponseType(typeof(MoneyFlowDTO))]
        public async Task<IHttpActionResult> PostMoneyFlowAsync(EcaMoneyFlow moneyFlow)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var newMoneyFlow = await moneyFlowService.CreateAsync(moneyFlow, businessUser);
                await moneyFlowService.SaveChangesAsync();
                var moneyFlowDTO = await moneyFlowService.GetMoneyFlowByIdAsync(newMoneyFlow.MoneyFlowId);
                return Ok(moneyFlowDTO);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Updates and returns the system's project given the client's updated project.
        /// </summary>
        /// <param name="model">The updated project.</param>
        /// <returns>The saved and updated project.</returns>
        [ResponseType(typeof(MoneyFlowDTO))]
        public async Task<IHttpActionResult> PutMoneyFlowAsync(DraftMoneyFlow model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                await moneyFlowService.UpdateAsync(model);
                await moneyFlowService.SaveChangesAsync();
                return null;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        
        public async Task<IHttpActionResult> DeleteMoneyFlowAsync(int moneyFlowId)
        {
            // TODO - add audit trail? Scheme for making items 'inactive' but not deleting from DB?
            var currentUser = userProvider.GetCurrentUser();
            var businessUser = userProvider.GetBusinessUser(currentUser);
            await moneyFlowService.DeleteAsync(moneyFlowId, businessUser);

            return null;
        }
    }
}