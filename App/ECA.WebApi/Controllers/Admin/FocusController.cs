﻿using ECA.Business.Queries.Models.Admin;
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
    /// The Focus Controller provides lookup and crud operations for a focus.
    /// </summary>
    [Authorize]
    public class FocusController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of foci.
        /// </summary>
        private static readonly ExpressionSorter<FocusDTO> DEFAULT_FOCUS_DTO_SORTER = new ExpressionSorter<FocusDTO>(x => x.Name, SortDirection.Ascending);

        private IFocusService service;

        /// <summary>
        /// Creates a new FocusController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        public FocusController(IFocusService service)
        {
            Contract.Requires(service != null, "The focus service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Returns a listing of the foci..
        /// </summary>
        /// <param name="queryModel">The page, filter and sort information.</param>
        /// <returns>The list of foci.</returns>
        [ResponseType(typeof(PagedQueryResults<FocusDTO>))]
        public async Task<IHttpActionResult> GetFociAsync([FromUri]PagingQueryBindingModel<FocusDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetAsync(queryModel.ToQueryableOperator(DEFAULT_FOCUS_DTO_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
