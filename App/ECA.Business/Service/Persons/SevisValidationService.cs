using ECA.Business.Validation;
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
        public async Task<IQueryable<ValidationResult>> PreSevisValidationAsync(int participantId)
        {
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            var validator = new PersonSevisServiceValidator();
            var results = await validator.ValidateSevis(participantId).AsQueryable().ToListAsync();

            return results.AsQueryable();
        }

        /// <summary>
        /// Test validation on a sevis object.
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public IQueryable<ValidationResult> PreSevisValidation(int participantId)
        {
            Contract.Requires(participantId > 0, "The participant ID must not be null.");
            var validator = new PersonSevisServiceValidator();
            var results = validator.ValidateSevis(participantId).AsQueryable();

            return results;
        }
    }
}
