using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    public class SevisValidationService
    {
        public SevisValidationService()
        {

        }

        /// <summary>
        /// Test validation on a sevis object.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>List of errors</returns>
        public async Task<List<ValidationResult>> PreSevisValidationAsync(int participantId)
        {
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            var validator = new PersonSevisServiceValidator();
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
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            var validator = new PersonSevisServiceValidator();
            var results = validator.ValidateSevis(participantId);

            return results;
        }
    }
}
