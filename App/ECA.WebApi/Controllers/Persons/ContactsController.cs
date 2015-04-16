using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
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

namespace ECA.WebApi.Controllers.Persons
{
    /// <summary>
    /// The ContactsController is used for crud operations on points of contact in the eca system.
    /// </summary>
    [Authorize]
    public class ContactsController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of contacts.
        /// </summary>
        private static readonly ExpressionSorter<ContactDTO> DEFAULT_SORTER = new ExpressionSorter<ContactDTO>(x => x.FullName, SortDirection.Ascending);

        private IContactService service;

        /// <summary>
        /// Creates a new ContactsController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        public ContactsController(IContactService service)
        {
            Contract.Requires(service != null, "The contact service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Retrieves a listing of the paged, sorted, and filtered list of contacts.
        /// </summary>
        /// <param name="queryModel">The paging, filtering, and sorting model.</param>
        /// <returns>The list of contacts.</returns>
        [ResponseType(typeof(PagedQueryResults<ContactDTO>))]
        public async Task<IHttpActionResult> GetContactsAsync([FromUri]PagingQueryBindingModel<ContactDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetContactsAsync(queryModel.ToQueryableOperator(DEFAULT_SORTER, x => x.FullName, x => x.Position));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
