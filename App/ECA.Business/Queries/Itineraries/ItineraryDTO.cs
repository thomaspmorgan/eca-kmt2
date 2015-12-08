using ECA.Business.Queries.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Itineraries
{
    /// <summary>
    /// An ItineraryDTO contains overview details about an itinerary.
    /// </summary>
    public class ItineraryDTO
    {
        /// <summary>
        /// Gets or sets the id of the itinerary.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the participants participating in the itinerary.
        /// </summary>
        public int ParticipantsCount { get; set; }

        /// <summary>
        /// Gets or sets the number of groups in the itinerary.
        /// </summary>
        public int GroupsCount { get; set; }

        /// <summary>
        /// Gets or sets the arrival location.
        /// </summary>
        public LocationDTO ArrivalLocation { get; set; }

        /// <summary>
        /// Gets or sets the departure location.
        /// </summary>
        public LocationDTO DepartureLocation { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        public DateTimeOffset EndDate { get; set; }

        /// <summary>
        /// Gets or sets the last revised on date.
        /// </summary>
        public DateTimeOffset LastRevisedOn { get; set; }
    }
}
