using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Reports
{
    public class FocusAwardDTO : BaseAwardDTO
    {
        public string Focus { get; set; }
    }
    public class FocusCategoryAwardDTO : BaseAwardDTO
    {
        public string Focus { get; set; }
        public string Category { get; set; }
    }
    public class PostAwardDTO : BaseAwardDTO
    {
        public string Post { get; set; }
        public string Region { get; set; }
    }
    public class RegionAwardDTO : BaseAwardDTO
    {
        public string Region { get; set; }
    }
    public class CountryAwardDTO: BaseAwardDTO
    {
        public string Country { get; set; }
        public string Region { get; set; }
        public string Post { get; set; }
    }
}
