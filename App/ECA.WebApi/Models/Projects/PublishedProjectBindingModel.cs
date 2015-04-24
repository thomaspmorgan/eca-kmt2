using ECA.Business.Service;
using ECA.Business.Service.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Projects
{
    /// <summary>
    /// The model used by a client to update a project
    /// </summary>
    public class PublishedProjectBindingModel
    {
        /// <summary>
        /// The id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the project.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The description of the project.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// The status of the project by id.
        /// </summary>
        public int ProjectStatusId { get; set; }

        /// <summary>
        /// The themes of the project by id.
        /// </summary>
        [Required]
        public IEnumerable<int> ThemeIds { get; set; }

        /// <summary>
        /// The goals of the project by Id.
        /// </summary>
        [Required]
        public IEnumerable<int> GoalIds { get; set; }

        /// <summary>
        /// The points of contact of the project by id.
        /// </summary>
        [Required]
        public IEnumerable<int> PointsOfContactIds { get; set; }

        /// <summary>
        /// The start date of the project.
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// The end date of the project.
        /// </summary>
        public DateTimeOffset EndDate { get; set; }

        /// <summary>
        /// The focus of the project by id.
        /// </summary>
        public int FocusId { get; set; }

        /// <summary>
        /// Returns a business entity from this binding model.
        /// </summary>
        /// <param name="user">The user making the change.</param>
        /// <returns>The PublishedProject business entity.</returns>
        public PublishedProject ToPublishedProject(User user)
        {
            return new PublishedProject(
                updatedBy: user,
                projectId: this.Id,
                name: this.Name,
                description: this.Description,
                projectStatusId: this.ProjectStatusId,
                goalIds: this.GoalIds,
                themeIds: this.ThemeIds,
                pointsOfContactIds: this.PointsOfContactIds,
                focusId: this.FocusId,
                startDate: this.StartDate,
                endDate: this.EndDate);
        }
    }
}