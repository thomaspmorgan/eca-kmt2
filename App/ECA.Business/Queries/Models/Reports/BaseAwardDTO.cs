using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Reports
{
    public class BaseAwardDTO
    {
        public decimal? ProgramValue { get; set; }
        public decimal? OtherValue { get; set; }
        public int? Projects { get; set; }
        public decimal? Average { get; set; }
    }
}
