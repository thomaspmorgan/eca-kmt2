using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class DeletePhaseValidator : AbstractValidator<DeletePhase>
    {
        public const int ID_MAX_LENGTH = 22;

        public DeletePhaseValidator()
        {
            RuleFor(phase => phase.PhaseId).Length(1, ID_MAX_LENGTH).WithMessage("T/IPP: Phase ID is required and can be from 1 to " + ID_MAX_LENGTH.ToString() + " characters");
        }
    }
}