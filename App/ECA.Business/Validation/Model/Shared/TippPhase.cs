﻿using System;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(TippPhaseValidator))]
    public class TippPhase
    {
        public string phaseId { get; set; }

        public string phaseName { get; set; }
        
        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }

        public string trainingField { get; set; }
        
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