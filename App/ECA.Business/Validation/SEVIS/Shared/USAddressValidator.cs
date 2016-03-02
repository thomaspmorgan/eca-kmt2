using ECA.Business.Validation.SEVIS;
using FluentValidation;
using System.Text.RegularExpressions;

namespace ECA.Business.Validation.Model.Shared
{
    public class USAddressValidator : AbstractValidator<USAddress>
    {
        public const int ADDRESS_MAX_LENGTH = 64;
        public const int CITY_MAX_LENGTH = 60;
        public const int POSTAL_CODE_LENGTH = 5;
        public const int EXPLANATION_CODE_LENGTH = 2;
        public const int EXPLANATION_MIN_LENGTH = 5;
        public const int EXPLANATION_MAX_LENGTH = 200;

        public static string ADDRESS_1_ERROR_MESSAGE = string.Format("Address: Address Line 1 is required and can be up to {0} characters", ADDRESS_MAX_LENGTH);

        public const int MAX_POSTAL_CODE_LENGTH = 5;

        public const string POSTAL_CODE_REGEX = @"^\d{5}$";

        public static string POSTAL_CODE_ERROR_MESSAGE = string.Format("Address: Postal Code is required and must be {0} digits", MAX_POSTAL_CODE_LENGTH);

        public static string ADDRESS_2_ERROR_MESSAGE = string.Format("Address: Address Line 2 can be up to {0} characters", ADDRESS_MAX_LENGTH);

        public static string CITY_ERROR_MESSAGE = string.Format("Address: City can be up to {0} characters", CITY_MAX_LENGTH);

        public static string EXPLANATION_CODE_ERROR_MESSAGE = string.Format("Address: Explanation Code must be {0} characters", EXPLANATION_CODE_LENGTH);

        public static string EXPLAINATION_ERROR_MESSAGE = string.Format("Address: Explanation must be between {0} and {1} characters", EXPLANATION_MIN_LENGTH, EXPLANATION_MAX_LENGTH);

        public USAddressValidator()
        {
            RuleFor(visitor => visitor.Address1)
                .NotNull()
                .WithMessage(ADDRESS_1_ERROR_MESSAGE)
                .WithState(x => new PiiErrorPath())
                .Length(1, ADDRESS_MAX_LENGTH)
                .WithMessage(ADDRESS_1_ERROR_MESSAGE)
                .WithState(x => new PiiErrorPath());

            RuleFor(visitor => visitor.Address2)
                .Length(0, ADDRESS_MAX_LENGTH)
                .WithMessage(ADDRESS_2_ERROR_MESSAGE)
                .WithState(x => new PiiErrorPath());

            RuleFor(visitor => visitor.City)
                .Length(0, CITY_MAX_LENGTH)
                .WithMessage(CITY_ERROR_MESSAGE)
                .WithState(x => new PiiErrorPath());

            RuleFor(visitor => visitor.PostalCode)
                .NotNull()
                .WithMessage(POSTAL_CODE_ERROR_MESSAGE)
                .WithState(x => new PiiErrorPath())
                .Matches(new Regex(POSTAL_CODE_REGEX))
                .WithMessage(POSTAL_CODE_ERROR_MESSAGE)
                .WithState(x => new PiiErrorPath());

            RuleFor(visitor => visitor.ExplanationCode)
                .Length(EXPLANATION_CODE_LENGTH)
                .WithMessage(EXPLANATION_CODE_ERROR_MESSAGE)
                .WithState(x => new PiiErrorPath());

            RuleFor(visitor => visitor.Explanation)
                .Length(EXPLANATION_MIN_LENGTH, EXPLANATION_MAX_LENGTH)
                .WithMessage(EXPLAINATION_ERROR_MESSAGE)
                .WithState(x => new PiiErrorPath());
        }
    }
}