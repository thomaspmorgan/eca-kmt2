using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class TippPhaseTippPhaseUpdateValidator : AbstractValidator<TippPhaseUpdate>
    {
        public TippPhaseTippPhaseUpdateValidator()
        {
            RuleFor(visitor => visitor.PhaseId).Length(1, 22).WithMessage("T/IPP: Phase ID can be from 1 to 22 characters");
            RuleFor(visitor => visitor.StartDate).NotNull().WithMessage("T/IPP: Phase start date is required");
            RuleFor(visitor => visitor.EndDate).NotNull().WithMessage("T/IPP: Phase end date is required");
        }
    }
}