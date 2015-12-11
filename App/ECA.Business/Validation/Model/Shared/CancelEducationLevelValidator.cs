using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class CancelEducationLevelValidator : AbstractValidator<CancelEducationLevel>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public CancelEducationLevelValidator()
        {
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Cancel Education Level: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}