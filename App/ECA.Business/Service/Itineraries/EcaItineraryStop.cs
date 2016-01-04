using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    public class EcaItineraryStop
    {
        public EcaItineraryStop(
            int projectId,
            string name,
            DateTimeOffset arrivalDate,
            DateTimeOffset departureDate,
            int destinationLocationId
            )
        {
            this.Name = name;
            this.ArrivalDate = arrivalDate;
            this.DepartureDate = departureDate;
            this.DestinationLocationId = destinationLocationId;
            this.ProjectId = projectId;
        }        

        public string Name { get; private set; }

        public DateTimeOffset ArrivalDate { get; private set; }

        public DateTimeOffset DepartureDate { get; private set; }

        public int DestinationLocationId { get; private set; }

        public int ProjectId { get; private set; }

    }
}
