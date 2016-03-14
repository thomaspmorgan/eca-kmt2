using ECA.Business.Validation.SEVIS.ErrorPaths;
using ECA.Data;
using FluentValidation;

namespace ECA.Business.Validation.Sevis.Bio
{
    public class BiographicalValidator<T> : AbstractValidator<T> where T : IBiographical
    {
        public const int CITY_MAX_LENGTH = 50;
        public const int COUNTRY_CODE_LENGTH = 2;
        public const int VISA_TYPE_LENGTH = 2;
        public const int BIRTH_COUNTRY_REASON_LENGTH = 2;
        public const int EMAIL_MAX_LENGTH = 255;

        public const string BIRTH_DATE_NULL_ERROR_MESSAGE = "EV Biographical Info: Date of Birth is required and must not be estimated.";

        public const string FULL_NAME_NULL_ERROR_MESSAGE = "EV Biographical Info: Full Name is required.";

        public const string GENDER_REQUIRED_ERROR_MESSAGE = "EV Biographical Info: Gender is required. The individual's gender must either be male or female.";

        public static string GENDER_MUST_BE_A_VALUE_ERROR_MESSAGE = string.Format("EV Biographical Info: Gender must be {0} or {1}.", Gender.SEVIS_MALE_GENDER_CODE_VALUE, Gender.SEVIS_FEMALE_GENDER_CODE_VALUE);

        public const string CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE = "EV Biographical Info: The City of Birth is required.";

        public const string BIRTH_COUNTRY_CODE_ERROR_MESSAGE = "EV Biographical Info: The Country of Birth is required.";

        public const string CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE = "EV Biographical Info: One and only one country of citizenship is required.";

        public const string PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE = "EV Biographical Info: Permanent Residence Country is required.  Add a non US address to this person.";

        public static string BIRTH_COUNTRY_REASON_ERROR_MESSAGE = string.Format("EV Biographical Info: Birth Country Reason must be {0} characters.", BIRTH_COUNTRY_REASON_LENGTH);

        public static string EMAIL_ERROR_MESSAGE = string.Format("EV Biographical Info: Email can be up to {0} characters.", EMAIL_MAX_LENGTH);

        public const string INVALID_EMAIL_ERROR_MESSAGE = "EV Biographical Info: Email is invalid.";

        public const int MAX_PHONE_NUMBER_LENGTH = 10;

        public static string PHONE_NUMBER_ERROR_MESSAGE = string.Format("EV Biographical Info:  The phone number may be up to {0} characters long.", MAX_PHONE_NUMBER_LENGTH);

        public BiographicalValidator()
        {
            RuleFor(visitor => visitor.FullName)
                .NotNull()
                .WithMessage(FULL_NAME_NULL_ERROR_MESSAGE)
                .SetValidator(new FullNameValidator());

            RuleFor(visitor => visitor.BirthDate)
                .NotNull()
                .WithMessage(BIRTH_DATE_NULL_ERROR_MESSAGE)
                .WithState(x => new BirthDateErrorPath());

            RuleFor(visitor => visitor.Gender)
                .NotNull()
                .WithMessage(GENDER_REQUIRED_ERROR_MESSAGE)
                .Matches(string.Format("({0}|{1})", Gender.SEVIS_MALE_GENDER_CODE_VALUE, Gender.SEVIS_FEMALE_GENDER_CODE_VALUE))
                .WithMessage(GENDER_MUST_BE_A_VALUE_ERROR_MESSAGE)
                .WithState(x => new GenderErrorPath());

            RuleFor(visitor => visitor.BirthCity)
                .NotNull()
                .WithMessage(CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE)
                .WithState(x => new CityOfBirthErrorPath())
                .Length(1, CITY_MAX_LENGTH)
                .WithMessage(CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE)
                .WithState(x => new CityOfBirthErrorPath());

            RuleFor(visitor => visitor.BirthCountryCode)
                .NotNull()
                .WithMessage(BIRTH_COUNTRY_CODE_ERROR_MESSAGE)
                .WithState(x => new CountryOfBirthErrorPath())
                .Length(COUNTRY_CODE_LENGTH)
                .WithMessage(BIRTH_COUNTRY_CODE_ERROR_MESSAGE)
                .WithState(x => new CountryOfBirthErrorPath());

            RuleFor(visitor => visitor.CitizenshipCountryCode)
                .NotNull()
                .WithMessage(CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE)
                .WithState(x => new CitizenshipErrorPath())
                .Length(COUNTRY_CODE_LENGTH)
                .WithMessage(CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE)
                .WithState(x => new CitizenshipErrorPath());

            RuleFor(visitor => visitor.PermanentResidenceCountryCode)
                .NotNull()
                .WithMessage(PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE)
                .WithState(x => new PermanentResidenceCountryErrorPath())
                .Length(COUNTRY_CODE_LENGTH)
                .WithMessage(PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE)
                .WithState(x => new PermanentResidenceCountryErrorPath());

            RuleFor(visitor => visitor.BirthCountryReason)
                .Length(0, BIRTH_COUNTRY_REASON_LENGTH)
                .WithMessage(BIRTH_COUNTRY_REASON_ERROR_MESSAGE)
                .WithState(x => new CountryOfBirthErrorPath());

            RuleFor(visitor => visitor.EmailAddress)
                .Length(0, EMAIL_MAX_LENGTH)
                .WithMessage(EMAIL_ERROR_MESSAGE)
                .WithState(x => new EmailErrorPath())
                .EmailAddress()
                .WithMessage(INVALID_EMAIL_ERROR_MESSAGE)
                .WithState(x => new EmailErrorPath());

            When(x => x.PhoneNumber != null, () =>
            {
                RuleFor(x => x.PhoneNumber)
                .Length(0, MAX_PHONE_NUMBER_LENGTH)
                .WithMessage(PHONE_NUMBER_ERROR_MESSAGE)
                .WithState(x => new PhoneNumberErrorPath());
            });

            When(x => x.MailAddress != null, () =>
            {
                RuleFor(x => x.MailAddress)
                .SetValidator(new AddressDTOValidator(AddressDTOValidator.PERSON_HOST_ADDRESS));
            });

            When(x => x.USAddress != null, () =>
            {
                RuleFor(x => x.USAddress)
                .SetValidator(new AddressDTOValidator(AddressDTOValidator.C_STREET_ADDRESS));
            });
        }
    }
}