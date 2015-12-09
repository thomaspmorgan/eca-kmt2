using ECA.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    public class SevisValidationService : EcaService, ISevisValidationService
    {
        private IPersonSevisServiceValidator validator;
        
        public SevisValidationService(EcaContext context, IPersonSevisServiceValidator validator) : base(context)
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
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            var results = await validator.ValidateSevisAsync(this.Context, participantId);

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
            var results = validator.ValidateSevis(this.Context, participantId);

            return results;
        }
    }
}
