using ECA.Business.Validation;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    [ContractClass(typeof(SevisValidationContract))]
    public interface ISevisValidationService
    {
        /// <summary>
        /// Test validation on a create sevis object.
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>
        VerifyResult PreCreateSevisValidation(int participantId, User user);

        /// <summary>
        /// Test validation on a create sevis object.
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>
        Task<FluentValidation.Results.ValidationResult> PreCreateSevisValidationAsync(int participantId, User user);

        /// <summary>
        /// Test validation on a update sevis object.
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>
        FluentValidation.Results.ValidationResult PreUpdateSevisValidation(int participantId, User user);

        /// <summary>
        /// Test validation on a update sevis object.
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>
        Task<FluentValidation.Results.ValidationResult> PreUpdateSevisValidationAsync(int participantId, User user);
    }

    [ContractClassFor(typeof(ISevisValidationService))]
    public abstract class SevisValidationContract : ISevisValidationService
    {
        public VerifyResult PreCreateSevisValidation(int participantId, User user)
        {
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            return null;
        }

        public Task<FluentValidation.Results.ValidationResult> PreCreateSevisValidationAsync(int participantId, User user)
        {
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            return Task.FromResult<FluentValidation.Results.ValidationResult>(null);
        }

        public FluentValidation.Results.ValidationResult PreUpdateSevisValidation(int participantId, User user)
        {
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            return null;
        }

        public Task<FluentValidation.Results.ValidationResult> PreUpdateSevisValidationAsync(int participantId, User user)
        {
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            return Task.FromResult<FluentValidation.Results.ValidationResult>(null);
        }
    }
}