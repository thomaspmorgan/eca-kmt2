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
        /// <param name="contactIds">The list of contacts by id.</param>
        /// <param name="parentProgram">The parent program.</param>
        /// <param name="goalIds">The goals by id.</param>
        /// <param name="regionIds">The regions by id.</param>
        /// <param name="themeIds">The themes by id.</param>
        public ProgramServiceValidationEntity(string name, 
            string description, 
            List<int> regionLocationTypeIds, 
            List<int> contactIds, 
            List<int> themeIds, 
            List<int> goalIds, 
            List<int> regionIds,
            List<int> categoryIds,
            List<int> objectiveIds,
            Focus focus, 
            Organization owner, 
            int? parentProgramId, 
            Program parentProgram)
        {
            this.RegionLocationTypeIds = regionLocationTypeIds;
            this.Focus = focus;
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
        }

        public List<int> CategoryIds { get; private set; }
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
