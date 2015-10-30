using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Admin
{

    /// <summary>
    /// Binding model for a participant organization
    /// </summary>
    public class ParticipantOrganizationBindingModel : CreateOrganizationBindingModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ParticipantOrganizationBindingModel() : base()
        {
        }

        /// <summary>
        /// Gets or sets the project id
        /// </summary>
        [Required]
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the participant type id
        /// </summary>
        [Required]
        public int ParticipantTypeId { get; set; }

        /// <summary>
        /// Converts binding to business model 
        /// </summary>
        /// <returns>Participant organization business model</returns>
        public ParticipantOrganization ToParticipantOrganization(User user)
        {
            return new ParticipantOrganization(user, this.ProjectId, this.ParticipantTypeId, this.Name, this.Description, this.OrganizationType, this.OrganizationRoles,
                this.Website, this.PointsOfContact);
        }
    }
}