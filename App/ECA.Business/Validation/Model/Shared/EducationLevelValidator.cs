using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class EducationLevelValidator : AbstractValidator<EducationLevel>
    {
        public EducationLevelValidator()
        {
            RuleFor(student => student.cancelEducationLevel).SetValidator(new CancelEducationLevelValidator());
        }
    }
}