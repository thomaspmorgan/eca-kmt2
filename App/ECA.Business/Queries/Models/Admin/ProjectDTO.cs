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
            this.Goals = new List<SimpleLookupDTO>();
            this.Contacts = new List<SimpleLookupDTO>();
            this.Categories = new List<SimpleLookupDTO>();
            this.Objectives = new List<SimpleLookupDTO>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int ProjectStatusId { get; set; }
        public string Focus { get; set; }
        public int ProgramId { get; set; }
        public int FocusId { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset RevisedOn { get; set; }
        public IEnumerable<SimpleLookupDTO> Themes { get; set; }
        public IEnumerable<SimpleLookupDTO> CountryIsos { get; set; }
        public IEnumerable<SimpleLookupDTO> Goals { get; set; }
        public IEnumerable<SimpleLookupDTO> Contacts { get; set; }
        public IEnumerable<SimpleLookupDTO> Categories { get; set; }
        public IEnumerable<SimpleLookupDTO> Objectives { get; set; }
    }
}
