using ECA.Business.Service.Admin;
using ECA.Core.Exceptions;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Service.Projects
{
    /// <summary>
    /// A ProjectServiceUpdateValidationEntity is used by a ProjectServiceValidator when validating updates
    /// on a project.
    /// </summary>
    public class ProjectServiceUpdateValidationEntity
    {
        /// <summary>
        /// Creates a new ProjectServiceValidationEntity.
        /// </summary>
        /// <param name="updatedProject">The updated project.</param>
        /// <param name="projectToUpdate">The project to be updated.</param>
        /// <param name="locationsExist">The boolean value indicating whether all locations exist in the system.</param>
        /// <param name="categoriesExist">The boolean value indicating whether all categories exist in the system.</param>
        /// <param name="objectivesExist">The boolean value indicating whether all objectives exist in the system.</param>
        /// <param name="newInactiveLocations">The ids of locations that are inactive and were not previously set on the project.</param>
        /// <param name="goalsExist">The boolean value indicating whether all goals exist in the system.</param>
        /// <param name="themesExist">The boolean value indicating whether all themes exist in the system.</param>
        /// <param name="pointsOfContactExist">the boolean valud indicating whether all points of contact in the system.</param>
        /// <param name="numberOfCategories">The number of categories.</param>
        /// <param name="numberOfObjectives">The number of objectives.</param>
        /// <param name="officeSettings">The office settings.</param>
        /// <param name="allowedCategoryIds">The category ids the project can be assigned as deteremined by the parent program.</param>
        /// <param name="allowedObjectiveIds">The objective ids the project can be assigned as determined by the parent program.</param>
        /// <param name="regionLocationTypeIds">The location type ids for the given project regions.</param>
        public ProjectServiceUpdateValidationEntity(
            PublishedProject updatedProject, 
            Project projectToUpdate, 
            List<int> newInactiveLocations,
            bool goalsExist, 
            bool themesExist, 
            bool pointsOfContactExist,
            bool categoriesExist,
            bool objectivesExist,
            bool locationsExist,
            int numberOfObjectives,
            int numberOfCategories,
            IEnumerable<int> allowedCategoryIds,
            IEnumerable<int> allowedObjectiveIds,
            IEnumerable<int> regionLocationTypeIds,
            OfficeSettings officeSettings)
        {
            Contract.Requires(updatedProject != null, "The updated project must not be null.");
            Contract.Requires(projectToUpdate != null, "The project to update must not be null.");

            var updatedProjectStatus = ProjectStatus.GetStaticLookup(updatedProject.ProjectStatusId);
            if (updatedProjectStatus == null)
            {
                throw new UnknownStaticLookupException(String.Format("The project status with id [{0}] is not recognized.", updatedProject.ProjectStatusId));
            }
            Contract.Assert(ProjectStatus.GetStaticLookup(projectToUpdate.ProjectStatusId) != null, "The project to update should have a valid project status.");
            this.Name = updatedProject.Name;
            this.Description = updatedProject.Description;
            this.GoalsExist = goalsExist;
            this.ThemesExist = themesExist;
            this.LocationsExist = locationsExist;
            this.PointsOfContactExist = pointsOfContactExist;
            this.CategoriesExist = categoriesExist;
            this.ObjectivesExist = objectivesExist;
            this.UpdatedProjectStatusId = updatedProject.ProjectStatusId;
            this.OriginalProjectStatusId = projectToUpdate.ProjectStatusId;
            this.StartDate = updatedProject.StartDate;
            this.EndDate = updatedProject.EndDate;
            this.NumberOfCategories = numberOfCategories;
            this.NumberOfObjectives = numberOfObjectives;
            this.OfficeSettings = officeSettings;
            this.CategoryIds = updatedProject.CategoryIds;
            this.ObjectiveIds = updatedProject.ObjectiveIds;
            this.AllowedCategoryIds = allowedCategoryIds == null ? new List<int>() : allowedCategoryIds.Distinct();
            this.AllowedObjectiveIds = allowedObjectiveIds == null ? new List<int>() : allowedObjectiveIds.Distinct();
            this.NewInactiveLocationIds = newInactiveLocations == null ? new List<int>() : newInactiveLocations.Distinct();
            this.RegionLocationTypeIds = regionLocationTypeIds == null ? new List<int>() : regionLocationTypeIds.Distinct();
        }

        /// <summary>
        /// Gets the ids of locations that are inactive and were not previously set on the project.
        /// </summary>
        public IEnumerable<int> NewInactiveLocationIds { get; private set; }

        /// <summary>
        /// Gets the region location type ids.
        /// </summary>
        public IEnumerable<int> RegionLocationTypeIds { get; private set; }

        /// <summary>
        /// Gets the category ids.
        /// </summary>
        public IEnumerable<int> CategoryIds { get; private set; }

        /// <summary>
        /// Gets the objective ids.
        /// </summary>
        public IEnumerable<int> ObjectiveIds { get; private set; }

        /// <summary>
        /// Gets the allowed category (focus) ids for the project.
        /// </summary>
        public IEnumerable<int> AllowedCategoryIds { get; private set; }

        /// <summary>
        /// Gets the allowed objective (justification) ids for the project.
        /// </summary>
        public IEnumerable<int> AllowedObjectiveIds { get; private set; }

        /// <summary>
        /// Gets the office settings.
        /// </summary>
        public OfficeSettings OfficeSettings { get; private set; }

        /// <summary>
        /// Gets the number of categories associated to the project.
        /// </summary>
        public int NumberOfCategories { get; private set; }

        /// <summary>
        /// Gets the number of objectives associated to the project.
        /// </summary>
        public int NumberOfObjectives { get; private set; }

        /// <summary>
        /// Gets the start date.
        /// </summary>
        public DateTimeOffset StartDate { get; private set; }

        /// <summary>
        /// Gets the end date.
        /// </summary>
        public DateTimeOffset EndDate { get; private set; }

        /// <summary>
        /// Gets or sets the original project status id.
        /// </summary>
        public int OriginalProjectStatusId { get; private set; }

        /// <summary>
        /// Gets the new project status id.
        /// </summary>
        public int UpdatedProjectStatusId { get; private set; }

        /// <summary>
        /// Gets the name of the project.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the description of the project.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the value indicating whether all goals exist.
        /// </summary>
        public bool GoalsExist { get; private set; }

        /// <summary>
        /// Gets the value indicating whether all themes exist.
        /// </summary>
        public bool ThemesExist { get; private set; }

        /// <summary>
        /// Gets the value indicating all points of contact exist.
        /// </summary>
        public bool PointsOfContactExist { get; private set; }

        /// <summary>
        /// Gets the value indicating all categories exist.
        /// </summary>
        public bool CategoriesExist { get; private set; }

        /// <summary>
        /// Gets the value indicating all objectives exist.
        /// </summary>
        public bool ObjectivesExist { get; private set; }

        /// <summary>
        /// Gets the value indicating all locations exist.
        /// </summary>
        public bool LocationsExist { get; private set; }
    }
}
