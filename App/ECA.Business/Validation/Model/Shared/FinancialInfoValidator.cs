using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class FinancialInfoValidator : AbstractValidator<FinancialInfo>
    {
        public const int TERM_MAX_LENGTH = 20;

        public FinancialInfoValidator()
        {
            RuleFor(student => student.AcademicTerm).NotNull().Length(1, TERM_MAX_LENGTH).WithMessage("Financial: Academic Term is required and can be up to " + TERM_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.Expense).NotNull().WithMessage("Financial: Expense is required").SetValidator(new ExpenseValidator());
            RuleFor(student => student.Funding).NotNull().WithMessage("Financial: Funding is required").SetValidator(new FundingValidator());
        }
    }
}