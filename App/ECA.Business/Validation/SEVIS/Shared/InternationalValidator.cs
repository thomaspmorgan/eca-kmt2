using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class InternationalValidator : AbstractValidator<International>
    {
        public const int ORG_LENGTH = 3;
        public const int AMOUNT_MAX_LENGTH = 8;

        public InternationalValidator()
        {
            RuleFor(student => student.Org1).Length(1, ORG_LENGTH).WithMessage("Exch. Visitor Intl Funds: International Org Code is required and must be " + ORG_LENGTH.ToString() + " characters");
            RuleFor(student => student.Amount1).Length(1, AMOUNT_MAX_LENGTH).WithMessage("Exch. Visitor Intl Funds: International Org Amount is required and can be up to " + AMOUNT_MAX_LENGTH.ToString() + " characters");
        }
    }
}