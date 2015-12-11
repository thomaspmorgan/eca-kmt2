using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class OPTReportParticipationValidator : AbstractValidator<OPTReportParticipation>
    {
        public const int FPT_LENGTH = 2;

        public OPTReportParticipationValidator()
        {
            RuleFor(student => student.printForm).NotNull().WithMessage("OPT Report Participation: Print form option is required");
            RuleFor(student => student.FullPartTimeIndicator).Length(FPT_LENGTH).WithMessage("OPT Report Participation: Full/Part-Time code must be " + FPT_LENGTH.ToString() + " characters");
        }
    }
}