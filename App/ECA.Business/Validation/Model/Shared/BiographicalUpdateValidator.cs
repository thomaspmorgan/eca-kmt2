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
            RuleFor(student => student.PhoneNumber).Length(0, PHONE_MAX_LENGTH).WithMessage("Student Print form option is required");
            RuleFor(student => student.PositionCode).Length(0, PHONE_MAX_LENGTH).WithMessage("Student Print form option is required");
            RuleFor(student => student.Remarks).Length(0, PHONE_MAX_LENGTH).WithMessage("Student Print form option is required");
        }
    }
}