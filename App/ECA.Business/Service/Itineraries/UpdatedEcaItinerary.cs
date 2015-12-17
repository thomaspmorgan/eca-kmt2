using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An UpdatedEcaItinerary represents a business layer client's request to update a traveling period for participants in a project.
    /// </summary>
    public class UpdatedEcaItinerary : EcaItinerary, IAuditable
    {
        /// <summary>
        /// Creates and initializes a new UpdatedEcaItinerary instance.
        /// </summary>
        /// <param name="id">The id of the itinerary.</param>
        /// <param name="updator">The updator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <param name="startDate">The start date of the itinerary.</param>
        /// <param name="endDate">The end date of the itinerary.</param>
        /// <param name="name">The name of the itinerary.</param>
        /// <param name="arrivalLocationId">The location id of the itineraries arrival location.</param>
        /// <param name="departureLocationId">The location id of the itineraries departure location.</param>
        public UpdatedEcaItinerary(
            int id,
            User updator,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            string name,
            int projectId,
            int arrivalLocationId,
            int departureLocationId)
            : base(
                startDate: startDate,
                endDate: endDate,
                arrivalLocationId: arrivalLocationId,
                projectId: projectId,
                departureLocationId: departureLocationId,
                name: name)
        {
            this.Id = id;
            this.Audit = new Update(updator);
        }

        /// <summary>
        /// Gets the id of the itinerary.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the audit info.
        /// </summary>
        public Audit Audit { get; private set; }
    }
}
