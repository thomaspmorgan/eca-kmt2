using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Itineraries
{
    /// <summary>
    /// An ItineraryGroupDTO represents an itinerary group in the eca system.
    /// </summary>
    public class ItineraryGroupDTO
    {
        /// <summary>
        /// Gets or sets the itinerary id.
        /// </summary>
        public int ItineraryId { get; set; }

        /// <summary>
        /// Gets or sets the itinerary name.
        /// </summary>
        public string ItineraryName { get; set; }

        /// <summary>
        /// Gets or sets the itinerary group id.
        /// </summary>
        public int ItineraryGroupId { get; set; }

        /// <summary>
        /// Gets or sets the name of the itinerary group.
        /// </summary>
        public string ItineraryGroupName { get; set; }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        public int ProjectId { get; set; }
    }
}
