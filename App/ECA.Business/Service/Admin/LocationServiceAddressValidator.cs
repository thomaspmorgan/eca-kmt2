using ECA.Business.Validation;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// The LocationServiceAddressValidator is used to validate new and updated addresses.
    /// </summary>
    public class LocationServiceAddressValidator : BusinessValidatorBase<EcaAddressValidationEntity, EcaAddressValidationEntity>
    {
        /// <summary>
        /// The error message to add when the address type is not recognized.
        /// </summary>
        public const string INVALID_ADDRESS_TYPE_MESSAGE = "The address type is invalid.";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<BusinessValidationResult> DoValidateCreate(EcaAddressValidationEntity validationEntity)
        {
            return DoValidate(validationEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(EcaAddressValidationEntity validationEntity)
        {
            return DoValidate(validationEntity);
        }

        /// <summary>
        /// Validates the given entity.  To be used by both create and update validations.
        /// </summary>
        /// <param name="validationEntity">The validation entity.</param>
        /// <returns>The validation results.</returns>
        public IEnumerable<BusinessValidationResult> DoValidate(EcaAddressValidationEntity validationEntity)
        {
            if (AddressType.GetStaticLookup(validationEntity.AddressTypeId) == null)
            {
                yield return new BusinessValidationResult<EcaAddress>(x => x.AddressTypeId, INVALID_ADDRESS_TYPE_MESSAGE);
            }
        }
    }
}
