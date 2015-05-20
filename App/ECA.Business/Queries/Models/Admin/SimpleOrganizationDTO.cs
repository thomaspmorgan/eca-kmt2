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
        /// Gets or sets the organization type
        /// </summary>
        public string OrganizationType { get; set; }

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
