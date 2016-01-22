using ECA.Business.Service;
using ECA.Business.Service.Itineraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Itineraries
{
    /// <summary>
    /// An ItineraryParticipantsBindingModel is used to process a client's request to add participants to an itinerary.
    /// </summary>
    public class ItineraryParticipantsBindingModel
    {
        /// <summary>
        /// The ids of the participants on the itinerary.
        /// </summary>
        public IEnumerable<int> ParticipantIds { get; set; }

        /// <summary>
        /// Returns an itinerary participants business entity to add participants to an itinerary via the business layer.
        /// </summary>
        /// <param name="user">The user adding the participants.</param>
        /// <param name="projectId">The project id.</param>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <returns>The ItineraryParticipants entity.</returns>
        public ItineraryParticipants ToItineraryParticipants(User user, int projectId, int itineraryId)
        {
            return new ItineraryParticipants(user, projectId, itineraryId, this.ParticipantIds);
        }
    }
}