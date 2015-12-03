using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class ExpenseOtherValidator : AbstractValidator<ExpenseOther>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public ExpenseOtherValidator()
        {
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Expense: Other Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}