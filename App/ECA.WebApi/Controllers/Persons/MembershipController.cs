using ECA.Business.Service.Persons;
using ECA.Business.Queries.Models.Persons;
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

namespace ECA.WebApi.Controllers.Persons
{
    /// <summary>
    /// The ProminentCategoryController provides clients with the Prominent Categories in the eca system.
    /// </summary>
    [Authorize]
    public class MembershipController : ApiController
    {
        private static readonly ExpressionSorter<MembershipDTO> DEFAULT_SORTER = new ExpressionSorter<MembershipDTO>(x => x.Name, SortDirection.Ascending);

        private readonly IMembershipService service;

        /// <summary>
        /// Creates a new ProminentCategoryController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        public MembershipController(IMembershipService service)
        {
            Contract.Requires(service != null, "The service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Returns the Memberships in the system.
        /// </summary>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The Memberships.</returns>
        [ResponseType(typeof(PagedQueryResults<MembershipDTO>))]
        public async Task<IHttpActionResult> GetAsync([FromUri]PagingQueryBindingModel<MembershipDTO> queryModel)
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
