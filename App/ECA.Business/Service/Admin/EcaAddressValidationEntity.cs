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
        public EcaAddressValidationEntity(int addressTypeId)
        {
            this.AddressTypeId = addressTypeId;
        }

        /// <summary>
        /// Gets or sets the address type id.
        /// </summary>
        public int AddressTypeId { get; private set; }
    }
}
