using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// The EcaAddressValidationEntity is used to validate created and updated addresses in the eca system.
    /// </summary>
    public class EcaAddressValidationEntity
    {
        /// <summary>
        /// Creates a new instance of the validation entity.
        /// </summary>
        /// <param name="addressTypeId">The address type id.</param>
        /// <param name="city">The city of the address.</param>
        /// <param name="country">The country of the address.</param>
        /// <param name="division">The division of the address.</param>
        public EcaAddressValidationEntity(int addressTypeId, 
            Location country, 
            Location division, 
            Location city)
        {
            this.AddressTypeId = addressTypeId;
            this.Country = country;
            this.Division = division;
            this.City = city;
        }

        /// <summary>
        /// Gets the country of the address.
        /// </summary>
        public Location Country { get; private set; }

        /// <summary>
        /// Gets the division of the address.
        /// </summary>
        public Location Division { get; private set; }

        /// <summary>
        /// Gets the city of the address.
        /// </summary>
        public Location City { get; private set; }

        /// <summary>
        /// Gets or sets the address type id.
        /// </summary>
        public int AddressTypeId { get; private set; }
    }
}
