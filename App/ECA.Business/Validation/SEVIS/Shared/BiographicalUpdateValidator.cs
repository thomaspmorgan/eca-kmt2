using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class BiographicalUpdateValidator : AbstractValidator<BiographicalUpdate>
    {
        public const int PHONE_MAX_LENGTH = 10;
        public const int POSITION_MAX_LENGTH = 25;
        public const int REMARKS_MAX_LENGTH = 500;

        public BiographicalUpdateValidator()
        {
            RuleFor(bio => bio.PhoneNumber).Length(0, PHONE_MAX_LENGTH).WithMessage("EV Biographical Info: Print form option is required");
            RuleFor(bio => bio.PositionCode).Length(0, PHONE_MAX_LENGTH).WithMessage("EV Biographical Info: Print form option is required");
            RuleFor(bio => bio.Remarks).Length(0, PHONE_MAX_LENGTH).WithMessage("EV Biographical Info: Print form option is required");
        }
    }
}