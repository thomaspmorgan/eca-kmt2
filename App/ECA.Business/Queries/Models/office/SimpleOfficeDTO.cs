using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Office
{
    public class SimpleOfficeDTO
    {
        public int OrganizationId { get; set; }
        public int OrganizationTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentOrganization_OrganizationId { get; set; }
        public int OfficeLevel { get; set; }
    }
}
