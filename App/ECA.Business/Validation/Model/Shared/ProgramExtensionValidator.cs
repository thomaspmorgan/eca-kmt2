using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class ProgramExtensionValidator : AbstractValidator<ProgramExtension>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public ProgramExtensionValidator()
        {
            RuleFor(visitor => visitor.printForm).NotNull().WithMessage("Program Extension: Print form option is required");
            RuleFor(visitor => visitor.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Program Extension: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}