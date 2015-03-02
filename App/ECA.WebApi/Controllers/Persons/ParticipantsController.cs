using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
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

namespace ECA.WebApi.Controllers.Persons
{
    public class ParticipantsController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of participants.
        /// </summary>
        private static readonly ExpressionSorter<SimpleParticipantDTO> DEFAULT_SORTER = new ExpressionSorter<SimpleParticipantDTO>(x => x.Name, SortDirection.Ascending);

        private IParticipantService service;

        /// <summary>
        /// Creates a new ContactsController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        public ParticipantsController(IParticipantService service)
        {
            Debug.Assert(service != null, "The program service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Retrieves a listing of the paged, sorted, and filtered list of contacts.
        /// </summary>
        /// <param name="queryModel">The paging, filtering, and sorting model.</param>
        /// <returns>The list of contacts.</returns>
        [ResponseType(typeof(PagedQueryResults<SimpleParticipantDTO>))]
        public async Task<IHttpActionResult> GetParticipantsAsync([FromUri]PagingQueryBindingModel queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetParticipantsAsync(queryModel.ToQueryableOperator<SimpleParticipantDTO>(DEFAULT_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
