using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class TerminateVisitorValidator : AbstractValidator<TerminateVisitor>
    {
        public const int REASON_MAX_LENGTH = 6;

        public TerminateVisitorValidator()
        {
            RuleFor(visitor => visitor.Reason).Length(1, REASON_MAX_LENGTH).WithMessage("Terminate Exch. Visitor: Reason is required and can be up to " + REASON_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.EffectiveDate).NotNull().WithMessage("Terminate Exch. Visitor: Termination Date is required");
        }
    }
}