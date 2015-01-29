using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models
{
    /// <summary>
    /// A program is an umbrella for a set of projects and sub-programs.
    /// </summary>
    public class Program
    {
        public int ProgramId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Organization Owner { get; set; }
        public Program ParentProgram { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<Program> ChildPrograms { get; set; }
        public ICollection<Theme> Themes { get; set; } 
        //public ICollection<Goal> Goals { get; set; }

        public DateTimeOffset RevisedOn { get; set; }
    }
}