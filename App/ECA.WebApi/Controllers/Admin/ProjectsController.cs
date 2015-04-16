﻿using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Projects;
using ECA.WebApi.Models.Query;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Admin
{
    /// <summary>
    /// The ProjectsController is used for managing projects in the ECA system.
    /// </summary>
    [RoutePrefix("api")]
    //[Authorize]
    public class ProjectsController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of projects.
        /// </summary>
        private static readonly ExpressionSorter<SimpleProjectDTO> DEFAULT_SIMPLE_PROJECT_DTO_SORTER = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectName, SortDirection.Ascending);

        private IProjectService projectService;

        /// <summary>
        /// Creates a new ProjectsController with the given project service.
        /// </summary>
        /// <param name="projectService">The project service.</param>
        public ProjectsController(IProjectService projectService)
        {
            Contract.Requires(projectService != null, "The project service must not be null.");
            this.projectService = projectService;
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
                var userId = 0;
                var project = await projectService.CreateAsync(model.ToDraftProject(userId));
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
                var userId = 0;
                await projectService.UpdateAsync(model.ToPublishedProject(userId));
                await projectService.SaveChangesAsync();
                var dto = await projectService.GetProjectByIdAsync(model.Id);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}