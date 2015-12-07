using FluentValidation;

namespace ECA.Business.Validation.Model
{
    internal class ReactivateDependentValidator : AbstractValidator<ReactivateDependent>
    {
        public const int SEVIS_MAX_LENGTH = 11;

        public ReactivateDependentValidator()
        {
            RuleFor(dependent => dependent.dependentSevisID).Length(0, SEVIS_MAX_LENGTH).WithMessage("Reactivate Dependent: Sevis ID can be up to " + SEVIS_MAX_LENGTH.ToString() + " characters");
        }
    }
}