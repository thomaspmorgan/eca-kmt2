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
        /// <param name="projectId">The id of the project the participant belongs to.</param>
        /// <param name="user">The user performing the validation.</param>
        /// <returns>Sevis object validation results</returns>
        FluentValidation.Results.ValidationResult PreCreateSevisValidation(int projectId, int participantId, User user);

        /// <summary>
        /// Test validation on a create sevis object.
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="projectId">The id of the project the participant belongs to.</param>
        /// <param name="user">The user performing the validation.</param>
        /// <returns>Sevis object validation results</returns>
        Task<FluentValidation.Results.ValidationResult> PreCreateSevisValidationAsync(int projectId, int participantId, User user);

        /// <summary>
        /// Test validation on a update sevis object.
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="projectId">The id of the project the participant belongs to.</param>
        /// <param name="user">The user performing the validation.</param>
        /// <returns>Sevis object validation results</returns>
        FluentValidation.Results.ValidationResult PreUpdateSevisValidation(int projectId, int participantId, User user);

        /// <summary>
        /// Test validation on a update sevis object.
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="projectId">The id of the project the participant belongs to.</param>
        /// <param name="user">The user performing the validation.</param>
        /// <returns>Sevis object validation results</returns>
        Task<FluentValidation.Results.ValidationResult> PreUpdateSevisValidationAsync(int projectId, int participantId, User user);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(ISevisValidationService))]
    public abstract class SevisValidationContract : ISevisValidationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="participantId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public FluentValidation.Results.ValidationResult PreCreateSevisValidation(int projectId, int participantId, User user)
        {
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="participantId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<FluentValidation.Results.ValidationResult> PreCreateSevisValidationAsync(int projectId, int participantId, User user)
        {
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            return Task.FromResult<FluentValidation.Results.ValidationResult>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="participantId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public FluentValidation.Results.ValidationResult PreUpdateSevisValidation(int projectId, int participantId, User user)
        {
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="participantId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<FluentValidation.Results.ValidationResult> PreUpdateSevisValidationAsync(int projectId, int participantId, User user)
        {
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            return Task.FromResult<FluentValidation.Results.ValidationResult>(null);
        }
    }
}