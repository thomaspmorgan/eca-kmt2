using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class EduLevelValidator : AbstractValidator<EduLevel>
    {
        public const int LEVEL_LENGTH = 2;
        public const int REMARKS_MAX_LENGTH = 500;

        public EduLevelValidator()
        {
            RuleFor(student => student.Level).NotNull().WithMessage("Education Level: Level is required").Length(LEVEL_LENGTH).WithMessage("Education Level: Level must be " + LEVEL_LENGTH.ToString() + " characters");
            RuleFor(student => student.OtherRemarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Education Level: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}