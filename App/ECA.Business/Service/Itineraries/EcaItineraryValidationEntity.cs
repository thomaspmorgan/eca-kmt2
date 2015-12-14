using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    public class EcaItineraryValidationEntity
    {
        public EcaItineraryValidationEntity(Location arrivalLocation, Location departureLocation)
        {   
            this.ArrivalLocation = arrivalLocation;
            this.DepartureLocation = departureLocation;
        }
        
        public UpdatedEcaItinerary UpdatedItinerary { get; private set; }

        public Location ArrivalLocation { get; private set; }

        public Location DepartureLocation { get; private set; }
    }
}
