using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// An UpdatedOrganizationBindingModel is used to handle a client's request to update an organization.
    /// </summary>
    public class UpdatedOrganizationBindingModel
    {
        /// <summary>
        /// The id of the organization.
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// The name.
        /// </summary>
        [Required]
        [MaxLength(Organization.NAME_MAX_LENGTH)]
        public string Name { get; set; }

        /// <summary>
        /// The description.
        /// </summary>
        [Required]
        [MaxLength(Organization.DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }

        /// <summary>
        /// The organization's website.
        /// </summary>
        [MaxLength(Organization.WEBSITE_MAX_LENGTH)]
        public string Website { get; set; }

        /// <summary>
        /// The type of organization by id.
        /// </summary>
        public int OrganizationTypeId { get; set; }

        /// <summary>
        /// Gets and sets the organization role ids
        /// </summary>
        public IEnumerable<int> OrganizationRoleIds { get; set; }

        /// <summary>
        /// The points of contact of the project by id.
        /// </summary>
        public IEnumerable<int> PointsOfContactIds { get; set; }

        /// <summary>
        /// The parent organization by id.
        /// </summary>
        public int? ParentOrganizationId { get; set; }

        /// <summary>
        /// Returns the business eca organization entity.
        /// </summary>
        /// <param name="user">The user performing the update.</param>
        /// <returns>The business entity.</returns>
        public EcaOrganization ToEcaOrganization(ECA.Business.Service.User user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            return new EcaOrganization(
                user: user,
                organizationId: this.OrganizationId,
                website: this.Website,
                organizationTypeId: this.OrganizationTypeId,
                organizationRoleIds: this.OrganizationRoleIds,
                contactIds: this.PointsOfContactIds,
                parentOrganizationId: this.ParentOrganizationId,
                name: this.Name,
                description: this.Description
                );
        }
    }
}