using ECA.Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// The validator is used to validation Participant Person business layer client requests.
    /// </summary>
    public class ParticipantPersonServiceValidator : BusinessValidatorBase<object, UpdatedParticipantPersonValidationEntity>
    {
        /// <summary>
        /// The error message to return to a business layer client when a participant type is not a person participant type.
        /// </summary>
        public const string PARTICIPANT_TYPE_IS_NOT_A_PERSON_PARTICIPANT_TYPE_ERROR_MESSAGE = "The participant type for the person must be a person participant type, i.e. not an Organization participant type.";

        /// <summary>
        /// Returns an empty set of validation results.
        /// </summary>
        /// <param name="validationEntity">The validation entity.</param>
        /// <returns>An empty set of validation results.</returns>
        public override IEnumerable<BusinessValidationResult> DoValidateCreate(object validationEntity)
        {
            return Enumerable.Empty<BusinessValidationResult>();
        }

        /// <summary>
        /// Executes validation on the given entity.
        /// </summary>
        /// <param name="validationEntity">The validation entity to validate.</param>
        /// <returns>The validation results.</returns>
        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(UpdatedParticipantPersonValidationEntity validationEntity)
        {
            if (!validationEntity.ParticipantType.IsPerson)
            {
                yield return new BusinessValidationResult<UpdatedParticipantPerson>(x => x.ParticipantTypeId, PARTICIPANT_TYPE_IS_NOT_A_PERSON_PARTICIPANT_TYPE_ERROR_MESSAGE);
            }
        }
    }
}
