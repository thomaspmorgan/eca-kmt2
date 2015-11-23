using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// A business entity to represent a participant that must be deleted by the business layer.
    /// </summary>
    public class DeletedParticipant : IAuditable
    {
        /// <summary>
        /// Creates a new DeletedParticipant business entity to represent a participant that must be dleted.
        /// </summary>
        /// <param name="deletor">The user performing the delete.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <param name="participantId">the id of the participant.</param>
        public DeletedParticipant(User deletor, int projectId, int participantId)
        {
            this.Audit = new Delete(deletor);
            this.ParticipantId = participantId;
            this.ProjectId = projectId;
        }

        /// <summary>
        /// Gets the delete audit.
        /// </summary>
        public Audit Audit { get; private set; }

        /// <summary>
        /// Gets the participant id.
        /// </summary>
        public int ParticipantId { get; private set; }

        /// <summary>
        /// Gets the project id of the participant.
        /// </summary>
        public int ProjectId { get; private set; }
    }
}
