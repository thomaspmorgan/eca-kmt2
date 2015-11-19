using ECA.Business.Validation;
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
        public async Task<IQueryable<SevisValidationResult>> PreSevisValidation(UpdatedParticipantPersonSevisValidationEntity validationEntity)
        {
            Contract.Requires(validationEntity != null, "The validation entity must not be null.");
            var validator = new PersonSevisServiceValidator();
            var results = await validator.ValidateSevis(validationEntity).AsQueryable().ToListAsync();

            return results.AsQueryable();
        }

    }
}
