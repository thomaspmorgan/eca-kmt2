using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using ECA.Core.Data;

namespace ECA.Data
{
    /// <summary>
    /// A program is an umbrella for a set of projects and sub-programs.
    /// </summary>
    public class Program : IHistorical, 
        IValidatableObject, 
        IConcurrentEntity, 
        IGoalable, 
        IThemeable, 
        IContactable,
        ICategorizable,
        IObjectivable,
        IRegionable,
        IPermissable
    {
        /// <summary>
        /// The maximum length of the program name.
        /// </summary>
        public const int MAX_NAME_LENGTH = 255;

        /// <summary>
        /// The maximum length of a program description.
        /// </summary>
        public const int MAX_DESCRIPTION_LENGTH = 3000;
        

        /// <summary>
        /// Creates a new default program.
        /// </summary>
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
            this.Categories = new HashSet<Category>();
            this.Objectives = new HashSet<Objective>();
            this.Websites = new HashSet<Website>();

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
        [MaxLength(MAX_NAME_LENGTH)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Required]
        [MaxLength(MAX_DESCRIPTION_LENGTH)]
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
        /// Gets or sets the parent program id.
        /// </summary>
        [Column("ParentProgram_ProgramId")]
        public int? ParentProgramId { get; set; }

        /// <summary>
        /// Gets or sets the Parent Program.
        /// </summary>
        [ForeignKey("ParentProgramId")]
        public virtual Program ParentProgram { get; set; }

        /// <summary>
        /// Gets or sets the Start Date.
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Gets or sets the End date.
        /// </summary>
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the ProgramStatus.
        /// </summary>
        public virtual ProgramStatus ProgramStatus { get; set; }

        /// <summary>
        /// Gets or sets the program status id.
        /// </summary>
        public int ProgramStatusId { get; set; }

        /// <summary>
        /// Gets or sets the RowVersion.
        /// </summary>
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// Gets or sets the regions.
        /// </summary>
        [Required]
        [InverseProperty("RegionPrograms")]
        public ICollection<Location> Regions { get; set; }

        /// <summary>
        /// Gets or sets the locations.
        /// </summary>
        [InverseProperty("LocationPrograms")]
        public ICollection<Location> Locations { get; set; }

        /// <summary>
        /// Gets or sets the target programs.
        /// </summary>
        [InverseProperty("TargetPrograms")]
        public ICollection<Location> Targets { get; set; }

        /// <summary>
        /// Gets or sets the child programs.
        /// </summary>
        public virtual ICollection<Program> ChildPrograms { get; set; }

        /// <summary>
        /// Gets or sets source money flows.
        /// </summary>
        public ICollection<MoneyFlow> SourceProgramMoneyFlows { get; set; }

        /// <summary>
        /// Gets or sets recipient money flows.
        /// </summary>
        public ICollection<MoneyFlow> RecipientProgramMoneyFlows { get; set; }

        /// <summary>
        /// Gets or sets projects.
        /// </summary>
        public ICollection<Project> Projects { get; set; }

        /// <summary>
        /// Gets or sets themes.
        /// </summary>
        public ICollection<Theme> Themes { get; set; }

        /// <summary>
        /// Gets or sets goals.
        /// </summary>
        public ICollection<Goal> Goals { get; set; }

        /// <summary>
        /// Gets or sets artifacts.
        /// </summary>
        public ICollection<Artifact> Artifacts { get; set; }
        /// <summary>
        /// Gets or sets the program type.
        /// </summary>
        public ICollection<ProgramType> ProgramType { get; set; }

        /// <summary>
        /// Gets or sets impacts.
        /// </summary>
        public ICollection<Impact> Impacts { get; set; }

        /// <summary>
        /// Gets or sets contacts.
        /// </summary>
        public ICollection<Contact> Contacts { get; set; }

        /// <summary>
        /// Gets or sets objectives.
        /// </summary>
        public ICollection<Objective> Objectives { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        public ICollection<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets websites
        /// </summary>
        public ICollection<Website> Websites { get; set; }

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        public History History { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns the program id.
        /// </summary>
        /// <returns>The program id.</returns>
        public int GetId()
        {
            return this.ProgramId;
        }

        /// <summary>
        /// Returns the proram permissable type.
        /// </summary>
        /// <returns>The program permissable type.</returns>
        public PermissableType GetPermissableType()
        {
            return PermissableType.Program;
        }

        /// <summary>
        /// Returns the owner id.
        /// </summary>
        /// <returns>The owner id.</returns>
        public int? GetParentId()
        {
            return this.OwnerId;
        }

        /// <summary>
        /// Returns the office permissable type.
        /// </summary>
        /// <returns>The office permissable type.</returns>
        public PermissableType GetParentPermissableType()
        {
            return PermissableType.Office;
        }

        /// <summary>
        /// Returns false a program is not exempt from permission protection.
        /// </summary>
        /// <returns>False.</returns>
        public bool IsExempt()
        {
            return false;
        }
    }
}


