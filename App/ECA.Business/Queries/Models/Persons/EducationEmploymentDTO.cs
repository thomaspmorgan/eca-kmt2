using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Queries.Models.Persons
{
    public class EducationEmploymentDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Role { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public int OrganizationId { get; set; }

        public SimpleOrganizationDTO Organization { get; set; }
    }
}
