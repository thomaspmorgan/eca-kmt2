using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECA.Business.Queries.Models.Reports
{
    public class ProjectWithGrantNumberDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Focus { get; set; }
        public string Location { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string GrantNumber { get; set; }
        public string Organization {get; set;}

    }
}
