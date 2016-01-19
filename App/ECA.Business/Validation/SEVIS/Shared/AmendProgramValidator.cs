using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class AmendProgramValidator : AbstractValidator<AmendProgram>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public AmendProgramValidator()
        {
            RuleFor(program => program.printForm).NotNull().WithMessage("Amend Program: Print form option is required");
            RuleFor(program => program.PrgStartDate).NotNull().WithMessage("Amend Program: Program start date is required");
            RuleFor(program => program.PrgEndDate).NotNull().WithMessage("Amend Program: Program end date is required");
            RuleFor(program => program.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Amend Program: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}