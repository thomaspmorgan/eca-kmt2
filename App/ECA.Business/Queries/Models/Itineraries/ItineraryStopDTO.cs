using ECA.Business.Queries.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Itineraries
{
    /// <summary>
    /// An ItineraryStopDTO is used to represent an itinerary stop to a BL client.
    /// </summary>
    public class ItineraryStopDTO
    {
        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public ItineraryStopDTO()
        {
            this.Participants = new List<ItineraryStopParticipantDTO>();
        }

        /// <summary>
        /// Gets or sets the timezone id.
        /// </summary>
        public string TimezoneId { get; set; }

        /// <summary>
        /// Gets or sets the itinerary id.
        /// </summary>
        public int ItineraryId { get; set; }

        /// <summary>
        /// Gets or sets the itinerary stop id.
        /// </summary>
        public int ItineraryStopId { get; set; }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the arrival date of the itinerary stop.
        /// </summary>
        public DateTimeOffset? ArrivalDate { get; set; }

        /// <summary>
        /// Gets or sets the departure date of the itinerary stop.
        /// </summary>
        public DateTimeOffset? DepartureDate { get; set; }

        /// <summary>
        /// Gets or sets the destination of the itinerary stop.
        /// </summary>
        public LocationDTO DestinationLocation { get; set; }

        /// <summary>
        /// Gets or sets the last revised date.
        /// </summary>
        public DateTimeOffset LastRevisedOn { get; set; }

        /// <summary>
        /// Gets or sets the count of participants in the itinerary stop.
        /// </summary>
        public int ParticipantsCount { get; set; }

        /// <summary>
        /// Gets or sets the participants.
        /// </summary>
        public IEnumerable<ItineraryStopParticipantDTO> Participants { get; set; }
    }
}
