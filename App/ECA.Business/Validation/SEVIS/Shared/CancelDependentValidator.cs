using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class CancelDependentValidator : AbstractValidator<CancelDependent>
    {
        public const int SEVIS_MAX_LENGTH = 11;
        public const int REASON_LENGTH = 2;
        public const int REMARKS_MAX_LENGTH = 500;

        public CancelDependentValidator()
        {
            RuleFor(dependent => dependent.dependentSevisID).Length(0, SEVIS_MAX_LENGTH).WithMessage("Cancel Dependent: Sevis ID can be up to " + SEVIS_MAX_LENGTH.ToString() + " characters");
            RuleFor(dependent => dependent.Reason).Length(REASON_LENGTH).WithMessage("Cancel Dependent: Reason code must be " + REMARKS_MAX_LENGTH.ToString() + " characters");
            RuleFor(dependent => dependent.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Cancel Dependent: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}