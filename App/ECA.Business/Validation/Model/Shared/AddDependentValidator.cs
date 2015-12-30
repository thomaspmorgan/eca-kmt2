using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class AddDependentValidator : AbstractValidator<AddDependent>
    {
        public const int CITY_MAX_LENGTH = 50;
        public const int COUNTRY_CODE_LENGTH = 2;
        public const int EMAIL_MAX_LENGTH = 255;
        public const int BIRTH_COUNTRY_REASON_LENGTH = 2;
        public const int USERDEFINEDA_MAX_LENGTH = 10;
        public const int USERDEFINEDB_MAX_LENGTH = 14;

        public AddDependentValidator()
        {
            RuleFor(student => student.FullName).NotNull().WithMessage("Dependent: Full Name is required");
            RuleFor(student => student.FullName.LastName).NotNull().WithMessage("Dependent: Last Name is required");
            RuleFor(student => student.BirthDate).NotNull().WithMessage("Dependent: Date of birth is required");
            RuleFor(student => student.Gender).NotNull().WithMessage("Dependent: Gender is required");
            RuleFor(student => student.BirthCity).Length(1, CITY_MAX_LENGTH).WithMessage("Personal Info: City of Birth can be up to " + CITY_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.BirthCountryCode).NotNull().WithMessage("Dependent: Country of birth is required").Length(2);
            RuleFor(student => student.CitizenshipCountryCode).Length(2).WithMessage("Dependent: Country of citizenship is required");
            RuleFor(student => student.PermanentResidenceCountryCode).NotNull().WithMessage("Dependent: Permanent Residence Country is required").Length(COUNTRY_CODE_LENGTH).WithMessage("Personal Info: Permanent Residence Country must be " + COUNTRY_CODE_LENGTH.ToString() + " characters");
            RuleFor(student => student.BirthCountryReason).Length(BIRTH_COUNTRY_REASON_LENGTH).WithMessage("Dependent: Birth Country Reason must be " + BIRTH_COUNTRY_REASON_LENGTH.ToString() + " characters");
            RuleFor(student => student.EmailAddress).Length(0, EMAIL_MAX_LENGTH).WithMessage("Dependent: Email can be up to " + EMAIL_MAX_LENGTH.ToString() + " characters").EmailAddress().WithMessage("Dependent: Email is invalid");
            RuleFor(dependent => dependent.UserDefinedA).Length(0, USERDEFINEDA_MAX_LENGTH).WithMessage("Dependent: User Defined A can be up to " + USERDEFINEDA_MAX_LENGTH.ToString() + " characters");
            RuleFor(dependent => dependent.UserDefinedB).Length(0, USERDEFINEDB_MAX_LENGTH).WithMessage("Dependent: User Defined B can be up to " + USERDEFINEDB_MAX_LENGTH.ToString() + " characters");
        }
    }
}