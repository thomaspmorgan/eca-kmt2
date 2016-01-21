using ECA.Business.Service;
using ECA.Business.Service.Itineraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Itineraries
{
    /// <summary>
    /// An ItineraryStopParticipantsBindingModel is used to process a client's request to add participants to an itinerary stop.
    /// </summary>
    public class ItineraryStopParticipantsBindingModel : ItineraryParticipantsBindingModel
    {
        /// <summary>
        /// Returns an itinerary stop participants business entity to add participants to an itinerary stop via the business layer.
        /// </summary>
        /// <param name="user">The user adding the participants.</param>
        /// <param name="projectId">The project id.</param>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <param name="itineraryStopId">The itinerary stop id.</param>
        /// <returns>The ItineraryStopParticipants entity.</returns>
        public ItineraryStopParticipants ToItineraryStopParticipants(User user, int projectId, int itineraryId, int itineraryStopId)
        {
            return new ItineraryStopParticipants(
                updator: user,
                projectId: projectId,
                itineraryId: itineraryId,
                itineraryStopId: itineraryStopId,
                participantIds: this.ParticipantIds
                );
        }
    }
}