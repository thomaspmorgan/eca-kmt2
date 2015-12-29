using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class SiteOfActivityExemptUpdateValidator : AbstractValidator<SiteOfActivityExemptUpdate>
    {
        public SiteOfActivityExemptUpdateValidator()
        {
            RuleFor(visitor => visitor.printForm).NotNull().WithMessage("Site of Activity: Print request indicator is required");
        }
    }
}