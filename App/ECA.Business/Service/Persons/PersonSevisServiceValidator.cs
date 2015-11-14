using ECA.Business.Validation;
using System.Collections.Generic;

namespace ECA.Business.Service.Persons
{
    public class PersonSevisServiceValidator : SevisValidatorBase<UpdatedParticipantPersonSevisValidationEntity>
    {
        /// <summary>
        /// Person not found
        /// </summary>
        public const string PERSON_NOT_FOUND = "The participant person could not be found.";

        /// <summary>
        /// Do validation for sevis object, which includes participant person object
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
