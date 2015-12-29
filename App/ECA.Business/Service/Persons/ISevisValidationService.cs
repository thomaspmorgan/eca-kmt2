using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    [ContractClass(typeof(SevisValidationContract))]
    public interface ISevisValidationService
    {
        List<ValidationResult> PreSevisValidation(int participantId);
        Task<List<ValidationResult>> PreSevisValidationAsync(int participantId);
    }

    [ContractClassFor(typeof(ISevisValidationService))]
    public abstract class SevisValidationContract : ISevisValidationService
    {
        public List<ValidationResult> PreSevisValidation(int participantId)
        {
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            return null;
        }

        public Task<List<ValidationResult>> PreSevisValidationAsync(int participantId)
        {
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            return Task.FromResult<List<ValidationResult>>(null);
        }
    }
}