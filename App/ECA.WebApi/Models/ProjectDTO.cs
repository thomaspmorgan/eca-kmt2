using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models
{
    public class ProjectDTO
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public ProjectType ProjectType { get; set; }
        public string Status { get; set; }
        public string FocusArea { get; set; }
        //public virtual ICollection<MoneyFlow> MoneyFlows { get; set; }
        //public virtual Organization NominationSource { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string Region { get; set; }
        //public ICollection<Location> Locations { get; set; }
        public string Language { get; set; }
        //public virtual ICollection<Location> Targets { get; set; }
        public ICollection<ThemeDTO> Themes { get; set; }
        public ICollection<GoalDTO> Goals { get; set; }
        public ProgramDTO ParentProgram { get; set; }
        public int AudienceReach { get; set; }
        //public virtual ICollection<Artifact> Artifacts { get; set; }
        //public virtual ICollection<ParticipantStatus> ParticipantsStatus { get; set; }
        //public virtual ICollection<Project> RelatedProjects { get; set; }
        //public virtual ICollection<Project> OtherRelatedProjects { get; set; }
        public ICollection<string> TreatiesAgreementsContracts { get; set; }
        //public virtual ICollection<Impact> Impacts { get; set; }
        //public virtual Event Event { get; set; }
        //public int? EventId { get; set; }
    }
}