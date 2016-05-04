using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
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
        /// <param name="programId">The id of the program.</param>
        /// <param name="name">The name of the program.</param>
        /// <param name="description">The description of the program.</param>
        /// <param name="regionLocationTypeIds">The region ids.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="inactiveRegionIds">The regions by id that are inactive.</param>
        /// <param name="parentProgramId">The parent program id.</param>
        /// <param name="contactIds">The list of contacts by id.</param>
        /// <param name="parentProgram">The parent program.</param>
        /// <param name="goalIds">The goals by id.</param>
        /// <param name="regionIds">The regions by id.</param>
        /// <param name="themeIds">The themes by id.</param>
        /// <param name="categoryIds">The categories by id.</param>
        /// <param name="objectiveIds">The objectives by id.</param>
        /// <param name="ownerOfficeSettings">The office settings for the owner.</param>
        /// <param name="parentProgramParentPrograms">The list of parent programs for the given parent program.  In other words, 
        /// the program may have a new parent program set, so to prevent circular references, the tree of programs for the parent program must
        /// be known and checked.</param>
        public ProgramServiceValidationEntity(
            int programId,
            string name, 
            string description, 
            List<int> regionLocationTypeIds, 
            List<int> inactiveRegionIds,
            List<int> contactIds, 
            List<int> themeIds, 
            List<int> goalIds, 
            List<int> regionIds,
            List<int> categoryIds,
            List<int> objectiveIds,
            Organization owner, 
            OfficeSettings ownerOfficeSettings,
            int? parentProgramId, 
            Program parentProgram,
            List<OrganizationProgramDTO> parentProgramParentPrograms,
            List<int> allowedThemeIds,
            List<int> allowedGoalIds)
        {
            this.ProgramId = programId;
            this.RegionLocationTypeIds = regionLocationTypeIds;
            this.OwnerOrganization = owner;
            this.ParentProgramId = parentProgramId;
            this.ParentProgram = parentProgram;
            this.Name = name;
            this.Description = description;
            this.ContactIds = contactIds;
            this.GoalIds = goalIds;
            this.ThemeIds = themeIds;
            this.RegionIds = regionIds;
            this.CategoryIds = categoryIds;
            this.ObjectiveIds = objectiveIds;
            this.OwnerOfficeSettings = ownerOfficeSettings;
            this.InactiveRegionIds = inactiveRegionIds == null ? new List<int>() : inactiveRegionIds.Distinct();
            this.ParentProgramParentPrograms = parentProgramParentPrograms ?? new List<OrganizationProgramDTO>();
            this.AllowedThemeIds = allowedThemeIds == null ? new List<int>() : allowedThemeIds.Distinct();
            this.AllowedGoalIds = allowedGoalIds == null ? new List<int>() : allowedGoalIds.Distinct();
        }

        /// <summary>
        /// Gets the id of the program being validated.
        /// </summary>
        public int ProgramId { get; private set; }

        /// <summary>
        /// Gets the owner office settings.
        /// </summary>
        public OfficeSettings OwnerOfficeSettings { get; private set; }

        /// <summary>
        /// Gets the category ids assigned to the program.
        /// </summary>
        public List<int> CategoryIds { get; private set; }

        /// <summary>
        /// Gets the objective ids assigned to the program.
        /// </summary>
        public List<int> ObjectiveIds { get; private set; }

        /// <summary>
        /// Gets the goal ids.
        /// </summary>
        public List<int> GoalIds { get; private set; }

        /// <summary>
        /// Gets the theme ids.
        /// </summary>
        public List<int> ThemeIds { get; private set; }

        /// <summary>
        /// Gets the region ids.
        /// </summary>
        public List<int> RegionIds { get; private set; }

        /// <summary>
        /// Gets the contact ids.
        /// </summary>
        public List<int> ContactIds { get; private set; }

        /// <summary>
        /// Gets the region ids.
        /// </summary>
        public List<int> RegionLocationTypeIds { get; private set; }

        /// <summary>
        /// Gets or the owner.
        /// </summary>
        public Organization OwnerOrganization { get; private set; }

        /// <summary>
        /// Gets the parent program.
        /// </summary>
        public Program ParentProgram { get; private set; }

        /// <summary>
        /// Gets or sets all parent programs of the parent program.
        /// </summary>
        public IList<OrganizationProgramDTO> ParentProgramParentPrograms { get; private set; }

        /// <summary>
        /// Gets the parent program id.
        /// </summary>
        public int? ParentProgramId { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gest the description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets or sets the regions by id that are inactive.
        /// </summary>
        public IEnumerable<int> InactiveRegionIds { get; private set; }

        /// <summary>
        /// Gets or sets the allowed theme ids
        /// </summary>
        public IEnumerable<int> AllowedThemeIds { get; private set; }

        /// <summary>
        /// Gets or sets the allowed goal ids
        /// </summary>
        public IEnumerable<int> AllowedGoalIds { get; private set; }
    }
}
