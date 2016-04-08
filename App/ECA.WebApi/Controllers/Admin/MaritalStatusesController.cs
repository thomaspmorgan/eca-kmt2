using ECA.Business.Service.Admin;
using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.WebApi.Models.Query;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;

namespace ECA.WebApi.Controllers.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class MaritalStatusesController : ApiController
    {
        private static readonly ExpressionSorter<SimpleLookupDTO> DEFAULT_MARITAL_STATUS_DTO_SORTER =
           new ExpressionSorter<SimpleLookupDTO>(x => x.Value, SortDirection.Ascending);
        private IMaritalStatusService service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">Service to inject</param>
        public MaritalStatusesController(IMaritalStatusService service)
        {
            Contract.Requires(service != null, "The service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Gets list of marital statuses
        /// </summary>
        /// <param name="queryModel">The queryModel to use</param>
        /// <returns>A list of marital statuses</returns>
        public async Task<IHttpActionResult> GetMaritalStatuses([FromUri]PagingQueryBindingModel<SimpleLookupDTO> queryModel) 
        {
            if (ModelState.IsValid)
            {
                var maritalStatuses = await service.GetAsync(queryModel.ToQueryableOperator(DEFAULT_MARITAL_STATUS_DTO_SORTER));
                return Ok(maritalStatuses);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
