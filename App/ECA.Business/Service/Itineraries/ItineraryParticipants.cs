using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    public class ItineraryParticipants : IAuditable
    {
        public ItineraryParticipants(User updator, int projectId, int itineraryId, IEnumerable<int> participantIds)
        {
            this.ProjectId = projectId;
            this.ItineraryId = itineraryId;
            this.Audit = new Update(updator);
            this.ParticipantIds = participantIds ?? new List<int>();
            this.ParticipantIds = this.ParticipantIds.Distinct();
        }

        /// <summary>
        /// Gets the itinerary id.
        /// </summary>
        public int ItineraryId { get; private set; }

        /// <summary>
        /// Gets the project id.
        /// </summary>
        public int ProjectId { get; private set; }

        /// <summary>
        /// Gets the participants by id.
        /// </summary>
        public IEnumerable<int> ParticipantIds { get; private set; }

        /// <summary>
        /// Gets the audit details.
        /// </summary>
        public Audit Audit { get; protected set; }
    }
}
