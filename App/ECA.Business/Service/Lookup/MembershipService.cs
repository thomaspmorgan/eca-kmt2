using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// The MembershipService is a lookup service for handling Memberships.
    /// </summary>
    public class MembershipService : LookupService<MembershipDTO>, IMembershipService
    {
        /// <summary>
        /// Creates a new Membership instance with the context to query.
        /// </summary>
        /// <param name="context">The context to query.</param>
        public MembershipService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        protected override IQueryable<MembershipDTO> GetSelectDTOQuery()
        {
            return Context.Memberships.Select(x => new MembershipDTO
            {
                Id = x.MembershipId,
                Name = x.Name
            });
        }
    }
}