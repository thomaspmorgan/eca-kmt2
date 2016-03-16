using ECA.Business.Validation.Sevis.ErrorPaths;
using ECA.Data;
using FluentValidation;
using PhoneNumbers;
using System;
using System.Text.RegularExpressions;

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

        public const string PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE = "EV Biographical Info: Permanent Residence Country is required.  Add a home address outside of the United States to this person.";

        public static string BIRTH_COUNTRY_REASON_ERROR_MESSAGE = string.Format("EV Biographical Info: Birth Country Reason must be {0} characters.", BIRTH_COUNTRY_REASON_LENGTH);

        public const string EMAIL_ERROR_MESSAGE = "EV Biographical Info: The email address '{0}' is invalid.  It may be up to {1} characters long.";

        public const int MAX_PHONE_NUMBER_LENGTH = 10;

        public const string VISITING_PHONE_REQUIRED_ERROR_MESSAGE = "EV Biographical Info:  A '{0}' US phone number is required.";

        public const string PHONE_NUMBER_ERROR_MESSAGE = "EV Biographical Info:  The '{0}' US phone number '{1}' may be up to {2} characters long.";

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
                .WithState(x => new GenderErrorPath())
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

            When(x => x.EmailAddress != null, () =>
            {
                Func<T, object> emailDelegate = (b) =>
                {
                    return b.EmailAddress != null ? b.EmailAddress : null;
                };
                Func<T, object> maxEmailLengthDelete = (b) =>
                {
                    return EMAIL_MAX_LENGTH.ToString();
                };
                RuleFor(x => x.EmailAddress)
                    .EmailAddress()
                    .WithMessage(EMAIL_ERROR_MESSAGE, emailDelegate, maxEmailLengthDelete)
                    .WithState(x => new EmailErrorPath())
                    .Length(0, EMAIL_MAX_LENGTH)
                    .WithMessage(EMAIL_ERROR_MESSAGE, emailDelegate, maxEmailLengthDelete)
                    .WithState(x => new EmailErrorPath());
            });

            RuleFor(x => x.PhoneNumber)
                .NotNull()
                .WithMessage(VISITING_PHONE_REQUIRED_ERROR_MESSAGE, Data.PhoneNumberType.Visiting.Value)
                .WithState(x => new PhoneNumberErrorPath());

            When(x => x.PhoneNumber != null, () =>
            {
                Func<T, object> numberTypeDelegate = (b) =>
                {
                    return Data.PhoneNumberType.Visiting.Value;
                };
                Func<T, object> maxLengthDelegate = (b) =>
                {
                    return MAX_PHONE_NUMBER_LENGTH.ToString();
                };
                Func<T, object> phoneNumberDelegate = (b) =>
                {
                    return b.PhoneNumber != null ? b.PhoneNumber : null;
                };
                RuleFor(x => x.PhoneNumber)
                    .Must((phone) =>
                    {
                        try
                        {
                            var phonenumberUtil = PhoneNumberUtil.GetInstance();
                            var usPhoneNumber = phonenumberUtil.Parse(phone, "US");
                            return phonenumberUtil.IsValidNumber(usPhoneNumber);
                        }
                        catch(Exception)
                        {
                            return false;
                        }
                    })
                    .WithMessage(PHONE_NUMBER_ERROR_MESSAGE, numberTypeDelegate, phoneNumberDelegate, maxLengthDelegate)
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