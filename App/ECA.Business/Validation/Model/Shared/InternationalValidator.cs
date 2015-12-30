using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class InternationalValidator : AbstractValidator<International>
    {
        public const int ORG_LENGTH = 3;
        public const int AMOUNT_MAX_LENGTH = 8;

        public InternationalValidator()
        {
            RuleFor(student => student.Org1).NotNull().WithMessage("International Funds: International Organization 1 is required").Length(1, ORG_LENGTH).WithMessage("International Funds: International Organization code must be " + ORG_LENGTH.ToString() + " characters");
            RuleFor(student => student.Amount1).NotNull().WithMessage("International Funds: International Organization 1 Amount is required").Length(1, AMOUNT_MAX_LENGTH).WithMessage("International Funds: International Organization Amount can be up to " + AMOUNT_MAX_LENGTH.ToString() + " characters");
        }
    }
}