using ECA.Core.Service;
using ECA.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Validation
{
    /// <summary>
    /// The SevisValidatorBase class is used to validate entities before creation or update.
    /// </summary>
    //[ContractClass(typeof(SevisValidatorBaseContract<>))]
    public abstract class SevisValidatorBase<SEVISBatchUpdateStudent> : DbContextService<EcaContext>, ISevisValidator
    {
        private bool throwExceptionOnValidation;

        public SevisValidatorBase(EcaContext context, bool throwExceptionOnValidation = true) : base(context)
        {
            this.throwExceptionOnValidation = throwExceptionOnValidation;
        }

        public IEnumerable<ValidationResult> ValidateSevis(Validation.SEVISBatchUpdateStudent validationEntity)
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
        public abstract List<ValidationResult> DoValidateSevis(Validation.SEVISBatchUpdateStudent validationEntity);
    }

    ///// <summary>
    ///// 
    ///// </summary>
    //[ContractClassFor(typeof(SevisValidatorBase<>))]
    //public abstract class SevisValidatorBaseContract<SEVISBatchUpdateStudent> : SevisValidatorBase<SEVISBatchUpdateStudent>
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="validationEntity"></param>
    //    /// <returns></returns>
    //    public IEnumerable<ValidationResult> ValidateSevis(SEVISBatchUpdateStudent validationEntity)
    //    {
    //        Contract.Ensures(Contract.Result<IEnumerable<ValidationResult>>() != null, "The sevis validator must return a non null value.");
    //        return new List<ValidationResult>();
    //    }
    //}
}
