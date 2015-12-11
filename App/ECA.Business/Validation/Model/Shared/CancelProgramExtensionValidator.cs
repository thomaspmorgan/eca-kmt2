using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class CancelProgramExtensionValidator : AbstractValidator<CancelProgramExtension>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public CancelProgramExtensionValidator()
        {
            RuleFor(student => student.printForm).NotNull().WithMessage("Cancel Program Extension: Print form option is required");
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Cancel Program Extension: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}