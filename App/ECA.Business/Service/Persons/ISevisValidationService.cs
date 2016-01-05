using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    [ContractClass(typeof(SevisValidationContract))]
    public interface ISevisValidationService
    {
        FluentValidation.Results.ValidationResult PreCreateSevisValidation(int participantId);
        Task<FluentValidation.Results.ValidationResult> PreCreateSevisValidationAsync(int participantId);
        FluentValidation.Results.ValidationResult PreUpdateSevisValidation(int participantId);
        Task<FluentValidation.Results.ValidationResult> PreUpdateSevisValidationAsync(int participantId);
    }

    [ContractClassFor(typeof(ISevisValidationService))]
    public abstract class SevisValidationContract : ISevisValidationService
    {
        public FluentValidation.Results.ValidationResult PreCreateSevisValidation(int participantId)
        {
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            return null;
        }

        public Task<FluentValidation.Results.ValidationResult> PreCreateSevisValidationAsync(int participantId)
        {
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            return Task.FromResult<FluentValidation.Results.ValidationResult>(null);
        }

        public FluentValidation.Results.ValidationResult PreUpdateSevisValidation(int participantId)
        {
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            return null;
        }

        public Task<FluentValidation.Results.ValidationResult> PreUpdateSevisValidationAsync(int participantId)
        {
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            return Task.FromResult<FluentValidation.Results.ValidationResult>(null);
        }
    }
}