using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    public class AdditionalPersonAddress : AdditionalAddress<Person>
    {
        private readonly int personId;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="creator">The user creating the address.</param>
        /// <param name="addressTypeId">The address type id.</param>
        /// <param name="addressDisplayName">The address display name.</param>
        /// <param name="street1">The street1.</param>
        /// <param name="street2">The street2.</param>
        /// <param name="street3">The street3.</param>
        /// <param name="postalCode">The postal code.</param>
        /// <param name="locationName">The name of the location.</param>
        /// <param name="countryId">The country id.</param>
        /// <param name="cityId">The city id.</param>
        /// <param name="divisionId">The division id.</param>
        /// <param name="personId">The id of the person.</param>
        public AdditionalPersonAddress(
            User creator,
            int addressTypeId,
            string addressDisplayName,
            string street1,
            string street2,
            string street3,
            string postalCode,
            string locationName,
            int countryId,
            int cityId,
            int divisionId,
            int personId
            )
            : base(creator, addressTypeId, addressDisplayName, street1, street2, street3, postalCode, locationName, countryId, cityId, divisionId)
        {
            Contract.Requires(creator != null, "The creator must not be null.");
            this.personId = personId;
        }

        /// <summary>
        /// Returns the person id.
        /// </summary>
        /// <returns>The person id.</returns>
        public override int GetAddressableEntityId()
        {
            return personId;
        }
    }
}
