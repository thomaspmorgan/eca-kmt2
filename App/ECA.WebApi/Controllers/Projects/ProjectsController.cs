using CAM.Business.Model;
using CAM.Business.Queries.Models;
using CAM.Business.Service;
using CAM.Data;
using ECA.Business.Queries.Models.Admin;
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

namespace ECA.WebApi.Controllers.Projects
{
    /// <summary>
    /// The ProjectsController is used for managing projects in the ECA system.
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class ProjectsController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of projects.
        /// </summary>
        private static readonly ExpressionSorter<SimpleProjectDTO> DEFAULT_SIMPLE_PROJECT_DTO_SORTER = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectName, SortDirection.Ascending);

        /// <summary>
        /// The default sorter to resource authorizations.
        /// </summary>
        private static readonly ExpressionSorter<ResourceAuthorization> DEFAULT_RESOURCE_AUTHORIZATION_SORTER = new ExpressionSorter<ResourceAuthorization>(x => x.DisplayName, SortDirection.Ascending);

        private IProjectService projectService;
        private IResourceAuthorizationHandler authorizationHandler;
        private IUserProvider userProvider;
        private IResourceService resourceService;

        /// <summary>
        /// Creates a new ProjectsController with the given project service.
        /// </summary>
        /// <param name="projectService">The project service.</param>
        /// <param name="userProvider">The user provider.</param>
        /// <param name="authorizationHandler">The authorization handler;</param>
        public ProjectsController(IProjectService projectService, IResourceAuthorizationHandler authorizationHandler, IUserProvider userProvider, IResourceService resourceService)
        {
            Contract.Requires(projectService != null, "The project service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            Contract.Requires(authorizationHandler != null, "The authorization handler must not be null.");
            Contract.Requires(resourceService != null, "The resource service must not be null.");
            this.projectService = projectService;
            this.resourceService = resourceService;
            this.authorizationHandler = authorizationHandler;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Returns a listing of the projects by program.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="queryModel">The page, filter and sort information.</param>
        /// <returns>The list of projects by program.</returns>
        [ResponseType(typeof(PagedQueryResults<SimpleProjectDTO>))]
        [Route("Programs/{programId:int}/Projects")]
        public async Task<IHttpActionResult> GetProjectsByProgramAsync(int programId, [FromUri]PagingQueryBindingModel<SimpleProjectDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.projectService.GetProjectsByProgramIdAsync(programId, queryModel.ToQueryableOperator(DEFAULT_SIMPLE_PROJECT_DTO_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Returns a project by id
        /// </summary>
        /// <param name="id">The project id to fetch</param>
        /// <returns>Project</returns>
        [ResponseType(typeof(ProjectDTO))]
        [ResourceAuthorize(CAM.Data.Permission.VIEW_PROJECT_VALUE, CAM.Data.ResourceType.PROJECT_VALUE)]
        public async Task<IHttpActionResult> GetProjectByIdAsync(int id)
        {
            var project = await this.projectService.GetProjectByIdAsync(id);
            if (project != null)
            {
                return Ok(project);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Creates and return a new draft project 
        /// </summary>
        /// <param name="model">The new project to create</param>
        /// <returns>The created project</returns>
        [ResponseType(typeof(ProjectDTO))]
        public async Task<IHttpActionResult> PostProjectAsync(DraftProjectBindingModel model)
        {
            if(ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var project = await projectService.CreateAsync(model.ToDraftProject(businessUser));
                await projectService.SaveChangesAsync();
                var dto = await projectService.GetProjectByIdAsync(project.ProjectId);
                return Ok(dto);
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
        [ResponseType(typeof(ProjectDTO))]
        public async Task<IHttpActionResult> PutProjectAsync(PublishedProjectBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                await projectService.UpdateAsync(model.ToPublishedProject(businessUser));
                await projectService.SaveChangesAsync();
                var dto = await projectService.GetProjectByIdAsync(model.Id);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Adds a collaborator to a project.
        /// </summary>
        /// <param name="model">The add collaborator model.</param>
        /// <returns>An ok result.</returns>
        [Route("Projects/Collaborator/Add")]
        [ResponseType(typeof(OkResult))]
        [ResourceAuthorize(CAM.Data.Permission.PROJECT_OWNER_VALUE, ResourceType.PROJECT_VALUE, typeof(CollaboratorBindingModel), "ProjectId")]
        public Task<IHttpActionResult> PostAddCollaboratorAsync(CollaboratorBindingModel model)
        {
            return authorizationHandler.HandleGrantedPermissionBindingModelAsync(model, this);
        }

        /// <summary>
        /// Remove a collaborator to a project.
        /// </summary>
        /// <param name="model">The remove collaborator model.</param>
        /// <returns>An ok result.</returns>
        [Route("Projects/Collaborator/Remove")]
        [ResponseType(typeof(OkResult))]
        [ResourceAuthorize(CAM.Data.Permission.PROJECT_OWNER_VALUE, ResourceType.PROJECT_VALUE, typeof(CollaboratorBindingModel), "ProjectId")]
        public Task<IHttpActionResult> PostRemoveCollaboratorAsync(CollaboratorBindingModel model)
        {
            return authorizationHandler.HandleDeletedPermissionBindingModelAsync(model, this);
        }

        /// <summary>
        /// Revokes a collaborator permission from a project.
        /// </summary>
        /// <param name="model">The add collaborator model.</param>
        /// <returns>An ok result.</returns>
        [Route("Projects/Collaborator/Revoke")]
        [ResponseType(typeof(OkResult))]
        [ResourceAuthorize(CAM.Data.Permission.PROJECT_OWNER_VALUE, ResourceType.PROJECT_VALUE, typeof(CollaboratorBindingModel), "ProjectId")]
        public Task<IHttpActionResult> PostRevokeCollaboratorAsync(CollaboratorBindingModel model)
        {
            return authorizationHandler.HandleRevokedPermissionBindingModelAsync(model, this);
        }

        /// <summary>
        /// Adds collaborators to a project.
        /// </summary>
        /// <param name="projectId">The id of the project to get collaborators for.</param>
        /// <param name="queryModel">The filtering, paging, and sorting parameters.</param>
        /// <returns>An ok result.</returns>
        [ResponseType(typeof(PagedQueryResults<ResourceAuthorization>))]
        [Route("Projects/{projectId}/Collaborators")]
        [ResourceAuthorize(CAM.Data.Permission.PROJECT_OWNER_VALUE, CAM.Data.ResourceType.PROJECT_VALUE, "projectId")]
        public async Task<IHttpActionResult> GetCollaboratorsAsync([FromUri]int projectId, [FromUri]PagingQueryBindingModel<ResourceAuthorization> queryModel)
        {
            if (ModelState.IsValid)
            {
                var authorizations = await GetResourceAuthorizationsAsync(GetQueryableOperator(projectId, queryModel));
                return Ok(authorizations);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Adds collaborators to a project.
        /// </summary>
        /// <param name="projectId">The id of the project to get collaborators for.</param>
        /// <returns>An ok result.</returns>
        [ResponseType(typeof(ResourceAuthorizationInfoDTO))]
        [Route("Projects/{projectId}/Collaborators/Details")]
        [ResourceAuthorize(CAM.Data.Permission.VIEW_PROJECT_VALUE, CAM.Data.ResourceType.PROJECT_VALUE, "projectId")]
        public async Task<IHttpActionResult> GetCollaboratorDetailsAsync([FromUri]int projectId)
        {
            var info = await resourceService.GetResourceAuthorizationInfoDTOAsync(ResourceType.Project.Value, projectId);
            if (info != null)
            {
                return Ok(info);
            }
            else
            {
                return NotFound();
            }
        }

        private QueryableOperator<ResourceAuthorization> GetQueryableOperator(int projectId, PagingQueryBindingModel<ResourceAuthorization> queryModel)
        {
            var queryOperator = queryModel.ToQueryableOperator(DEFAULT_RESOURCE_AUTHORIZATION_SORTER);
            queryOperator.Filters.Add(new ExpressionFilter<ResourceAuthorization>(x => x.ForeignResourceId, ComparisonType.Equal, projectId));
            queryOperator.Filters.Add(new ExpressionFilter<ResourceAuthorization>(x => x.ResourceTypeId, ComparisonType.Equal, ResourceType.Project.Id));
            return queryOperator;
        }

        private Task<PagedQueryResults<ResourceAuthorization>> GetResourceAuthorizationsAsync(QueryableOperator<ResourceAuthorization> queryOperator)
        {
            return resourceService.GetResourceAuthorizationsAsync(queryOperator);
        }
    }
}