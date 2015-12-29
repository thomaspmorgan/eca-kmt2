using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class OtherFundsValidator : AbstractValidator<OtherFunds>
    {
        public const int AMOUNT_MAX_LENGTH = 8;

        public OtherFundsValidator()
        {
            RuleFor(student => student.Other).SetValidator(new OtherValidator()).When(student => student.Other != null);
            RuleFor(student => student.EVGovt).Length(0, AMOUNT_MAX_LENGTH).WithMessage("Financial Info: Government funding can be up to " + AMOUNT_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.BinationalCommission).Length(0, AMOUNT_MAX_LENGTH).WithMessage("Financial Info: Binational Commission funding can be up to " + AMOUNT_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.Personal).Length(0, AMOUNT_MAX_LENGTH).WithMessage("Financial Info: Personal funding can be up to " + AMOUNT_MAX_LENGTH.ToString() + " characters");
        }
    }
}