using ECA.Business.Service;
using ECA.Business.Service.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Admin
{
    public class UpdatedOrganizationBindingModel
    {
        /// <summary>
        /// The id of the organization.
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// The name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The organization's website.
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// The type of organization by id.
        /// </summary>
        public int OrganizationTypeId { get; set; }

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
            return new EcaOrganization(
                user: user,
                organizationId: this.OrganizationId,
                website: this.Website,
                organizationTypeId: this.OrganizationTypeId,
                contactIds: this.PointsOfContactIds,
                parentOrganizationId: this.ParentOrganizationId,
                name: this.Name,
                description: this.Description
                );
        }
    }
}