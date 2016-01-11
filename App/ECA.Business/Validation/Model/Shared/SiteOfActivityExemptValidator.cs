using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class SiteOfActivityExemptValidator : AbstractValidator<SiteOfActivityExempt>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public SiteOfActivityExemptValidator()
        {
            RuleFor(visitor => visitor.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Site of Activity: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}