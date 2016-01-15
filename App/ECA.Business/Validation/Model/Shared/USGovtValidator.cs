using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class USGovtValidator : AbstractValidator<USGovt>
    {
        public const int ORG_LENGTH = 3;
        public const int AMOUNT_MAX_LENGTH = 8;

        public USGovtValidator()
        {
            RuleFor(visitor => visitor.Org1).NotNull().WithMessage("U.S. Gov Funds: U.S. Government Organization 1 is required").Length(1, ORG_LENGTH).WithMessage("U.S. Gov Funds: U.S. Government Organization code must be " + ORG_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.Amount1).NotNull().WithMessage("U.S. Gov Funds: U.S. Government Organization 1 Amount is required").Length(1, AMOUNT_MAX_LENGTH).WithMessage("U.S. Gov Funds: U.S. Government Organization Amount can be up to " + AMOUNT_MAX_LENGTH.ToString() + " characters");
        }
    }
}