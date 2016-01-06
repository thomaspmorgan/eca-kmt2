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
    public class UpdatedEcaItineraryStopBindingModel : AddedEcaItineraryStopBindingModel
    {   
        /// <summary>
        /// Gets the destination location id.
        /// </summary>
        public int ItineraryStopId { get; set; }

        /// <summary>
        /// Returns a business layer updated itinerary stop model.
        /// </summary>
        /// <param name="updator">The user updating the itinerary stop.</param>
        /// <param name="projectId">The project by id.</param>
        /// <returns>The updated eca itinerary stop business layer model.</returns>
        public UpdatedEcaItineraryStop ToUpdatedEcaItineraryStop(User updator, int projectId)
        {
            return new UpdatedEcaItineraryStop(
                updator: updator,
                itineraryStopId: this.ItineraryStopId,
                projectId: projectId,
                name: this.Name,
                arrivalDate: this.ArrivalDate,
                departureDate: this.DepartureDate,
                destinationLocationId: this.DestinationLocationId
                );
        }
    }
}