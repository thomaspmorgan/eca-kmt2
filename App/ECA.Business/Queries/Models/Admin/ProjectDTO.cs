using ECA.Business.Service.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ECA.Business.Queries.Models.Admin
{
    public class ProjectDTO
    {
        public ProjectDTO()
        {
            this.Themes = new List<SimpleLookupDTO>();
            this.CountryIsos = new List<SimpleLookupDTO>();
        }


        public string Name { get; set; }
        public string Focus { get; set; }
        public IEnumerable<SimpleLookupDTO> Themes { get; set; }
        public IEnumerable<SimpleLookupDTO> CountryIsos { get; set; }
    }
}
