using ECA.Business.Service;
using ECA.Business.Service.Itineraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Itineraries
{
    /// <summary>
    /// An AddedEcaItineraryStopBindingModel represents a web api's client request to create a new itinerary stop.
    /// </summary>
    public class AddedEcaItineraryStopBindingModel
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the arrival date.
        /// </summary>
        public DateTimeOffset ArrivalDate { get; set; }

        /// <summary>
        /// Gets the depature date.
        /// </summary>
        public DateTimeOffset DepartureDate { get; set; }

        /// <summary>
        /// Gets the destination location id.
        /// </summary>
        public int DestinationLocationId { get; set; }

        /// <summary>
        /// Returns a business layer added itinerary stop model.
        /// </summary>
        /// <param name="creator">The user creating the itinerary stop.</param>
        /// <param name="itineraryId">The itinerary by id.</param>
        /// <param name="projectId">The project by id.</param>
        /// <returns>The added eca itinerary stop business layer model.</returns>
        public AddedEcaItineraryStop ToAddedEcaItineraryStop(User creator, int itineraryId, int projectId)
        {
            return new AddedEcaItineraryStop(
                creator: creator,
                itineraryId: itineraryId,
                projectId: projectId,
                name: this.Name,
                arrivalDate: this.ArrivalDate,
                departureDate: this.DepartureDate,
                destinationLocationId: this.DestinationLocationId
                );
        }
    }
}