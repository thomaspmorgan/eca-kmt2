using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    public interface IPersonSevisServiceValidator
    {
        /// <summary>
        /// Do validation for sevis exchange visitor create
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="user">The user performing the validation.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <returns>Sevis object validation results</returns>
        FluentValidation.Results.ValidationResult ValidateSevisCreateEV(int projectId, int participantId, User user);

        /// <summary>
        /// Do validation for sevis exchange visitor create
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="user">The user performing the validation.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <returns>Sevis object validation results</returns>
        Task<FluentValidation.Results.ValidationResult> ValidateSevisCreateEVAsync(int projectId, int participantId, User user);

        /// <summary>
        /// Do validation for sevis exchange visitor update
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="user">The user performing the validation.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <returns>Sevis object validation results</returns>
        FluentValidation.Results.ValidationResult ValidateSevisUpdateEV(int projectId, int participantId, User user);

        /// <summary>
        /// Do validation for sevis exchange visitor update
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="user">The user performing the validation.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <returns>Sevis object validation results</returns>
        Task<FluentValidation.Results.ValidationResult> ValidateSevisUpdateEVAsync(int projectId, int participantId, User user);
    }
}