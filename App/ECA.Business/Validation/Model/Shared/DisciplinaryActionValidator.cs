using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class DisciplinaryActionValidator : AbstractValidator<DisciplinaryAction>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public DisciplinaryActionValidator()
        {
            RuleFor(student => student.Explanation).Length(0, REMARKS_MAX_LENGTH).WithMessage("Disciplinary Action: Explanation can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}