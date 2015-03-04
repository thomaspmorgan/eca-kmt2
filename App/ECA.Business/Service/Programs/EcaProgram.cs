using ECA.Business.Service;
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
            List<int> themeIds)
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

            var programStatus = ProgramStatus.GetStaticLookup(programStatusId);
            if (programStatus == null)
            {
                throw new Exception("The program status is not supported.");
            }
            else
            {
                this.ProgramStatusId = programStatus.Id;
            }
            this.Audit = new Update(updatedBy);
        }

        public int Id { get; private set; }

        public int ProgramStatusId { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public string Website { get; private set; }

        public string Focus { get; private set; }

        public DateTimeOffset StartDate { get; private set; }

        public DateTimeOffset EndDate { get; private set; }

        public int OwnerOrganizationId { get; private set; }

        public int? ParentProgramId { get; private set; }

        public List<int> GoalIds { get; private set; }

        public List<int> ThemeIds { get; private set; }

        public List<int> ContactIds { get; private set; }

        public Audit Audit { get; protected set; }
    }

}
