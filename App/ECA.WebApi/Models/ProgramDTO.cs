using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models
{
    /// <summary>
    /// A program is an umbrella for a set of projects and sub-programs.
    /// </summary>
    public class ProgramDTO
    {
        public int ProgramId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public OrganizationDTO Owner { get; set; }
        public ProgramDTO ParentProgram { get; set; }
        public DateTimeOffset StartDate { get; set; }
        //public ICollection<ProjectDTO> Projects { get; set; }
        public ICollection<ProgramDTO> ChildPrograms { get; set; }
        public ICollection<ThemeDTO> Themes { get; set; }
        public ICollection<RegionDTO> Regions { get; set; }
        //public ICollection<Goal> Goals { get; set; }

        public DateTimeOffset RevisedOn { get; set; }
    }
}