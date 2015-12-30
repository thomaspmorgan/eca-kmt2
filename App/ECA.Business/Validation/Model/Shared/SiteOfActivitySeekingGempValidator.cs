using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class SiteOfActivitySeekingGempValidator : AbstractValidator<SiteOfActivitySeekingGemp>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public SiteOfActivitySeekingGempValidator()
        {
            RuleFor(visitor => visitor.printForm).NotNull().WithMessage("Site of Activity (seeking emp): Print request indicator is required");
            RuleFor(visitor => visitor.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Site of Activity (seeking emp): Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}