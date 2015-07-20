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
    /// An UpdatedEcaAddress is an address that must be updated within the system.
    /// </summary>
    public class UpdatedEcaAddress : EcaAddress
    {
        /// <summary>
        /// Creates an updated eca address.
        /// </summary>
        /// <param name="updator">The user performing the update.</param>
        /// <param name="addressId">The address id.</param>
        /// <param name="addressTypeId">The address type id.</param>
        /// <param name="addressDisplayName">The address display name.</param>
        /// <param name="street1">The street 1.</param>
        /// <param name="street2">The street 2.</param>
        /// <param name="street3">The street 3.</param>
        /// <param name="postalCode">The postal code.</param>
        /// <param name="locationName">The location name.</param>
        /// <param name="countryId">The country id.</param>
        /// <param name="cityId">The city id.</param>
        /// <param name="divisionId">The division id.</param>
        public UpdatedEcaAddress(
            User updator,
            int addressId,
            int addressTypeId,
            string addressDisplayName,
            string street1,
            string street2,
            string street3,
            string postalCode,
            string locationName,
            int countryId,
            int cityId,
            int divisionId
            )
            : base(addressTypeId, addressDisplayName, street1, street2, street3, postalCode, locationName, countryId, cityId, divisionId)
        {
            Contract.Requires(updator != null, "The updator must not be null.");
            this.Update = new Update(updator);
            this.AddressId = addressId;
        }

        /// <summary>
        /// Gets the id of the address.
        /// </summary>
        public int AddressId { get; private set; }

        /// <summary>
        /// Gets the update audit details.
        /// </summary>
        public Audit Update { get; private set; }
    }
}
