using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class CancelOPTEmploymentValidator : AbstractValidator<CancelOPTEmployment>
    {
        public const int FPT_LENGTH = 2;
        public const int EMPLOYER_MAX_LENGTH = 100;

        public CancelOPTEmploymentValidator()
        {
            RuleFor(student => student.printForm).NotNull().WithMessage("Cancel OPT Employment: Print form option is required");
            RuleFor(student => student.FullPartTimeIndicator).Length(FPT_LENGTH).WithMessage("Cancel OPT Employment: Full/Part-Time code must be " + FPT_LENGTH.ToString() + " characters");
            RuleFor(student => student.EmployerName).Length(0, EMPLOYER_MAX_LENGTH).WithMessage("Cancel OPT Employment: Employer Name can be up to " + EMPLOYER_MAX_LENGTH.ToString() + " characters");
        }
    }
}