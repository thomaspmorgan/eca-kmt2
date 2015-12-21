using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Itineraries
{
    /// <summary>
    /// An ItineraryGroupParticipantDTO represents a person who is a participant on a project that is part of an itinerary group.
    /// </summary>
    public class ItineraryGroupParticipantDTO
    {
        /// <summary>
        /// Gets or sets the itinerary id.
        /// </summary>
        public int ItineraryId { get; set; }

        /// <summary>
        /// Gets or sets the itinerary group id.
        /// </summary>
        public int ItineraryGroupId { get; set; }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the participant id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Gets or sets the itinerary name.
        /// </summary>
        public string ItineraryName { get; set; }

        /// <summary>
        /// Gets or sets the itinerary group name.
        /// </summary>
        public string ItineraryGroupName { get; set; }

        /// <summary>
        /// Gets or sets the full name of the person.
        /// </summary>
        public string FullName { get; set; }
    }
}
