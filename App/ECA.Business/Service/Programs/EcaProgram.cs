using ECA.Business.Service;
using ECA.Core.Data;
using ECA.Core.Exceptions;
using ECA.Core.Generation;
using ECA.Data;
using System;
using System.Collections.Generic;

namespace ECA.Business.Models.Programs
{
    /// <summary>
    /// A EcaProgram is a program in the ECA system.
    /// </summary>
    public class EcaProgram : IAuditable
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
        /// <param name="focus">The focus.</param>
        /// <param name="website">The website.</param>
        /// <param name="goalIds">The goals of the program by id.</param>
        /// <param name="pointOfContactIds">The points of contact by id.</param>
        /// <param name="regionIds">The program's regions by id.</param>
        /// <param name="themeIds">The themes by id.</param>
        public EcaProgram(
            User updatedBy,
            int id,
            string name,
            string description,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            int ownerOrganizationId,
            int? parentProgramId,
            int programStatusId,
            string focus,
            string website,
            List<int> goalIds,
            List<int> pointOfContactIds,
            List<int> themeIds,
            List<int> regionIds)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.OwnerOrganizationId = ownerOrganizationId;
            this.ParentProgramId = parentProgramId;
            this.Focus = focus;
            this.Website = website;
            this.GoalIds = goalIds ?? new List<int>();
            this.ContactIds = pointOfContactIds ?? new List<int>();
            this.ThemeIds = themeIds ?? new List<int>();
            this.RegionIds = regionIds ?? new List<int>();

            var programStatus = ProgramStatus.GetStaticLookup(programStatusId);
            if (programStatus == null)
            {
                throw new UnknownStaticLookupException(String.Format("The program status [{0}] is not supported.", programStatusId));
            }
            else
            {
                this.ProgramStatusId = programStatus.Id;
            }
            this.Audit = new Update(updatedBy);
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
        /// gets the program website.
        /// </summary>
        public string Website { get; private set; }

        /// <summary>
        /// Gets the program focus.
        /// </summary>
        public string Focus { get; private set; }

        /// <summary>
        /// Gets the program state date.
        /// </summary>
        public DateTimeOffset StartDate { get; private set; }

        /// <summary>
        /// Gets the program end date.
        /// </summary>
        public DateTimeOffset EndDate { get; private set; }

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

    }
}
