using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Admin
{
    /// <summary>
    /// An OrganizationProgram is an object relating an organization to a program.  The object is used by both stored procedures and linq queries.
    /// The program level is the level from the root owner.
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
        /// Gets or sets the status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the program level.
        /// </summary>
        public int ProgramLevel { get; set; }

        /// <summary>
        /// Gets or sets the program status id.
        /// </summary>
        public int ProgramStatusId { get; set; }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        public decimal SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the id of the creator.
        /// </summary>
        public int CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }
    }

    /// <summary>
    /// An IEqualiltyComparer for OrganizationProgramDTO, useful once the entities have been marshalled by the service.
    /// </summary>
    public class OrganizationProgramDTOComparer : IEqualityComparer<OrganizationProgramDTO>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(OrganizationProgramDTO x, OrganizationProgramDTO y)
        {
            return x.ProgramId == y.ProgramId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(OrganizationProgramDTO obj)
        {
            return obj.ProgramId;
        }
    }
}
