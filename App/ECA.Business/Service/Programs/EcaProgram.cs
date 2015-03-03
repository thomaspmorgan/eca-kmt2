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
            int programStatusId) 
        {
            this.Name = name;
            this.Description = description;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.OwnerOrganizationId = ownerOrganizationId;
            this.History = new RevisedHistory(updatedBy);

            var programStatus = ProgramStatus.GetStaticLookup(this.ProgramStatusId);
            if(programStatus == null)
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

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public int OwnerOrganizationId { get; set; }

        public RevisedHistory History { get; set; }

    }

}
