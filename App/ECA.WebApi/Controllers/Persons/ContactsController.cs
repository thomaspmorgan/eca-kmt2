using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.Data;
using ECA.WebApi.Models.Admin;
using ECA.WebApi.Models.Person;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

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
        private IEmailAddressHandler emailAddressHandler;
        private IPhoneNumberHandler phoneNumberHandler;

        /// <summary>
        /// Creates a new ContactsController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="userProvider">The user provider.</param>
        /// <param name="phoneNumberHandler">The phone number handler.</param>
        /// <param name="emailAddressHandler">The Email Address handler.</param>
        public ContactsController(IContactService service, 
            IUserProvider userProvider,
            IPhoneNumberHandler phoneNumberHandler,
            IEmailAddressHandler emailAddressHandler)
        {
            Contract.Requires(service != null, "The contact service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            Contract.Requires(emailAddressHandler != null, "The email address handler must not be null.");
            Contract.Requires(phoneNumberHandler != null, "The phone number handler must not be null.");
            this.service = service;
            this.userProvider = userProvider;
            this.emailAddressHandler = emailAddressHandler;
            this.phoneNumberHandler = phoneNumberHandler;
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

        /// <summary>
        /// Delete a contact from the datastore.
        /// </summary>
        /// <param name="id">The id of the contact to be deleted</param>
        /// <returns></returns>
        public Task<IHttpActionResult> DeleteContactAsync(int id)
        {
            return DoDeleteAsync(id);
        }

        private async Task<IHttpActionResult> DoDeleteAsync(int contactId)
        {
            var currentUser = this.userProvider.GetCurrentUser();
            var businessUser = this.userProvider.GetBusinessUser(currentUser);
            await service.DeletePointOfContactAsync(contactId);
            await service.SaveChangesAsync();
            return Ok();
        }


        #region emails
        /// <summary>
        /// Adds a new email address to the person.
        /// </summary>
        /// <param name="model">The new email address.</param>
        /// <param name="contactId">The id of the person.</param>
        /// <returns>The saved email address.</returns>
        [Route("Contact/{contactId:int}/EmailAddress")]
        [ResponseType(typeof(EmailAddressDTO))]
        public Task<IHttpActionResult> PostEmailAddressAsync(int contactId, [FromBody]ContactEmailAddressBindingModel model)
        {
            return emailAddressHandler.HandleEmailAddressAsync<Contact>(model, this);
        }

        /// <summary>
        /// Updates the new email address of the person.
        /// </summary>
        /// <param name="model">The new email address.</param>
        /// <param name="contactId">The person id.</param>
        /// <returns>The saved email address.</returns>
        [Route("Contact/{contactId:int}/EmailAddress")]
        [ResponseType(typeof(EmailAddressDTO))]
        public Task<IHttpActionResult> PutUpdateEmailAddressAsync(int contactId, [FromBody]UpdatedEmailAddressBindingModel model)
        {
            return emailAddressHandler.HandleUpdateEmailAddressAsync(model, this);
        }

        /// <summary>
        /// Deletes the email address presence from the person.
        /// </summary>
        /// <param name="emailAddressId">The emailAddressId id.</param>
        /// <param name="contactId">The person id.</param>
        /// <returns>An ok result.</returns>
        [Route("Contact/{contactId:int}/EmailAddress/{emailAddressId:int}")]
        [ResponseType(typeof(OkResult))]
        public Task<IHttpActionResult> DeleteEmailAddressAsync(int contactId, int emailAddressId)
        {
            return emailAddressHandler.HandleDeleteEmailAddressAsync(emailAddressId, this);
        }
        #endregion

        #region Phone Numbers

        /// <summary>
        /// Adds a new phone number to the contact.
        /// </summary>
        /// <param name="contactId">The id of the person.</param>
        /// <param name="model">The new phone number.</param>
        /// <returns>The saved phone number.</returns>
        [Route("Contact/{contactId:int}/PhoneNumber")]
        [ResponseType(typeof(PhoneNumberDTO))]
        public Task<IHttpActionResult> PostPhoneNumberAsync(int contactId, [FromBody]ContactPhoneNumberBindingModel model)
        {
            return phoneNumberHandler.HandlePhoneNumberAsync<Contact>(model, this);
        }
        /// <summary>
        /// Updates the phone number of the contact.
        /// </summary>
        /// <param name="model">The new phone number.</param>
        /// <returns>The saved phone number.</returns>
        [Route("Contact/{contactId:int}/PhoneNumber")]
        [ResponseType(typeof(PhoneNumberDTO))]
        public Task<IHttpActionResult> PutUpdatePhoneNumberAsync([FromBody]UpdatedPhoneNumberBindingModel model)
        {
            return phoneNumberHandler.HandleUpdatePhoneNumberAsync(model, this);
        }
        /// <summary>
        /// Deletes the phone number from the phoneNumberable.
        /// </summary>
        /// <param name="phoneNumberId">The phoneNumber id.</param>
        /// <returns>An ok result.</returns>
        [Route("Contact/{contactId:int}/PhoneNumber/{phoneNumberId:int}")]
        [ResponseType(typeof(OkResult))]
        public Task<IHttpActionResult> DeletePhoneNumberAsync(int phoneNumberId)
        {
            return phoneNumberHandler.HandleDeletePhoneNumberAsync(phoneNumberId, this);
        }
        #endregion
    }
}
