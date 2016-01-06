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
        public ValidationResult PreCreateSevisValidation(int participantId)
        {
            var results = validator.ValidateSevisCreateEV(participantId);

            return results;
        }

        /// <summary>
        /// Test validation on a create sevis object.
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>
        public async Task<ValidationResult> PreCreateSevisValidationAsync(int participantId)
        {
            var results = await validator.ValidateSevisCreateEVAsync(participantId);

            return results;
        }

        /// <summary>
        /// Test validation on a update sevis object.
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>
        public ValidationResult PreUpdateSevisValidation(int participantId)
        {
            var results = validator.ValidateSevisUpdateEV(participantId);

            return results;
        }

        /// <summary>
        /// Test validation on a update sevis object.
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>
        public async Task<ValidationResult> PreUpdateSevisValidationAsync(int participantId)
        {
            var results = await validator.ValidateSevisUpdateEVAsync(participantId);

            return results;
        }
    }
}
