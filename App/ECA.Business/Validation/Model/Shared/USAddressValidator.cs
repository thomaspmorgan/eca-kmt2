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
            RuleFor(visitor => visitor.Address1).Length(1, ADDRESS_MAX_LENGTH).WithMessage("Address: Address Line 1 is required and can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.Address2).Length(0, ADDRESS_MAX_LENGTH).WithMessage("Address: Address Line 2 can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.City).Length(0, CITY_MAX_LENGTH).WithMessage("Address: City can be up to " + CITY_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.PostalCode).Length(POSTAL_CODE_LENGTH).WithMessage("Address: Postal Code is required and must be " + POSTAL_CODE_LENGTH.ToString() + " digits").Matches(@" ^\d{5}$").WithMessage("Address: Postal Code must be numeric");
            RuleFor(visitor => visitor.ExplanationCode).Length(EXPLANATION_CODE_LENGTH).WithMessage("Address: Explanation Code must be " + EXPLANATION_CODE_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.Explanation).Length(EXPLANATION_MIN_LENGTH, EXPLANATION_MAX_LENGTH).WithMessage("Address: Explanation must be between " + EXPLANATION_MIN_LENGTH.ToString() + " and " + EXPLANATION_MAX_LENGTH.ToString() + " characters");
        }
    }
}