using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class CapGapExtensionValidator : AbstractValidator<CapGapExtension>
    {
        public const int CAP_GAP_LENGTH = 2;

        public CapGapExtensionValidator()
        {
            RuleFor(student => student._capGapExtension).Length(CAP_GAP_LENGTH).WithMessage("Cap Gap Extension must be " + CAP_GAP_LENGTH.ToString() + " characters");
        }
    }
}