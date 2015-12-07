using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class ExpenseValidator : AbstractValidator<Expense>
    {
        public ExpenseValidator()
        {
            RuleFor(student => student.Tuition).NotNull().WithMessage("Financial Expense: Tuition is required");
            RuleFor(student => student.LivingExpense).NotNull().WithMessage("Financial Expense: Living Expense is required");
            When(student => student.Other != null, () => {
                RuleFor(student => student.Other).SetValidator(new ExpenseOtherValidator());
            });                
        }
    }
}