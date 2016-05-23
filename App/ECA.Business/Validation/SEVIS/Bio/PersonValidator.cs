using ECA.Business.Validation.Sevis.ErrorPaths;
using ECA.Data;
using FluentValidation;
using System.Text.RegularExpressions;
using System;
using ECA.Business.Service.Admin;
using ECA.Business.Validation.Sevis.Exceptions;
using ECA.Business.Service.Persons;

namespace ECA.Business.Validation.Sevis.Bio
{
    public class PersonValidator : BiographicalValidator<Person>
    {
        /// <summary>
        /// The max length of the position code.
        /// </summary>
        public const int POSITION_CODE_LENGTH = 3;

        /// <summary>
        /// The max length of the program category code.
        /// </summary>
        public const int CATEGORY_CODE_LENGTH = 2;

        /// <summary>
        /// The error message to return when the program category code is required.
        /// </summary>
        public const string CATEGORY_CODE_REQUIRED_ERROR_MESSAGE = "The participant's program category is required.";

        /// <summary>
        /// The error message to return when a program category code exceeds max length.
        /// </summary>
        public static string PROGRAM_CATEGORY_CODE_ERROR_MESSAGE = string.Format("The participant's program category code can be up to {0} characters.", CATEGORY_CODE_LENGTH);

        /// <summary>
        /// The error message to return when a participant's position is required.
        /// </summary>
        public const string POSITION_CODE_REQUIRED_ERROR_MESSAGE = "The participant's position is required.";

        /// <summary>
        /// The position code regular expression.
        /// </summary>
        public static Regex POSITION_CODE_REGEX = new Regex(@"^\d{1,3}$");

        /// <summary>
        /// The error message to return when a participant position code is not between 1 and 3 digits.
        /// </summary>
        public const string POSITION_CODE_MUST_BE_DIGITS_ERROR_MESSAGE = "The participant's positon code must be a number with 1 to 3 digits.";

        /// <summary>
        /// The error message to return when a participant's field of study is required.
        /// </summary>
        public const string SUBJECT_FIELD_REQUIRED_ERROR_MESSAGE = "The participant's field of study is required.";

        /// <summary>
        /// The error message to format when a participant's email address is required.
        /// </summary>
        public const string EMAIL_ADDRESS_REQUIRED_FORMAT_MESSAGE = "A '{0}' email address is required for the participant.";

        /// <summary>
        /// The error message to format when a mailing address is required.
        /// </summary>
        public const string MAILING_ADDRESS_REQUIRED_FORMAT_MESSAGE = "A '{0}' address is required.  It must be in the {1}.";

        /// <summary>
        /// The error message to format when a permanent residence country is not specified via a home address.
        /// </summary>
        public static string PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE = "The Permanent Residence Country is required for the {0}, {1}.  Add one and only one '{2}' address that is outside of the {3}.";

        /// <summary>
        /// The error message to format when a permanent residence country is not supported by sevis.
        /// </summary>
        public static string PERMANENT_RESIDENCE_COUNTRY_NOT_SUPPORTED = "The Permanent Residence Country Code '{0}' is not supported for the {1}, {2}.  Please update the '{3}' address.";

        /// <summary>
        /// The person type for the Participant.
        /// </summary>
        public const string PERSON_TYPE = "participant";

        public const string G_PROGRAM_PREFIX = "G";

