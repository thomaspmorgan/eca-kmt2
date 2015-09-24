using ECA.Core.Exceptions;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    public class EcaOrganization
    {
        /// <summary>
        /// Creates a new EcaOrganization.
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="website">The website of the organization.</param>
        /// <param name="organizationTypeId">The organization type by id.</param>
        /// <param name="organizationRoleIds">The ids of the organization roles</param>
        /// <param name="contactIds">The ids of the organization contacts.</param>
        /// <param name="parentOrganizationId">The parent organization by id.</param>
        /// <param name="name">The name of the organization.</param>
        /// <param name="description">The description of the organization.</param>
        /// <param name="organizationId">The id of the organization.</param>
        public EcaOrganization(
            User user, 
            int organizationId,
            string website, 
            int organizationTypeId, 
            IEnumerable<int> organizationRoleIds,
            IEnumerable<int> contactIds, 
            int? parentOrganizationId,
            string name,
            string description)
        {
            Contract.Requires(user != null, "The user must not be null.");
            if (OrganizationType.GetStaticLookup(organizationTypeId) == null)
            {
                throw new UnknownStaticLookupException(String.Format("The organization type id [{0}] does not exist.", organizationTypeId));
            }
            this.Update = new Update(user);
            this.Website = website;
            this.OrganizationTypeId = organizationTypeId;
            this.OrganizationRoleIds = organizationRoleIds ?? new List<int>();
            this.ContactIds = contactIds ?? new List<int>();
            this.ParentOrganizationId = parentOrganizationId;
            this.Name = name;
            this.Description = description;
            this.OrganizationId = organizationId;
        }

        /// <summary>
        /// Gets the update audit.
        /// </summary>
        public Audit Update { get; private set; }

        /// <summary>
        /// Gets the organization id.
        /// </summary>
        public int OrganizationId { get; private set; }

        /// <summary>
        /// Gets the website.
        /// </summary>
        public string Website { get; private set; }

        /// <summary>
        /// Gets the organization type id.
        /// </summary>
        public int OrganizationTypeId { get; private set; }

        /// <summary>
        /// Gets the contact ids.
        /// </summary>
        public IEnumerable<int> ContactIds { get; private set; }

        /// <summary>
        /// Gets the organization role ids
        /// </summary>
        public IEnumerable<int> OrganizationRoleIds { get; private set; }

        /// <summary>
        /// Gets the parent organization id.
        /// </summary>
        public int? ParentOrganizationId { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; private set; }
    }
}
