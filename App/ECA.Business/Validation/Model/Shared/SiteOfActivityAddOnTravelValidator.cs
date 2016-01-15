using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class SiteOfActivityAddOnTravelValidator : AbstractValidator<SiteOfActivityAddOnTravel>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public SiteOfActivityAddOnTravelValidator()
        {
            RuleFor(visitor => visitor.printForm).NotNull().WithMessage("Site of Activity Travel: Print form option is required");
            RuleFor(visitor => visitor.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Site of Activity Travel: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}