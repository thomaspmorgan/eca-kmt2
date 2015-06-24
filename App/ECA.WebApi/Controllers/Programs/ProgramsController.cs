using AutoMapper;
using ECA.Business.Models.Programs;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Common;
using ECA.WebApi.Models.Programs;
using ECA.WebApi.Models.Query;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Diagnostics.Contracts;
using ECA.WebApi.Security;
using ECA.Business.Service.Admin;
using CAM.Business.Service;
using CAM.Business.Model;
using ECA.Core.DynamicLinq.Filter;
using CAM.Data;

namespace ECA.WebApi.Controllers.Programs
{
    /// <summary>
    /// The ProgramsController is capable of handling program requests from a client.
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class ProgramsController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of programs.
        /// </summary>
        private static readonly ExpressionSorter<SimpleProgramDTO> ALPHA_PROGRAM_SORTER = new ExpressionSorter<SimpleProgramDTO>(x => x.Name, SortDirection.Ascending);

        private static readonly ExpressionSorter<OrganizationProgramDTO> HIERARCHY_PROGRAM_SORTER = new ExpressionSorter<OrganizationProgramDTO>(x => x.SortOrder, SortDirection.Ascending);

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


        private IProgramService programService;
        private IUserProvider userProvider;
        private IFocusCategoryService categoryService;
        private IJustificationObjectiveService justificationObjectiveService;
        private IResourceService resourceService;

