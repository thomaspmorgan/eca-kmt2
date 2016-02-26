using CAM.Business.Model;
using CAM.Business.Service;
using CAM.Data;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Office;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Admin;
using ECA.WebApi.Models.Offices;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace ECA.WebApi.Controllers.Admin
{
    /// <summary>
    /// The OfficesController is capable of performing crud operations for an office.
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class OfficesController : ApiController
    {
        private static readonly ExpressionSorter<OrganizationProgramDTO> DEFAULT_ORGANIZATION_PROGRAM_SORTER =
            new ExpressionSorter<OrganizationProgramDTO>(x => x.Name, SortDirection.Ascending);

        private static readonly ExpressionSorter<SimpleOfficeDTO> DEFAULT_OFFICE_SORTER =
            new ExpressionSorter<SimpleOfficeDTO>(x => x.OfficeSymbol, SortDirection.Ascending);

        /// <summary>
        /// The default sorter for a list of foci.
        /// </summary>
        private static readonly ExpressionSorter<FocusCategoryDTO> DEFAULT_FOCUSCATEGORY_DTO_SORTER = 
            new ExpressionSorter<FocusCategoryDTO>(x => x.FocusName, SortDirection.Ascending);

        /// <summary>
        /// The default sorter for a list of justifications.
        /// </summary>
        private static readonly ExpressionSorter<JustificationObjectiveDTO> DEFAULT_JUSTIFICATION_OBJECTIVE_DTO_SORTER =
            new ExpressionSorter<JustificationObjectiveDTO>(x => x.JustificationName, SortDirection.Ascending);

        private static readonly ExpressionSorter<ResourceAuthorization> DEFAULT_RESOURCE_AUTHORIZATION_SORTER = new ExpressionSorter<ResourceAuthorization>(x => x.DisplayName, SortDirection.Ascending);


        private IOfficeService service;
        private IFocusCategoryService focusCategoryService;
        private IJustificationObjectiveService justificationObjectiveService;
        private IResourceService resourceService;
        private IResourceAuthorizationHandler authorizationHandler;
        private IUserProvider userProvider;

        /// <summary>
        /// Creates a new controller instance.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="focusCategoryService">The focus category service.</param>
        /// <param name="justificationObjectiveService">The justification objective service.</param>
        public OfficesController(IOfficeService service, IFocusCategoryService focusCategoryService, IJustificationObjectiveService justificationObjectiveService, IResourceService resourceService, IResourceAuthorizationHandler authorizationHandler, IUserProvider userProvider)
        {
            Contract.Requires(service != null, "The office service must not be null.");
            Contract.Requires(focusCategoryService != null, "The focus category service must not be null.");
            Contract.Requires(justificationObjectiveService != null, "The justification service must not be null.");
            Contract.Requires(resourceService != null, "The resource service must not be null.");
            Contract.Requires(authorizationHandler != null, "The authorization handler must not be null.");
            Contract.Requires(userProvider != null, "The authorization handler must not be null.");
            this.service = service;
            this.focusCategoryService = focusCategoryService;
            this.justificationObjectiveService = justificationObjectiveService;
            this.resourceService = resourceService;
            this.authorizationHandler = authorizationHandler;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Returns the office with the given id.
        /// </summary>
        /// <param name="id">The id of the office.</param>
        /// <returns>The office.</returns>
        [ResponseType(typeof(OfficeDTO))]
        [Route("Offices/{id}")]
        public async Task<IHttpActionResult> GetOfficeByIdAsync(int id)
        {
            var dto = await this.service.GetOfficeByIdAsync(id);
            if (dto != null)
            {
                return Ok(dto);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Returns the office settings with the given office id.
        /// </summary>
        /// <param name="id">The id of the office.</param>
        /// <returns>The office settings.</returns>
        [ResponseType(typeof(List<OfficeSettings>))]
        [Route("Offices/{id}/Settings")]
        public async Task<IHttpActionResult> GetOfficeSettingsByIdAsync(int id)
        {
            var settings = await this.service.GetOfficeSettingsAsync(id);
            return Ok(settings);
        }

        /// <summary>
        /// Returns the first level child offices/branches/divisions of the office with the given id.
        /// </summary>
        /// <param name="id">The office id.</param>
        /// <returns>The child offices, branches, and divisions.</returns>
        [Route("Offices/{id}/ChildOffices")]
        [ResponseType(typeof(List<SimpleOfficeDTO>))]
        public async Task<IHttpActionResult> GetChildOfficesByOfficeIdAsync(int id)
        {
            var dto = await this.service.GetChildOfficesAsync(id);
            if (dto != null)
            {
                return Ok(dto);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Returns the child programs of the office with the given id.
        /// </summary>
        /// <param name="id">The id of the office.</param>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The child programs.</returns>
        [Route("Offices/{id}/Programs")]
        [ResponseType(typeof(PagedQueryResults<OrganizationProgramDTO>))]
        public async Task<IHttpActionResult> GetProgramsByOfficeIdAsync(int id, [FromUri]PagingQueryBindingModel<OrganizationProgramDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await service.GetProgramsAsync(
                    id,
                    queryModel.ToQueryableOperator(
                    DEFAULT_ORGANIZATION_PROGRAM_SORTER,
                    x => x.Name,
                    x => x.Description,
                    x => x.OfficeSymbol));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Returns the categories of the office with the given id.
        /// </summary>
        /// <param name="officeId">The id of the office.</param>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The categories.</returns>
        [Route("Offices/{officeId}/Categories")]
        [ResponseType(typeof(PagedQueryResults<FocusCategoryDTO>))]
        public async Task<IHttpActionResult> GetCategoriesByOfficeIdAsync(int officeId, [FromUri]PagingQueryBindingModel<FocusCategoryDTO> queryModel)
        {
            if (ModelState.IsValid)
            {                
                var results = await focusCategoryService.GetFocusCategoriesByOfficeIdAsync(
                    officeId, 
                    queryModel.ToQueryableOperator(DEFAULT_FOCUSCATEGORY_DTO_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Returns the objectives of the office with the given id.
        /// </summary>
        /// <param name="officeId">The id of the office.</param>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The objectives.</returns>
        [Route("Offices/{officeId}/Objectives")]
        [ResponseType(typeof(PagedQueryResults<JustificationObjectiveDTO>))]
        public async Task<IHttpActionResult> GetObjectivesByOfficeIdAsync(int officeId, [FromUri]PagingQueryBindingModel<JustificationObjectiveDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await justificationObjectiveService.GetJustificationObjectivesByOfficeIdAsync(
                    officeId,
                    queryModel.ToQueryableOperator(DEFAULT_JUSTIFICATION_OBJECTIVE_DTO_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Returns a hierarchical list of offices
        /// </summary>
        /// <param name="queryModel">The query model</param>
        /// <returns>The offices</returns>
        [ResponseType(typeof(PagedQueryResults<SimpleOfficeDTO>))]
        [Route("Offices")]
        public async Task<IHttpActionResult> GetOfficesAsync([FromUri]PagingQueryBindingModel<SimpleOfficeDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await service.GetOfficesAsync(
                    queryModel.ToQueryableOperator(
                        DEFAULT_OFFICE_SORTER,
                        x => x.OfficeSymbol,
                        x => x.Name,
                        x => x.Description));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Updates the office
        /// </summary>
        /// <param name="model">The model to update</param>
        /// <returns>The updated office</returns>
        [ResponseType(typeof(OfficeDTO))]
        [Route("Offices")]
        public async Task<IHttpActionResult> PutOfficeAsync([FromBody]UpdatedOfficeBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                await service.UpdateOfficeAsync(model.ToUpdatedOffice(businessUser));
                await service.SaveChangesAsync();
                var updatedOffice = await service.GetOfficeByIdAsync(model.OfficeId);
                return Ok(updatedOffice);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Adds a collaborator to an office.
        /// </summary>
        /// <param name="model">The add collaborator model.</param>
        /// <returns>An ok result.</returns>
        [Route("Offices/Collaborator/Add")]
        [ResponseType(typeof(OkResult))]
        [ResourceAuthorize(CAM.Data.Permission.OFFICE_OWNER_VALUE, ResourceType.OFFICE_VALUE, typeof(OfficeCollaboratorBindingModel), "OfficeId")]
        public Task<IHttpActionResult> PostAddCollaboratorAsync(OfficeCollaboratorBindingModel model)
        {
            return authorizationHandler.HandleGrantedPermissionBindingModelAsync(model, this);
        }

        /// <summary>
        /// Remove a collaborator from an office.
        /// </summary>
        /// <param name="model">The remove collaborator model.</param>
        /// <returns>An ok result.</returns>
        [Route("Offices/Collaborator/Remove")]
        [ResponseType(typeof(OkResult))]
        [ResourceAuthorize(CAM.Data.Permission.OFFICE_OWNER_VALUE, ResourceType.OFFICE_VALUE, typeof(OfficeCollaboratorBindingModel), "OfficeId")]
        public Task<IHttpActionResult> PostRemoveCollaboratorAsync(OfficeCollaboratorBindingModel model)
        {
            return authorizationHandler.HandleDeletedPermissionBindingModelAsync(model, this);
        }

        /// <summary>
        /// Revokes a collaborator permission from an office.
        /// </summary>
        /// <param name="model">The add collaborator model.</param>
        /// <returns>An ok result.</returns>
        [Route("Offices/Collaborator/Revoke")]
        [ResponseType(typeof(OkResult))]
        [ResourceAuthorize(CAM.Data.Permission.OFFICE_OWNER_VALUE, ResourceType.OFFICE_VALUE, typeof(OfficeCollaboratorBindingModel), "OfficeId")]
        public Task<IHttpActionResult> PostRevokeCollaboratorAsync(OfficeCollaboratorBindingModel model)
        {
            return authorizationHandler.HandleRevokedPermissionBindingModelAsync(model, this);
        }

        /// <summary>
        /// Returns the collaborators associated with the given office id 
        /// </summary>
        /// <param name="officeId">The id of the office</param>
        /// <param name="queryModel">The query model</param>
        /// <returns>The collaborators</returns>
        [ResponseType(typeof(PagedQueryResults<ResourceAuthorization>))]
        [Route("Offices/{officeId}/Collaborators")]
        [ResourceAuthorize(CAM.Data.Permission.OFFICE_OWNER_VALUE, CAM.Data.ResourceType.OFFICE_VALUE, "officeId")]
        public async Task<IHttpActionResult> GetCollaboratorsAsync([FromUri]int officeId, [FromUri]PagingQueryBindingModel<ResourceAuthorization> queryModel)
        {
            if (ModelState.IsValid)
            {
                var authorizations = await GetResourceAuthorizationsAsync(GetQueryableOperator(officeId, queryModel));
                return Ok(authorizations);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        private QueryableOperator<ResourceAuthorization> GetQueryableOperator(int officeId, PagingQueryBindingModel<ResourceAuthorization> queryModel)
        {
            var queryOperator = queryModel.ToQueryableOperator(DEFAULT_RESOURCE_AUTHORIZATION_SORTER);
            queryOperator.Filters.Add(new ExpressionFilter<ResourceAuthorization>(x => x.ForeignResourceId, ComparisonType.Equal, officeId));
            queryOperator.Filters.Add(new ExpressionFilter<ResourceAuthorization>(x => x.ResourceTypeId, ComparisonType.Equal, ResourceType.Office.Id));
            return queryOperator;
        }

        private Task<PagedQueryResults<ResourceAuthorization>> GetResourceAuthorizationsAsync(QueryableOperator<ResourceAuthorization> queryOperator)
        {
            return resourceService.GetResourceAuthorizationsAsync(queryOperator);
        }
    }
}
