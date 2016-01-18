using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An AddedEcaItineraryStop is used to represent a client's request to add a new itinerary stop.
    /// </summary>
    public class AddedEcaItineraryStop : EcaItineraryStop, IAuditable
    {
        /// <summary>
        /// Creates a new AddedEcaItineraryStop with the given data.
        /// </summary>
        /// <param name="creator">The creator.</param>
        /// <param name="itineraryId">The itinerary by id.</param>
        /// <param name="projectId">The project by id.</param>
        /// <param name="name">The name of the stop.</param>
        /// <param name="arrivalDate">The arrival date.</param>
        /// <param name="departureDate">The departure date.</param>
        /// <param name="destinationLocationId">The destination location by id.</param>
        public AddedEcaItineraryStop(
            User creator,
            int itineraryId,
            int projectId,
            string name,
            DateTimeOffset arrivalDate,
            DateTimeOffset departureDate,
            int destinationLocationId,
            string timezoneId
            ) : base(
                projectId: projectId,
                name: name,
                arrivalDate: arrivalDate,
                departureDate: departureDate,
                destinationLocationId: destinationLocationId,
                timezoneId: timezoneId
                )
        {
            this.Audit = new Create(creator);
            this.ItineraryId = itineraryId;
        }

        /// <summary>
        /// Gets the create audit.
        /// </summary>
        public Audit Audit { get; private set; }

        /// <summary>
        /// Gets the itinerar id.
        /// </summary>
        public int ItineraryId { get; private set; }
    }
}
