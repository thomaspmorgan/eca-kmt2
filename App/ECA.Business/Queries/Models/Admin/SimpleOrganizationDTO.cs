using ECA.Business.Service.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Admin
{
    /// <summary>
    /// Simple dto for organization
    /// </summary>
    public class SimpleOrganizationDTO
    {
        /// <summary>
        /// Gets or sets the organization id
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the organization type
        /// </summary>
        public string OrganizationType { get; set; }

        /// <summary>
        /// Gets or sets the organization role ids
        /// </summary>
        public IEnumerable<int> OrganizationRoleIds { get; set; }

        /// <summary>
        /// Gets or sets the organization role values
        /// </summary>
        public IEnumerable<string> OrganizationRoleNames { get; set; }
        /// <summary>
        /// Gets or sets the status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the location
        /// </summary>
        public string Location { get; set; }
    }
}
