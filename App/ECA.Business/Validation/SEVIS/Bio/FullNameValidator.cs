using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Sevis.ErrorPaths;
using FluentValidation;
using System;
using System.Text.RegularExpressions;

namespace ECA.Business.Validation.Sevis.Bio
{
    /// <summary>
    /// A FullName validator is used to validate a sevis exchange visitor's name.
    /// </summary>
    public class FullNameValidator : AbstractValidator<FullName>
    {
        /// <summary>
        /// The max length of the first name.
        /// </summary>
        public const int FIRST_NAME_MAX_LENGTH = 80;

        /// <summary>
        /// The max length of the last name.
        /// </summary>
        public const int LAST_NAME_MAX_LENGTH = 40;

        /// <summary>
        /// The max length of the passport name.
        /// </summary>
        public const int PASSPORT_NAME_MAX_LENGTH = 39;

        /// <summary>
        /// The max length of the preferred or alias name.
        /// </summary>
        public const int PREFERRED_NAME_MAX_LENGTH = 145;

        /// <summary>
        /// The error message to format when a person's first name is to long.
        /// </summary>
        public const string FIRST_NAME_ERROR_MESSAGE = "The {0} person's first name is required, can be up to {1} characters long, and may not contain digits.";

        /// <summary>
        /// The error message to format when a person's last name is to long.
        /// </summary>
        public const string LAST_NAME_ERROR_MESSAGE = "The {0} person's last name, is required, can be up to {1} characters long, and may not contain digits.";

        /// <summary>
        /// The error message to return when a person's passport name is to long.
        /// </summary>
        public const string PASSPORT_NAME_ERROR_MESSAGE = "The {0} person's passport name can be up to {1} characters long and may not contain digits.";

        /// <summary>
        /// The error message to return when a person's preferred name is to long.
        /// </summary>

        public const string PREFFERED_NAME_ERROR_MESSAGE = "The {0} person's preferred name can be up to {1} characters long and may not contain digits.";

        /// <summary>
        /// The sevis junior suffix.
        /// </summary>
        public static string JUNIOR_SUFFIX = NameSuffixCodeType.Jr.ToString();

        /// <summary>
        /// The sevis senior suffix.
        /// </summary>
        public static string SENIOR_SUFFIX = NameSuffixCodeType.Sr.ToString();

        /// <summary>
        /// The sevis first suffix.
        /// </summary>
        public static string FIRST_SUFFIX = NameSuffixCodeType.I.ToString();

        /// <summary>
        /// The sevis second suffix.
        /// </summary>
        public static string SECOND_SUFFIX = NameSuffixCodeType.II.ToString();

        /// <summary>
        /// The sevis third suffix.
        /// </summary>
        public static string THIRD_SUFFIX = NameSuffixCodeType.III.ToString();

        /// <summary>
        /// The sevis fourth suffix.
        /// </summary>
        public static string FOURTH_SUFFIX = NameSuffixCodeType.IV.ToString();

        /// <summary>
        /// The possible suffix values to match.
        /// </summary>
        public static string SUFFIX_MATCHES_STRING = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", JUNIOR_SUFFIX, SENIOR_SUFFIX, FIRST_SUFFIX, SECOND_SUFFIX, THIRD_SUFFIX, FOURTH_SUFFIX);

        /// <summary>
        /// The error message to format when a suffix is not a valid value.
        /// </summary>
        public const string SUFFIX_VALUE_ERROR_MESSAGE = "The {0} name suffix '{1}' is invalid and must be one of the following values:  '{2}'.";



        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public FullNameValidator(string personType)
        {
            var firstNameRegex = new Regex(String.Format("^\\D{{1,{0}}}$", FIRST_NAME_MAX_LENGTH));
            var lastNameRegex = new Regex(String.Format("^\\D{{1,{0}}}$", LAST_NAME_MAX_LENGTH));
            var passportRegex = new Regex(String.Format("^\\D{{0,{0}}}$", PASSPORT_NAME_MAX_LENGTH));
            var preferredNameRegex = new Regex(String.Format("^\\D{{0,{0}}}$", PREFERRED_NAME_MAX_LENGTH));

            RuleFor(visitor => visitor.FirstName)
                .NotNull()
                .WithMessage(FIRST_NAME_ERROR_MESSAGE, (n) => personType, (n) => FIRST_NAME_MAX_LENGTH)
                .WithState(x => new FullNameErrorPath())
                .Matches(firstNameRegex)
                .WithMessage(FIRST_NAME_ERROR_MESSAGE, (n) => personType, (n) => FIRST_NAME_MAX_LENGTH)
                .WithState(x => new FullNameErrorPath());

            RuleFor(visitor => visitor.LastName)
                .NotNull()
                .WithMessage(LAST_NAME_ERROR_MESSAGE, (n) => personType, (n) => LAST_NAME_MAX_LENGTH)
                .WithState(x => new FullNameErrorPath())
                .Matches(lastNameRegex)
                .WithMessage(LAST_NAME_ERROR_MESSAGE, (n) => personType, (n) => LAST_NAME_MAX_LENGTH)
                .WithState(x => new FullNameErrorPath());

            Func<FullName, object> getValidSuffixValues = (o) =>
            {
                return String.Join(", ", JUNIOR_SUFFIX, SENIOR_SUFFIX, FIRST_SUFFIX, SECOND_SUFFIX, THIRD_SUFFIX, FOURTH_SUFFIX);
            };

            Func<FullName, object> getNameSuffixValue = (n) =>
            {
                return n.Suffix;
            };

            RuleFor(visitor => visitor.Suffix)
                .Matches(SUFFIX_MATCHES_STRING)
                .WithMessage(SUFFIX_VALUE_ERROR_MESSAGE, (n) => personType, getNameSuffixValue, getValidSuffixValues)
                .WithState(x => new FullNameErrorPath());

            RuleFor(visitor => visitor.PassportName)
                .Matches(passportRegex)
                .WithMessage(PASSPORT_NAME_ERROR_MESSAGE, (n) => personType, (n) => PASSPORT_NAME_MAX_LENGTH)
                .WithState(x => new FullNameErrorPath());

            RuleFor(visitor => visitor.PreferredName)
                .Matches(preferredNameRegex)
                .WithMessage(PREFFERED_NAME_ERROR_MESSAGE, (n) => personType, (n) => PREFERRED_NAME_MAX_LENGTH)
                .WithState(x => new FullNameErrorPath());
        }
    }
}
