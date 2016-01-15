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
        /// <param name="participantIds">The participants by id that are participanting in this project itinerary.</param>
        public AddedEcaItinerary(
            User creator,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            string name,
            int projectId,
            int arrivalLocationId,
            int departureLocationId,
            IEnumerable<int> participantIds)
            : base(
                startDate: startDate,
                endDate: endDate,
                arrivalLocationId: arrivalLocationId,
                projectId: projectId,
                departureLocationId: departureLocationId,
                name: name,
                participantIds: participantIds)
        {
            this.Audit = new Create(creator);
        }

        /// <summary>
        /// Gets the audit.
        /// </summary>
        public Audit Audit { get; private set; }
    }
}
