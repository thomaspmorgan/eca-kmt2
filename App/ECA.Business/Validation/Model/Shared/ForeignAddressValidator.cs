using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class ForeignAddressValidator : AbstractValidator<ForeignAddress>
    {
        public const int ADDRESS_MAX_LENGTH = 60;
        public const int CITY_MAX_LENGTH = 60;
        public const int PROVINCE_MAX_LENGTH = 30;
        public const int COUNTRY_CODE_LENGTH = 2;
        public const int POSTAL_CODE_LENGTH = 20;

        public ForeignAddressValidator()
        {
            RuleFor(student => student.address1).NotNull().WithMessage("Foreign Address: Address Line 1 is required").Length(1, ADDRESS_MAX_LENGTH).WithMessage("Foreign Address: Address Line 1 can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.address2).Length(0, ADDRESS_MAX_LENGTH).WithMessage("Foreign Address: Address Line 2 can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.city).Length(0, CITY_MAX_LENGTH).WithMessage("Foreign Address: City can be up to " + CITY_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.province).Length(0, PROVINCE_MAX_LENGTH).WithMessage("Foreign Address: Province can be up to " + PROVINCE_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.countryCode).NotNull().WithMessage("Foreign Address: Country Code is required").Length(COUNTRY_CODE_LENGTH).WithMessage("Foreign Address: Country Code must be " + COUNTRY_CODE_LENGTH.ToString() + " characters");
            RuleFor(student => student.postalCode).Length(POSTAL_CODE_LENGTH).WithMessage("Foreign Address: Postal Code must be " + POSTAL_CODE_LENGTH.ToString() + " digits");
        }

    }
}