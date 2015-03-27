using ECA.Core.Exceptions;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
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
        /// <param name="focus">The focus of the project.</param>
        /// <param name="goalsExist">The boolean value indicating whether all goals exist in the system.</param>
        /// <param name="themesExist">The boolean value indicating whether all themes exist in the system.</param>
        /// <param name="pointsOfContactExist">the boolean valud indicating whether all points of contact in the system.</param>
        public ProjectServiceUpdateValidationEntity(
            PublishedProject updatedProject, 
            Project projectToUpdate, 
            Focus focus,
            bool goalsExist, 
            bool themesExist, 
            bool pointsOfContactExist)
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
            this.Focus = focus;
            this.GoalsExist = goalsExist;
            this.ThemesExist = themesExist;
            this.PointsOfContactExist = pointsOfContactExist;
            this.UpdatedProjectStatusId = updatedProject.ProjectStatusId;
            this.OriginalProjectStatusId = projectToUpdate.ProjectStatusId;
            this.StartDate = updatedProject.StartDate;
            this.EndDate = updatedProject.EndDate;
        }

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
        /// Gets the focus of the project.
        /// </summary>
        public Focus Focus { get; private set; }

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
    }
}
