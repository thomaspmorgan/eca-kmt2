using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class OtherValidator : AbstractValidator<Other>
    {
        public const int NAME_MAX_LENGTH = 60;
        public const int AMOUNT_MAX_LENGTH = 8;

        public OtherValidator()
        {
            RuleFor(visitor => visitor.name).Length(1, NAME_MAX_LENGTH).WithMessage("U.S. Gov Funds: U.S. Government Org Code is required and must be " + NAME_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.amount).Length(1, AMOUNT_MAX_LENGTH).WithMessage("U.S. Gov Funds: U.S. Government Org Amount is required and can be up to " + AMOUNT_MAX_LENGTH.ToString() + " characters");
        }
    }
}