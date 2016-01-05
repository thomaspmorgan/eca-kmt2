using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    [ContractClass(typeof(SevisValidationContract))]
    public interface ISevisValidationService
    {
        FluentValidation.Results.ValidationResult PreSevisValidation(int participantId);
        Task<FluentValidation.Results.ValidationResult> PreSevisValidationAsync(int participantId);
    }

    [ContractClassFor(typeof(ISevisValidationService))]
    public abstract class SevisValidationContract : ISevisValidationService
    {
        public FluentValidation.Results.ValidationResult PreSevisValidation(int participantId)
        {
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            return null;
        }

        public Task<FluentValidation.Results.ValidationResult> PreSevisValidationAsync(int participantId)
        {
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            return Task.FromResult<FluentValidation.Results.ValidationResult>(null);
        }
    }
}