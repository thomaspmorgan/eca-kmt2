using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// A EcaItinerary is an eca business layer travel object that has a name and start and end dates.
    /// </summary>
    public class EcaItinerary
    {
        /// <summary>
        /// Creates a new instance and initializes it with the given values.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="name">The name</param>
        /// <param name="projectId">The id of the project.</param>
        /// <param name="arrivalLocationId">The location id of the itineraries arrival location.</param>
        /// <param name="departureLocationId">The location id of the itineraries departure location.</param>
        public EcaItinerary(
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            string name,
            int projectId,
            int arrivalLocationId,
            int departureLocationId)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Name = name;
            this.ArrivalLocationId = arrivalLocationId;
            this.DepartureLocationId = departureLocationId;
            this.ProjectId = projectId;
        }

        /// <summary>
        /// Gets the id of the project.
        /// </summary>
        public int ProjectId { get; private set; }

        /// <summary>
        /// Gets the start date.
        /// </summary>
        public DateTimeOffset StartDate { get; private set; }

        /// <summary>
        /// Gets the end date.
        /// </summary>
        public DateTimeOffset EndDate { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the location id of the arrival location.
        /// </summary>
        public int ArrivalLocationId { get; private set; }

        /// <summary>
        /// Gets the location id of the departure location.
        /// </summary>
        public int DepartureLocationId { get; private set; }
    }
}
