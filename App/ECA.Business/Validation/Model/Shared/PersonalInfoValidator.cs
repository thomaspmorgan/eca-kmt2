using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class PersonalInfoValidator : AbstractValidator<PersonalInfo>
    {
        public const int GENDER_CODE_LENGTH = 1;
        public const int CITY_MAX_LENGTH = 50;
        public const int COUNTRY_CODE_LENGTH = 2;
        public const int EMAIL_MAX_LENGTH = 255;
        public const int VISA_TYPE_LENGTH = 2;
        public const int BIRTH_COUNTRY_REASON_LENGTH = 2;

        public PersonalInfoValidator()
        {
            RuleFor(student => student.fullName).NotNull().WithMessage("Personal Info: Full Name is required");
            When(student => student.fullName != null, () =>
            {
                RuleFor(student => student.fullName).SetValidator(new FullNameValidator());
            });
            RuleFor(student => student.BirthDate).NotNull().WithMessage("Personal Info: Date of Birth is required");
            RuleFor(student => student.Gender).NotNull().WithMessage("Personal Info: Gender is required").Length(GENDER_CODE_LENGTH).WithMessage("Personal Info: Gender must be " + GENDER_CODE_LENGTH.ToString() + " character");
            RuleFor(student => student.BirthCity).Length(1, CITY_MAX_LENGTH).WithMessage("Personal Info: City of Birth can be up to " + CITY_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.BirthCountryCode).NotNull().WithMessage("Personal Info: Country of Birth is required").Length(COUNTRY_CODE_LENGTH).WithMessage("Personal Info: Country of Birth must be " + COUNTRY_CODE_LENGTH.ToString() + " characters");
            RuleFor(student => student.CitizenshipCountryCode).NotNull().WithMessage("Personal Info: Country of Citizenship is required").Length(COUNTRY_CODE_LENGTH).WithMessage("Personal Info: Country of Citizenship must be " + COUNTRY_CODE_LENGTH.ToString() + " characters");
            RuleFor(student => student.PermanentResidenceCountryCode).NotNull().WithMessage("Personal Info: Permanent Residence Country is required").Length(COUNTRY_CODE_LENGTH).WithMessage("Personal Info: Permanent Residence Country must be " + COUNTRY_CODE_LENGTH.ToString() + " characters");
            RuleFor(student => student.BirthCountryReason).Length(BIRTH_COUNTRY_REASON_LENGTH).WithMessage("Personal Info: Birth Country Reason must be " + BIRTH_COUNTRY_REASON_LENGTH.ToString() + " characters");
            RuleFor(student => student.Email).Length(0, EMAIL_MAX_LENGTH).WithMessage("Personal Info: Email can be up to " + EMAIL_MAX_LENGTH.ToString() + " characters").EmailAddress().WithMessage("Personal Info: Email is invalid");
            RuleFor(student => student.VisaType).NotNull().WithMessage("Personal Info: Visa Type is required").Length(VISA_TYPE_LENGTH).WithMessage("Personal Info: Visa Type must be " + VISA_TYPE_LENGTH.ToString() + " characters").When(student => student.PermanentResidenceCountryCode == null);
        }
    }
}
