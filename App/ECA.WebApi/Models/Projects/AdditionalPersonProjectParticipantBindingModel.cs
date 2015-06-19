using ECA.Business.Service;
using ECA.Business.Service.Projects;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Projects
{
    /// <summary>
    /// An AdditionalPersonProjectPariticipantBindingModel represents the model required of the client
    /// to add a person as a participant to a project.
    /// </summary>
    public class AdditionalPersonProjectParticipantBindingModel
    {
        /// <summary>
        /// The project to add the participant to by id.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// The person to add as a participant to a project by id.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// The participant type id of this person.
        /// </summary>
        public int ParticipantTypeId { get; set; }

        /// <summary>
        /// Returns an AdditionalPersonProjectParticipant for use in the ECA Business layer.
        /// </summary>
        /// <param name="user">The user adding the participant.</param>
        /// <returns>The AdditionalPersonProjectParticipant business entity.</returns>
        public AdditionalPersonProjectParticipant ToAdditionalPersonProjectParticipant(User user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            return new AdditionalPersonProjectParticipant(user, this.ProjectId, this.PersonId, this.ParticipantTypeId);
        }
    }
}