using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Persons
{
    /// <summary>
    /// DTO for location
    /// </summary>
    public class LocationDTO
    {
        /// <summary>
        /// Gets and sets street 1
        /// </summary>
        public string Street1 { get; set; }

        /// <summary>
        /// Gets and sets street 2
        /// </summary>
        public string Street2 { get; set; }

        /// <summary>
        /// Gets and sets street 3
        /// </summary>
        public string Street3 { get; set; }

        /// <summary>
        /// Gets and sets city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets and sets postal code
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets and sets country
        /// </summary>
        public string Country { get; set; }
    }
}
