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
    /// An AdditionalOrganizationProjectParticipant represents the model required of the client
    /// to add an organization as a participant to a project.
    /// </summary>
    public class AdditionalOrganizationProjectPariticipantBindingModel
    {
        /// <summary>
        /// The project to add the participant to by id.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// The organization to add as a participant to a project by id.
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// The participant type id of this person.
        /// </summary>
        public int ParticipantTypeId { get; set; }


        /// <summary>
        /// Returns an AdditionalOrganizationProjectParticipant for use in the ECA Business layer.
        /// </summary>
        /// <param name="user">The user adding the participant.</param>
        /// <returns>The AdditionalOrganizationProjectParticipant business entity.</returns>
        public AdditionalOrganizationProjectParticipant ToAdditionalOrganizationProjectParticipant(User user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            return new AdditionalOrganizationProjectParticipant(user, this.ProjectId, this.OrganizationId, this.ParticipantTypeId);
        }
    }
}