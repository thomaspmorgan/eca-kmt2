using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class FundingOtherValidator : AbstractValidator<FundingOther>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public FundingOtherValidator()
        {
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Funding: Other Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}