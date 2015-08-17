using ECA.Business.Queries.Models.Persons;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Persons
{
    /// <summary>
    /// Contains queries against an ECA Context for membership entities.
    /// </summary>
    public static class MembershipQueries
    {
        /// <summary>
        /// Returns a query to get membership dtos from the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve membership dtos.</returns>
        public static IQueryable<MembershipDTO> CreateGetMembershipDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return context.Memberships.Select(x => new MembershipDTO
            {
                Id = x.MembershipId,
                Name = x.Name
            });
        }

        /// <summary>
        /// Returns a query to get the membership dto for the membership entity with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="id">The membership id.</param>
        /// <returns>The membership dto with the given id.</returns>
        public static IQueryable<MembershipDTO> CreateGetMembershipDTOByIdQuery(EcaContext context, int id)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetMembershipDTOQuery(context).Where(x => x.Id == id);
        }
    }
}

