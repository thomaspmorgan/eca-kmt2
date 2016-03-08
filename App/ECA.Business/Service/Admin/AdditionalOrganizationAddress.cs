using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// An AdditionalOrganizationAddress business entity is used to add an address to an organization in the ECA system.
    /// </summary>
    public class AdditionalOrganizationAddress : AdditionalAddress<Organization>
    {
        private readonly int organizationId;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="creator">The user creating the address.</param>
        /// <param name="addressTypeId">The address type id.</param>
        /// <param name="isPrimary">True if the address is the primary address.</param>
        /// <param name="street1">The street1.</param>
        /// <param name="street2">The street2.</param>
        /// <param name="street3">The street3.</param>
        /// <param name="postalCode">The postal code.</param>
        /// <param name="locationName">The name of the location.</param>
        /// <param name="countryId">The country id.</param>
        /// <param name="cityId">The city id.</param>
        /// <param name="divisionId">The division id.</param>
        /// <param name="organizationId">The id of the organization.</param>
        public AdditionalOrganizationAddress(
            User creator,
            int addressTypeId,
            bool isPrimary,
            string street1,
            string street2,
            string street3,
            string postalCode,
            string locationName,
            int countryId,
            int cityId,
            int? divisionId,
            int organizationId
            )
            : base(creator, addressTypeId, isPrimary, street1, street2, street3, postalCode, locationName, countryId, cityId, divisionId)
        {
            Contract.Requires(creator != null, "The creator must not be null.");
            this.organizationId = organizationId;
        }

        /// <summary>
        /// Returns the organization id.
        /// </summary>
        /// <returns>The organization id.</returns>
        public override int GetAddressableEntityId()
        {
            return organizationId;
        }

        /// <summary>
        /// Returns a query to retrieve addresses for an organization from the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve all addresses for this person.</returns>
        public override IQueryable<Address> CreateGetAddressesQuery(EcaContext context)
        {
            return context.Addresses.Where(x => x.OrganizationId.HasValue && x.OrganizationId.Value == this.organizationId);
        }
    }
}
