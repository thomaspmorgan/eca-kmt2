using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(DeletePhaseValidator))]
    public class DeletePhase
    {
        public DeletePhase()
        { }

        public string PhaseId { get; set; }
    }
}