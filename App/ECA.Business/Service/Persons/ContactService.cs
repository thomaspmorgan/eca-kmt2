﻿using ECA.Business.Queries.Models.Persons;
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
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// The ContactService is capable of performing operations on contacts against a DbContext.
    /// </summary>
    public class ContactService : DbContextService<EcaContext>, IContactService
    {
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
        /// Returns the sorted, filtered, and paged contacts in the eca system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The sorted, filtered, and paged contacts.</returns>
        public PagedQueryResults<ContactDTO> GetContacts(QueryableOperator<ContactDTO> queryOperator)
        {
            var contacts = ContactQueries.CreateContactDTOQuery(this.Context, queryOperator).ToPagedQueryResults<ContactDTO>(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved contacts by query operator [{0}].", queryOperator);
            
            return contacts;
        }

        /// <summary>
        /// Returns the sorted, filtered, and paged contacts in the eca system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The sorted, filtered, and paged contacts.</returns>
        public async Task<PagedQueryResults<ContactDTO>> GetContactsAsync(QueryableOperator<ContactDTO> queryOperator)
        {
            var contacts = await ContactQueries.CreateContactDTOQuery(this.Context, queryOperator).ToPagedQueryResultsAsync<ContactDTO>(queryOperator.Start, queryOperator.Limit);
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
            return ContactQueries.CreateContactDTOQuery(this.Context).Where(c => c.Id == contactId);
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
            var matchingEmailAddresses = CreateGetLikeEmailAddressesQuery(pointOfContact.EmailAddresses.Select(x => x.Address).ToList()).ToList();
            return DoCreate(pointOfContact, matchingEmailAddresses);
        }

        /// <summary>
        /// Adds a new point of the contact to the datastore.
        /// </summary>
        /// <param name="pointOfContact">The point of contact.</param>
        /// <returns>The Contact that was added to the context.</returns>
        public async Task<Contact> CreateAsync(AdditionalPointOfContact pointOfContact)
        {
            var matchingEmailAddresses = await CreateGetLikeEmailAddressesQuery(pointOfContact.EmailAddresses.Select(x => x.Address).ToList()).ToListAsync();
            return DoCreate(pointOfContact, matchingEmailAddresses);
        }

        private Contact DoCreate(AdditionalPointOfContact pointOfContact, IEnumerable<EmailAddress> matchingEmailAddresses)
        {
            var validationEntity = GetAdditionalPointOfContactValidationEntity(pointOfContact, matchingEmailAddresses.Count());
            pointOfContactValidator.ValidateCreate(validationEntity);

            var contact = new Contact();
            //full name and position should have a value as caught by validator
            contact.FullName = pointOfContact.FullName.Trim();
            contact.Position = pointOfContact.Position.Trim();

            foreach(var email in pointOfContact.EmailAddresses.Where(x => !String.IsNullOrWhiteSpace(x.Address)))
            {
                var newEmail = new EmailAddress
                {
                    EmailAddressTypeId = email.EmailAddressTypeId,
                    Address = email.Address.Trim(),
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
                    PhoneNumberTypeId = phoneNumber.PhoneNumberTypeId
                };
                pointOfContact.Audit.SetHistory(newPhoneNumber);
                contact.PhoneNumbers.Add(newPhoneNumber);
                Context.PhoneNumbers.Add(newPhoneNumber);
            }

            pointOfContact.Audit.SetHistory(contact);
            this.Context.Contacts.Add(contact);
            return contact;
        }

        private AdditionalPointOfContactValidationEntity GetAdditionalPointOfContactValidationEntity(AdditionalPointOfContact pointOfContact, int likeEmailAddressesCount)
        {
            return new AdditionalPointOfContactValidationEntity(
                fullName: pointOfContact.FullName,
                position: pointOfContact.Position,
                likeEmailAddressCount: likeEmailAddressesCount);
        }
        #endregion
    }
}
