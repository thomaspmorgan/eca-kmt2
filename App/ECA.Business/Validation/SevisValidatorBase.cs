using ECA.Business.Exceptions;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Validation
{
    /// <summary>
    /// The SevisValidatorBase class is used to validate entities before creation or update.
    /// </summary>
    [ContractClass(typeof(SevisValidatorBaseContract<>))]
    public abstract class SevisValidatorBase<UpdatedParticipantPersonSevisValidationEntity> : ISevisValidator
    {
        private bool throwExceptionOnValidation;

        public SevisValidatorBase(bool throwExceptionOnValidation = true)
        {
            this.throwExceptionOnValidation = throwExceptionOnValidation;
        }

        public IEnumerable<SevisValidationResult> ValidateSevis(Service.Persons.UpdatedParticipantPersonSevisValidationEntity validationEntity)
        {
            if (throwExceptionOnValidation)
            {
                var results = DoValidateSevis(validationEntity);
                DoThrowException(results);
                return results;
            }
            else
            {
                return DoValidateSevis(validationEntity);
            }
        }

        private void DoThrowException(IEnumerable<SevisValidationResult> validationResults)
        {
            if (validationResults.Count() > 0)
            {
                throw new ValidationException("There was an error validating the changes.", validationResults);
            }
        }

        /// <summary>
        /// Override to perform the update entity validation.
        /// </summary>
        /// <param name="validationEntity">The entity to validate.</param>
        /// <returns>The collection of validation results.</returns>
        public abstract IEnumerable<SevisValidationResult> DoValidateSevis(Service.Persons.UpdatedParticipantPersonSevisValidationEntity validationEntity);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TUpdate"></typeparam>
    [ContractClassFor(typeof(SevisValidatorBase<>))]
    public abstract class SevisValidatorBaseContract<UpdatedParticipantPersonSevisValidationEntity> : SevisValidatorBase<UpdatedParticipantPersonSevisValidationEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<SevisValidationResult> DoValidateSevis(Service.Persons.UpdatedParticipantPersonSevisValidationEntity validationEntity)
        {
            Contract.Ensures(Contract.Result<IEnumerable<SevisValidationResult>>() != null, "The sevis validator must return a non null value.");
            return new List<SevisValidationResult>();
        }
    }
}
