using ECA.Business.Queries.Models.Admin;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Admin
{
    /// <summary>
    /// Contains queries against an ECA Context for phone number entities.
    /// </summary>
    public static class PhoneNumberQueries
    {
        /// <summary>
        /// Returns a query to get phone number dtos from the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve phone number dtos.</returns>
        public static IQueryable<PhoneNumberDTO> CreateGetPhoneNumberDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return context.PhoneNumbers.Select(x => new PhoneNumberDTO
            {
                Id = x.PhoneNumberId,
                Number = x.Number,
                PhoneNumberType = x.PhoneNumberType.PhoneNumberTypeName,
                PhoneNumberTypeId = x.PhoneNumberTypeId
            });
        }

        /// <summary>
        /// Returns a query to get the phone number dto for the phone number entity with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="id">The phone number id.</param>
        /// <returns>The phone number dto with the given id.</returns>
        public static IQueryable<PhoneNumberDTO> CreateGetPhoneNumberDTOByIdQuery(EcaContext context, int id)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetPhoneNumberDTOQuery(context).Where(x => x.Id == id);
        }
    }
}
