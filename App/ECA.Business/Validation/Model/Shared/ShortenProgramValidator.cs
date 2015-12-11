using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class ShortenProgramValidator : AbstractValidator<ShortenProgram>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public ShortenProgramValidator()
        {
            RuleFor(student => student.printForm).NotNull().WithMessage("Shorten Program: Print form option is required");
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Shorten Program: Student Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}