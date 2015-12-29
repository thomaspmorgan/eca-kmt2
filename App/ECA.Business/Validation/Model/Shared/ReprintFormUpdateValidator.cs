using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class ReprintFormUpdateValidator : AbstractValidator<ReprintFormUpdate>
    {
        public const int SEVIS_MAX_LENGTH = 11;

        public ReprintFormUpdateValidator()
        {
            RuleFor(visitor => visitor.dependentSevisID).NotNull().WithMessage("Reprint: Dependent Sevis ID is required").Length(1, SEVIS_MAX_LENGTH).WithMessage("Reprint: Dependent Sevis ID can be up to " + SEVIS_MAX_LENGTH.ToString() + " characters");
        }
    }
}