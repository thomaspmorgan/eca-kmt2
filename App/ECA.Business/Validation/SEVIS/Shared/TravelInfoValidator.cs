using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class TravelInfoValidator : AbstractValidator<TravelInfo>
    {
        public const int PASSPORT_MAX_LENGTH = 25;
        
        public TravelInfoValidator()
        {
            RuleFor(student => student.PassportNumber).Length(0, PASSPORT_MAX_LENGTH).WithMessage("Travel Info: Passport Number can be up to " + PASSPORT_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.VisaNumber).Length(0, PASSPORT_MAX_LENGTH).WithMessage("Travel Info: Visa Number can be up to " + PASSPORT_MAX_LENGTH.ToString() + " characters");
        }
    }
}