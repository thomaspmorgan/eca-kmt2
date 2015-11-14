using ECA.Business.Exceptions;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Validation
{
    /// <summary>
    /// The SevisValidatorBase class is used to validate entities before creation or update.
    /// </summary>
    /// <typeparam name="TUpdate">The update sevis entity type.</typeparam>
    [ContractClass(typeof(SevisValidatorBaseContract<>))]
    public abstract class SevisValidatorBase<TUpdate> : ISevisValidator<TUpdate>
        where TUpdate : class
    {
        private bool throwExceptionOnValidation;
        
        /// <summary>
        /// Creates a new SevisValidatorBase.  The validator can be configured to throw an exception
        /// if sevis validation rules are broken, i.e. validation fails.
        /// </summary>
        /// <param name="throwExceptionOnValidation">True, to throw on exception if validation fails.</param>
        public SevisValidatorBase(bool throwExceptionOnValidation = true)
        {
            this.throwExceptionOnValidation = throwExceptionOnValidation;
        }

        /// <summary>
        /// Validates the update sevis entity.
        /// </summary>
        /// <param name="validationEntity">The validation entity.</param>
        /// <returns>The collection of validation results.</returns>
        public IEnumerable<SevisValidationResult> ValidateUpdate(TUpdate validationEntity)
        {
            if (throwExceptionOnValidation)
            {
                var results = DoValidateUpdate(validationEntity).ToList();
                DoThrowException(results);
                return results;
            }
            else
            {
                return DoValidateUpdate(validationEntity);
            }
        }

        private void DoThrowException(List<SevisValidationResult> validationResults)
        {
            if (validationResults.Count > 0)
            {
                throw new ValidationException("There was an error validating the changes.", validationResults);
            }
        }

        /// <summary>
        /// Override to perform the update entity validation.
        /// </summary>
        /// <param name="validationEntity">The entity to validate.</param>
        /// <returns>The collection of validation results.</returns>
        public abstract IEnumerable<SevisValidationResult> DoValidateUpdate(TUpdate validationEntity);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TUpdate"></typeparam>
    [ContractClassFor(typeof(SevisValidatorBase<>))]
    public abstract class SevisValidatorBaseContract<TUpdate> : SevisValidatorBase<TUpdate>
        where TUpdate : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<SevisValidationResult> DoValidateUpdate(TUpdate validationEntity)
        {
            Contract.Ensures(Contract.Result<IEnumerable<SevisValidationResult>>() != null, "The sevis validator must return a non null value.");
            return new List<SevisValidationResult>();
        }
    }
}
