using System;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(TippPhaseValidator))]
    public class AddPhase
    {
        public AddPhase()
        {
        }

        public string SiteId { get; set; }

        public string PhaseName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string TrainingField { get; set; }

        public string SuperLastName { get; set; }

        public string SuperFirstName { get; set; }

        public string SuperMiddleInitial { get; set; }

        public string SuperTitle { get; set; }

        public string SuperEmail { get; set; }

        public string SuperPhone { get; set; }

        public string SuperPhoneExt { get; set; }

        public DateTime SuperSignatureDate { get; set; }

        public string EvRole { get; set; }

        public string GoalsAndObjectives { get; set; }

        public string SupervisorAndQualifications { get; set; }

        public string CulturalActivities { get; set; }

        public string SkillsLearned { get; set; }

        public string TeachingMethod { get; set; }

        public string HowCompetencyMeasured { get; set; }

        public string AdditionalRemarks { get; set; }
    }
}