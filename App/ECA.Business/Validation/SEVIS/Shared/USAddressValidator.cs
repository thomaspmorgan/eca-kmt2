using ECA.Business.Validation.SEVIS;
using ECA.Business.Validation.SEVIS.ErrorPaths;
using ECA.Core.Generation;
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

        public const string ADDRESS_1_ERROR_MESSAGE = "{0}: Address Line 1 is required and can be up to {1} characters.";

        public const int MAX_POSTAL_CODE_LENGTH = 5;

        public const string POSTAL_CODE_REGEX = @"^\d{5}$";

        public const string POSTAL_CODE_ERROR_MESSAGE = "{0}: Postal Code is required and must be {1} digits.";

        public const string ADDRESS_2_ERROR_MESSAGE = "{0}: Address Line 2 can be up to {1} characters";

        public const string CITY_ERROR_MESSAGE = "{0}: City can be up to {0} characters.";

        public const string EXPLANATION_CODE_ERROR_MESSAGE = "{0}: Explanation Code must be {0} characters.";

        public const string EXPLANATION_ERROR_MESSAGE = "{0}: Explanation must be between {1} and {2} characters.";

        public const string HOST_INSTITUTION_ADDRESS_NAME = "Host Institution";

        public const string HOME_INSTITUTION_ADDRESS_NAME = "Home Institution";

        
        public USAddressValidator(string addressName)
        {
            RuleFor(visitor => visitor.Address1)
                .NotNull()
                .WithMessage(ADDRESS_1_ERROR_MESSAGE, addressName, ADDRESS_MAX_LENGTH)
                .WithState(x => new AddressErrorPath())
                .Length(1, ADDRESS_MAX_LENGTH)
                .WithMessage(ADDRESS_1_ERROR_MESSAGE, addressName, ADDRESS_MAX_LENGTH)
                .WithState(x => new AddressErrorPath());

            RuleFor(visitor => visitor.Address2)
                .Length(0, ADDRESS_MAX_LENGTH)
                .WithMessage(ADDRESS_2_ERROR_MESSAGE, addressName, ADDRESS_MAX_LENGTH)
                .WithState(x => new AddressErrorPath());

            RuleFor(visitor => visitor.City)
                .Length(0, CITY_MAX_LENGTH)
                .WithMessage(CITY_ERROR_MESSAGE, addressName, CITY_MAX_LENGTH)
                .WithState(x => new AddressErrorPath());

            RuleFor(visitor => visitor.PostalCode)
                .NotNull()
                .WithMessage(POSTAL_CODE_ERROR_MESSAGE, addressName, POSTAL_CODE_LENGTH)
                .WithState(x => new AddressErrorPath())
                .Matches(new Regex(POSTAL_CODE_REGEX))
                .WithMessage(POSTAL_CODE_ERROR_MESSAGE, addressName, POSTAL_CODE_LENGTH)
                .WithState(x => new AddressErrorPath());

            RuleFor(visitor => visitor.ExplanationCode)
                .Length(EXPLANATION_CODE_LENGTH)
                .WithMessage(EXPLANATION_CODE_ERROR_MESSAGE, addressName, EXPLANATION_CODE_LENGTH)
                .WithState(x => new AddressErrorPath());

            RuleFor(visitor => visitor.Explanation)
                .Length(EXPLANATION_MIN_LENGTH, EXPLANATION_MAX_LENGTH)
                .WithMessage(EXPLANATION_ERROR_MESSAGE, addressName, EXPLANATION_MIN_LENGTH, EXPLANATION_MAX_LENGTH)
                .WithState(x => new AddressErrorPath());
        }
    }
}