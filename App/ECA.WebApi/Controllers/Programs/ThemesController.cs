﻿using ECA.Business.Queries.Models.Programs;
using ECA.Business.Service.Programs;
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

namespace ECA.WebApi.Controllers.Programs
{
    /// <summary>
    /// The ThemeseController handles returning program themes.
    /// </summary>
    public class ThemesController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of themes.
        /// </summary>
        private static readonly ExpressionSorter<ThemeDTO> DEFAULT_SORTER = new ExpressionSorter<ThemeDTO>(x => x.Name, SortDirection.Ascending);

        private IThemeService service;

        /// <summary>
        /// Creates a new ThemesController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        public ThemesController(IThemeService service)
        {
            Debug.Assert(service != null, "The program service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Retrieves a listing of the paged, sorted, and filtered list of programs.
        /// </summary>
        /// <param name="queryModel">The paging, filtering, and sorting model.</param>
        /// <returns>The list of programs.</returns>
        [ResponseType(typeof(PagedQueryResults<ThemeDTO>))]
        public async Task<IHttpActionResult> GetThemesAsync(PagingQueryBindingModel queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetThemesAsync(queryModel.ToQueryableOperator<ThemeDTO>(DEFAULT_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
