using ECA.Business.Validation.Sevis.ErrorPaths;
using ECA.Data;
using FluentValidation;
using System.Text.RegularExpressions;
using System;

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
        /// The person type for the Participant.
        /// </summary>
        public const string PERSON_TYPE = "participant";

        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public PersonValidator()
            : base()
        {
            RuleFor(x => x.ProgramCategoryCode)
                .NotNull()
                .WithMessage(CATEGORY_CODE_REQUIRED_ERROR_MESSAGE)
                .WithState(x => new ProgramCategoryCodeErrorPath())
                .Length(CATEGORY_CODE_LENGTH)
                .WithMessage(PROGRAM_CATEGORY_CODE_ERROR_MESSAGE)
                .WithState(x => new ProgramCategoryCodeErrorPath());

            RuleFor(visitor => visitor.PositionCode)
                .NotNull()
                .WithMessage(POSITION_CODE_REQUIRED_ERROR_MESSAGE)
                .WithState(x => new PositionCodeErrorPath())
                .Matches(POSITION_CODE_REGEX)
                .WithMessage(POSITION_CODE_MUST_BE_DIGITS_ERROR_MESSAGE)
                .WithState(x => new PositionCodeErrorPath());

            RuleFor(visitor => visitor.SubjectField)
                .NotNull()
                .WithMessage(SUBJECT_FIELD_REQUIRED_ERROR_MESSAGE)
                .WithState(x => new FieldOfStudyErrorPath())
                .SetValidator(new SubjectFieldValidator());

            RuleFor(x => x.EmailAddress)
                .NotNull()
                .WithMessage(EMAIL_ADDRESS_REQUIRED_FORMAT_MESSAGE, EmailAddressType.Personal.Value)
                .WithState(x => new EmailErrorPath());

            RuleFor(x => x.PhoneNumber)
                .NotNull()
                .WithMessage(VISITING_PHONE_REQUIRED_ERROR_MESSAGE, (p) => Data.PhoneNumberType.Visiting.Value, GetPersonTypeDelegate(), GetNameDelegate())
                .WithState(x => new PhoneNumberErrorPath());
        }

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
    }
}
