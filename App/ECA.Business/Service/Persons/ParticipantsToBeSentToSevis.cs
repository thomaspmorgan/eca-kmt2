using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// A ParticipantsToBeSentToSevis model is used by a business layer client to set participants to be sent to sevis.
    /// </summary>
    public class ParticipantsToBeSentToSevis : IAuditable
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="user">The user requesting the participants be sent to sevis.</param>
        /// <param name="participantIds">The participants by id to send.</param>
        /// <param name="sevisUsername">The sevis username to use when batching participants.</param>
        /// <param name="sevisOrgId">The sevis org id of the participants being sent to sevis.</param>
        /// <param name="projectId">The id of the project the participants belong to.</param>
        public ParticipantsToBeSentToSevis(
            User user, 
            int projectId,
            IEnumerable<int> participantIds,
            string sevisUsername,
            string sevisOrgId)
        {
            Contract.Requires(user != null, "The user must not be null.");
            Contract.Requires(sevisUsername != null, "The sevis username must not be null.");
            Contract.Requires(sevisOrgId != null, "The sevis org id must not be null.");
            this.Audit = new Create(user);
            this.ProjectId = projectId;
            this.ParticipantIds = participantIds ?? new List<int>();
            this.SevisUsername = sevisUsername;
            this.SevisOrgId = sevisOrgId;
        }

        /// <summary>
        /// Gets the audit.
        /// </summary>
        public Audit Audit { get; private set; }

        /// <summary>
        /// Gets the project id the participants belong to.
        /// </summary>
        public int ProjectId { get; private set; }

        /// <summary>
        /// Gets the ids of the participants to send to sevis.
        /// </summary>
        public IEnumerable<int> ParticipantIds { get; private set; }

        /// <summary>
        /// Gets the sevis username of the username submitting participants to sevis.
        /// </summary>
        public string SevisUsername { get; private set; }

        /// <summary>
        /// Gets the sevis org id of the participants to send to sevis.
        /// </summary>
        public string SevisOrgId { get; private set; }
    }
}
