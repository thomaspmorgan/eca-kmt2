using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Business model to for a new organization
    /// </summary>
    public class NewOrganization: IAuditable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user">The user that created the organization</param>
        /// <param name="name">The name of the organization</param>
        /// <param name="description">The description of the organization</param>
        /// <param name="organizationType">The organization type</param>
        /// <param name="organizationRoles">The organization roles</param>
        /// <param name="website">The website</param>
        /// <param name="pointsOfContact">The points of contact</param>
        public NewOrganization(User user, string name, string description, int organizationType,
            List<int> organizationRoles, string website, List<int> pointsOfContact)
        {
            this.Name = name;
            this.Description = description;
            this.OrganizationType = organizationType;
            this.OrganizationRoles = organizationRoles;
            this.Website = website;
            this.PointsOfContact = pointsOfContact;
            this.Audit = new Create(user);
        }

        /// <summary>
        /// Gets and sets the name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets and sets the description
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets and sets the organization type
        /// </summary>
        public int OrganizationType { get; private set; }

        /// <summary>
        /// Gets and sets the organization roles
        /// </summary>
        public List<int> OrganizationRoles { get; private set; }

        /// <summary>
        /// Gets and sets the website
        /// </summary>
        public string Website { get; private set; }

        /// <summary>
        /// Gets and sets the points of contact
        /// </summary>
        public List<int> PointsOfContact { get; private set; }

        /// <summary>
        /// Gets and sets the audit
        /// </summary>
        public Audit Audit { get; private set; }

    }
}
