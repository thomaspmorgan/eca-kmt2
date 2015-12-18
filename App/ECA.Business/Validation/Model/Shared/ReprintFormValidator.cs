using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class ReprintFormValidator : AbstractValidator<ReprintForm>
    {
        public const int SEVIS_MAX_LENGTH = 11;

        public ReprintFormValidator()
        {
            RuleFor(form => form.dependentSevisID).Length(0, SEVIS_MAX_LENGTH).WithMessage("Reprint Dependent: Sevis ID can be up to " + SEVIS_MAX_LENGTH.ToString() + " characters");
        }
    }
}