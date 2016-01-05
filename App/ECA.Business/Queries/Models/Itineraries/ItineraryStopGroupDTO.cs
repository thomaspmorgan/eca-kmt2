using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Itineraries
{
    /// <summary>
    /// An ItineraryStopGroupDTO is used to represent an ItineraryStopGroup to a BL client.
    /// </summary>
    public class ItineraryStopGroupDTO
    {
        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public ItineraryStopGroupDTO()
        {
            this.Participants = new List<ItineraryStopParticipantDTO>();
        }

        /// <summary>
        /// Gets or sets the itinerary group id.
        /// </summary>
        public int ItineraryGroupId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the participants of the group.
        /// </summary>
        public IEnumerable<ItineraryStopParticipantDTO> Participants { get; set; }
    }
}
