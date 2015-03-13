using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace ECA.Data
{
    /// <summary>
    /// A program is an umbrella for a set of projects and sub-programs.
    /// </summary>
    public class Program : IHistorical, IValidatableObject
    {
        public Program()
        {
            this.Regions = new HashSet<Location>();
            this.Locations = new HashSet<Location>();
            this.Targets = new HashSet<Location>();
            this.SourceProgramMoneyFlows = new HashSet<MoneyFlow>();
            this.RecipientProgramMoneyFlows = new HashSet<MoneyFlow>();
            this.Projects = new HashSet<Project>();
            this.ChildPrograms = new HashSet<Program>();
            this.Themes = new HashSet<Theme>();
            this.Goals = new HashSet<Goal>();
            this.Artifacts = new HashSet<Artifact>();
            this.ProgramType = new HashSet<ProgramType>();
            this.Impacts = new HashSet<Impact>();
            this.Contacts = new HashSet<Contact>();

            this.History = new History();
        }

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
        public virtual Program ParentProgram { get; set; }

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
        /// Gets or sets the Focus.
        /// </summary>
        public virtual Focus Focus { get; set; }

        /// <summary>
        /// Gets or sets the program status id.
        /// </summary>
        public int ProgramStatusId { get; set; }

        /// <summary>
        /// Gets or sets the Website.
        /// </summary>
        public string Website { get; set; }

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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Contract.Assert(validationContext != null, "The validation context must not be null.");
            Contract.Assert(validationContext.Items.ContainsKey(EcaContext.VALIDATABLE_CONTEXT_KEY), "The validation context must have a context to query.");
            var context = validationContext.Items[EcaContext.VALIDATABLE_CONTEXT_KEY] as EcaContext;
            Contract.Assert(context != null, "The context must not be null.");

            var existingProgramsByName = context.Programs
                .Where(x => x.Name.ToLower().Trim() == this.Name.ToLower().Trim() && x.ProgramId != this.ProgramId)
                .FirstOrDefault();
            if (existingProgramsByName != null)
            {
                yield return new ValidationResult(String.Format("The program with the name [{0}] already exists.", this.Name), new List<string> { "Name" });
            }
        }
    }
}


