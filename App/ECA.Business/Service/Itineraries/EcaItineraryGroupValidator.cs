using ECA.Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    public class EcaItineraryGroupValidator : BusinessValidatorBase<AddedEcaItineraryGroupValidationEntity, object>
    {
        /// <summary>
        /// The error message to return when the participants are not all people.
        /// </summary>
        public const string ALL_PARTICIPANTS_MUST_BE_A_PERSON_PARTICIPANT_TYPE = "The given participants must be a participant type representing a person.";

        /// <summary>
        /// The error message to return when the itinerary group already exists.
        /// </summary>
        public const string ITINERARY_GROUP_ALREADY_EXISTS = "The itinerary group already exists in the system.";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<BusinessValidationResult> DoValidateCreate(AddedEcaItineraryGroupValidationEntity validationEntity)
        {
            //will need a check in here for the participants all being participant types of person
            if(validationEntity.Participants != null && validationEntity.Participants.Where(x => !x.ParticipantType.IsPerson).Count() != 0)
            {
                yield return new BusinessValidationResult<AddedEcaItineraryGroup>(x => x.ParticipantIds, ALL_PARTICIPANTS_MUST_BE_A_PERSON_PARTICIPANT_TYPE);
            }
            if(validationEntity.ExistingItineraryGroups != null && validationEntity.ExistingItineraryGroups.Count() != 0)
            {
                yield return new BusinessValidationResult<AddedEcaItineraryGroup>(x => x.Name, ITINERARY_GROUP_ALREADY_EXISTS);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(object validationEntity)
        {
            throw new NotImplementedException();
        }
    }
}
