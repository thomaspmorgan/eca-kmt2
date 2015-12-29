using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class ReprintFormValidator : AbstractValidator<ReprintForm>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public ReprintFormValidator()
        {
            RuleFor(visitor => visitor.printForm).NotNull().WithMessage("Dependent Reprint: Print request indicator is required");
            RuleFor(visitor => visitor.Reason).NotNull().WithMessage("Dependent Reprint: Reason is required").Length(2).WithMessage("Dependent Reprint: Reason must be 2 characters");
            RuleFor(student => student.OtherRemarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Dependent Reprint: Other Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Dependent Reprint: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}