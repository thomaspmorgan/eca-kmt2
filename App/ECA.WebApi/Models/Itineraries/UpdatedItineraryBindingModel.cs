using ECA.Business.Service;
using ECA.Business.Service.Itineraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Itineraries
{
    /// <summary>
    /// An UpdatedItineraryBindingModel represents a web api client's request
    /// </summary>
    public class UpdatedItineraryBindingModel : AddedItineraryBindingModel
    {
        /// <summary>
        /// The id of the itinerary.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Returns a business layer business entity for updating an itinerary.
        /// </summary>
        /// <param name="projectId">The id of the project.</param>
        /// <param name="user">The user performing the update.</param>
        /// <returns>The updated itinerary business layer.</returns>
        public UpdatedEcaItinerary ToUpdatedEcaItinerary(int projectId, User user)
        {
            return new UpdatedEcaItinerary(
                id: this.Id,
                updator: user,
                arrivalLocationId: this.ArrivalLocationId,
                endDate: this.EndDate,
                name: this.Name,
                projectId: projectId,
                departureLocationId: this.DepartureLocationId,
                startDate: this.StartDate
                );
        }
    }
}