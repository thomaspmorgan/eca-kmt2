using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class ShortenProgramValidator : AbstractValidator<ShortenProgram>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public ShortenProgramValidator()
        {
            RuleFor(visitor => visitor.printForm).NotNull().WithMessage("Shorten Program: Print form option is required");
            RuleFor(visitor => visitor.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Shorten Program: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}