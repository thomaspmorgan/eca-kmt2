using ECA.Business.Sevis.Validation;
using System;
using System.Collections.Generic;

namespace ECA.Business.Service.Persons
{
    public class PersonSevisServiceValidator : SevisValidatorBase<PersonSevisServiceValidationEntity, PersonSevisServiceValidationEntity>
    {
        /// <summary>
        /// Person not found
        /// </summary>
        public const string PERSON_NOT_FOUND = "The person could not be found.";

        public override IEnumerable<SevisValidationResult> DoValidateCreate(PersonSevisServiceValidationEntity validationEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Do validation for update
        /// </summary>
        /// <param name="validationEntity">Entity to validate</param>
        /// <returns>Business validation results</returns>
        public override IEnumerable<SevisValidationResult> DoValidateUpdate(PersonSevisServiceValidationEntity validationEntity)
        {
            if (validationEntity.sevisPerson == null)
            {
                yield return new SevisValidationResult<PersonSevisServiceValidationEntity>(x => x.sevisPerson, PERSON_NOT_FOUND);
            }






        }
    }
}
