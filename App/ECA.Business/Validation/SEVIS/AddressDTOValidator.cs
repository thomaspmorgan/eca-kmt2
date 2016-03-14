using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Business.Validation.Sevis.ErrorPaths;
using FluentValidation;
using System.Text.RegularExpressions;

namespace ECA.Business.Validation.Sevis
{
    public class AddressDTOValidator : AbstractValidator<AddressDTO>
    {
        public const int ADDRESS_MAX_LENGTH = 64;
        public const int CITY_MAX_LENGTH = 60;
        public const int POSTAL_CODE_LENGTH = 5;
        public const int EXPLANATION_CODE_LENGTH = 2;
        public const int EXPLANATION_MIN_LENGTH = 5;
        public const int EXPLANATION_MAX_LENGTH = 200;

        public const string COUNTRY_ERROR_MESSAGE = "{0}: The country must be the {1}.";

        public const string ADDRESS_1_ERROR_MESSAGE = "{0}: Address Line 1 is required and can be up to {1} characters.";

        public const int MAX_POSTAL_CODE_LENGTH = 5;

        public const string POSTAL_CODE_REGEX = @"^\d{5}$";

        public const string POSTAL_CODE_ERROR_MESSAGE = "{0}: Postal Code is required and must be {1} digits.";

        public const string ADDRESS_2_ERROR_MESSAGE = "{0}: Address Line 2 can be up to {1} characters";

        public const string CITY_ERROR_MESSAGE = "{0}: City can be up to {0} characters.";

        public const string PERSON_HOST_ADDRESS = "Person Host Address";

        public const string C_STREET_ADDRESS = "US State Dept Address";
        
        public AddressDTOValidator(string addressName)
        {
            RuleFor(visitor => visitor.Street1)
                .NotNull()
                .WithMessage(ADDRESS_1_ERROR_MESSAGE, addressName, ADDRESS_MAX_LENGTH)
                .WithState(x => new AddressErrorPath())
                .Length(1, ADDRESS_MAX_LENGTH)
                .WithMessage(ADDRESS_1_ERROR_MESSAGE, addressName, ADDRESS_MAX_LENGTH)
                .WithState(x => new AddressErrorPath());

            RuleFor(visitor => visitor.Street2)
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

            RuleFor(visitor => visitor.Country)
                .Equal(LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME)
                .WithMessage(COUNTRY_ERROR_MESSAGE, addressName, LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME)
                .WithState(x => new AddressErrorPath());
        }
    }
}