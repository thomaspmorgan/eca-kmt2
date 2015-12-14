using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An AddedEcaItinerary represents a business layer client's request to add a new traveling period for participants in a project.
    /// </summary>
    public class AddedEcaItinerary : EcaItinerary, IAuditable
    {
        /// <summary>
        /// Creates and initializes a new AddedEcaItinerary instance.
        /// </summary>
        /// <param name="creator">The creator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <param name="startDate">The start date of the itinerary.</param>
        /// <param name="endDate">The end date of the itinerary.</param>
        /// <param name="name">The name of the itinerary.</param>
        /// <param name="arrivalLocationId">The location id of the itineraries arrival location.</param>
        /// <param name="departureLocationId">The location id of the itineraries departure location.</param>
        public AddedEcaItinerary(
            User creator,
            int projectId,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            string name,
            int arrivalLocationId,
            int departureLocationId)
            : base(
                startDate,
                endDate,
                name,
                arrivalLocationId,
                departureLocationId)
        {
            this.Audit = new Create(creator);
            this.ProjectId = projectId;
        }

        /// <summary>
        /// Gets the id of the project.
        /// </summary>
        public int ProjectId { get; private set; }

        /// <summary>
        /// Gets the audit.
        /// </summary>
        public Audit Audit { get; private set; }
    }
}
