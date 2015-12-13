using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Admin
{
    /// <summary>
    /// The VisitorTypesController provides clients with the visitor types in the eca system.
    /// </summary>
    [Authorize]
    public class VisitorTypesController : ApiController
    {
        private static readonly ExpressionSorter<SimpleLookupDTO> DEFAULT_SORTER = new ExpressionSorter<SimpleLookupDTO>(x => x.Id, SortDirection.Ascending);

        private readonly IVisitorTypeService service;

        /// <summary>
        /// Creates a new VisitorTypesController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        public VisitorTypesController(IVisitorTypeService service)
        {
            Contract.Requires(service != null, "The service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Returns the visitor types in the system.
        /// </summary>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The visitor types.</returns>
        [ResponseType(typeof(PagedQueryResults<SimpleLookupDTO>))]
        public async Task<IHttpActionResult> GetAsync([FromUri]PagingQueryBindingModel<SimpleLookupDTO> queryModel)
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
