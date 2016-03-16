using ECA.Business.Validation.SEVIS;
using ECA.Business.Validation.SEVIS.ErrorPaths;
using FluentValidation;
using System;

namespace ECA.Business.Validation.Sevis.Bio
{
    public class FullNameValidator : AbstractValidator<FullName>
    {
        public const int FIRST_NAME_MAX_LENGTH = 80;
        public const int LAST_NAME_MAX_LENGTH = 40;
        public const int PASSPORT_NAME_MAX_LENGTH = 39;
        public const int PREFERRED_NAME_MAX_LENGTH = 145;

        public static string FIRST_NAME_ERROR_MESSAGE = string.Format("Full Name: First Name can be up to {0} characters.", FIRST_NAME_MAX_LENGTH);

        public static string LAST_NAME_ERROR_MESSAGE = string.Format("Full Name: Last Name can be up to {0} characters.", LAST_NAME_MAX_LENGTH);

        public static string PASSPORT_NAME_ERROR_MESSAGE = string.Format("Full Name: Passport Name can be up to {0} characters.", PASSPORT_NAME_MAX_LENGTH);

        public static string PREFFERED_NAME_ERROR_MESSAGE = string.Format("Full Name: Preferred Name can be up to {0} characters.", PREFERRED_NAME_MAX_LENGTH);

        public const string JUNIOR_SUFFIX = "Jr.";
        public const string SENIOR_SUFFIX = "Sr.";
        public const string FIRST_SUFFIX = "I";
        public const string SECOND_SUFFIX = "II";
        public const string THIRD_SUFFIX = "III";
        public const string FOURTH_SUFFIX = "IV";


        public static string SUFFIX_MATCHES_STRING = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", JUNIOR_SUFFIX, SENIOR_SUFFIX, FIRST_SUFFIX, SECOND_SUFFIX, THIRD_SUFFIX, FOURTH_SUFFIX);
        public static string SUFFIX_VALUE_ERROR_MESSAGE = string.Format("Full Name: The name suffix must be one of the following values:  {0}.", String.Join(JUNIOR_SUFFIX, SENIOR_SUFFIX, FIRST_SUFFIX, SECOND_SUFFIX, THIRD_SUFFIX, FOURTH_SUFFIX));

        public FullNameValidator()
        {
            RuleFor(visitor => visitor.FirstName)
                .Length(1, FIRST_NAME_MAX_LENGTH)
                .WithMessage(FIRST_NAME_ERROR_MESSAGE)
                .WithState(x => new FullNameErrorPath());

            RuleFor(visitor => visitor.LastName)
                .NotNull()
                .WithMessage(LAST_NAME_ERROR_MESSAGE)
                .WithState(x => new FullNameErrorPath())
                .Length(1, LAST_NAME_MAX_LENGTH)
                .WithMessage(LAST_NAME_ERROR_MESSAGE)
                .WithState(x => new FullNameErrorPath());

            RuleFor(visitor => visitor.Suffix)
                .Matches(SUFFIX_MATCHES_STRING)
                .WithMessage(SUFFIX_VALUE_ERROR_MESSAGE)
                .WithState(x => new FullNameErrorPath());

            RuleFor(visitor => visitor.PassportName)
                .Length(0, PASSPORT_NAME_MAX_LENGTH)
                .WithMessage(PASSPORT_NAME_ERROR_MESSAGE)
                .WithState(x => new FullNameErrorPath());

            RuleFor(visitor => visitor.PreferredName)
                .Length(0, PREFERRED_NAME_MAX_LENGTH)
                .WithMessage(PREFFERED_NAME_ERROR_MESSAGE)
                .WithState(x => new FullNameErrorPath());
        }
    }
}
