using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Itineraries
{
    /// <summary>
    /// The ItineraryGroupPersonDTO is used to represent a person participant within an itinerary group.
    /// </summary>
    public class ItineraryGroupPersonDTO
    {
        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Gets or sets the participant id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the participant type id.
        /// </summary>
        public int ParticipantTypeId { get; set; }

        /// <summary>
        /// Gets or sets the full name of the person.
        /// </summary>
        public string FullName { get; set; }
    }
}
