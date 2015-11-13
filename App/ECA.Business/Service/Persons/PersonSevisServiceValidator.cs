using ECA.Business.Sevis.Validation;
using System;
using System.Collections.Generic;

namespace ECA.Business.Service.Persons
{
    public class PersonSevisServiceValidator : SevisValidatorBase<object, UpdatedParticipantPersonSevisValidationEntity>
    {
        /// <summary>
        /// Person not found
        /// </summary>
        public const string PERSON_NOT_FOUND = "The participant person could not be found.";

        /// <summary>
        /// Do validation for create
        /// </summary>
        /// <param name="validationEntity">Entity to validate</param>
        /// <returns>Business validation results</returns>
        public override IEnumerable<SevisValidationResult> DoValidateCreate(object validationEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Do validation for update
        /// </summary>
        /// <param name="validationEntity">Entity to validate</param>
        /// <returns>Business validation results</returns>
        public override IEnumerable<SevisValidationResult> DoValidateUpdate(UpdatedParticipantPersonSevisValidationEntity validationEntity)
        {
            if (validationEntity.participantPerson == null)
            {
                yield return new SevisValidationResult<PersonSevisServiceValidationEntity>(x => x.sevisPerson, PERSON_NOT_FOUND);
            }
        }

    }
}
