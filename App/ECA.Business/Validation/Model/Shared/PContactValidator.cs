using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class PContactValidator : AbstractValidator<PContact>
    {
        public const int FIRST_NAME_MAX_LENGTH = 80;
        public const int LAST_NAME_MAX_LENGTH = 40;

        public PContactValidator()
        {
            RuleFor(student => student.FirsName).Length(0, FIRST_NAME_MAX_LENGTH).WithMessage("Primary Host Family contact: First Name can be up to " + FIRST_NAME_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.LastName).Length(0, LAST_NAME_MAX_LENGTH).WithMessage("Primary Host Family contact: Last Name can be up to " + LAST_NAME_MAX_LENGTH.ToString() + " characters");
        }
    }
}