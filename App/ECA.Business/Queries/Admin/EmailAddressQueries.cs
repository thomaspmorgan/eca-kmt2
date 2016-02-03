using ECA.Business.Queries.Models.Admin;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Admin
{
    /// <summary>
    /// Contains queries against an ECA Context for email address entities.
    /// </summary>
    public static class EmailAddressQueries
    {
        /// <summary>
        /// Returns a query to get email addresss dtos from the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve email address dtos.</returns>
        public static IQueryable<EmailAddressDTO> CreateGetEmailAddressDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return context.EmailAddresses.Select(x => new EmailAddressDTO
            {
                Id = x.EmailAddressId,
                Address = x.Address,
                EmailAddressType = x.EmailAddressType.EmailAddressTypeName,
                EmailAddressTypeId = x.EmailAddressTypeId,
                PersonId = x.PersonId,
                ContactId = x.ContactId,
                IsPrimary = x.IsPrimary
            });
        }

        /// <summary>
        /// Returns a query to get the social media dto for the social media entity with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="id">The social media id.</param>
        /// <returns>The social media dto with the given id.</returns>
        public static IQueryable<EmailAddressDTO> CreateGetEmailAddressDTOByIdQuery(EcaContext context, int id)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetEmailAddressDTOQuery(context).Where(x => x.Id == id);
        }
    }
}
