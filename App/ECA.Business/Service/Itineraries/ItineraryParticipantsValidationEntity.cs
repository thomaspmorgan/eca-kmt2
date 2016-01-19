using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// The ItineraryParticipantsValidationEntity is used to validate a business layers request to set participants on an itinerary.
    /// </summary>
    public class ItineraryParticipantsValidationEntity
    {
        /// <summary>
        /// Creates a new instance with the given values.
        /// </summary>
        /// <param name="orphanedParticipantsByParticipantId">The orphaned participants by participant id.</param>
        /// <param name="nonPersonParticipantsByParticipantIds">The participants by id whose participant type is not a person typer.</param>
        public ItineraryParticipantsValidationEntity(IEnumerable<int> orphanedParticipantsByParticipantId, IEnumerable<int> nonPersonParticipantsByParticipantIds)
        {
            this.OrphanedParticipantsByParticipantId = orphanedParticipantsByParticipantId;
            this.NonPersonParticipantsByParticipantId = nonPersonParticipantsByParticipantIds;
        }

        /// <summary>
        /// Gets the participants by id that have been removed from the itinerary but not all itinerary stops.
        /// </summary>
        public IEnumerable<int> OrphanedParticipantsByParticipantId { get; private set; }

        /// <summary>
        /// Gets the ids of participants whose participant type is not a person.
        /// </summary>
        public IEnumerable<int> NonPersonParticipantsByParticipantId { get; private set; }
    }
}
