using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Validation
{
    /// <summary>
    /// The SevisValidatorBase class is used to validate entities before creation or update.
    /// </summary>
    [ContractClass(typeof(SevisValidatorBaseContract<>))]
    public abstract class SevisValidatorBase<SEVISBatchCreateUpdateEV> : ISevisValidator
    {
        private bool throwExceptionOnValidation;

        public SevisValidatorBase(bool throwExceptionOnValidation = true)
        {
            this.throwExceptionOnValidation = throwExceptionOnValidation;
        }

        public IEnumerable<ValidationResult> ValidateSevis(Validation.SEVISBatchCreateUpdateEV validationEntity)
        {
            if (throwExceptionOnValidation)
            {
                var results = DoValidateSevis(validationEntity);
                //DoThrowException(results);
                return results;
            }
            else
            {
                return DoValidateSevis(validationEntity);
            }
        }

        private void DoThrowException(IEnumerable<ValidationResult> validationResults)
        {
            if (validationResults.Count() > 0)
            {
                throw new Exceptions.ValidationException("There was an error validating the record.", validationResults);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity">The entity to validate.</param>
        /// <returns>The collection of validation results.</returns>
        public abstract List<ValidationResult> DoValidateSevis(Validation.SEVISBatchCreateUpdateEV validationEntity);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(SevisValidatorBase<>))]
    public abstract class SevisValidatorBaseContract<SEVISBatchCreateUpdateEV> : SevisValidatorBase<SEVISBatchCreateUpdateEV>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> ValidateSevis(SEVISBatchCreateUpdateEV validationEntity)
        {
            Contract.Ensures(Contract.Result<IEnumerable<ValidationResult>>() != null, "The sevis validator must return a non null value.");
            return new List<ValidationResult>();
        }
    }
}
