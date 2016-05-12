using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Person;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Persons
{
    /// <summary>
    /// The ContactsController is used for crud operations on points of contact in the eca system.
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class ContactsController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of contacts.
        /// </summary>
        private static readonly ExpressionSorter<ContactDTO> DEFAULT_SORTER = new ExpressionSorter<ContactDTO>(x => x.FullName, SortDirection.Ascending);

        private IContactService service;
        private IUserProvider userProvider;

        /// <summary>
        /// Creates a new ContactsController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="userProvider">The user provider.</param>
        public ContactsController(IContactService service, IUserProvider userProvider)
        {
            Contract.Requires(service != null, "The contact service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            this.service = service;
            this.userProvider = userProvider;
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

        /// <summary>
        /// Adds a new contact to the system.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The saved contact.</returns>
        public async Task<IHttpActionResult> PostCreateContactAsync(AdditionalPointOfContactBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var instance = model.ToAdditionalPointOfContact(businessUser);
                var contact = await service.CreateAsync(instance);
                await service.SaveChangesAsync();
                var dto = await service.GetContactByIdAsync(contact.ContactId);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Updates a contact in the system.
        /// </summary>
        /// <param name="model">The updated contact</param>
        /// <returns></returns>
        [ResponseType(typeof(ContactDTO))]
        public async Task<IHttpActionResult> PutContactAsync(UpdatedPointOfContactBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var instance = model.ToUpdatePointOfContact(businessUser);
                var contact = await service.UpdateContactAsync(instance);
                await service.SaveChangesAsync();
                var dto = await service.GetContactByIdAsync(contact.Id);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        
    }
}