        /// <summary>
        /// Creates a new default instance.
        /// <param name="isExchangeVisitorValidated">True, if the exchange visitor has been validated in the sevis api.</param>
        /// <param name="sevisId">The sevis id of the exchange visitor.</param>
        /// <param name="participantStartDate">The particiant/exchange visitor start date.</param>
        /// <param name="sevisOrgId">The sevis org id.</param>
        /// </summary>
        public PersonValidator(string sevisId, string sevisOrgId, bool isExchangeVisitorValidated, DateTime participantStartDate)
            : base()
        {
            this.SevisId = sevisId;
            this.ParticipantStartDate = participantStartDate;
            this.IsValidated = isExchangeVisitorValidated;
            this.SevisOrgId = sevisOrgId;
            Func<Person, object> homeAddressTypeDelelgate = (p) => AddressType.Home.Value;
            Func<Person, object> hostAddressTypeDelegate = (p) => AddressType.Host.Value;

            RuleFor(x => x.ProgramCategoryCode)
                .NotNull()
                .WithMessage(CATEGORY_CODE_REQUIRED_ERROR_MESSAGE)
                .WithState(x => new ProgramCategoryCodeErrorPath())
                .Length(CATEGORY_CODE_LENGTH)
                .WithMessage(PROGRAM_CATEGORY_CODE_ERROR_MESSAGE)
                .WithState(x => new ProgramCategoryCodeErrorPath());

            RuleFor(x => x.PositionCode)
                .NotNull()
                .WithMessage(POSITION_CODE_REQUIRED_ERROR_MESSAGE)
                .WithState(x => new PositionCodeErrorPath())
                .Matches(POSITION_CODE_REGEX)
                .WithMessage(POSITION_CODE_MUST_BE_DIGITS_ERROR_MESSAGE)
                .WithState(x => new PositionCodeErrorPath());

            RuleFor(x => x.SubjectField)
                .NotNull()
                .WithMessage(SUBJECT_FIELD_REQUIRED_ERROR_MESSAGE)
                .WithState(x => new FieldOfStudyErrorPath())
                .SetValidator(new SubjectFieldValidator());

            When(x => !String.IsNullOrWhiteSpace(this.SevisId)
            && !this.IsValidated
            && this.ParticipantStartDate < ParticipantPersonsSevisService.GetEarliestNeedsValidationInfoParticipantDate(),
            () =>
            {
                When(x => this.SevisOrgId == null || !this.SevisOrgId.ToUpper().StartsWith(G_PROGRAM_PREFIX), () =>
                {
                    RuleFor(x => x.EmailAddress)
                    .NotNull()
                    .WithMessage(EMAIL_ADDRESS_REQUIRED_FORMAT_MESSAGE, EmailAddressType.Personal.Value)
                    .WithState(x => new EmailErrorPath());

                    RuleFor(x => x.PhoneNumber)
                        .NotNull()
                        .WithMessage(VISITING_PHONE_REQUIRED_ERROR_MESSAGE, (p) => Data.PhoneNumberType.Visiting.Value, GetPersonTypeDelegate(), GetNameDelegate())
                        .WithState(x => new PhoneNumberErrorPath());
                });

                RuleFor(visitor => visitor.MailAddress)
                    .NotNull()
                    .WithMessage(MAILING_ADDRESS_REQUIRED_FORMAT_MESSAGE, hostAddressTypeDelegate, p => LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME)
                    .WithState(x => new AddressErrorPath())
                    .SetValidator(new AddressDTOValidator(a => AddressType.Host.Value));
            });

            RuleFor(x => x.PermanentResidenceCountryCode)
                .NotNull()
                .WithMessage(PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE, GetPersonTypeDelegate(), GetNameDelegate(), homeAddressTypeDelelgate, (p) => LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME)
                .WithState(x => GetPermanentResidenceCountryCodeErrorPath(x));

            When(x => x.PermanentResidenceCountryCode != null, () =>
            {
                RuleFor(x => x.PermanentResidenceCountryCode)
                .Must((code) =>
                {
                    try
                    {
                        var codeType = code.GetCountryCodeWithType();
                        return true;
                    }
                    catch (CodeTypeConversionException)
                    {
                        return false;
                    }
                })
                .WithMessage(PERMANENT_RESIDENCE_COUNTRY_NOT_SUPPORTED, (p) => p.PermanentResidenceCountryCode, GetPersonTypeDelegate(), GetNameDelegate(), homeAddressTypeDelelgate)
                .WithState(x => GetPermanentResidenceCountryCodeErrorPath(x));
            });
        }

        /// <summary>
        /// Gets the sevis id of the person.
        /// </summary>
        public string SevisId { get; private set; }

        /// <summary>
        /// Gets the participant's start date.
        /// </summary>
        public DateTime ParticipantStartDate { get; private set; }

        /// <summary>
        /// Gets the sevis org id.
        /// </summary>
        public string SevisOrgId { get; private set; }

        /// <summary>
        /// Gets the is validated flag.
        /// </summary>
        public bool IsValidated { get; private set; }

        /// <summary>
        /// Returns a delegate that creates a full name string, or the empty string.
        /// </summary>
        /// <returns>A delegate that creates a full name string, or the empty string.</returns>
        public override Func<Person, object> GetNameDelegate()
        {
            return (p) =>
            {
                return p.FullName != null ? String.Format("{0} {1}", p.FullName.FirstName, p.FullName.LastName).Trim() : String.Empty;
            };
        }

        /// <summary>
        /// Returns the person type.
        /// </summary>
        /// <param name="instance">The person.</param>
        /// <returns>The person type.</returns>
        public override string GetPersonType(Person instance)
        {
            return PERSON_TYPE;
        }

        /// <summary>
        /// Returns the birth date error path.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The error path.</returns>
        public override ErrorPath GetBirthDateErrorPath(Person instance)
        {
            return new BirthDateErrorPath();
        }

        /// <summary>
        /// Returns the gender error path.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The error path.</returns>
        public override ErrorPath GetGenderErrorPath(Person instance)
        {
            return new GenderErrorPath();
        }

        /// <summary>
        /// Returns the birth city error path.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The error path.</returns>
        public override ErrorPath GetBirthCityErrorPath(Person instance)
        {
            return new CityOfBirthErrorPath();
        }

        /// <summary>
        /// Returns the birth country code error path.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The error path.</returns>
        public override ErrorPath GetBirthCountryCodeErrorPath(Person instance)
        {
            return new CountryOfBirthErrorPath();
        }

        /// <summary>
        /// Returns the citizneship country code error path.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The error path.</returns>
        public override ErrorPath GetCitizenshipCountryCodeErrorPath(Person instance)
        {
            return new CitizenshipErrorPath();
        }

        /// <summary>
        /// Returns the birth date error path.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The error path.</returns>
        public override ErrorPath GetPermanentResidenceCountryCodeErrorPath(Person instance)
        {
            return new PermanentResidenceCountryErrorPath();
        }

        /// <summary>
        /// Returns the email address error path.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The error path.</returns>
        public override ErrorPath GetEmailAddressErrorPath(Person instance)
        {
            return new EmailErrorPath();
        }

        /// <summary>
        /// Returns the phone number error path.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The error path.</returns>
        public override ErrorPath GetPhoneNumberErrorPath(Person instance)
        {
            return new PhoneNumberErrorPath();
        }
    }
}
