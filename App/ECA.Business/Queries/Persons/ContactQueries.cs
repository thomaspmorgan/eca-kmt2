using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Projects;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Persons
{
    /// <summary>
    /// The ContactQueries contains queries for retrieving contacts from the db context.
    /// </summary>
    public static class ContactQueries
    {
        /// <summary>
        /// Returns a query to get contacts.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to get contacts.</returns>
        public static IQueryable<Contact> CreateContactQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var emailAddressQuery = EmailAddressQueries.CreateGetEmailAddressDTOQuery(context);
            var phoneNumbersQuery = PhoneNumberQueries.CreateGetPhoneNumberDTOQuery(context);
            var projectsQuery = ProjectQueries.CreateGetProjectDTOQuery(context);

            var query = from contact in context.Contacts
                        let phoneNumbers = phoneNumbersQuery.Where(x => x.ContactId == contact.ContactId).Select(x => new PhoneNumber { PhoneNumberId = x.Id, Number = x.Number, ContactId = x.ContactId, PhoneNumberTypeId = x.PhoneNumberTypeId, IsPrimary = x.IsPrimary }).ToList()
                        let emailAddresses = emailAddressQuery.Where(x => x.ContactId == contact.ContactId).Select(x => new EmailAddress { EmailAddressId = x.Id, Address = x.Address, ContactId = x.ContactId, EmailAddressTypeId = x.EmailAddressTypeId, IsPrimary = x.IsPrimary }).ToList()
                        select new Contact
                        {
                            ContactId = contact.ContactId,
                            FullName = contact.FullName,
                            Position = contact.Position,
                            EmailAddresses = emailAddresses,
                            PhoneNumbers = phoneNumbers
                            //Projects = projectsQuery.Where(x => x.Contacts.Contains(new Service.Lookup.SimpleLookupDTO { Id = contact.ContactId, Value = contact.FullName + " (" + contact.Position + ")" }))
                        };
            return query;
        }

        /// <summary>
        /// Returns a query to filtered and sorted contacts in the system.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query to retrieve filtered and sorted contacts.</returns>
        public static IQueryable<Contact> CreateContactQuery(EcaContext context, QueryableOperator<Contact> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateContactQuery(context);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns a query to get contact DTOs.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IQueryable<ContactDTO> CreateContactDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var emailAddressQuery = EmailAddressQueries.CreateGetEmailAddressDTOQuery(context);
            var phoneNumbersQuery = PhoneNumberQueries.CreateGetPhoneNumberDTOQuery(context);
            var projectsQuery = ProjectQueries.CreateGetProjectDTOQuery(context);

            var query = from contact in context.Contacts
                        let phoneNumbers = phoneNumbersQuery.Where(x => x.ContactId == contact.ContactId)
                        let emailAddresses = emailAddressQuery.Where(x => x.ContactId == contact.ContactId)
                        select new ContactDTO
                        {
                            Id = contact.ContactId,
                            FullName = contact.FullName,
                            Position = contact.Position,
                            EmailAddresses = emailAddresses,
                            PhoneNumbers = phoneNumbers
                            //Projects = projectsQuery.Where(x => x.Contacts.Contains(new Service.Lookup.SimpleLookupDTO { Id = contact.ContactId, Value = contact.FullName + " (" + contact.Position + ")" }))
                        };
            return query;
        }

        /// <summary>
        /// Returns a query to filtered and sorted contact DTOs in the system.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public static IQueryable<ContactDTO> CreateContactDTOQuery(EcaContext context, QueryableOperator<ContactDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateContactDTOQuery(context);
            query = query.Apply(queryOperator);
            return query;
        }

    }
}
