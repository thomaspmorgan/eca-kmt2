using ECA.Business.Queries.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Itineraries
{
    public class ItineraryStopDTO
    {
        public ItineraryStopDTO()
        {
            this.Groups = new List<ItineraryStopGroupDTO>();
            this.Participants = new List<ItineraryStopParticipantDTO>();
        }

        public int ItineraryId { get; set; }

        public int ItineraryStopId { get; set; }

        public int ProjectId { get; set; }

        public string Name { get; set; }

        public DateTimeOffset ArrivalDate { get; set; }

        public DateTimeOffset DepartureDate { get; set; }

        public LocationDTO Destination { get; set; }

        public DateTimeOffset LastRevisedOn { get; set; }

        public int ParticipantsCount { get; set; }

        public IEnumerable<ItineraryStopGroupDTO> Groups { get; set; }

        public IEnumerable<ItineraryStopParticipantDTO> Participants { get; set; }
    }
}
