using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An EcaItineraryGroup represents a business layer's client's required data.
    /// </summary>
    public class EcaItineraryGroup
    {
        /// <summary>
        /// Creates a new instance with the given data.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="name">The name of the itinerary group.</param>
        /// <param name="participantIds">The participants by id that will be a part of the group.</param>
        public EcaItineraryGroup(int projectId, string name, IEnumerable<int> participantIds)
        {
            this.ProjectId = projectId;
            this.Name = name;
            this.ParticipantIds = participantIds ?? new List<int>();
            this.ParticipantIds = this.ParticipantIds.Distinct();
        }

        /// <summary>
        /// Gets the project id.
        /// </summary>
        public int ProjectId { get; private set; }

        /// <summary>
        /// Gets the group name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the participants by id of the group.
        /// </summary>
        public IEnumerable<int> ParticipantIds { get; private set; }
    }
}
