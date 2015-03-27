﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// A PublishedProject is a business entity that can be used to update projects that current exist in the ECA system.
    /// </summary>
    public class PublishedProject : IAuditable
    {
        public PublishedProject(
            User updatedBy,
            int projectId,
            string name,
            string description,
            int projectStatusId,
            IEnumerable<int> goalIds,
            IEnumerable<int> themeIds,
            IEnumerable<int> pointsOfContactIds,
            int focusId,
            DateTimeOffset startDate,
            DateTimeOffset endDate
            )
        {
            Contract.Requires(updatedBy != null, "The updated by user must not be null.");
            this.ProjectId = projectId;
            this.Name = name;
            this.Description = description;
            this.ProjectStatusId = projectStatusId;
            this.GoalIds = goalIds ?? new List<int>();
            this.ThemeIds = themeIds ?? new List<int>();
            this.PointsOfContactIds = pointsOfContactIds ?? new List<int>();
            this.FocusId = focusId;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Audit = new Update(updatedBy);
        }

        /// <summary>
        /// Gets the project id.
        /// </summary>
        public int ProjectId { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the project status id.
        /// </summary>
        public int ProjectStatusId { get; private set; }

        /// <summary>
        /// Gets the Themes by id.
        /// </summary>
        public IEnumerable<int> ThemeIds { get; private set; }

        /// <summary>
        /// Gets the goals by Id.
        /// </summary>
        public IEnumerable<int> GoalIds { get; private set; }

        /// <summary>
        /// Gets the points of contact by id.
        /// </summary>
        public IEnumerable<int> PointsOfContactIds { get; private set; }

        /// <summary>
        /// Gets the start date.
        /// </summary>
        public DateTimeOffset StartDate { get; private set; }

        /// <summary>
        /// Gets the end date.
        /// </summary>
        public DateTimeOffset EndDate { get; private set; }

        /// <summary>
        /// Gets the focus by id.
        /// </summary>
        public int FocusId { get; private set; }

        /// <summary>
        /// Gets the Audit.
        /// </summary>
        public Audit Audit
        {
            get;
            private set;
        }
    }
}