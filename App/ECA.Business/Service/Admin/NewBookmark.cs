using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Business model for new bookmark
    /// </summary>
    public class NewBookmark
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="officeId">The office id</param>
        /// <param name="programId">The program id</param>
        /// <param name="projectId">The project id</param>
        /// <param name="personId">The person id</param>
        /// <param name="organizationId">The organization id</param>
        /// <param name="principalId">The principal id</param>
        /// <param name="automatic">The automatic flag</param>
        public NewBookmark(int? officeId, int? programId, int? projectId, int? personId, int? organizationId, int principalId, bool automatic)
        {
            this.OfficeId = officeId;
            this.ProgramId = programId;
            this.ProjectId = projectId;
            this.PersonId = personId;
            this.OrganizationId = organizationId;
            this.PrincipalId = principalId;
            this.AddedOn = new DateTimeOffset();
            this.Automatic = automatic;
        }

        /// <summary>
        /// Gets or sets the office id
        /// </summary>
        public int? OfficeId { get; private set; }

        /// <summary>
        /// Gets or sets the program id
        /// </summary>
        public int? ProgramId { get; private set; }

        /// <summary>
        /// Gets or sets the project id
        /// </summary>
        public int? ProjectId { get; private set; }

        /// <summary>
        /// Gets or sets the person id
        /// </summary>
        public int? PersonId { get; private set; }

        /// <summary>
        /// Gets or sets the organization id
        /// </summary>
        public int? OrganizationId { get; private set; }

        /// <summary>
        /// Gets or sets the principal id
        /// </summary>
        public int PrincipalId { get; private set; }

        /// <summary>
        /// The date the bookmark was created on
        /// </summary>
        public DateTimeOffset AddedOn { get; private set; }

        /// <summary>
        /// The flag to determine if the bookmark is automatic
        /// </summary>
        public bool Automatic { get; private set; }
    }
}
