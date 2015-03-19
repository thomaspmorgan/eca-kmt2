using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Admin
{
    /// <summary>
    /// An OrganizationProgram is an object from the GetPrograms stored procedure.  The program level
    /// is the level from the root owner.
    /// </summary>
    public class OrganizationProgramDTO
    {
        /// <summary>
        /// Gets or sets the Program Id.
        /// </summary>
        public int ProgramId { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the parent program id.
        /// </summary>
        public int? ParentProgram_ProgramId { get; set; }

        /// <summary>
        /// Gets or sets the owner organization id.
        /// </summary>
        public int Owner_OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the org name.
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// Gets or sets the office symbol.
        /// </summary>
        public string OfficeSymbol { get; set; }

        /// <summary>
        /// Gets or sets the program level.
        /// </summary>
        public int ProgramLevel { get; set; }
    }
}
