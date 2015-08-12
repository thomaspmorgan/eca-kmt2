using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    /// <summary>
    /// A bookmark is a saved page to a record in KMT
    /// </summary>
    public class Bookmark
    {
        /// <summary>
        /// Gets or sets the BookmarkId
        /// </summary>
        public int BookmarkId { get; set; }

        /// <summary>
        /// Gets or sets the OfficeId - foreign key to OrganizationId
        /// </summary>
        public int? OfficeId { get; set; }

        /// <summary>
        /// Gets or sets the Office
        /// </summary>
        public Organization Office { get; set; }

        /// <summary>
        /// Gets or sets the ProgramId foreign key
        /// </summary>
        public int? ProgramId { get; set; }

        /// <summary>
        /// Gets or sets the Program
        /// </summary>
        public Program Program { get; set; }

        /// <summary>
        /// Gets or sets the Project Id foreign key
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// gets or sets the Project
        /// </summary>
        public Project Project { get; set; }

        /// <summary>
        /// Gets or sets the person id
        /// </summary>
        public int? PersonId { get; set; }

        /// <summary>
        /// Gets or sets the person
        /// </summary>
        public Person Person { get; set; }

        /// <summary>
        /// Gets or sets the OrganizationId foreign key
        /// </summary>
        public int? OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the Organization
        /// </summary>
        public Organization Organization { get; set; }

        /// <summary>
        /// Gets or sets the user that owns the bookmark
        /// </summary>
        public int PrincipalId { get; set; }

        /// <summary>
        /// The user who owns the bookmark
        /// </summary>
        public virtual UserAccount User { get; set; }

        /// <summary>
        /// When the bookmark was added
        /// </summary>
        public DateTimeOffset AddedOn { get; set; }

        /// <summary>
        /// True if the bookmark was added automatically, false if manual
        /// </summary>
        public bool Automatic { get; set; }



    }
}
