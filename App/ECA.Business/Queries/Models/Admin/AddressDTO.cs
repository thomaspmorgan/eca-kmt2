using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Admin
{
    /// <summary>
    /// An AddressDTO is a representation of both an Address and its related Location in the ECA system.
    /// </summary>
    public class AddressDTO
    {
        /// <summary>
        /// Gets or sets the address id.
        /// </summary>
        public int AddressId { get; set; }

        /// <summary>
        /// Gets or sets the location id.
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// Gets or sets street 1.
        /// </summary>
        public string Street1 { get; set; }

        /// <summary>
        /// Gets or sets street 2.
        /// </summary>
        public string Street2 { get; set; }

        /// <summary>
        /// Gets or sets street 3.
        /// </summary>
        public string Street3 { get; set; }

        /// <summary>
        /// Gets or sets the city id.
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the country id.
        /// </summary>
        public int? CountryId { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the division id.
        /// </summary>
        public int? DivisionId { get; set; }

        /// <summary>
        /// Gets or sets the division.
        /// </summary>
        public string Division { get; set; }

        /// <summary>
        /// Gets or sets the address display name.
        /// </summary>
        public string AddressDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the organization id.
        /// </summary>
        public int? OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        public int? PersonId { get; set; }

        /// <summary>
        /// Gets or sets the location name.
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// Gets or sets the address type id.
        /// </summary>
        public int AddressTypeId { get; set; }

        /// <summary>
        /// Gets or sets the address type.
        /// </summary>
        public string AddressType { get; set; }
    }
}
