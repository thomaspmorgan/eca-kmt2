using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class OtherFundsValidator : AbstractValidator<OtherFunds>
    {
        public const int EVGOVT_MAX_LENGTH = 8;
        public const int BINATIONAL_MAX_LENGTH = 8;
        public const int PERSONAL_MAX_LENGTH = 8;

        public OtherFundsValidator()
        {
            RuleFor(student => student.EVGovt).Length(1, EVGOVT_MAX_LENGTH).WithMessage("Other Funds: Visitor's Government amount can be up to " + EVGOVT_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.BinationalCommission).Length(1, BINATIONAL_MAX_LENGTH).WithMessage("Other Funds: Binational Commission amount can be up to " + BINATIONAL_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.Personal).Length(1, PERSONAL_MAX_LENGTH).WithMessage("Other Funds: Personal Funds amount can be up to " + PERSONAL_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.Other).SetValidator(new OtherValidator()).When(student => student.Other != null);
        }
    }
}