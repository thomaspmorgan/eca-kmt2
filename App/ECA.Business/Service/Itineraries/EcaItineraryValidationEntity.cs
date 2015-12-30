using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An EcaItineraryValidationEntity is the base class used to validate created and updated itineraries.
    /// </summary>
    public class EcaItineraryValidationEntity
    {
        /// <summary>
        /// Creates and initializes a new instance.
        /// </summary>
        /// <param name="arrivalLocation">The arrival location of an itinerary.</param>
        /// <param name="departureLocation">The departure destination location of a itinerary.</param>
        public EcaItineraryValidationEntity(Location arrivalLocation, Location departureLocation)
        {   
            this.ArrivalLocation = arrivalLocation;
            this.DepartureLocation = departureLocation;
        }

        /// <summary>
        /// Gets the arrival location.
        /// </summary>
        public Location ArrivalLocation { get; private set; }

        /// <summary>
        /// Gets the departure destination location.
        /// </summary>
        public Location DepartureLocation { get; private set; }
    }
}
