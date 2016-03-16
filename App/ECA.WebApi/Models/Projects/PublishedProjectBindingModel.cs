using ECA.Business.Service;
using ECA.Business.Service.Projects;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [MaxLength(Project.MAX_NAME_LENGTH)]
        public string Name { get; set; }

        /// <summary>
        /// The description of the project.
        /// </summary>
        [Required]
        [MaxLength(Project.MAX_DESCRIPTION_LENGTH)]
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
        /// The category ids.
        /// </summary>
        [Required]
        public IEnumerable<int> CategoryIds { get; set; }

        /// <summary>
        /// The objective ids.
        /// </summary>
        [Required]
        public IEnumerable<int> ObjectiveIds { get; set; }

        /// <summary>
        /// The location ids.
        /// </summary>
        [Required]
        public IEnumerable<int> LocationIds { get; set; }

        /// <summary>
        /// The region ids.
        /// </summary>
        public IEnumerable<int> RegionIds { get; set; }

        /// <summary>
        /// The start date of the project.
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// The end date of the project.
        /// </summary>
        public DateTimeOffset EndDate { get; set; }

        /// <summary>
        /// The Visitor Type Id for the app
        /// </summary>
        public int  VisitorTypeId { get; set; }

        /// <summary>
        /// The number of US Participants, Esimated
        /// </summary>
        public int? UsParticipantsEst { get; set; }

        /// <summary>
        /// The number of Non-US Participants, Esimated
        /// </summary>
        public int? NonUsParticipantsEst { get; set; }

        /// <summary>
        /// The number of US Participants, Actual
        /// </summary>
        public int? UsParticipantsActual { get; set; }

        /// <summary>
        /// The number of Non-US Participants, Actual
        /// </summary>
        public int? NonUsParticipantsActual { get; set; }

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
                categoryIds: this.CategoryIds,
                objectiveIds: this.ObjectiveIds,
                locationIds: this.LocationIds,
                regionIds: this.RegionIds,
                startDate: this.StartDate,
                endDate: this.EndDate,
                visitorTypeId: this.VisitorTypeId,
                usParticipantsEst: this.UsParticipantsEst,
                nonUsParticipantsEst: this.NonUsParticipantsEst,
                usParticipantsActual: this.UsParticipantsActual,
                nonUsParticipantsActual: this.NonUsParticipantsActual);
        }
    }
}