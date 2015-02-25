using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models
{
    public class SimpleProjectDTO
    {
        public SimpleProjectDTO()
        {
            this.LocationNames = new List<string>();
        }

        public int ProjectId { get; set; }

        public int ProgramId { get; set; }

        public string ProjectName { get; set; }

        public int ProjectStatusId { get; set; }

        public string ProjectStatusName { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public int StartYear { get; set; }

        public IEnumerable<string> LocationNames { get; set; }
    }
}
