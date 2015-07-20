using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Reports
{
    public class ObjectiveAwardDTO
    {
        public string Objective { get; set; }
        public string Country { get; set; }
        public string Project { get; set; }
        public decimal? ProgramValue { get; set; }
        public decimal? OtherValue { get; set; }
        public int Year { get; set; }
    }
}
