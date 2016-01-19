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
        /// <returns>Sevis object validation results</returns>
        public ValidationResult PreCreateSevisValidation(int participantId, User user)
        {
            var results = validator.ValidateSevisCreateEV(participantId, user);
            
            return results;
        }

        /// <summary>
        /// Test validation on a create sevis object.
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>
        public async Task<ValidationResult> PreCreateSevisValidationAsync(int participantId, User user)
        {
            var results = await validator.ValidateSevisCreateEVAsync(participantId, user);

            return results;
        }

        /// <summary>
        /// Test validation on a update sevis object.
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>
        public ValidationResult PreUpdateSevisValidation(int participantId, User user)
        {
            var results = validator.ValidateSevisUpdateEV(participantId, user);

            return results;
        }

        /// <summary>
        /// Test validation on a update sevis object.
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>
        public async Task<ValidationResult> PreUpdateSevisValidationAsync(int participantId, User user)
        {
            var results = await validator.ValidateSevisUpdateEVAsync(participantId, user);

            return results;
        }
    }
}
