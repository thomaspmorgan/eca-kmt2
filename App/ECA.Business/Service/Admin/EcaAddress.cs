using ECA.Core.Exceptions;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    public class EcaAddress
    {
        public EcaAddress(
            int addressTypeId,
            string addressDisplayName,
            string street1,
            string street2,
            string street3,
            string postalCode,
            string locationName,
            int countryId,
            int cityId,
            int? divisionId
            )
        {
            if (AddressType.GetStaticLookup(addressTypeId) == null)
            {
                throw new UnknownStaticLookupException(String.Format("The address type id [{0}] is not known.", addressTypeId));
            }
            this.AddressDisplayName = addressDisplayName;
            this.LocationTypeId = LocationType.Address.Id;
            this.AddressTypeId = addressTypeId;
            this.Street1 = street1;
            this.Street2 = street2;
            this.Street3 = street3;
            this.PostalCode = postalCode;
            this.LocationName = locationName;
            this.CountryId = countryId;
            this.CityId = cityId;
            this.DivisionId = divisionId;
        }

        /// <summary>
        /// Gets or sets the address display name.
        /// </summary>
        public string AddressDisplayName { get; private set; }

        /// <summary>
        /// Gets or sets the address type id.
        /// </summary>
        public int AddressTypeId { get; private set; }

        /// <summary>
        /// Gets or sets the location type id.
        /// </summary>
        public int LocationTypeId { get; private set; }

        /// <summary>
        /// Gets or sets the Street 1 address information.
        /// </summary>
        public string Street1 { get; private set; }

        /// <summary>
        /// Gets or sets the Street 2 address information.
        /// </summary>
        public string Street2 { get; private set; }

        /// <summary>
        /// Gets or sets the Street 3 address information.
        /// </summary>
        public string Street3 { get; private set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        public string PostalCode { get; private set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string LocationName { get; private set; }

        /// <summary>
        /// Gets or sets the country id
        /// </summary>
        public int CountryId { get; private set; }

        /// <summary>
        /// Gets or sets the city id
        /// </summary>
        public int CityId { get; private set; }

        /// <summary>
        /// Gets or sets the division id
        /// </summary>
        public int? DivisionId { get; private set; }
    }
}
