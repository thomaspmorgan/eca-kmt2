using ECA.Business.Queries.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Itineraries
{
    public class ItineraryParticipantDTO
    {
        public int ParticipantId { get; set; }

        public int PersonId { get; set; }

        public string FullName { get; set; }
    }

    public class ItineraryStopParticipantDTO : ItineraryParticipantDTO
    {
        public int ItineraryInformationId { get; set; }

        public LocationDTO TravelingFrom { get; set; }

        public int ItineraryStopId { get; set; }
    }
}
