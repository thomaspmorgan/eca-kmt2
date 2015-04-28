using ECA.Business.Queries.Models.Admin;
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
using ECA.Business.Service.Admin;
using System.Diagnostics.Contracts;

namespace ECA.WebApi.Controllers.Admin
{
    /// <summary>
    /// The Focus Controller provides lookup and crud operations for a focus.
    /// </summary>
    [Authorize]
    public class FocusCategoryController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of foci.
        /// </summary>
        private static readonly ExpressionSorter<FocusCategoryDTO> DEFAULT_FOCUSCATEGORY_DTO_SORTER = new ExpressionSorter<FocusCategoryDTO>(x => x.FocusName, SortDirection.Ascending);

        private IFocusCategoryService service;

        /// <summary>
        /// Creates a new FocusController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        public FocusCategoryController(IFocusCategoryService service)
        {
            Contract.Requires(service != null, "The focusCategory service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Returns a listing of the foci..
        /// </summary>
        /// <param name="queryModel">The page, filter and sort information.</param>
        /// <returns>The list of foci.</returns>
        [ResponseType(typeof(PagedQueryResults<FocusCategoryDTO>))]
        public async Task<IHttpActionResult> GetLocationsAsync([FromUri]PagingQueryBindingModel<FocusCategoryDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetAsync(queryModel.ToQueryableOperator(DEFAULT_FOCUSCATEGORY_DTO_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
