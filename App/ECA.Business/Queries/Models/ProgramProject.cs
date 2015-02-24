using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models
{
    public class ProgramProject
    {
        public int ProjectId { get; set; }

        public int ProgramId { get; set; }

        public string ProjectName { get; set; }

        public string ProgramName { get; set; }

        public DateTimeOffset LastRevisedOn { get; set; }

        public string LastRevisedBy { get; set; }

        public string RegionName { get; set; }

        public int RegionId { get; set; }

        public int ProjectStatusId { get; set; }

        public string ProjectStatusName { get; set; }

        public int OwnerOrganizationId { get; set; }

    }
}
