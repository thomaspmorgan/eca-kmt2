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
    /// A project is a specific, time-bounded instance of a program, such as a cohort, an event or an exchange.
    /// </summary>
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public ProjectType ProjectType { get; set; }
        [Required]
        public ProjectStatus Status { get; set; }
        public string FocusArea { get; set; }
        public ICollection<MoneyFlow> MoneyFlows { get; set; }
        public Organization NominationSource { get; set; }
        [Required]
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        [InverseProperty("RegionProjects")]
        public ICollection<Location> Regions { get; set; }
        [InverseProperty("LocationProjects")]
        public ICollection<Location> Locations { get; set; }
        public string Language { get; set;}
        [InverseProperty("TargetProjects")]
        public ICollection<Location> Targets { get; set; }
        public ICollection<Theme> Themes { get; set; }
        public ICollection<Goal> Goals {get; set;}
        public Program ParentProgram { get; set; }
        public int AudienceReach { get; set; }
        public ICollection<Artifact> Artifacts { get; set; }
        public ICollection<ParticipantStatus> ParticipantsStatus { get; set; }
        public ICollection<Project> RelatedProjects { get; set; }
        public ICollection<Project> OtherRelatedProjects { get; set; }
        public ICollection<string> TreatiesAgreementsContracts { get; set; }
        public ICollection<Impact> Impacts { get; set; }
        public Event Event { get; set; }
        public int? EventId { get; set; }
        public ICollection<Contact> Contacts { get; set; }

        public History History { get; set; }
    }
}
