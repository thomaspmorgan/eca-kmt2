using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class SContactValidator : AbstractValidator<SContact>
    {
        public const int FIRST_NAME_MAX_LENGTH = 80;
        public const int LAST_NAME_MAX_LENGTH = 40;

        public SContactValidator()
        {
            RuleFor(visitor => visitor.FirsName).Length(0, FIRST_NAME_MAX_LENGTH).WithMessage("Secondary Host Family contact: First Name can be up to " + FIRST_NAME_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.LastName).Length(0, LAST_NAME_MAX_LENGTH).WithMessage("Secondary Host Family contact: Last Name can be up to " + LAST_NAME_MAX_LENGTH.ToString() + " characters");
        }
    }
}