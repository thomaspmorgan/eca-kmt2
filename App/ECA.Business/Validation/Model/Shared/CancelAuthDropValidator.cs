using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class CancelAuthDropValidator : AbstractValidator<CancelAuthDrop>
    {
        public const int REASON_LENGTH = 2;

        public CancelAuthDropValidator()
        {
            RuleFor(student => student.printForm).NotNull().WithMessage("Cancel Auth Drop: Print form option is required");
            RuleFor(student => student.Reason).Length(REASON_LENGTH).WithMessage("Cancel Auth Drop: Reason code must be " + REASON_LENGTH.ToString() + " characters");
        }
    }
}