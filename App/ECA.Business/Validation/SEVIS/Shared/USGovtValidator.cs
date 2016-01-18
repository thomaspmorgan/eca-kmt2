using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class USGovtValidator : AbstractValidator<USGovt>
    {
        public const int ORG_LENGTH = 3;
        public const int AMOUNT_MAX_LENGTH = 8;

        public USGovtValidator()
        {
            RuleFor(visitor => visitor.Org1).Length(1, ORG_LENGTH).WithMessage("U.S. Gov Funds: U.S. Government Org Code is required and must be " + ORG_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.Amount1).Length(1, AMOUNT_MAX_LENGTH).WithMessage("U.S. Gov Funds: U.S. Government Org Amount is required and can be up to " + AMOUNT_MAX_LENGTH.ToString() + " characters");
        }
    }
}