using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    public class UpdatedEcaItineraryValidationEntity : EcaItineraryValidationEntity
    {
        public UpdatedEcaItineraryValidationEntity(UpdatedEcaItinerary updatedItinerary, Itinerary itineraryToUpdate, Location arrivalLocation, Location departureLocation)
            : base(
                  arrivalLocation: arrivalLocation, 
                  departureLocation: departureLocation)
        {
            this.ItineraryToUpdate = itineraryToUpdate;
            this.UpdatedItinerary = updatedItinerary;
        }
        public UpdatedEcaItinerary UpdatedItinerary { get; private set; }

        public Itinerary ItineraryToUpdate { get; private set; }
    }
}
