using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using System;
using System.Collections.Generic;
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
    /// The LanguagesController provides clients with the languages in the eca system.
    /// </summary>
    [Authorize]
    public class LanguagesController : ApiController
    {
        private static readonly ExpressionSorter<LanguageDTO> DEFAULT_SORTER = new ExpressionSorter<LanguageDTO>(x => x.Name, SortDirection.Ascending);

        private readonly ILanguageService service;

        /// <summary>
        /// Creates a new ProminentCategoryController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        public LanguagesController(ILanguageService service)
        {
            Contract.Requires(service != null, "The service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Returns the languages in the system.
        /// </summary>
        /// <param name="queryModel">The query model.</param>
        /// <returns>LanguageDTO</returns>
        [ResponseType(typeof(PagedQueryResults<LanguageDTO>))]
        public async Task<IHttpActionResult> GetAsync([FromUri]PagingQueryBindingModel<LanguageDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var dtos = await service.GetAsync(queryModel.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(dtos);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
