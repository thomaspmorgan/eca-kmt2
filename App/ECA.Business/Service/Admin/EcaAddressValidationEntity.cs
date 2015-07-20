﻿using System;
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
        /// <param name="addressDisplayName">The address display name.</param>
        /// <param name="addressTypeId">The address type id.</param>
        public EcaAddressValidationEntity(string addressDisplayName, int addressTypeId)
        {
            this.AddressDisplayName = addressDisplayName;
            this.AddressTypeId = addressTypeId;
        }

        /// <summary>
        /// Gets or sets the address display name.
        /// </summary>
        public string AddressDisplayName { get; private set; }

        /// <summary>
        /// Gets or sets the address type id.
        /// </summary>
        public int AddressTypeId { get; private set; }
    }
}
