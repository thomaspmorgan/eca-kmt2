using ECA.Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    public class ItineraryParticipantsStopValidator : BusinessValidatorBase<ItineraryStopParticipantsValidationEntity, ItineraryStopParticipantsValidationEntity>
    {
        /// <summary>
        /// The error message to return a participant was placed in an itinerary stop but not the itinerary.
        /// </summary>
        public const string ALL_PARTICIPANTS_OF_ITINERARY_STOP_MUST_PARTICIPANT_IN_ITINERARY_ERROR_MESSAGE = "The itinerary stop participants must be participants on the itinerary.";


        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<BusinessValidationResult> DoValidateCreate(ItineraryStopParticipantsValidationEntity validationEntity)
        {
            return DoValidation(validationEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(ItineraryStopParticipantsValidationEntity validationEntity)
        {
            return DoValidation(validationEntity);
        }

        private IEnumerable<BusinessValidationResult> DoValidation(ItineraryStopParticipantsValidationEntity validationEntity)
        {
            if (validationEntity.NotAllowedParticipantsByParticipantId.Count() > 0)
            {
                yield return new BusinessValidationResult<ItineraryStopParticipants>(x => x.ParticipantIds, ALL_PARTICIPANTS_OF_ITINERARY_STOP_MUST_PARTICIPANT_IN_ITINERARY_ERROR_MESSAGE);
            }
        }
    }
}