using System;

namespace ECA.Business.Queries.Models.Persons
{
    public class EducationEmploymentDTO
    {
        public int ProfessionEducationId { get; set; }

        public string Title { get; set; }

        public string Role { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public int? OrganizationId { get; set; }
        //public SimpleOrganizationDTO Organization { get; set; }
        
        public int? PersonOfEducation_PersonId { get; set; }
        
        public int? PersonOfProfession_PersonId { get; set; }
    }
}
