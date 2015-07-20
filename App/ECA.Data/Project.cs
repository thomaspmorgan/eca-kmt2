﻿using System;
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
        IPermissable
    {
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
        }

        [Key]
        public int ProjectId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public ProjectType ProjectType { get; set; }
        public int? ProjectTypeId { get; set; }
        public ProjectStatus Status { get; set; }
        [Required]
        public int ProjectStatusId { get; set; }
        public ICollection<MoneyFlow> SourceProjectMoneyFlows { get; set; }
        public ICollection<MoneyFlow> RecipientProjectMoneyFlows { get; set; }
        public Organization NominationSource { get; set; }
        [Required]
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        [InverseProperty("RegionProjects")]
        public ICollection<Location> Regions { get; set; }
        [InverseProperty("LocationProjects")]
        public ICollection<Location> Locations { get; set; }
        public string Language { get; set; }
        [InverseProperty("TargetProjects")]
        public ICollection<Location> Targets { get; set; }
        public ICollection<Theme> Themes { get; set; }
        public ICollection<Goal> Goals { get; set; }
        public Program ParentProgram { get; set; }
        [Required]
        public int ProgramId { get; set; }
        public int AudienceReach { get; set; }
        public ICollection<Artifact> Artifacts { get; set; }
        public ICollection<Participant> Participants { get; set; }
        public ICollection<Project> RelatedProjects { get; set; }
        public ICollection<Project> OtherRelatedProjects { get; set; }
        public ICollection<string> TreatiesAgreementsContracts { get; set; }
        public ICollection<Impact> Impacts { get; set; }
        public Activity Activity { get; set; }
        public int? ActivityId { get; set; }
        public ICollection<Contact> Contacts { get; set; }
        
        public ICollection<Objective> Objectives { get; set; }
        public ICollection<Category> Categories { get; set; }

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
                .Where(x => x.Name.ToLower().Trim() == this.Name.ToLower().Trim() && x.ProjectId != this.ProjectId)
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
    }
}
