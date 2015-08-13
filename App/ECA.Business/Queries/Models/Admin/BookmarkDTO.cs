using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Admin
{
    /// <summary>
    /// Data transfer object for bookmark
    /// </summary>
    public class BookmarkDTO
    {
        /// <summary>
        /// Gets or sets bookmark id
        /// </summary>
        public int BookmarkId { get; set; }

        /// <summary>
        /// Gets or sets office id
        /// </summary>
        public int? OfficeId { get; set; }

        /// <summary>
        /// Gets or sets program id
        /// </summary>
        public int? ProgramId { get; set; }

        /// <summary>
        /// Gets or sets project id
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// Gets or sets person id
        /// </summary>
        public int? PersonId { get; set; }

        /// <summary>
        /// Gets or sets organization id
        /// </summary>
        public int? OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets principal id
        /// </summary>
        public int PrincipalId { get; set; }

        /// <summary>
        /// Gets or sets added on
        /// </summary>
        public DateTimeOffset AddedOn { get; set; }

        /// <summary>
        /// Gets or sets automatic
        /// </summary>
        public bool Automatic { get; set; }

        /// <summary>
        /// Gets or sets type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets office symbol or status
        /// </summary>
        public string OfficeSymbolOrStatus { get; set; }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name { get; set; }
    }
}
