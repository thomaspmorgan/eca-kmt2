using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Reports
{
    public class PostAwardDTO
    {
        public string Post { get; set; }
        public string Region { get; set; }
        public float? ProgramValue { get; set; }
        public float? OtherValue { get; set; }
        public int? Projects { get; set; }
        public float? Average { get; set; }
    }
}
