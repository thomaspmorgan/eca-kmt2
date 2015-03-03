using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Business.Models.Lookups;


namespace ECA.Business.Queries.Models.Admin
{
    public class ProjectDTO
    {
        public ProjectDTO()
        {
            this.Themes = new List<SimpleLookupDTO>();
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<SimpleLookupDTO> Themes { get; set; }
    }
}
