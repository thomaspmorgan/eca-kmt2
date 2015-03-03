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
    public class Program : IHistorical
    {
        /// <summary>
        /// Gets or sets the Program Id.
        /// </summary>
        [Key]
        public int ProgramId { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Owner.
        /// </summary>
        [Required]
        [InverseProperty("OwnerPrograms")]
        public Organization Owner { get; set; }

        /// <summary>
        /// Gets or sets the Owner Id.
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the Parent Program.
        /// </summary>
        public Program ParentProgram { get; set; }

        /// <summary>
        /// Gets or sets the Parent Program Id.
        /// </summary>
        public int? ParentProgramId { get; set; }

        /// <summary>
        /// Gets or sets the Start Date.
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Gets or sets the End date.
        /// </summary>
        public DateTimeOffset EndDate { get; set; }

        /// <summary>
        /// Gets or sets the ProgramStatus.
        /// </summary>
        public virtual ProgramStatus ProgramStatus { get; set; }

        /// <summary>
        /// Gets or sets the program status id.
        /// </summary>
        public int ProgramStatusId { get; set; }

        [Required]
        [InverseProperty("RegionPrograms")]
        public ICollection<Location> Regions { get; set; }
        [InverseProperty("LocationPrograms")]
        public ICollection<Location> Locations { get; set; }
        [InverseProperty("TargetPrograms")]
        public ICollection<Location> Targets { get; set; }
        public ICollection<MoneyFlow> SourceProgramMoneyFlows { get; set; }
        public ICollection<MoneyFlow> RecipientProgramMoneyFlows { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<Program> ChildPrograms { get; set; }
        public ICollection<Theme> Themes { get; set; }
        public ICollection<Goal> Goals { get; set; }
        public ICollection<Artifact> Artifacts { get; set; }
        public ICollection<ProgramType> ProgramType { get; set; }
        public ICollection<Impact> Impacts { get; set; }
        public ICollection<Contact> Contacts { get; set; }

        public History History { get; set; }
    }
}


