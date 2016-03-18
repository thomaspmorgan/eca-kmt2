using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Business.Validation.Sevis.ErrorPaths;
using FluentValidation;
using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace ECA.Business.Validation.Sevis
{
    /// <summary>
    /// An AddressDTO validator is used to validate an address that will be converted into a USAddress for a sevis exchange visitor.
    /// </summary>
    public class AddressDTOValidator : AbstractValidator<AddressDTO>
    {
        /// <summary>
        /// The max length of a street address.
        /// </summary>
        public const int ADDRESS_MAX_LENGTH = 64;

        /// <summary>
        /// The city max length.
        /// </summary>
        public const int CITY_MAX_LENGTH = 60;

        /// <summary>
        /// The postal code max length.
        /// </summary>
        public const int POSTAL_CODE_LENGTH = 5;

        /// <summary>
        /// The explanation code max length.
        /// </summary>
        public const int EXPLANATION_CODE_LENGTH = 2;

        /// <summary>
        /// The explanation min length.
        /// </summary>
        public const int EXPLANATION_MIN_LENGTH = 5;

        /// <summary>
        /// The explanation max length.
        /// </summary>
        public const int EXPLANATION_MAX_LENGTH = 200;

        /// <summary>
        /// The error message to format when the address' country must be a certain country.
        /// </summary>
        public const string COUNTRY_ERROR_MESSAGE = "The country for the '{0}' address must be the {1}.";

        /// <summary>
        /// The error message to format when the street 1 of the address is invalid.
        /// </summary>
        public const string ADDRESS_1_ERROR_MESSAGE = "The street 1 for the '{0}' is required and can be up to {1} characters.";

        /// <summary>
        /// The postal code regex value.
        /// </summary>
        public const string POSTAL_CODE_REGEX = @"^\d{5}$";

        /// <summary>
        /// The error message to format when a postal code is invalid.
        /// </summary>
        public const string POSTAL_CODE_ERROR_MESSAGE = "The postal code for the '{0}' address is required and must be {1} digits.";

        /// <summary>
        /// The error message to format when the address's street 2 is invalid.
        /// </summary>
        public const string ADDRESS_2_ERROR_MESSAGE = " The street 2 for the '{0}' address can be up to {1} characters";

        /// <summary>
        /// The error message to format when the city is invalid.
        /// </summary>
        public const string CITY_ERROR_MESSAGE = "The city for the '{0}' address can be up to {1} characters.";

        /// <summary>
        /// The name of the person's host address.
        /// </summary>
        public const string PERSON_HOST_ADDRESS = "Person Host Address";

        /// <summary>
        /// The name of the C Street Address for the state department.
        /// </summary>
        public const string C_STREET_ADDRESS = "US State Dept Address";

        /// <summary>
        /// Creates an instance where the name of the validator is supplied by the given delegate.
        /// </summary>
        /// <param name="addressNameDelegate">The address name delegate.</param>
        public AddressDTOValidator(Func<AddressDTO, object> addressNameDelegate)
        {
            Contract.Requires(addressNameDelegate != null, "The address name delegate must not be null.");
            this.AddressNameDelegate = addressNameDelegate;
            Func<AddressDTO, object> maxLengthDelegate = (a) =>
            {
                return ADDRESS_MAX_LENGTH;
            };
            Func<AddressDTO, object> requiredCountryNameDelegate = (a) =>
            {
                return LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            };
            Func<AddressDTO, object> postalCodeLengthValueDelegate = (a) =>
            {
                return POSTAL_CODE_LENGTH;
            };
            Func<AddressDTO, object> cityLengthValueDelegate = (a) =>
            {
                return CITY_MAX_LENGTH;
            };

            RuleFor(visitor => visitor.Street1)
                .NotNull()
                .WithMessage(ADDRESS_1_ERROR_MESSAGE, this.AddressNameDelegate, maxLengthDelegate)
                .WithState(x => new AddressErrorPath())
                .Length(1, ADDRESS_MAX_LENGTH)
                .WithMessage(ADDRESS_1_ERROR_MESSAGE, this.AddressNameDelegate, maxLengthDelegate)
                .WithState(x => new AddressErrorPath());

            RuleFor(visitor => visitor.Street2)
                .Length(0, ADDRESS_MAX_LENGTH)
                .WithMessage(ADDRESS_2_ERROR_MESSAGE, this.AddressNameDelegate, maxLengthDelegate)
                .WithState(x => new AddressErrorPath());

            RuleFor(visitor => visitor.City)
                .Length(0, CITY_MAX_LENGTH)
                .WithMessage(CITY_ERROR_MESSAGE, this.AddressNameDelegate, cityLengthValueDelegate)
                .WithState(x => new AddressErrorPath());

            RuleFor(visitor => visitor.PostalCode)
                .NotNull()
                .WithMessage(POSTAL_CODE_ERROR_MESSAGE, this.AddressNameDelegate, postalCodeLengthValueDelegate)
                .WithState(x => new AddressErrorPath())
                .Matches(new Regex(POSTAL_CODE_REGEX))
                .WithMessage(POSTAL_CODE_ERROR_MESSAGE, this.AddressNameDelegate, postalCodeLengthValueDelegate)
                .WithState(x => new AddressErrorPath());

            RuleFor(visitor => visitor.Country)
                .Equal(LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME)
                .WithMessage(COUNTRY_ERROR_MESSAGE, addressNameDelegate, requiredCountryNameDelegate)
                .WithState(x => new AddressErrorPath());
        }

        /// <summary>
        /// Creates an where the name of the address is the C Street Address of the State Department.
        /// </summary>
        public AddressDTOValidator() : this((a) => C_STREET_ADDRESS) { }

        /// <summary>
        /// Gets the address name delegate.
        /// </summary>
        public Func<AddressDTO, object> AddressNameDelegate { get; private set; }
    }
}