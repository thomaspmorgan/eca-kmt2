using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class OPTEmploymentValidator : AbstractValidator<OPTEmployment>
    {
        public OPTEmploymentValidator()
        {
            When(student => student.addOPTEmployment != null, () =>
            {
                RuleFor(student => student.addOPTEmployment).SetValidator(new AddOPTEmploymentValidator());
            });
            When(student => student.cancelOPTEmployment != null, () =>
            {
                RuleFor(student => student.cancelOPTEmployment).SetValidator(new CancelOPTEmploymentValidator());
            });
            When(student => student.editOPTEmployment != null, () =>
            {
                RuleFor(student => student.editOPTEmployment).SetValidator(new EditOPTEmploymentValidator());
            });
            When(student => student.extendOPTEmployment != null, () =>
            {
                RuleFor(student => student.extendOPTEmployment).SetValidator(new ExtendOPTEmploymentValidator());
            });
            When(student => student.otpReportParticipation != null, () =>
            {
                RuleFor(student => student.otpReportParticipation).SetValidator(new OPTReportParticipationValidator());
            });
        }
    }
}