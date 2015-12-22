using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Itineraries
{
    /// <summary>
    /// An ItineraryGroupParticipantsDTO represents a collection of people that are in an itinerary group.
    /// </summary>
    public class ItineraryGroupParticipantsDTO : ItineraryGroupDTO
    {
        public ItineraryGroupParticipantsDTO()
        {
            this.People = new List<ItineraryGroupPersonDTO>();
        }

        /// <summary>
        /// Gets or sets the people in the itinerary group.
        /// </summary>
        public IEnumerable<ItineraryGroupPersonDTO> People { get; set; }
    }
}
