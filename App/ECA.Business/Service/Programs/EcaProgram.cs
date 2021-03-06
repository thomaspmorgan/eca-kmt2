﻿using ECA.Business.Exceptions;
using ECA.Business.Service;
using ECA.Business.Validation;
using ECA.Core.Data;
using ECA.Core.Exceptions;
using ECA.Core.Generation;
using ECA.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using ECA.Business.Service.Programs;

namespace ECA.Business.Models.Programs
{
    /// <summary>
    /// A EcaProgram is a program in the ECA system.
    /// </summary>
    public class EcaProgram : IAuditable, IConcurrent
    {
        /// <summary>
        /// Creates a new EcaProgram instance.
        /// </summary>
        /// <param name="updatedBy">The user updating the program.</param>
        /// <param name="id">The id of the program.</param>
        /// <param name="name">The name of the program.</param>
        /// <param name="description">The description of the program.</param>
        /// <param name="startDate">The program start date.</param>
        /// <param name="endDate">The program end date.</param>
        /// <param name="ownerOrganizationId">The owner organization id.</param>
        /// <param name="parentProgramId">The parent program id.</param>
        /// <param name="programStatusId">The program status id.</param>
        /// <param name="websites">The websites</param>
        /// <param name="goalIds">The goals of the program by id.</param>
        /// <param name="programRowVersion">The row version of the program.</param>
        /// <param name="pointOfContactIds">The points of contact by id.</param>
        /// <param name="regionIds">The program's regions by id.</param>
        /// <param name="themeIds">The themes by id.</param>
        /// <param name="categoryIds">The focus categories by id.</param>
        /// <param name="objectiveIds">The objectivs by id.</param>
        public EcaProgram(
            User updatedBy,
            int id,
            string name,
            string description,
            DateTimeOffset startDate,
            DateTimeOffset? endDate,
            int ownerOrganizationId,
            int? parentProgramId,
            int programStatusId,
            byte[] programRowVersion,
            List<int> goalIds,
            List<int> pointOfContactIds,
            List<int> themeIds,
            List<int> regionIds,
            List<int> categoryIds,
            List<int> objectiveIds,
            List<WebsiteDTO> websites)
        {
            Contract.Requires(updatedBy != null, "The updated by user must not be null.");
            var programStatus = ProgramStatus.GetStaticLookup(programStatusId);
            if (programStatus == null)
            {
                throw new UnknownStaticLookupException(String.Format("The program status [{0}] is not supported.", programStatusId));
            }

            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.OwnerOrganizationId = ownerOrganizationId;
            this.ParentProgramId = parentProgramId;
            this.GoalIds = goalIds ?? new List<int>();
            this.ContactIds = pointOfContactIds ?? new List<int>();
            this.ThemeIds = themeIds ?? new List<int>();
            this.RegionIds = regionIds ?? new List<int>();
            this.FocusCategoryIds = categoryIds ?? new List<int>();
            this.JustificationObjectiveIds = objectiveIds ?? new List<int>();
            this.Websites = websites ?? new List<WebsiteDTO>();
            this.ProgramStatusId = programStatusId;
            this.Audit = new Update(updatedBy);
            this.RowVersion = programRowVersion;

            this.GoalIds = this.GoalIds.Distinct().ToList();
            this.ContactIds = this.ContactIds.Distinct().ToList();
            this.ThemeIds = this.ThemeIds.Distinct().ToList();
            this.RegionIds = this.RegionIds.Distinct().ToList();
            this.FocusCategoryIds = this.FocusCategoryIds.Distinct().ToList();
            this.JustificationObjectiveIds = this.JustificationObjectiveIds.Distinct().ToList();
        }

        /// <summary>
        /// Gets the Id of the program.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the program status id.
        /// </summary>
        public int ProgramStatusId { get; private set; }

        /// <summary>
        /// Gets the name of the program.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the program description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the program state date.
        /// </summary>
        public DateTimeOffset StartDate { get; private set; }

        /// <summary>
        /// Gets the program end date.
        /// </summary>
        public DateTimeOffset? EndDate { get; private set; }

        /// <summary>
        /// Gets the program's owning organization id.
        /// </summary>
        public int OwnerOrganizationId { get; private set; }

        /// <summary>
        /// Gets the program's parent program id.
        /// </summary>
        public int? ParentProgramId { get; private set; }

        /// <summary>
        /// Gets the program goals by id.
        /// </summary>
        public List<int> GoalIds { get; private set; }

        /// <summary>
        /// Gets the program themes by id.
        /// </summary>
        public List<int> ThemeIds { get; private set; }

        /// <summary>
        /// Gets or sets the focus category ids.
        /// </summary>
        public List<int> FocusCategoryIds { get; set; }

        /// <summary>
        /// Gets or sets the justification objective ids.
        /// </summary>
        public List<int> JustificationObjectiveIds { get; set; }

        /// <summary>
        /// Gets or sets the websites
        /// </summary>
        public List<WebsiteDTO> Websites { get; set; }

        /// <summary>
        /// Gets the program contacts by id.
        /// </summary>
        public List<int> ContactIds { get; private set; }

        /// <summary>
        /// Gets the program regions by id.
        /// </summary>
        public List<int> RegionIds { get; private set; }

        /// <summary>
        /// Gets the program audit.
        /// </summary>
        public Audit Audit { get; protected set; }

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        public byte[] RowVersion { get; set; }
    }
}
