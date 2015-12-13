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
    /// A project is a specific, time-bounded instance of a program, such as a cohort, an event or an exchange.
    /// </summary>
    public class Project :
        IConcurrentEntity,
        IHistorical,
        IValidatableObject,
        IGoalable,
        IThemeable,
        IContactable,
        ICategorizable,
        IObjectivable,
        IRegionable,
        IPermissable
    {
        /// <summary>
        /// The max length of the name.
        /// </summary>
        public const int MAX_NAME_LENGTH = 500;

        /// <summary>
        /// The max description length.
        /// </summary>
        public const int MAX_DESCRIPTION_LENGTH = 3000;

        /// <summary>
        /// Creates a new Project and initializes the collections.
        /// </summary>
        public Project()
        {
            this.SourceProjectMoneyFlows = new HashSet<MoneyFlow>();
            this.RecipientProjectMoneyFlows = new HashSet<MoneyFlow>();
            this.Regions = new HashSet<Location>();
            this.Locations = new HashSet<Location>();
            this.Targets = new HashSet<Location>();
            this.Themes = new HashSet<Theme>();
            this.Goals = new HashSet<Goal>();
            this.Artifacts = new HashSet<Artifact>();
            this.Participants = new HashSet<Participant>();
            this.RelatedProjects = new HashSet<Project>();
            this.OtherRelatedProjects = new HashSet<Project>();
            this.TreatiesAgreementsContracts = new HashSet<string>();
            this.Impacts = new HashSet<Impact>();
            this.Contacts = new HashSet<Contact>();
            this.History = new History();
            this.Categories = new HashSet<Category>();
            this.Objectives = new HashSet<Objective>();
            this.Itineraries = new HashSet<Itinerary>();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Key]
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the name.
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
        /// Gets or sets the project type.
        /// </summary>
        public virtual ProjectType ProjectType { get; set; }

        /// <summary>
        /// Gets or sets the project type id.
        /// </summary>
        public int? ProjectTypeId { get; set; }

        /// <summary>
        /// The type of visitor for this project (exchange, student or null)
        /// </summary>
        public VisitorType VisitorType { get; set; }

        /// <summary>
        /// The Visitor Type Id
        /// </summary>
        public int? VisitorTypeId { get; set; }

        /// <summary>
        /// Gets or sets the project status.
        /// </summary>
        public virtual ProjectStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the project status id.
        /// </summary>
        [Required]
        public int ProjectStatusId { get; set; }

        /// <summary>
        /// Gets or sets the source project money flows.
        /// </summary>
        public virtual ICollection<MoneyFlow> SourceProjectMoneyFlows { get; set; }

        /// <summary>
        /// Gets or sets the recipient project money flows.
        /// </summary>
        public virtual ICollection<MoneyFlow> RecipientProjectMoneyFlows { get; set; }

        /// <summary>
        /// Gets or sets the nomination source.
        /// </summary>
        public virtual Organization NominationSource { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        [Required]
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the regions.
        /// </summary>
        [InverseProperty("RegionProjects")]
        public virtual ICollection<Location> Regions { get; set; }

        /// <summary>
        /// Gets or sets the locations.
        /// </summary>
        [InverseProperty("LocationProjects")]
        public virtual ICollection<Location> Locations { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the targets.
        /// </summary>
        [InverseProperty("TargetProjects")]
        public virtual ICollection<Location> Targets { get; set; }

        /// <summary>
        /// Gets or sets the themes.
        /// </summary>
        public virtual ICollection<Theme> Themes { get; set; }

        /// <summary>
        /// Gets or sets the goals.
        /// </summary>
        public virtual ICollection<Goal> Goals { get; set; }

        /// <summary>
        /// Gets or sets the parent program.
        /// </summary>
        public virtual Program ParentProgram { get; set; }

        /// <summary>
        /// Gets or sets the parent program id.
        /// </summary>
        [Required]
        public int ProgramId { get; set; }

        /// <summary>
        /// Gets or sets the audience reach.
        /// </summary>
        public int AudienceReach { get; set; }

        /// <summary>
        /// Gets or sets the artifacts.
        /// </summary>
        public virtual ICollection<Artifact> Artifacts { get; set; }

        /// <summary>
        /// Gets or sets the participants.
        /// </summary>
        public virtual ICollection<Participant> Participants { get; set; }

        /// <summary>
        /// Gets or sets the related projects.
        /// </summary>
        public virtual ICollection<Project> RelatedProjects { get; set; }

        /// <summary>
        /// Gets or sets other related projects.
        /// </summary>
        public virtual ICollection<Project> OtherRelatedProjects { get; set; }

        /// <summary>
        /// Gets or sets the treaties agreements contracts.
        /// </summary>
        public virtual ICollection<string> TreatiesAgreementsContracts { get; set; }

        /// <summary>
        /// Gets or sets the impacts.
        /// </summary>
        public virtual ICollection<Impact> Impacts { get; set; }

        /// <summary>
        /// Gets or sets the activity.
        /// </summary>
        public virtual Activity Activity { get; set; }

        /// <summary>
        /// Gets or sets the activity id.
        /// </summary>
        public int? ActivityId { get; set; }

        /// <summary>
        /// Gets or sets the contacts.
        /// </summary>
        public virtual ICollection<Contact> Contacts { get; set; }
        
        /// <summary>
        /// Gets or sets the objectives.
        /// </summary>
        public virtual ICollection<Objective> Objectives { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        public virtual ICollection<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the itineraries.
        /// </summary>
        public virtual ICollection<Itinerary> Itineraries { get; set; }

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        public History History { get; set; }

        /// <summary>
        /// Gets or sets the RowVersion.
        /// </summary>
        public byte[] RowVersion { get; set; }
        
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

            var existingProjectsByName = context.Projects
                .Where(x => 
                x.Name.ToLower().Trim() == this.Name.ToLower().Trim() 
                && x.ProjectId != this.ProjectId
                && x.ProgramId == this.ProgramId)
                .FirstOrDefault();
            if (existingProjectsByName != null)
            {
                yield return new ValidationResult(
                    String.Format("The project with the name [{0}] already exists.",
                        this.Name),
                    new List<string> { "Name" });
            }
        }

        /// <summary>
        /// Returns the project id.
        /// </summary>
        /// <returns>The project id.</returns>
        public int GetId()
        {
            return this.ProjectId;
        }

        /// <summary>
        /// Returns the project permissable type.
        /// </summary>
        /// <returns>The project permissable type.</returns>
        public PermissableType GetPermissableType()
        {
            return PermissableType.Project;
        }

        /// <summary>
        /// Returns the parent program id.
        /// </summary>
        /// <returns>The parent program id.</returns>
        public int? GetParentId()
        {
            return this.ProgramId;
        }

        /// <summary>
        /// Returns the program permissable type.
        /// </summary>
        /// <returns>The program permissable type.</returns>
        public PermissableType GetParentPermissableType()
        {
            return PermissableType.Program;
        }

        /// <summary>
        /// Returns false a project is not exempt from permission protection.
        /// </summary>
        /// <returns>False.</returns>
        public bool IsExempt()
        {
            return false;
        }

        /// <summary>
        /// Returns true if the given role name is equal to the kmt user role name.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="permissionName">The name of the permission.</param>
        /// <returns>True, if the given role name is equal to the kmt super user role.</returns>
        public bool AssignPermissionToRoleOnCreate(string roleName, string permissionName)
        {
            if (String.IsNullOrWhiteSpace(roleName))
            {
                return false;
            }
            else
            {
                return roleName == UserAccount.KMT_SUPER_USER_ROLE_NAME;
            }
        }
    }
}
