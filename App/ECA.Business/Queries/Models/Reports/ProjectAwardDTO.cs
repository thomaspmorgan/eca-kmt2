using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Reports
{
    public class ProjectAwardDTO
    {
        public int Year { get; set; }
        public float Award { get; set; }
        public string Title { get; set;  }
        public string Summary { get; set; }
    }
}
