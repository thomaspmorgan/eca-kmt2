using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class ProgramExtensionValidator : AbstractValidator<ProgramExtension>
    {
        public const int REMARKS_MAX_LENGTH = 500;
        public const int EXPLANATION_MAX_LENGTH = 500;

        public ProgramExtensionValidator()
        {
            RuleFor(student => student.printForm).NotNull().WithMessage("Program Extension: Print form option is required");
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Program Extension: Student Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.Explanation).Length(0, EXPLANATION_MAX_LENGTH).WithMessage("Program Extension: Remarks can be up to " + EXPLANATION_MAX_LENGTH.ToString() + " characters");
        }
    }
}