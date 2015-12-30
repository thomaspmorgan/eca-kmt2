using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class DeletePhaseValidator : AbstractValidator<DeletePhase>
    {
        public DeletePhaseValidator()
        {
            RuleFor(student => student.PhaseId).NotNull().WithMessage("T/IPP: Phase ID is required").Length(1, 22).WithMessage("T/IPP: Phase ID can be from 1 to 22 characters");
        }
    }
}