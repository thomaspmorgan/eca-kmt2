﻿using ECA.Business.Service.Admin;
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
    public abstract class BiographicalValidator<T> : AbstractValidator<T> where T : IBiographical
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
        public const int BIRTH_COUNTRY_REASON_LENGTH = 6;

        /// <summary>
        /// The maximum length of an email address.
        /// </summary>
        public const int EMAIL_MAX_LENGTH = 255;

        /// <summary>
        /// The error message to form when a birth date is required.
        /// </summary>
        public const string BIRTH_DATE_NULL_ERROR_MESSAGE = "A Date of Birth for the {0}, {1}, is required and must not be estimated.";

        /// <summary>
        /// The error message to return when a full name is required.
        /// </summary>
        public const string FULL_NAME_NULL_ERROR_MESSAGE = "The Full Name is required.";

        /// <summary>
        /// The error message to format when a person's gender is required.
        /// </summary>
        public const string GENDER_REQUIRED_ERROR_MESSAGE = "The gender of the {0}, {1}, is required. The gender must either be male or female.";

        /// <summary>
        /// The error message to format when a gender is specified but not of one of the correct values.
        /// </summary>
        public static string GENDER_MUST_BE_A_VALUE_ERROR_MESSAGE = "The Gender of the {0}, {1}, must be '{2}' or '{3}'.";

        /// <summary>
        /// The error message to format when a city of birth is required.
        /// </summary>
        public const string CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE = "The City of Birth for the {0}, {1}, is required.";

        /// <summary>
        /// The error message to return when a country of birth is required.
        /// </summary>
        public const string BIRTH_COUNTRY_CODE_ERROR_MESSAGE = "The Country of Birth for the {0}, {1}, is required.";

        /// <summary>
        /// The error message to format when a country of citizenship is not specified, or more than country of citizenship is specified.
        /// </summary>
        public const string CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE = "One and only one country of citizenship for the {0}, {1}, is required.";

        /// <summary>
        /// The error message to format when a permanent residence country is not specified via a home address.
        /// </summary>
        public static string PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE = "The Permanent Residence Country is required for the {0}, {1}.  Add a home address outside of the {0}.";

        /// <summary>
        /// The error message to format when a birth country reason is not valid.
        /// </summary>
        public const string BIRTH_COUNTRY_REASON_ERROR_MESSAGE = "A Birth Country Reason for the {0}, {1}, must be {0} characters.";

        /// <summary>
        /// The error message to format when an email is invalid.
        /// </summary>
        public const string EMAIL_ERROR_MESSAGE = "The email address '{0}' for the {1}, {2}, is invalid.  It may be up to {3} characters long.";

        /// <summary>
        /// The error message to return when a visiting phone number is required.
        /// </summary>
        public const string VISITING_PHONE_REQUIRED_ERROR_MESSAGE = "A '{0}' US phone number for the {1}, {2}, is required.";

        /// <summary>
        /// The error message to format when a phone number is required and the specifics of that phone number.
        /// </summary>
        public const string PHONE_NUMBER_ERROR_MESSAGE = "The '{0}' phone number '{1}' for the {2}, {3} must be valid in the United States for example '{4}'.";

        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public BiographicalValidator()
        {
            RuleFor(visitor => visitor.FullName)
                .NotNull()
                .WithMessage(FULL_NAME_NULL_ERROR_MESSAGE)
                .SetValidator(x => (new FullNameValidator(GetPersonType(x))));

            RuleFor(visitor => visitor.BirthDate)
                .NotNull()
                .WithMessage(BIRTH_DATE_NULL_ERROR_MESSAGE, GetPersonTypeDelegate(), GetNameDelegate())
                .WithState(x => new BirthDateErrorPath());

            RuleFor(visitor => visitor.Gender)
                .NotNull()
                .WithMessage(GENDER_REQUIRED_ERROR_MESSAGE, GetPersonTypeDelegate(), GetNameDelegate())
                .WithState(x => new GenderErrorPath())
                .Matches(string.Format("({0}|{1})", Gender.SEVIS_MALE_GENDER_CODE_VALUE, Gender.SEVIS_FEMALE_GENDER_CODE_VALUE))
                .WithMessage(GENDER_MUST_BE_A_VALUE_ERROR_MESSAGE, GetPersonTypeDelegate(), GetNameDelegate(), (p) => Gender.SEVIS_MALE_GENDER_CODE_VALUE, (p) => Gender.SEVIS_FEMALE_GENDER_CODE_VALUE)
                .WithState(x => new GenderErrorPath());

            RuleFor(visitor => visitor.BirthCity)
                .NotNull()
                .WithMessage(CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE, GetPersonTypeDelegate(), GetNameDelegate())
                .WithState(x => new CityOfBirthErrorPath())
                .Length(1, CITY_MAX_LENGTH)
                .WithMessage(CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE, GetPersonTypeDelegate(), GetNameDelegate())
                .WithState(x => new CityOfBirthErrorPath());

            RuleFor(visitor => visitor.BirthCountryCode)
                .NotNull()
                .WithMessage(BIRTH_COUNTRY_CODE_ERROR_MESSAGE, GetPersonTypeDelegate(), GetNameDelegate())
                .WithState(x => new CountryOfBirthErrorPath())
                .Length(COUNTRY_CODE_LENGTH)
                .WithMessage(BIRTH_COUNTRY_CODE_ERROR_MESSAGE, GetPersonTypeDelegate(), GetNameDelegate())
                .WithState(x => new CountryOfBirthErrorPath());

            RuleFor(visitor => visitor.CitizenshipCountryCode)
                .NotNull()
                .WithMessage(CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE, GetPersonTypeDelegate(), GetNameDelegate())
                .WithState(x => new CitizenshipErrorPath())
                .Length(COUNTRY_CODE_LENGTH)
                .WithMessage(CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE, GetPersonTypeDelegate(), GetNameDelegate())
                .WithState(x => new CitizenshipErrorPath());

            RuleFor(visitor => visitor.PermanentResidenceCountryCode)
                .NotNull()
                .WithMessage(PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE, GetPersonTypeDelegate(), GetNameDelegate(), (p) => LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME)
                .WithState(x => new PermanentResidenceCountryErrorPath())
                .Length(COUNTRY_CODE_LENGTH)
                .WithMessage(PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE, GetPersonTypeDelegate(), GetNameDelegate(), (p) => LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME)
                .WithState(x => new PermanentResidenceCountryErrorPath());

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
                    .WithMessage(EMAIL_ERROR_MESSAGE, emailDelegate, GetPersonTypeDelegate(), GetNameDelegate(), maxEmailLengthDelete)
                    .WithState(x => new EmailErrorPath())
                    .Length(0, EMAIL_MAX_LENGTH)
                    .WithMessage(EMAIL_ERROR_MESSAGE, emailDelegate, GetPersonTypeDelegate(), GetNameDelegate(), maxEmailLengthDelete)
                    .WithState(x => new EmailErrorPath());
            });

            When(x => x.PhoneNumber != null, () =>
            {
                Func<T, object> numberTypeDelegate = (p) =>
                {
                    return Data.PhoneNumberType.Visiting.Value;
                };
                Func<T, object> phoneNumberDelegate = (p) =>
                {
                    return p.PhoneNumber != null ? p.PhoneNumber : null;
                };
                Func<T, object> getExampleUSPhoneNumberDelegate = (p) =>
                {
                    var phonenumberUtil = PhoneNumberUtil.GetInstance();
                    var example = phonenumberUtil.GetExampleNumber(Data.PhoneNumber.US_PHONE_NUMBER_REGION_KEY);
                    return phonenumberUtil.Format(example, PhoneNumberFormat.INTERNATIONAL);
                };
                RuleFor(x => x.PhoneNumber)
                    .Must((phone) =>
                    {
                        try
                        {
                            var phonenumberUtil = PhoneNumberUtil.GetInstance();
                            var usPhoneNumber = phonenumberUtil.Parse(phone, Data.PhoneNumber.US_PHONE_NUMBER_REGION_KEY);
                            return phonenumberUtil.IsValidNumber(usPhoneNumber);
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    })
                    .WithMessage(PHONE_NUMBER_ERROR_MESSAGE, numberTypeDelegate, phoneNumberDelegate, GetPersonTypeDelegate(), GetNameDelegate(), getExampleUSPhoneNumberDelegate)
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

        /// <summary>
        /// Returns a Func capable of creating a full name string.
        /// </summary>
        /// <returns>The delegate to return a full name as a string.</returns>
        public abstract Func<T, object> GetNameDelegate();

        /// <summary>
        /// Returns a string detailing the person type, such as Participant or Dependent.
        /// </summary>
        /// <returns>The person type as a string.</returns>
        protected Func<T, object> GetPersonTypeDelegate()
        {
            return (i) => GetPersonType(i);
        }

        /// <summary>
        /// Returns a string representing the person type.
        /// </summary>
        /// <returns>The person type as a string.</returns>
        public abstract string GetPersonType(T instance);
    }
}