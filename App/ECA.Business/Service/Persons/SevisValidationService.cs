using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

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
        /// Test validation on a sevis object.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>List of errors</returns>
        public async Task<List<ValidationResult>> PreSevisValidationAsync(int participantId)
        {            
            var results = await validator.ValidateSevisAsync(participantId);

            return results;
        }

        /// <summary>
        /// Test validation on a sevis object.
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public List<ValidationResult> PreSevisValidation(int participantId)
        {            
            var results = validator.ValidateSevis(participantId);

            return results;
        }
    }
}
