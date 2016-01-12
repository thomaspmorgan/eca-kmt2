using ECA.Business.Validation.Model.CreateEV;
using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class TippPhaseValidator : AbstractValidator<TippPhase>
    {
        public const int PHASENAME_MAX_LENGTH = 25;
        public const int TRAININGFIELD_MAX_LENGTH = 100;
        public const int TEXT_MAX_LENGTH = 3000;

        public TippPhaseValidator()
        {
            RuleFor(visitor => visitor.PhaseId).Length(1, 22).WithMessage("T/IPP: Phase ID is required and can be from 1 to 22 characters");
            RuleFor(visitor => visitor.PhaseName).Length(1, PHASENAME_MAX_LENGTH).WithMessage("T/IPP: Phase Name is required and can be up to " + PHASENAME_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.StartDate).NotNull().WithMessage("T/IPP: Phase Start Date is required");
            RuleFor(visitor => visitor.EndDate).NotNull().WithMessage("T/IPP: Phase End Date is required");
            RuleFor(visitor => visitor.TrainingField).Length(1, TRAININGFIELD_MAX_LENGTH).WithMessage("T/IPP: Training Field is required and can be up to " + TRAININGFIELD_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.SuperLastName).Length(1, 40).WithMessage("T/IPP: Supervisor Last Name is required and can be up to 40 characters");
            RuleFor(visitor => visitor.SuperFirstName).Length(1, 40).WithMessage("T/IPP: Supervisor First Name is required and can be up to 40 characters");
            RuleFor(visitor => visitor.SuperTitle).Length(1, 100).WithMessage("T/IPP: Supervisor Title is required and can be up to 100 characters");
            RuleFor(visitor => visitor.SuperEmail).Length(1, 255).WithMessage("T/IPP: Supervisor Email Address is required and can be up to 255 characters");
            RuleFor(visitor => visitor.SuperPhone).Length(1, 10).WithMessage("T/IPP: Supervisor Phone Number is required and can be up to 10 characters");
            RuleFor(visitor => visitor.SuperPhoneExt).Length(0, 4).WithMessage("T/IPP: Supervisor Phone Number Extenstion can be up to 4 characters");
            RuleFor(visitor => visitor.EvRole).Length(1, TEXT_MAX_LENGTH).WithMessage("T/IPP: Exch. Visitor Role is required and can be up to " + TEXT_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.GoalsAndObjectives).Length(1, TEXT_MAX_LENGTH).WithMessage("T/IPP: Goals and Objectives is required and can be up to " + TEXT_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.SupervisorAndQualifications).Length(1, TEXT_MAX_LENGTH).WithMessage("T/IPP: Supervision and Qualtification is required and can be up to " + TEXT_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.CulturalActivities).Length(1, TEXT_MAX_LENGTH).WithMessage("T/IPP: Cultural Activities is required and can be up to " + TEXT_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.SkillsLearned).Length(1, TEXT_MAX_LENGTH).WithMessage("T/IPP: Skills Learned is required and can be up to " + TEXT_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.TeachingMethod).Length(1, TEXT_MAX_LENGTH).WithMessage("T/IPP: Teaching Methods is required and can be up to " + TEXT_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.HowCompetencyMeasured).Length(1, TEXT_MAX_LENGTH).WithMessage("T/IPP: Measure of Competency is required and can be up to " + TEXT_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.AdditionalRemarks).Length(1, TEXT_MAX_LENGTH).WithMessage("T/IPP: Remarks is required and can be up to " + TEXT_MAX_LENGTH.ToString() + " characters");
        }
    }
}