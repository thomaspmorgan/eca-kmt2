﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Office
{
    /// <summary>
    /// A SimpleOfficeDTO is used to represent an office in a hierarchy.
    /// </summary>
    public class SimpleOfficeDTO
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
        /// Gets or sets the organization symbol.
        /// </summary>
        public string OfficeSymbol { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the parent organization id.
        /// </summary>
        public int? ParentOrganization_OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the office level.
        /// </summary>
        public int OfficeLevel { get; set; }

        /// <summary>
        /// Gets or sets the number of children.
        /// </summary>
        public int NumberOfChildren { get; set; }
    }
}
