using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ECA.Data;
using ECA.Business.Service.Admin;
using ECA.Business.Service;

namespace ECA.WebApi.Models.Admin
{

    /// <summary>
    /// Binding model for creating an organization
    /// </summary>
    public class CreateOrganizationBindingModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CreateOrganizationBindingModel()
        {
            this.OrganizationRoles = new List<int>();
            this.PointsOfContact = new List<int>();
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [Required]
        [MaxLength(Organization.NAME_MAX_LENGTH)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        [Required]
        [MaxLength(Organization.DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the organization type
        /// </summary>
        [Required]
        public int OrganizationType { get; set; }

        /// <summary>
        /// Gets or sets the organization roles
        /// </summary>
        public List<int> OrganizationRoles { get; set; }

        /// <summary>
        /// Gets or sets the website
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Gets or sets the points of contacts
        /// </summary>
        public List<int> PointsOfContact { get; set; }

        /// <summary>
        /// Gets a new organzation
        /// </summary>
        /// <returns>Gets a new organzation</returns>
        public NewOrganization ToNewOrganization(User user)
        {
            return new NewOrganization(user, this.Name, this.Description, this.OrganizationType, 
                this.OrganizationRoles, this.Website, this.PointsOfContact);
        }
    }
}