using System.Threading.Tasks;
using FluentValidation.Results;

namespace ECA.Business.Service.Persons
{
    public class SevisValidationService : ISevisValidationService
    {
        private IPersonSevisServiceValidator validator;
        
        public SevisValidationService(IPersonSevisServiceValidator validator)
        {
            this.validator = validator;
        }

        /// <summary>
        /// Test validation on a create sevis object.
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="projectId">The id of the project the partipant belongs.</param>
        /// <param name="user">The user performing the validation.</param>
        /// <returns>Sevis object validation results</returns>
        public ValidationResult PreCreateSevisValidation(int projectId, int participantId, User user)
        {
            var results = validator.ValidateSevisCreateEV(projectId, participantId, user);
            
            return results;
        }

        /// <summary>
        /// Test validation on a create sevis object.
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="projectId">The id of the project the partipant belongs.</param>
        /// <param name="user">The user performing the validation.</param>
        /// <returns>Sevis object validation results</returns>
        public async Task<ValidationResult> PreCreateSevisValidationAsync(int projectId, int participantId, User user)
        {
            var results = await validator.ValidateSevisCreateEVAsync(projectId, participantId, user);

            return results;
        }

        /// <summary>
        /// Test validation on a update sevis object.
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="projectId">The id of the project the partipant belongs.</param>
        /// <param name="user">The user performing the validation.</param>
        /// <returns>Sevis object validation results</returns>
        public ValidationResult PreUpdateSevisValidation(int projectId, int participantId, User user)
        {
            var results = validator.ValidateSevisUpdateEV(projectId, participantId, user);

            return results;
        }

        /// <summary>
        /// Test validation on a update sevis object.
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="projectId">The id of the project the partipant belongs.</param>
        /// <param name="user">The user performing the validation.</param>
        /// <returns>Sevis object validation results</returns>
        public async Task<ValidationResult> PreUpdateSevisValidationAsync(int projectId, int participantId, User user)
        {
            var results = await validator.ValidateSevisUpdateEVAsync(projectId, participantId, user);

            return results;
        }
    }
}
