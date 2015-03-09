using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Programs
{
    /// <summary>
    /// The ProgramServiceValidationEntity is a container for all objects that must be validated for a Program.
    /// </summary>
    public class ProgramServiceValidationEntity
    {
        /// <summary>
        /// Creates a new ProgramServiceValidationEntity.
        /// </summary>
        /// <param name="name">The name of the program.</param>
        /// <param name="description">The description of the program.</param>
        /// <param name="regionLocationTypeIds">The region ids.</param>
        /// <param name="focus">The focus.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="parentProgramId">The parent program id.</param>
        /// <param name="parentProgram">The parent program.</param>
        public ProgramServiceValidationEntity(string name, string description, List<int> regionLocationTypeIds, Focus focus, Organization owner, int? parentProgramId, Program parentProgram)
        {
            this.RegionLocationTypeIds = regionLocationTypeIds;
            this.Focus = focus;
            this.OwnerOrganization = owner;
            this.ParentProgramId = parentProgramId;
            this.ParentProgram = parentProgram;
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// Gets the region ids.
        /// </summary>
        public List<int> RegionLocationTypeIds { get; private set; }

        /// <summary>
        /// Gets the focus.
        /// </summary>
        public Focus Focus { get; private set; }

        /// <summary>
        /// Gets or the owner.
        /// </summary>
        public Organization OwnerOrganization { get; private set; }

        /// <summary>
        /// Gets the parent program.
        /// </summary>
        public Program ParentProgram { get; private set; }

        /// <summary>
        /// Gets the parent program id.
        /// </summary>
        public int? ParentProgramId { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gest the description.
        /// </summary>
        public string Description { get; set; }
    }
}