        /// <summary>
        /// Creates a new ProgramController with the given program service.
        /// </summary>
        /// <param name="programService">The program service.</param>
        /// <param name="userProvider">The user provider.</param>
        /// <param name="categoryService">The focus category service.</param>
        /// <param name="justificationObjectiveService">The justification objective service.</param>
        public ProgramsController(IProgramService programService, IUserProvider userProvider, IFocusCategoryService categoryService, IJustificationObjectiveService justificationObjectiveService, IResourceService resourceService)
        {
            Contract.Requires(programService != null, "The program service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            Contract.Requires(categoryService != null, "The category service must not be null.");
            Contract.Requires(justificationObjectiveService != null, "The justification service must not be null.");
            Contract.Requires(resourceService != null, "The resource service must not be null.");
            this.programService = programService;
            this.userProvider = userProvider;
            this.categoryService = categoryService;
            this.justificationObjectiveService = justificationObjectiveService;
            this.resourceService = resourceService;
        }

        /// <summary>
        /// Retrieves a listing of the paged, sorted, and filtered list of programs.
        /// </summary>
        /// <param name="queryModel">The paging, filtering, and sorting model.</param>
        /// <returns>The list of programs.</returns>
        [ResponseType(typeof(PagedQueryResults<OrganizationProgramDTO>))]
        [Route("Programs/Alpha")]
        public async Task<IHttpActionResult> GetProgramsAsync([FromUri]PagingQueryBindingModel<OrganizationProgramDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.programService.GetProgramsAsync(
                    queryModel.ToQueryableOperator(
                    ALPHA_PROGRAM_SORTER, 
                    x => x.Name, 
                    x=> x.Description));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [ResponseType(typeof(PagedQueryResults<OrganizationProgramDTO>))]
        [Route("Programs/Hierarchy")]
        public async Task<IHttpActionResult> GetProgramsHierarchyAsync([FromUri]PagingQueryBindingModel<OrganizationProgramDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.programService.GetProgramsHierarchyAsync(
                    queryModel.ToQueryableOperator(
                    HIERARCHY_PROGRAM_SORTER,
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
        /// Returns the program with the given id.
        /// </summary>
        /// <returns>The program with the given id.</returns>
        [ResponseType(typeof(ProgramViewModel))]
        public async Task<IHttpActionResult> GetProgramByIdAsync(int id)
        {
            var program = await this.programService.GetProgramByIdAsync(id);
            if (program != null)
            {
                return Ok(new ProgramViewModel(program));
            }
            else
            {
                return NotFound();
            }            
        }

        /// <summary>
        /// Creates a new draft program and returns the saved program.
        /// </summary>
        /// <param name="model">The new draft program.</param>
        /// <returns>The saved program.</returns>
        [ResponseType(typeof(ProgramDTO))]
        public async Task<IHttpActionResult> PostProgramAsync(DraftProgramBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var program = await programService.CreateAsync(model.ToDraftProgram(businessUser));
                await programService.SaveChangesAsync();
                var dto = await programService.GetProgramByIdAsync(program.ProgramId);
                return Ok(new ProgramViewModel(dto));

            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Updates the system's program with the given program details.
        /// </summary>
        /// <param name="model">The updated program.</param>
        /// <returns>The system's updated program.</returns>
        [ResponseType(typeof(ProgramDTO))]
        public async Task<IHttpActionResult> PutProgramAsync(ProgramBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                await programService.UpdateAsync(model.ToEcaProgram(businessUser));
                await programService.SaveChangesAsync();
                var dto = await programService.GetProgramByIdAsync(model.Id);
                return Ok(new ProgramViewModel(dto));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Returns the child programs of the office with the given id.
        /// </summary>
        /// <param name="programId">The id of the program.</param>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The program categories.</returns>
        [Route("Programs/{programId}/Categories")]
        [ResponseType(typeof(PagedQueryResults<FocusCategoryDTO>))]
        public async Task<IHttpActionResult> GetCategoriesByProgramIdAsync(int programId, [FromUri]PagingQueryBindingModel<FocusCategoryDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await categoryService.GetFocusCategoriesByProgramIdAsync(
                    programId,
                    queryModel.ToQueryableOperator(DEFAULT_FOCUSCATEGORY_DTO_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Returns the objectives of the program with the given id.
        /// </summary>
        /// <param name="programId">The id of the program.</param>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The objectives.</returns>
        [Route("Programs/{programId}/Objectives")]
        [ResponseType(typeof(PagedQueryResults<JustificationObjectiveDTO>))]
        public async Task<IHttpActionResult> GetObjectivesByProgramIdAsync(int programId, [FromUri]PagingQueryBindingModel<JustificationObjectiveDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await justificationObjectiveService.GetJustificationObjectivesByProgramIdAsync(
                    programId,
                    queryModel.ToQueryableOperator(DEFAULT_JUSTIFICATION_OBJECTIVE_DTO_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [ResponseType(typeof(PagedQueryResults<ResourceAuthorization>))]
        [Route("Programs/{programId}/Collaborators")]
        [ResourceAuthorize(CAM.Data.Permission.PROGRAM_OWNER_VALUE, CAM.Data.ResourceType.PROGRAM_VALUE, "programId")]
        public async Task<IHttpActionResult> GetCollaboratorsAsync([FromUri]int programId, [FromUri]PagingQueryBindingModel<ResourceAuthorization> queryModel)
        {
            if (ModelState.IsValid)
            {
                var authorizations = await GetResourceAuthorizationsAsync(GetQueryableOperator(programId, queryModel));
                return Ok(authorizations);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        private QueryableOperator<ResourceAuthorization> GetQueryableOperator(int programId, PagingQueryBindingModel<ResourceAuthorization> queryModel)
        {
            var queryOperator = queryModel.ToQueryableOperator(DEFAULT_RESOURCE_AUTHORIZATION_SORTER);
            queryOperator.Filters.Add(new ExpressionFilter<ResourceAuthorization>(x => x.ForeignResourceId, ComparisonType.Equal, programId));
            queryOperator.Filters.Add(new ExpressionFilter<ResourceAuthorization>(x => x.ResourceTypeId, ComparisonType.Equal, ResourceType.Program.Id));
            return queryOperator;
        }

        private Task<PagedQueryResults<ResourceAuthorization>> GetResourceAuthorizationsAsync(QueryableOperator<ResourceAuthorization> queryOperator)
        {
            return resourceService.GetResourceAuthorizationsAsync(queryOperator);
        }
    }
}