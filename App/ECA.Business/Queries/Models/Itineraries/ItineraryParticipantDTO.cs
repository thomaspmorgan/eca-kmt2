using ECA.Business.Queries.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Itineraries
{
    /// <summary>
    /// an Itinerary Participant DTO represents an ECA system's participant that is assigned to an itinerary.
    /// </summary>
    public class ItineraryParticipantDTO
    {
        /// <summary>
        /// Gets or sets the participant id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        public string FullName { get; set; }
    }

    /// <summary>
    /// An ItineraryStopParticipantDTO represents a participant that has been assigned to an itinerary stop either by group or directly.
    /// </summary>
    public class ItineraryStopParticipantDTO : ItineraryParticipantDTO
    {
        /// <summary>
        /// Gets or sets the itinerary information id.
        /// </summary>
        public int ItineraryInformationId { get; set; }

        /// <summary>
        /// Gets or sets the location the participant is traveling from.
        /// </summary>
        public LocationDTO TravelingFrom { get; set; }

        /// <summary>
        /// Gets or sets the itinerary stop id.
        /// </summary>
        public int ItineraryStopId { get; set; }
    }
}
