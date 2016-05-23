using System.Data.Entity;
using ECA.Business.Queries.Persons;
using ECA.Business.Validation;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using ECA.Core.Exceptions;
using ECA.Business.Queries.Models.Persons;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// The ContactService is capable of performing operations on contacts against a DbContext.
    /// </summary>
    public class ContactService : DbContextService<EcaContext>, IContactService
    {
        /// <summary>
        /// Contact not found error
        /// </summary>
        public const string CONTACT_NOT_FOUND_ERROR = "The contact could not be found.";
        
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IBusinessValidator<AdditionalPointOfContactValidationEntity, object> pointOfContactValidator;
        /// <summary>
        /// Creates a new ContactService with the given context to operate against.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to operate against.</param>
        public ContactService(EcaContext context, IBusinessValidator<AdditionalPointOfContactValidationEntity, object> pointOfContactValidator, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(pointOfContactValidator != null, "The point of contact validator must not be null.");
            this.pointOfContactValidator = pointOfContactValidator;
        }

        #region Get

        /// <summary>
        /// Returns the sorted, filtered, and paged contact DTOs in the eca system.
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<ContactDTO> GetContacts(QueryableOperator<ContactDTO> queryOperator)
        {
            var contacts = ContactQueries.CreateContactQuery(this.Context, queryOperator)
                .ToPagedQueryResults<ContactDTO>(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved contacts by query operator [{0}].", queryOperator);

            return contacts;
        }

        /// <summary>
        /// Returns the sorted, filtered, and paged contact DTOs in the eca system.
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public async Task<PagedQueryResults<ContactDTO>> GetContactsAsync(QueryableOperator<ContactDTO> queryOperator)
        {
            var contacts = await ContactQueries.CreateContactQuery(this.Context, queryOperator)
                .ToPagedQueryResultsAsync<ContactDTO>(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved contacts by query operator [{0}].", queryOperator);

            return contacts;
        }

        /// <summary>
        /// Returns the contact with the given id.
        /// </summary>
        /// <param name="contactId">The id of the contact.</param>
        /// <returns>The contact, or null, if it does not exist.</returns>
        public ContactDTO GetContactById(int contactId)
        {
            return CreateGetContactByIdQuery(contactId).FirstOrDefault();
        }

        /// <summary>
        /// Returns the contact with the given id.
        /// </summary>
        /// <param name="contactId">The id of the contact.</param>
        /// <returns>The contact, or null, if it does not exist.</returns>
        public Task<ContactDTO> GetContactByIdAsync(int contactId)
        {
            return CreateGetContactByIdQuery(contactId).FirstOrDefaultAsync();
        }

        private IQueryable<ContactDTO> CreateGetContactByIdQuery(int contactId)
        {
            return ContactQueries.CreateContactQuery(this.Context).Where(c => c.Id == contactId);
        }

        /// <summary>
        /// Returns the contact DTO with the given id
        /// </summary>
        /// <param name="contactId">The id of the contact</param>
        /// <returns>The contact, or null, if it does not exist</returns>
        public ContactDTO GetContactDTOById(int contactId)
        {
            return CreateGetContactDTOByIdQuery(contactId).FirstOrDefault();
        }

        /// <summary>
        /// Returns the contact DTO with the given id
        /// </summary>
        /// <param name="contactId">The id of the contact</param>
        /// <returns>The contact, or null, if it does not exist</returns>
        public Task<ContactDTO> GetContactDTOByIdAsync(int contactId)
        {
            return CreateGetContactDTOByIdQuery(contactId).FirstOrDefaultAsync();
        }

        private IQueryable<ContactDTO> CreateGetContactDTOByIdQuery(int contactId)
        {
            return ContactQueries.CreateContactQuery(this.Context).Where(c => c.Id == contactId);
        }

        #endregion

        #region Create

        private IQueryable<EmailAddress> CreateGetLikeEmailAddressesQuery(IEnumerable<string> emailAddresses)
        {
            return this.Context.EmailAddresses
                .Where(x => x.ContactId.HasValue)
                .Where(x => emailAddresses.Select(e => e.Trim().ToLower()).Contains(x.Address.ToLower().Trim()));
        }

        /// <summary>
        /// Adds a new point of the contact to the datastore.
        /// </summary>
        /// <param name="pointOfContact">The point of contact.</param>
        /// <returns>The Contact that was added to the context.</returns>
        public Contact Create(AdditionalPointOfContact pointOfContact)
        {
            return DoCreate(pointOfContact);
        }

        /// <summary>
        /// Adds a new point of the contact to the datastore.
        /// </summary>
        /// <param name="pointOfContact">The point of contact.</param>
        /// <returns>The Contact that was added to the context.</returns>
        public async Task<Contact> CreateAsync(AdditionalPointOfContact pointOfContact)
        {
            return DoCreate(pointOfContact);
        }
        
        private Contact DoCreate(AdditionalPointOfContact pointOfContact)
        {
            var validationEntity = GetAdditionalPointOfContactValidationEntity(pointOfContact);
            pointOfContactValidator.ValidateCreate(validationEntity);

            var contact = new Contact();
            Contract.Assert(!String.IsNullOrWhiteSpace(pointOfContact.FullName), "The point of contact must have a full name value.");
            contact.FullName = pointOfContact.FullName.Trim();
            if(!string.IsNullOrWhiteSpace(pointOfContact.Position))
            {
                contact.Position = pointOfContact.Position.Trim();
            }

            foreach(var email in pointOfContact.EmailAddresses.Where(x => !String.IsNullOrWhiteSpace(x.Address)))
            {
                var newEmail = new EmailAddress
                {
                    EmailAddressTypeId = email.EmailAddressTypeId,
                    Address = email.Address.Trim(),
                    IsPrimary = email.IsPrimary
                };
                pointOfContact.Audit.SetHistory(newEmail);
                contact.EmailAddresses.Add(newEmail);
                Context.EmailAddresses.Add(newEmail);
            }
            foreach(var phoneNumber in pointOfContact.PhoneNumbers.Where(x => !String.IsNullOrWhiteSpace(x.Number)))
            {
                var newPhoneNumber = new PhoneNumber
                {
                    Number = phoneNumber.Number.Trim(),
                    Extension = phoneNumber.Extension.Trim(),
                    PhoneNumberTypeId = phoneNumber.PhoneNumberTypeId,
                    IsPrimary = phoneNumber.IsPrimary
                };
                pointOfContact.Audit.SetHistory(newPhoneNumber);
                contact.PhoneNumbers.Add(newPhoneNumber);
                Context.PhoneNumbers.Add(newPhoneNumber);
            }

            pointOfContact.Audit.SetHistory(contact);
            this.Context.Contacts.Add(contact);
            return contact;
        }
        #endregion

        #region update

        /// <summary>
        /// Updates a point of the contact in the datastore.
        /// </summary>
        /// <param name="updatedPointOfContact"></param>
        /// <returns></returns>
        public async Task<Contact> UpdateContactAsync(UpdatedPointOfContact updatedPointOfContact)
        {
            var contactToUpdate = await Context.Contacts.FindAsync(updatedPointOfContact.Id);
            var emails = await Context.EmailAddresses.Where(x => x.ContactId == updatedPointOfContact.Id).ToListAsync();
            var phones = await Context.PhoneNumbers.Where(x => x.ContactId == updatedPointOfContact.Id).ToListAsync();

            DoPointOfContactUpdate(updatedPointOfContact, contactToUpdate, emails, phones);

            return contactToUpdate;
        }

        public void DoPointOfContactUpdate(UpdatedPointOfContact updatedPointOfContact, Contact contactToUpdate,
            List<EmailAddress> emails, List<PhoneNumber> phones)
        {
            contactToUpdate.FullName = updatedPointOfContact.FullName;
            contactToUpdate.Position = updatedPointOfContact.Position;
            contactToUpdate.EmailAddresses = emails;
            contactToUpdate.PhoneNumbers = phones;
            updatedPointOfContact.Audit.SetHistory(contactToUpdate);
        }

        private AdditionalPointOfContactValidationEntity GetAdditionalPointOfContactValidationEntity(AdditionalPointOfContact pointOfContact)
        {
            var numberOfPrimaryEmailAddresses = pointOfContact.EmailAddresses.Where(x => x.IsPrimary).Count();
            var numberOfPrimaryPhoneNumbers = pointOfContact.PhoneNumbers.Where(x => x.IsPrimary).Count();
            return new AdditionalPointOfContactValidationEntity(
                fullName: pointOfContact.FullName,
                position: pointOfContact.Position,
                numberOfPrimaryEmailAddresses: numberOfPrimaryEmailAddresses,
                numberOfPrimaryPhoneNumbers: numberOfPrimaryPhoneNumbers);
        }

        #endregion

        #region delete
        /// <summary>
        /// Delete a point of contact from the datastore.
        /// </summary>
        /// <param name="id">contact id</param>
        /// <returns></returns>
        public async Task DeletePointOfContactAsync(int id)
        {
            var contact = await Context.Contacts.FindAsync(id);
            if (contact != null)
            {
                Context.Contacts.Remove(contact);
            }
            else
            {
                throw new ModelNotFoundException(CONTACT_NOT_FOUND_ERROR);
            }
        }

        #endregion


    }
}
