using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An EcaItineraryStop is the base class used to represent an added itinerary stop or an updated itinerar stop.
    /// </summary>
    public class EcaItineraryStop
    {
        /// <summary>
        /// Creates a new instance with the given data.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="name">The name of the stop.</param>
        /// <param name="arrivalDate">The itinerary stop arrival date.</param>
        /// <param name="departureDate">The departure date.</param>
        /// <param name="destinationLocationId">The destination location id.</param>
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

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the arrival date.
        /// </summary>
        public DateTimeOffset ArrivalDate { get; private set; }

        /// <summary>
        /// Gets the depature date.
        /// </summary>
        public DateTimeOffset DepartureDate { get; private set; }

        /// <summary>
        /// Gets the destination location id.
        /// </summary>
        public int DestinationLocationId { get; private set; }

        /// <summary>
        /// Gets the project id.
        /// </summary>
        public int ProjectId { get; private set; }

    }
}
