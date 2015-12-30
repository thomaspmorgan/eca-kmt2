using FluentValidation;

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
        
        public USAddressValidator()
        {
            RuleFor(student => student.Address1).NotNull().WithMessage("US Address: Address Line 1 is required").Length(1, ADDRESS_MAX_LENGTH).WithMessage("US Address: Address Line 1 can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.Address2).Length(0, ADDRESS_MAX_LENGTH).WithMessage("US Address: Address Line 2 can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.City).Length(0, CITY_MAX_LENGTH).WithMessage("US Address: City can be up to " + CITY_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.PostalCode).NotNull().WithMessage("US Address: Postal Code is required").Length(POSTAL_CODE_LENGTH).WithMessage("US Address: Postal Code must be " + POSTAL_CODE_LENGTH.ToString() + " digits").Matches(@" ^\d{5}$").WithMessage("US Address: Postal Code must be numeric");
            RuleFor(student => student.ExplanationCode).Length(EXPLANATION_CODE_LENGTH).WithMessage("US Address: Explanation Code must be " + EXPLANATION_CODE_LENGTH.ToString() + " characters");
            RuleFor(student => student.Explanation).Length(EXPLANATION_MIN_LENGTH, EXPLANATION_MAX_LENGTH).WithMessage("US Address: Explanation must be between " + EXPLANATION_MIN_LENGTH.ToString() + " and " + EXPLANATION_MAX_LENGTH.ToString() + " characters");
        }
    }
}