using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class USAddressValidator : AbstractValidator<USAddress>
    {
        public const int ADDRESS_MAX_LENGTH = 64;
        public const int CITY_MAX_LENGTH = 60;
        public const int POSTALCODE_LENGTH = 5;
        public const int EXPLANATION_CODE_LENGTH = 2;
        public const int EXPLANATION_MIN_LENGTH = 5;
        public const int EXPLANATION_MAX_LENGTH = 200;
        
        public USAddressValidator()
        {
            RuleFor(student => student.address1).NotNull().Length(1, ADDRESS_MAX_LENGTH).WithMessage("US Address: Address Line 1 is required and can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.address2).Length(0, ADDRESS_MAX_LENGTH).WithMessage("US Address: Address Line 2 can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.city).Length(0, CITY_MAX_LENGTH).WithMessage("US Address: City can be up to " + CITY_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.postalCode).NotNull().Length(POSTALCODE_LENGTH).Matches(@"^\d{5}$").WithMessage("US Address: Postal Code is required and must be " + POSTALCODE_LENGTH.ToString() + " digits");
            RuleFor(student => student.explanationCode).Length(EXPLANATION_CODE_LENGTH).WithMessage("US Address: Explanation Code must be " + EXPLANATION_CODE_LENGTH.ToString() + " characters");
            RuleFor(student => student.explanation).Length(EXPLANATION_MIN_LENGTH, EXPLANATION_MAX_LENGTH).WithMessage("US Address: Explanation must be between " + EXPLANATION_MIN_LENGTH.ToString() + " and " + EXPLANATION_MAX_LENGTH.ToString() + " characters");
        }

    }
}