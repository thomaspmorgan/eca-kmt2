using ECA.Business.Validation;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    public interface IPersonSevisServiceValidator
    {
        /// <summary>
        /// Do validation for sevis exchange visitor create
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>
        VerifyResult ValidateSevisCreateEV(int participantId, User user);

        /// <summary>
        /// Do validation for sevis exchange visitor create
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>
        Task<FluentValidation.Results.ValidationResult> ValidateSevisCreateEVAsync(int participantId, User user);

        /// <summary>
        /// Do validation for sevis exchange visitor update
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>
        FluentValidation.Results.ValidationResult ValidateSevisUpdateEV(int participantId, User user);

        /// <summary>
        /// Do validation for sevis exchange visitor update
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>
        Task<FluentValidation.Results.ValidationResult> ValidateSevisUpdateEVAsync(int participantId, User user);
    }
}