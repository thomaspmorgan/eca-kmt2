using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class FinancialInfoUpdateValidator : AbstractValidator<FinancialInfoUpdate>
    {
        public FinancialInfoUpdateValidator()
        {
            RuleFor(visitor => visitor.printForm).NotNull().WithMessage("Financial Info: Print request indicator is required");
        }
    }
}