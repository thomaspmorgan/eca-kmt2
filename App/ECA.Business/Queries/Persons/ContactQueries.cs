using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static IQueryable<ContactDTO> CreateContactDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var emailAddressQuery = EmailAddressQueries.CreateGetEmailAddressDTOQuery(context);
            var phoneNumbersQuery = PhoneNumberQueries.CreateGetPhoneNumberDTOQuery(context);

            var query = from contact in context.Contacts
                        let phoneNumbers = phoneNumbersQuery.Where(x => x.ContactId == contact.ContactId)
                        let emailAddresses = emailAddressQuery.Where(x => x.ContactId == contact.ContactId)
                        select new ContactDTO
                        {
                            EmailAddresses = emailAddresses,
                            EmailAddressValues = emailAddresses.Select(x => x.Address),
                            FullName = contact.FullName,
                            Id = contact.ContactId,
                            PhoneNumbers = phoneNumbers,
                            PhoneNumberValues = phoneNumbers.Select(x => x.Number),
                            Position = contact.Position
                        };
            return query;
        }

        /// <summary>
        /// Returns a query to filtered and sorted contacts in the system.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query to retrieve filtered and sorted contacts.</returns>
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
