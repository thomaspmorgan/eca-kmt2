using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class AddAuthDropValidator : AbstractValidator<AddAuthDrop>
    {
        public const int REASON_LENGTH = 2;
        public const int REMARKS_MAX_LENGTH = 500;

        public AddAuthDropValidator()
        {
            RuleFor(student => student.printForm).NotNull().WithMessage("Auth Drop: Print form option is required");
            RuleFor(student => student.Reason).Length(REASON_LENGTH).WithMessage("Auth Drop: Reason code must be " + REASON_LENGTH.ToString() + " characters");
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Auth Drop: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}