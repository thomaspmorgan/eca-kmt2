using ECA.Business.Validation;
using ECA.Data;
using System;
using System.Collections.Generic;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// Validator for person service
    /// </summary>
    public class PersonServiceValidator : BusinessValidatorBase<PersonServiceValidationEntity, PersonServiceValidationEntity>
    {
        /// <summary>
        /// Person not found
        /// </summary>
        public const string PERSON_NOT_FOUND = "The person could not be found.";

        /// <summary>
        /// Participant not found
        /// </summary>
        public const string PARTICIPANT_NOT_FOUND = "The participant could not be found.";

        /// <summary>
        /// Gender not found
        /// </summary>
        public const string GENDER_NOT_FOUND = "The gender could not be found.";

        /// <summary>
        /// Date of birth greater than today
        /// </summary>
        public const string DATE_OF_BIRTH_GREATER_THAN_TODAY = "The date of birth cannot be greater than today.";

        /// <summary>
        /// Date of birth required
        /// </summary>
        public const string DATE_OF_BIRTH_REQUIRED = "The date of birth is required.";

        /// <summary>
        /// City of birth not found
        /// </summary>
        public const string CITY_OF_BIRTH_NOT_FOUND = "The city of birth could not be found.";

        /// <summary>
        /// The date of the birth was estimated but was not provided error message.
        /// </summary>
        public const string DATE_OF_BIRTH_ESTIMATED_BUT_NO_DATE_OF_BIRTH_GIVEN = "The date of birth is estimated but does not have a value.";

        /// <summary>
        /// The date of the birth is unknown but was provided error message.
        /// </summary>
        public const string DATE_OF_BIRTH_UNKONWN_BUT_DATE_OF_BIRTH_GIVEN = "The date of birth is unknown and therefore can not have a value.";

        /// <summary>
        /// City of birth not unknown
        /// </summary>
        public const string PLACE_OF_BIRTH_ERROR = "A Place of birth must be set if the place of birth is not unknown.";

        public override IEnumerable<BusinessValidationResult> DoValidateCreate(PersonServiceValidationEntity validationEntity)
        {
            return DoValidateUpdate(validationEntity);
        }

        /// <summary>
        /// Do validation for update
        /// </summary>
        /// <param name="validationEntity">Entity to validate</param>
        /// <returns>Business validation results</returns>
        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(PersonServiceValidationEntity validationEntity)
        {
            if (validationEntity.Person == null)
            {
                yield return new BusinessValidationResult<PersonServiceValidationEntity>(x => x.Person, PERSON_NOT_FOUND);
            }
            if (Gender.GetStaticLookup(validationEntity.GenderId) == null)
            {
                yield return new BusinessValidationResult<PersonServiceValidationEntity>(x => x.GenderId, GENDER_NOT_FOUND);
            }
            if (validationEntity.PlaceOfBirthId.HasValue
                && validationEntity.IsPlaceOfBirthUnknown.HasValue
                && validationEntity.IsPlaceOfBirthUnknown.Value)
            {
                yield return new BusinessValidationResult<PersonServiceValidationEntity>(x => x.PlaceOfBirthId, PLACE_OF_BIRTH_ERROR);
            }
            if (validationEntity.IsDateOfBirthEstimated.HasValue
                && validationEntity.IsDateOfBirthEstimated.Value
                && !validationEntity.DateOfBirth.HasValue)
            {
                yield return new BusinessValidationResult<PersonServiceValidationEntity>(x => x.IsDateOfBirthEstimated, DATE_OF_BIRTH_ESTIMATED_BUT_NO_DATE_OF_BIRTH_GIVEN);
            }
            if (validationEntity.IsDateOfBirthUnknown.HasValue
                && validationEntity.IsDateOfBirthUnknown.Value
                && validationEntity.DateOfBirth.HasValue)
            {
                yield return new BusinessValidationResult<PersonServiceValidationEntity>(x => x.IsDateOfBirthUnknown, DATE_OF_BIRTH_UNKONWN_BUT_DATE_OF_BIRTH_GIVEN);
            }
        }
    }
}
