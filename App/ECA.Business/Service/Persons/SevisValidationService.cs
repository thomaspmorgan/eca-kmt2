using ECA.Core.Service;
using ECA.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    public class SevisValidationService : DbContextService<EcaContext>
    {
        public SevisValidationService(EcaContext context) : base(context)
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
            var validator = new PersonSevisServiceValidator(this.Context);
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
            var validator = new PersonSevisServiceValidator(this.Context);
            var results = validator.ValidateSevis(participantId);

            return results;
        }
    }
}
