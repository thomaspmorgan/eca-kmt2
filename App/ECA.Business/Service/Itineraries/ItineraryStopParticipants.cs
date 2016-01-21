using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An ItineraryParticipants business entity is used to set participants on an itinerary.
    /// </summary>
    public class ItineraryStopParticipants : ItineraryParticipants
    {
        public ItineraryStopParticipants(User updator, int projectId, int itineraryId, int itineraryStopId, IEnumerable<int> participantIds)
            : base(updator, projectId, itineraryId, participantIds)
        {
            this.ItineraryStopId = itineraryStopId;
        }

        /// <summary>
        /// Gets the itinerary stop id.
        /// </summary>
        public int ItineraryStopId { get; private set; }
    }
}
