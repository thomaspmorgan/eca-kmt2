using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class HostFamilyValidator : AbstractValidator<HostFamily>
    {
        public const int PHONE_MAX_LENGTH = 12;
        public const int PHONEXT_MAX_LENGTH = 4;

        public HostFamilyValidator()
        {
            RuleFor(student => student.Phone).Length(0, 12).WithMessage("Host Family: Phone number can be up to " + PHONE_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.PhoneExt).Length(0, 4).WithMessage("Host Family: Phone extension can be up to " + PHONEXT_MAX_LENGTH.ToString() + " characters");
        }
    }
}