using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An AddedEcaItineraryGroup represents a business layer's client request to add a new itinerary group.
    /// </summary>
    public class AddedEcaItineraryGroup : EcaItineraryGroup, IAuditable
    {
        /// <summary>
        /// Creates a new instance with the given data.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="name">The name of the itinerary group.</param>
        /// <param name="participantIds">The participants by id that will be a part of the group.</param>
        public AddedEcaItineraryGroup(User creator, int projectId, int itineraryId, string name, IEnumerable<int> participantIds)
            : base(projectId, name, participantIds)
        {
            this.ItineraryId = itineraryId;
            this.Audit = new Create(creator);
        }

        /// <summary>
        /// Gets the create audit.
        /// </summary>
        public Audit Audit { get; private set; }

        /// <summary>
        /// Gets the itinerary id.
        /// </summary>
        public int ItineraryId { get; private set; }
    }
}
