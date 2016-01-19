using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class OtherFundsValidator : AbstractValidator<OtherFunds>
    {
        public const int AMOUNT_MAX_LENGTH = 8;

        public OtherFundsValidator()
        {
            RuleFor(visitor => visitor.Other).SetValidator(new OtherValidator()).When(visitor => visitor.Other != null);
            RuleFor(visitor => visitor.EVGovt).Length(0, AMOUNT_MAX_LENGTH).WithMessage("Financial Info: Government funding can be up to " + AMOUNT_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.BinationalCommission).Length(0, AMOUNT_MAX_LENGTH).WithMessage("Financial Info: Binational Commission funding can be up to " + AMOUNT_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.Personal).Length(0, AMOUNT_MAX_LENGTH).WithMessage("Financial Info: Personal funding can be up to " + AMOUNT_MAX_LENGTH.ToString() + " characters");
        }
    }
}