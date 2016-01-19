using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class AddUpdateDependentValidator : AbstractValidator<AddUpdatedDependent>
    {
        public const int CITY_MAX_LENGTH = 50;
        public const int COUNTRY_CODE_LENGTH = 2;
        public const int EMAIL_MAX_LENGTH = 255;
        public const int BIRTH_COUNTRY_REASON_LENGTH = 2;
        public const int RELATIONSHIP_LENGTH = 2;
        public const int FORM_PURPOSE_LENGTH = 2;

        public AddUpdateDependentValidator()
        {
            RuleFor(dependent => dependent.printForm).NotNull().WithMessage("Dependent: Print form option is required");
            RuleFor(dependent => dependent.FullName).NotNull().WithMessage("Dependent: Full Name is required");
            RuleFor(dependent => dependent.FullName.LastName).NotNull().WithMessage("Dependent: Last Name is required");
            RuleFor(dependent => dependent.BirthDate).NotNull().WithMessage("Dependent: Date of birth is required");
            RuleFor(dependent => dependent.Gender).NotNull().WithMessage("Dependent: Gender is required");
            RuleFor(dependent => dependent.BirthCity).Length(1, CITY_MAX_LENGTH).WithMessage("Dependent: City of Birth is required and can be up to " + CITY_MAX_LENGTH.ToString() + " characters");
            RuleFor(dependent => dependent.BirthCountryCode).NotNull().WithMessage("Dependent: Country of birth is required").Length(2);
            RuleFor(dependent => dependent.CitizenshipCountryCode).Length(COUNTRY_CODE_LENGTH).WithMessage("Dependent: Country of citizenship is required and must be " + COUNTRY_CODE_LENGTH.ToString() + " characters");
            RuleFor(dependent => dependent.PermanentResidenceCountryCode).Length(COUNTRY_CODE_LENGTH).WithMessage("Dependent: Permanent Residence Country is required and must be " + COUNTRY_CODE_LENGTH.ToString() + " characters");
            RuleFor(dependent => dependent.BirthCountryReason).Length(0, BIRTH_COUNTRY_REASON_LENGTH).WithMessage("Dependent: Birth Country Reason must be " + BIRTH_COUNTRY_REASON_LENGTH.ToString() + " characters");
            RuleFor(dependent => dependent.EmailAddress).Length(0, EMAIL_MAX_LENGTH).WithMessage("Dependent: Email can be up to " + EMAIL_MAX_LENGTH.ToString() + " characters").EmailAddress().WithMessage("Dependent: Email is invalid");
            RuleFor(dependent => dependent.Relationship).Length(RELATIONSHIP_LENGTH).WithMessage("Dependent: Relationship is required and must be " + RELATIONSHIP_LENGTH.ToString() + " characters");
            RuleFor(dependent => dependent.FormPurpose).Length(FORM_PURPOSE_LENGTH).WithMessage("Dependent: Purpose of Form is required and must be " + FORM_PURPOSE_LENGTH.ToString() + " characters");
        }
    }
}