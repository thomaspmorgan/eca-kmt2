using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Itineraries
{
    public class ItineraryStopGroupDTO
    {
        public ItineraryStopGroupDTO()
        {
            this.Participants = new List<ItineraryStopParticipantDTO>();
        }

        public int ItineraryGroupId { get; set; }

        public string Name { get; set; }

        public IEnumerable<ItineraryStopParticipantDTO> Participants { get; set; }
    }
}
