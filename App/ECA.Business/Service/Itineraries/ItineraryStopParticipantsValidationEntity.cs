using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An ItineraryStopParticipantsValidationEntity is used to validate a business layer client's request to set participants on an itinerary stop.
    /// </summary>
    public class ItineraryStopParticipantsValidationEntity
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="notAllowedParticipantsByParticipantId">The id of the participants that are not allowed on the itinerary stop.</param>
        public ItineraryStopParticipantsValidationEntity(IEnumerable<int> notAllowedParticipantsByParticipantId)
        {
            this.NotAllowedParticipantsByParticipantId = notAllowedParticipantsByParticipantId;
        }

        /// <summary>
        /// Gets the participants that are not allowed on the stop by participant id.
        /// </summary>
        public IEnumerable<int> NotAllowedParticipantsByParticipantId { get; private set; }
    }
}
