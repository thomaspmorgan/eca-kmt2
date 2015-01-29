using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    /// <summary>
    /// A program is an umbrella for a set of projects and sub-programs.
    /// </summary>
    public class Program
    {
        [Key]
        public int ProgramId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [InverseProperty("OwnerPrograms")]
        public virtual Organization Owner { get; set; }
        public virtual Program ParentProgram { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        [Required]
        [InverseProperty("RegionPrograms")]
        public virtual ICollection<Location> Regions { get; set; }
        [InverseProperty("LocationPrograms")]
        public virtual ICollection<Location> Locations { get; set; }
        [InverseProperty("TargetPrograms")]
        public virtual ICollection<Location> Targets { get; set; }
        public virtual ICollection<MoneyFlow> MoneyFlows { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Program> ChildPrograms { get; set; }
        public virtual ICollection<Theme> Themes { get; set; }
        public virtual ICollection<Goal> Goals { get; set; }
        public virtual ICollection<Artifact> Artifacts { get; set; }
        public ICollection<ProgramType> ProgramType { get; set; }
        public virtual ICollection<Impact> Impacts { get; set; }

        public History History { get; set; }
    }
}


