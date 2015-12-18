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
            RuleFor(student => student.phaseId).Length(1, 22).WithMessage("T/IPP: Phase ID can be from 1 to 22 characters");
            RuleFor(student => student.phaseName).NotNull().WithMessage("T/IPP: Phase name is required").Length(1, PHASENAME_MAX_LENGTH).WithMessage("T/IPP: Phase name can be up to " + PHASENAME_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.startDate).NotNull().WithMessage("T/IPP: Phase start date is required");
            RuleFor(student => student.endDate).NotNull().WithMessage("T/IPP: Phase end date is required");
            RuleFor(student => student.trainingField).NotNull().WithMessage("T/IPP: Training field is required").Length(1, TRAININGFIELD_MAX_LENGTH).WithMessage("T/IPP: Training field can be up to " + TRAININGFIELD_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.SuperLastName).NotNull().WithMessage("T/IPP: Supervisor last name is required").Length(1, 40).WithMessage("T/IPP: Supervisor last name can be up to 40 characters");
            RuleFor(student => student.SuperFirstName).NotNull().WithMessage("T/IPP: Supervisor first name is required").Length(1, 40).WithMessage("T/IPP: Supervisor first name can be up to 40 characters");
            RuleFor(student => student.SuperTitle).NotNull().WithMessage("T/IPP: Supervisor title is required").Length(1, 100).WithMessage("T/IPP: Supervisor title can be up to 100 characters");
            RuleFor(student => student.SuperEmail).NotNull().WithMessage("T/IPP: Supervisor Email address is required").Length(1, 255).WithMessage("T/IPP: Supervisor Email address can be up to 255 characters");
            RuleFor(student => student.SuperPhone).NotNull().WithMessage("T/IPP: Supervisor telephone number is required").Length(1, 10).WithMessage("T/IPP: Supervisor telephone number can be up to 10 characters");
            RuleFor(student => student.SuperPhoneExt).Length(1, 4).WithMessage("T/IPP: Supervisor telephone number extenstion can be up to 4 characters");
            RuleFor(student => student.EvRole).NotNull().WithMessage("T/IPP: Exchange visitor role is required").Length(1, TEXT_MAX_LENGTH).WithMessage("T/IPP: Exchange visitor role can be up to " + TEXT_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.GoalsAndObjectives).NotNull().WithMessage("T/IPP: Goals and objectives is required").Length(1, TEXT_MAX_LENGTH).WithMessage("T/IPP: Goals and objectives can be up to " + TEXT_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.SupervisorAndQualifications).NotNull().WithMessage("T/IPP: Supervision and qualtification is required").Length(1, TEXT_MAX_LENGTH).WithMessage("T/IPP: Supervision and qualtification can be up to " + TEXT_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.CulturalActivities).NotNull().WithMessage("T/IPP: Cultural activities is required").Length(1, TEXT_MAX_LENGTH).WithMessage("T/IPP: Cultural activities can be up to " + TEXT_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.SkillsLearned).NotNull().WithMessage("T/IPP: Skills learned is required").Length(1, TEXT_MAX_LENGTH).WithMessage("T/IPP: Skills learned can be up to " + TEXT_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.TeachingMethod).NotNull().WithMessage("T/IPP: Teaching methods is required").Length(1, TEXT_MAX_LENGTH).WithMessage("T/IPP: Teaching methods can be up to " + TEXT_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.HowCompetencyMeasured).NotNull().WithMessage("T/IPP: Measure of competency is required").Length(1, TEXT_MAX_LENGTH).WithMessage("T/IPP: Measure of competency can be up to " + TEXT_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.AdditionalRemarks).Length(1, TEXT_MAX_LENGTH).WithMessage("T/IPP: Remarks can be up to " + TEXT_MAX_LENGTH.ToString() + " characters");
        }
    }
}