using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    public abstract class AdditionalAddress
    {
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
        public int? CountryId { get; private set; }

        /// <summary>
        /// Gets or sets the city id
        /// </summary>
        public int? CityId { get; private set; }

        /// <summary>
        /// Gets or sets the division id
        /// </summary>
        public int? DivisionId { get; private set; }

        /// <summary>
        /// Gets or sets the audit.
        /// </summary>
        public Audit Audit { get; protected set; }
    }
}
