using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class LCCoordinatorValidator : AbstractValidator<LCCoordinator>
    {
        public const int FIRST_NAME_MAX_LENGTH = 80;
        public const int LAST_NAME_MAX_LENGTH = 40;

        public LCCoordinatorValidator()
        {
            RuleFor(student => student.FirsName).Length(0, FIRST_NAME_MAX_LENGTH).WithMessage("Exch. Visitor LC Coordinator: First Name can be up to " + FIRST_NAME_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.LastName).Length(0, LAST_NAME_MAX_LENGTH).WithMessage("Exch. Visitor LC Coordinator: Last Name can be up to " + LAST_NAME_MAX_LENGTH.ToString() + " characters");
        }
    }
}