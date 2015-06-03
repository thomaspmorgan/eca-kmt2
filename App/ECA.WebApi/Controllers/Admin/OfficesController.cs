using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Office;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
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

        private IOfficeService service;
        private IFocusCategoryService focusCategoryService;
        private IJustificationObjectiveService justificationObjectiveService;

        /// <summary>
        /// Creates a new controller instance.
        /// </summary>
        /// <param name="service">The service.</param>
        public OfficesController(IOfficeService service, IFocusCategoryService focusCategoryService, IJustificationObjectiveService justificationObjectiveService)
        {
            Contract.Requires(service != null, "The office service must not be null.");
            Contract.Requires(focusCategoryService != null, "The focus category service must not be null.");
            Contract.Requires(justificationObjectiveService != null, "The justification service must not be null.");
            this.service = service;
            this.focusCategoryService = focusCategoryService;
            this.justificationObjectiveService = justificationObjectiveService;
        }

        /// <summary>
        /// Returns the office with the given id.
        /// </summary>
        /// <param name="id">The id of the office.</param>
        /// <returns>The office.</returns>
        [ResponseType(typeof(OfficeDTO))]
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
        public async Task<IHttpActionResult> GetOfficesAsync([FromUri]PagingQueryBindingModel<SimpleOfficeDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await service.GetOfficesAsync(
                    queryModel.ToQueryableOperator(
                        DEFAULT_OFFICE_SORTER,
                        x => x.Name,
                        x => x.Description));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
