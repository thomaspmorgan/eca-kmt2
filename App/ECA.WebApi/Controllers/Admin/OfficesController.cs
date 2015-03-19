﻿using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Office;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class OfficesController : ApiController
    {
        private static readonly ExpressionSorter<OrganizationProgramDTO> DEFAULT_ORGANIZATION_PROGRAM_SORTER =
            new ExpressionSorter<OrganizationProgramDTO>(x => x.Name, SortDirection.Ascending);

        private IOfficeService service;

        /// <summary>
        /// Creates a new controller instance.
        /// </summary>
        /// <param name="service">The service.</param>
        public OfficesController(IOfficeService service)
        {
            Debug.Assert(service != null, "The office service must not be null.");
            this.service = service;
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
                var results = await service.GetProgramsAsync(id, queryModel.ToQueryableOperator(DEFAULT_ORGANIZATION_PROGRAM_SORTER, x => x.Name, x => x.Description));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        ///
        [ResponseType(typeof(List<SimpleOfficeDTO>))]
        public async Task<IHttpActionResult> GetOffices()
        {
            if (ModelState.IsValid)
            {
                var results = this.service.GetOffices();
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}