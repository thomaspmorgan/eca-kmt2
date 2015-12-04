using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class EngProficiencyValidator : AbstractValidator<EngProficiency>
    {
        public const int REASON_MAX_LENGTH = 500;

        public EngProficiencyValidator()
        {
            RuleFor(student => student.EngRequired).NotNull().WithMessage("English Proficiency: English Required indicator is required.");
            RuleFor(student => student.NotRequiredReason).Length(0, REASON_MAX_LENGTH).WithMessage("English Proficiency: Not Required Reason can be up to " + REASON_MAX_LENGTH.ToString() + " characters");
        }
    }
}