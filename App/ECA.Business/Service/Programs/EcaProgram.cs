using ECA.Business.Service;
using ECA.Data;
using System;
using System.Collections.Generic;

namespace ECA.Business.Models.Programs
{
    /// <summary>
    /// A EcaProgram is a program is a non-draft program in the ECA system.
    /// </summary>
    public class EcaProgram
    {
        public EcaProgram(
            User updatedBy,
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
            this.Name = name;
            this.Description = description;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.OwnerOrganizationId = ownerOrganizationId;
            this.ParentProgramId = parentProgramId;
            this.History = new RevisedHistory(updatedBy);
            this.Focus = focus;
            this.Website = website;
            this.GoalIds = goalIds ?? new List<int>();
            this.PointOfContactIds = pointOfContactIds ?? new List<int>();
            this.ThemeIds = themeIds ?? new List<int>();
            this.History = new RevisedHistory(updatedBy);

            var programStatus = ProgramStatus.GetStaticLookup(programStatusId);
            if (programStatus == null)
            {
                throw new Exception("The program status is not supported.");
            }
            else
            {
                this.ProgramStatusId = programStatus.Id;
            }
        }

        public int ProgramStatusId { get; set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public string Website { get; private set; }

        public string Focus { get; private set; }

        public DateTimeOffset StartDate { get; private set; }

        public DateTimeOffset EndDate { get; private set; }

        public int OwnerOrganizationId { get; private set; }

        public int? ParentProgramId { get; private set; }

        public RevisedHistory History { get; private set; }

        public List<int> GoalIds { get; private set; }

        public List<int> ThemeIds { get; private set; }

        public List<int> PointOfContactIds { get; private set; }


    }

}
