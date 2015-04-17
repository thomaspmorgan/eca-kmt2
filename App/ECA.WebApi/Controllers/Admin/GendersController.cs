using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using ECA.WebApi.Models.Query;
using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq;
using ECA.Business.Service.Admin;
using System.Diagnostics.Contracts;

namespace ECA.WebApi.Controllers.Admin
{
    /// <summary>
    /// Controller for genders
    /// </summary>
    //[Authorize]
    public class GendersController: ApiController
    {
        private static readonly ExpressionSorter<SimpleLookupDTO> DEFAULT_GENDER_DTO_SORTER =
           new ExpressionSorter<SimpleLookupDTO>(x => x.Value, SortDirection.Ascending);
        private IGenderService service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">Service to inject</param>
        public GendersController(IGenderService service)
        {
            Contract.Requires(service != null, "The service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Gets list of genders
        /// </summary>
        /// <param name="queryModel">The queryModel to use</param>
        /// <returns>A list of genders</returns>
        public async Task<IHttpActionResult> GetGenders([FromUri]PagingQueryBindingModel<SimpleLookupDTO> queryModel) 
        {
            if (ModelState.IsValid)
            {
                var genders = await service.GetAsync(queryModel.ToQueryableOperator(DEFAULT_GENDER_DTO_SORTER));
                return Ok(genders);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}