using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class MatriculateProgramValidator : AbstractValidator<MatriculateProgram>
    {
        public const int CODE_MAX_LENGTH = 2;

        public MatriculateProgramValidator()
        {
            RuleFor(program => program.printForm).NotNull().WithMessage("Matriculate Program: Print form option is required");
            RuleFor(program => program.NewPrgEndDate).NotNull().WithMessage("Matriculate Program: New Program End Date is required");
            RuleFor(program => program.MatriculationCode).Length(CODE_MAX_LENGTH).WithMessage("Matriculate Program: Matriculation Code must be " + CODE_MAX_LENGTH.ToString() + " characters");
        }
    }
}