using ECA.Business.Service;
using ECA.Business.Service.Itineraries;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Itineraries
{
    /// <summary>
    /// The AddedItineraryBindingModel represents a web api client's request to create an itinerary for a project.
    /// </summary>
    public class AddedItineraryBindingModel
    {
        /// <summary>
        /// The start date of the itinerary.
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// The end date of the itinerary.
        /// </summary>
        public DateTimeOffset EndDate { get; set; }

        /// <summary>
        /// The arrival location id.
        /// </summary>
        public int ArrivalLocationId { get; set; }

        /// <summary>
        /// The id of the departure destination location.
        /// </summary>
        public int DepartureLocationId { get; set; }

        /// <summary>
        /// The name of the itinerary.
        /// </summary>
        [Required]
        [MaxLength(Itinerary.NAME_MAX_LENGTH)]
        public string Name { get; set; }

        /// <summary>
        /// Returns a business layer entity to create a new itinerary.
        /// </summary>
        /// <param name="projectId">The id of the project.</param>
        /// <param name="user">The user creating the itinerary.</param>
        /// <returns>The business layer business entity used to create the itinerary.</returns>
        public AddedEcaItinerary ToAddedEcaItinerary(int projectId, User user)
        {
            return new AddedEcaItinerary(
                creator: user,
                startDate: this.StartDate,
                endDate: this.EndDate,
                name: this.Name,
                projectId: projectId,
                arrivalLocationId: this.ArrivalLocationId,
                departureLocationId: this.DepartureLocationId
                );
        }
    }
}