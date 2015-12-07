using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class EmployerAddressValidator : AbstractValidator<EmployerAddress>
    {
        public const int ADDRESS_MAX_LENGTH = 60;
        public const int CITY_MAX_LENGTH = 60;
        public const int STATE_CODE_LENGTH = 2;
        public const int POSTAL_CODE_LENGTH = 5;
        public const int POSTAL_ROUDING_CODE_LENGTH = 4;

        public EmployerAddressValidator()
        {
            RuleFor(student => student.address1).Length(1, ADDRESS_MAX_LENGTH).WithMessage("Employer Address: Address Line 1 can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.address2).Length(0, ADDRESS_MAX_LENGTH).WithMessage("Employer Address: Address Line 2 can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.city).Length(0, CITY_MAX_LENGTH).WithMessage("Employer Address: City can be up to " + CITY_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.State).Length(STATE_CODE_LENGTH).WithMessage("Employer Address: State Code must be " + STATE_CODE_LENGTH.ToString() + " characters");
            RuleFor(student => student.PostalCode).Length(POSTAL_CODE_LENGTH).WithMessage("Employer Address: Postal Code must be " + POSTAL_CODE_LENGTH.ToString() + " digits").Matches(@" ^\d{5}$").WithMessage("Employer Address: Postal Code must be numeric");
            RuleFor(student => student.PostalRoundingCode).Length(POSTAL_ROUDING_CODE_LENGTH).WithMessage("Employer Address: Postal Rounding Code must be " + POSTAL_ROUDING_CODE_LENGTH.ToString() + " digits").Matches(@" ^\d{4}$").WithMessage("Employer Address: Postal Rounding Code must be numeric");
        }
    }
}