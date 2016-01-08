using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An UpdatedEcaItineraryValidationEntity is used to validate an updated itinerary.
    /// </summary>
    public class UpdatedEcaItineraryValidationEntity : EcaItineraryValidationEntity
    {
        /// <summary>
        /// Creates and initializes a new instance.
        /// </summary>
        /// <param name="updatedItinerary">The updated itinerary.</param>
        /// <param name="itineraryToUpdate">The itinerary that will be updated.</param>
        /// <param name="arrivalLocation">The itinerary arrival location.</param>
        /// <param name="departureLocation">The itinerary destination location.</param>
        /// <param name="itineraryStop">The itinerary stops of the itinerary that is updated.</param>
        public UpdatedEcaItineraryValidationEntity(UpdatedEcaItinerary updatedItinerary, Itinerary itineraryToUpdate, Location arrivalLocation, Location departureLocation, IEnumerable<ItineraryStop> itineraryStop)
            : base(
                  arrivalLocation: arrivalLocation, 
                  departureLocation: departureLocation)
        {
            this.ItineraryToUpdate = itineraryToUpdate;
            this.UpdatedItinerary = updatedItinerary;
            this.ItineraryStops = itineraryStop;
        }

        /// <summary>
        /// Gets the updated itinerary.
        /// </summary>
        public UpdatedEcaItinerary UpdatedItinerary { get; private set; }

        /// <summary>
        /// Gets the itinerary that will be updated.
        /// </summary>
        public Itinerary ItineraryToUpdate { get; private set; }

        /// <summary>
        /// Gets the itinerary stops of the itinerary.
        /// </summary>
        public IEnumerable<ItineraryStop> ItineraryStops { get; private set; }
    }
}
