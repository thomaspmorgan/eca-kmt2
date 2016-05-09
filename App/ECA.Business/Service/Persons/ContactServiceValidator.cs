using ECA.Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// A ContactServiceValidator is used to validate points of contact.
    /// </summary>
    public class ContactServiceValidator : BusinessValidatorBase<AdditionalPointOfContactValidationEntity, object>
    {
        /// <summary>
        /// The error message to return when more than one email address is set as primary.
        /// </summary>
        public const string MORE_THAN_ONE_PRIMARY_EMAIL_ADDRESS_ERROR = "There is more than one primary email address configured, at most one email address may be primary.";

        /// <summary>
        /// The error message to return when more than one phone number is set as primary.
        /// </summary>
        public const string MORE_THAN_ONE_PRIMARY_PHONE_NUMBER_ERROR = "There is more than one primary phone number configured, at most one phone number may be primary.";

        /// <summary>
        /// The error message to return when a point of contact is not provided a full name.
        /// </summary>
        public const string POINT_OF_CONTACT_MUST_HAVE_A_FULL_NAME_VALUE = "The point of contact must have a full name.";
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<BusinessValidationResult> DoValidateCreate(AdditionalPointOfContactValidationEntity validationEntity)
        {
            if (String.IsNullOrWhiteSpace(validationEntity.FullName))
            {
                yield return new BusinessValidationResult<AdditionalPointOfContact>(x => x.FullName, POINT_OF_CONTACT_MUST_HAVE_A_FULL_NAME_VALUE);
            }
            if(validationEntity.NumberOfPrimaryEmailAddresses > 1)
            {
                yield return new BusinessValidationResult<AdditionalPointOfContact>(x => x.EmailAddresses, MORE_THAN_ONE_PRIMARY_EMAIL_ADDRESS_ERROR);
            }
            if (validationEntity.NumberOfPrimaryPhoneNumbers > 1)
            {
                yield return new BusinessValidationResult<AdditionalPointOfContact>(x => x.PhoneNumbers, MORE_THAN_ONE_PRIMARY_PHONE_NUMBER_ERROR);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(object validationEntity)
        {
            return Enumerable.Empty<BusinessValidationResult>();
        }
    }
}
