using ECA.Business.Service.Admin;
using ECA.Business.Validation.Sevis.ErrorPaths;
using ECA.Data;
using FluentValidation;
using PhoneNumbers;
using System;
using System.Text.RegularExpressions;

namespace ECA.Business.Validation.Sevis.Bio
{
    /// <summary>
    /// The BiographicalValidator is used to validate biographical information for sevis exchange visitors.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BiographicalValidator<T> : AbstractValidator<T> where T : IBiographical
    {
        /// <summary>
        /// The maximum length of a city name.
        /// </summary>
        public const int CITY_MAX_LENGTH = 50;

        /// <summary>
        /// The maximum length of a country code.
        /// </summary>
        public const int COUNTRY_CODE_LENGTH = 2;

        /// <summary>
        /// The maximum length of a birth country reason.
        /// </summary>
        public const int BIRTH_COUNTRY_REASON_LENGTH = 2;

        /// <summary>
        /// The maximum length of an email address.
        /// </summary>
        public const int EMAIL_MAX_LENGTH = 255;

        /// <summary>
        /// The error message to return when a birth date is required.
        /// </summary>
        public const string BIRTH_DATE_NULL_ERROR_MESSAGE = "A Date of Birth is required and must not be estimated.";

        /// <summary>
        /// The error message to return when a full name is required.
        /// </summary>
        public const string FULL_NAME_NULL_ERROR_MESSAGE = "The Full Name is required.";

        /// <summary>
        /// The error message to return when a person's gender is required.
        /// </summary>
        public const string GENDER_REQUIRED_ERROR_MESSAGE = "The Gender is required. The individual's gender must either be male or female.";

        /// <summary>
        /// The error message to format when a gender is specified but not of one of the correct values.
        /// </summary>
        public static string GENDER_MUST_BE_A_VALUE_ERROR_MESSAGE = string.Format("The Gender must be '{0}' or '{1}'.", Gender.SEVIS_MALE_GENDER_CODE_VALUE, Gender.SEVIS_FEMALE_GENDER_CODE_VALUE);

        /// <summary>
        /// The error message to return when a city of birth is required.
        /// </summary>
        public const string CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE = "The City of Birth is required.";

        /// <summary>
        /// The error message to return when a country of birth is required.
        /// </summary>
        public const string BIRTH_COUNTRY_CODE_ERROR_MESSAGE = "The Country of Birth is required.";

        /// <summary>
        /// The error message to return when a country of citizenship is not specified, or more than country of citizenship is specified.
        /// </summary>
        public const string CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE = "One and only one country of citizenship is required.";

        /// <summary>
        /// The error message to reutnr when a permananet residence country is not specified via a home address.
        /// </summary>
        public static string PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE = 
            String.Format("The Permanent Residence Country is required.  Add a home address outside of the {0} to this person.", LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME);

        /// <summary>
        /// The error message to return when a birth country reason is not valid.
        /// </summary>
        public static string BIRTH_COUNTRY_REASON_ERROR_MESSAGE = string.Format("A Birth Country Reason must be {0} characters.", BIRTH_COUNTRY_REASON_LENGTH);

        /// <summary>
        /// The error message to format when an email is invalid.
        /// </summary>
        public const string EMAIL_ERROR_MESSAGE = "The email address '{0}' is invalid.  It may be up to {1} characters long.";

        /// <summary>
        /// The maximum length of a phone number.
        /// </summary>
        public const int MAX_PHONE_NUMBER_LENGTH = 10;

        /// <summary>
        /// The error message to return when a visiting phone number is required.
        /// </summary>
        public const string VISITING_PHONE_REQUIRED_ERROR_MESSAGE = "A '{0}' US phone number is required.";

        /// <summary>
        /// The error message to format when a phone number is required and the specifics of that phone number.
        /// </summary>
        public const string PHONE_NUMBER_ERROR_MESSAGE = "The '{0}' US phone number '{1}' may be up to {2} characters long.";

        /// <summary>
        /// Creates a new default instance.
        /// </summary>
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
                .SetValidator(new AddressDTOValidator((a) => AddressDTOValidator.PERSON_HOST_ADDRESS));
            });

            When(x => x.USAddress != null, () =>
            {
                RuleFor(x => x.USAddress)
                .SetValidator(new AddressDTOValidator());
            });
        }
    }
}