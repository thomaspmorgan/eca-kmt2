using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Admin
{
    /// <summary>
    /// A SimpleOfficeDTO is used to represent an office in a hierarchy.
    /// </summary>
    public class OrganizationHierarchyDTO
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the organization type id.
        /// </summary>
        public int OrganizationTypeId { get; set; }

        /// <summary>
        /// Gets or sets the organization type.
        /// </summary>
        public string OrganizationType { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parent organization id.
        /// </summary>
        public int? ParentOrganization_OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the organization level.
        /// </summary>
        public int OrganizationLevel { get; set; }

        /// <summary>
        /// Gets or sets whether this instance has children.
        /// </summary>
        public bool HasChildren { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the status
        /// </summary>
        public string Status { get; set; }
    }
}
