using ECA.Business.Service;
using ECA.Business.Service.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// A SendToSevisBindingModel is used by a client to request that participatnts be batched and sent to sevis.
    /// </summary>
    public class SendToSevisBindingModel
    {
        /// <summary>
        /// The participants by id to batch for sevis.
        /// </summary>
        public IEnumerable<int> ParticipantIds { get; set; }

        /// <summary>
        /// Gets or sets the sevis username, of the user to batch participants with.
        /// </summary>
        public string SevisUsername { get; set; }

        /// <summary>
        /// Gets or sets the sevis org id to batch participants with.
        /// </summary>
        public string SevisOrgId { get; set; }

        /// <summary>
        /// Returns a ParticipantsToBeSentToSevis business model entity.
        /// </summary>
        /// <param name="user">The user sending the participants to sevis.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>The business layer model.</returns>
        public ParticipantsToBeSentToSevis ToParticipantsToBeSentToSevis(User user, int projectId)
        {
            return new ParticipantsToBeSentToSevis(
                user: user,
                projectId: projectId,
                participantIds: this.ParticipantIds,
                sevisUsername: this.SevisUsername,
                sevisOrgId: this.SevisOrgId
                );
        }
    }
}