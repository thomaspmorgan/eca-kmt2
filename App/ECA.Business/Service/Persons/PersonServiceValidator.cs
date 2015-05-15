using ECA.Business.Validation;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Countries of citizenship not found
        /// </summary>
        public const string COUNTRIES_OF_CITIZENSHIP_NOT_FOUND = "The countries of citizenship could not be found.";

        /// <summary>
        /// Countries of citizenship required
        /// </summary>
        public const string COUNTRIES_OF_CITIZENSHIP_REQUIRED = "At least one country of citizenship is required.";

        public override IEnumerable<BusinessValidationResult> DoValidateCreate(PersonServiceValidationEntity validationEntity)
        {
            throw new NotImplementedException();
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

            if (validationEntity.Participant == null)
            {
                yield return new BusinessValidationResult<PersonServiceValidationEntity>(x => x.Participant, PARTICIPANT_NOT_FOUND);
            }

            if (Gender.GetStaticLookup(validationEntity.GenderId) == null)
            {
                yield return new BusinessValidationResult<PersonServiceValidationEntity>(x => x.GenderId, GENDER_NOT_FOUND);
            }

            if (validationEntity.DateOfBirth > DateTime.Now)
            {
                yield return new BusinessValidationResult<PersonServiceValidationEntity>(x => x.DateOfBirth, DATE_OF_BIRTH_GREATER_THAN_TODAY);
            }

            if (validationEntity.DateOfBirth == DateTime.MinValue)
            {
                yield return new BusinessValidationResult<PersonServiceValidationEntity>(x => x.DateOfBirth, DATE_OF_BIRTH_REQUIRED);
            }

            if (validationEntity.CityOfBirth == null)
            {
                yield return new BusinessValidationResult<PersonServiceValidationEntity>(x => x.CityOfBirth, CITY_OF_BIRTH_NOT_FOUND);
            }

            if (validationEntity.CountriesOfCitizenship == null) 
            {
                yield return new BusinessValidationResult<PersonServiceValidationEntity>(x => x.CountriesOfCitizenship, COUNTRIES_OF_CITIZENSHIP_NOT_FOUND);
            }

            if (validationEntity.CountriesOfCitizenship.Count == 0)
            {
                yield return new BusinessValidationResult<PersonServiceValidationEntity>(x => x.CountriesOfCitizenship, COUNTRIES_OF_CITIZENSHIP_REQUIRED);
            }
        }
    }
}
