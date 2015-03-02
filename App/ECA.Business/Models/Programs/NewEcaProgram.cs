using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Models.Programs
{
    public class NewEcaProgram
    {
        public NewEcaProgram(
            int creatorId,
            string name, 
            string description, 
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            int programTypeId,
            int? ownerOrganizationId,
            List<int> contactIds,
            List<int> goalIds,
            List<int> themeIds,
            List<int> regionIds) 
        {
            this.CreatorId = creatorId;
            this.Name = name;
            this.Description = description;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.ProgramTypeId = programTypeId;
            this.OwnerOrganizationId = ownerOrganizationId;
            this.ContactIds = contactIds ?? new List<int>();
            this.GoalIds = goalIds ?? new List<int>();
            this.ThemeIds = themeIds ?? new List<int>();
            this.RegionIds = regionIds ?? new List<int>();
        }

        public int CreatorId { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public int ProgramTypeId { get; set; }

        public int? OwnerOrganizationId { get; set; }

        public List<int> ContactIds { get; set; }

        public List<int> GoalIds { get; set; }

        public List<int> ThemeIds { get; set; }

        public List<int> RegionIds { get; set; }
    }
}
