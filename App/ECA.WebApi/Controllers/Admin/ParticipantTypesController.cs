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
    /// The ParticipantTypesController provides clients with the participant types in the eca system.
    /// </summary>
    [Authorize]
    public class ParticipantTypesController : ApiController
    {
        private static readonly ExpressionSorter<ParticipantTypeDTO> DEFAULT_SORTER = new ExpressionSorter<ParticipantTypeDTO>(x => x.Name, SortDirection.Ascending);

        private readonly IParticipantTypeService service;

        /// <summary>
        /// Creates a new ParticipantTypesController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        public ParticipantTypesController(IParticipantTypeService service)
        {
            Contract.Requires(service != null, "The service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Returns the participant types in the system.
        /// </summary>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The participant types.</returns>
        [ResponseType(typeof(PagedQueryResults<ParticipantTypeDTO>))]
        public async Task<IHttpActionResult> GetAsync([FromUri]PagingQueryBindingModel<ParticipantTypeDTO> queryModel)
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
