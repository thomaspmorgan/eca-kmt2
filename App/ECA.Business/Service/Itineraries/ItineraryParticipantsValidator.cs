using ECA.Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// The ItineraryParticipantsValidator is used to validate participants that are to be set on an itinerar.
    /// </summary>
    public class ItineraryParticipantsValidator : BusinessValidatorBase<ItineraryParticipantsValidationEntity, ItineraryParticipantsValidationEntity>
    {
        /// <summary>
        /// The error message to return when a participant has been removed from the itinerary but not all city stops.
        /// </summary>
        public const string ITINERARY_STOP_PARTICIPANT_ORPHANED_ERROR_MESSAGE = "Unable to remove all participants from the travel period because they are assigned to at least one city stop.";

        /// <summary>
        /// The error message to return when a participant's participant type is not a person.
        /// </summary>
        public const string PARTICIPANTS_MUST_BE_PEOPLE_PARTICIPANT_TYPES_ERROR_MESSAGE = "Participants on an itinerary must be a person participant type.";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<BusinessValidationResult> DoValidateCreate(ItineraryParticipantsValidationEntity validationEntity)
        {
            return DoValidation(validationEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(ItineraryParticipantsValidationEntity validationEntity)
        {
            return DoValidation(validationEntity);
        }

        private IEnumerable<BusinessValidationResult> DoValidation(ItineraryParticipantsValidationEntity validationEntity)
        {
            if(validationEntity.OrphanedParticipantsByParticipantId.Count() > 0)
            {
                yield return new BusinessValidationResult<ItineraryParticipants>(x => x.ParticipantIds, ITINERARY_STOP_PARTICIPANT_ORPHANED_ERROR_MESSAGE);
            }
            if(validationEntity.NonPersonParticipantsByParticipantId.Count() > 0)
            {
                yield return new BusinessValidationResult<ItineraryParticipants>(x => x.ParticipantIds, PARTICIPANTS_MUST_BE_PEOPLE_PARTICIPANT_TYPES_ERROR_MESSAGE);
            }
        }
    }
}
