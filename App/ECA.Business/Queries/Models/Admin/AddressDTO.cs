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
        /// Gets or sets the country iso2.
        /// </summary>
        public string CountryIso2 { get; set; }

        /// <summary>
        /// Gets or sets the division id.
        /// </summary>
        public int? DivisionId { get; set; }

        /// <summary>
        /// Gets or sets the division.
        /// </summary>
        public string Division { get; set; }

        /// <summary>
        /// Gets or sets the is primary flag.
        /// </summary>
        public bool IsPrimary { get; set; }

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

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Returns this address as a formatted string.
        /// </summary>
        /// <returns>The address as a formatted string.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (!String.IsNullOrEmpty(this.Street1))
            {
                sb.AppendLine(this.Street1);
            }
            if (!String.IsNullOrEmpty(this.Street2))
            {
                sb.AppendLine(this.Street2);
            }
            if (!String.IsNullOrEmpty(this.Street3))
            {
                sb.AppendLine(this.Street3);
            }
            if (!String.IsNullOrWhiteSpace(this.City) 
                && !String.IsNullOrWhiteSpace(this.Division) 
                && !String.IsNullOrWhiteSpace(this.Country)
                && !String.IsNullOrWhiteSpace(this.PostalCode))
            {
                sb.AppendFormat("{0}, {1} {2}", this.City, this.Division, this.PostalCode);
                sb.AppendLine(this.Country);
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(this.City))
                {
                    sb.AppendLine(this.City);
                }
                if (!String.IsNullOrWhiteSpace(this.Division))
                {
                    sb.AppendLine(this.Division);
                }
                if (!String.IsNullOrWhiteSpace(this.Country))
                {
                    sb.AppendLine(this.Country);
                }
                if (!String.IsNullOrWhiteSpace(this.PostalCode))
                {
                    sb.AppendLine(this.PostalCode);
                }
            }
            return sb.ToString();
        }
    }
}
