using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class FullNameValidator : AbstractValidator<FullName>
    {
        public const int FIRST_NAME_MAX_LENGTH = 80;
        public const int LAST_NAME_MAX_LENGTH = 40;
        public const int NAME_SUFFIX_MAX_LENGTH = 3;
        public const int PASSPORT_NAME_MAX_LENGTH = 39;
        public const int PREFERRED_NAME_MAX_LENGTH = 145;
        
        public FullNameValidator()
        {
            RuleFor(visitor => visitor.FirsName).Length(1, FIRST_NAME_MAX_LENGTH).WithMessage("Full Name: First Name is required and can be up to " + FIRST_NAME_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.LastName).Length(0, LAST_NAME_MAX_LENGTH).WithMessage("Full Name: Last Name can be up to " + LAST_NAME_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.Suffix).Length(0, NAME_SUFFIX_MAX_LENGTH).WithMessage("Full Name: Suffix can be up to " + NAME_SUFFIX_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.PassportName).Length(0, PASSPORT_NAME_MAX_LENGTH).WithMessage("Full Name: Passport Name can be up to " + PASSPORT_NAME_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.PreferredName).Length(0, PREFERRED_NAME_MAX_LENGTH).WithMessage("Full Name: Preferred Name can be up to " + PREFERRED_NAME_MAX_LENGTH.ToString() + " characters");
        }
    }
}
