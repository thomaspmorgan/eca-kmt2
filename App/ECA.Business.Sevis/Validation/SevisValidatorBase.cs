using ECA.Business.Sevis.Exceptions;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Sevis.Validation
{
    /// <summary>
    /// The SevisValidatorBase class is used to validate entities before creation or update.
    /// </summary>
    /// <typeparam name="TCreate">The create sevis entity type.</typeparam>
    /// <typeparam name="TUpdate">The update sevis entity type.</typeparam>
    [ContractClass(typeof(SevisValidatorBaseContract<,>))]
    public abstract class SevisValidatorBase<TCreate, TUpdate> : ISevisValidator<TCreate, TUpdate>
        where TCreate : class
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
        /// Validates the create sevis entity.
        /// </summary>
        /// <param name="validationEntity">The validation entity.</param>
        /// <returns>The collection of validation results.</returns>
        public IEnumerable<SevisValidationResult> ValidateCreate(TCreate validationEntity)
        {
            if (throwExceptionOnValidation)
            {
                var results = DoValidateCreate(validationEntity).ToList();
                DoThrowException(results);
                return results;
            }
            else
            {
                return DoValidateCreate(validationEntity);
            }
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
        /// Override to perform the create entity validation.
        /// </summary>
        /// <param name="validationEntity">The entity to validate.</param>
        /// <returns>The collection of validation results.</returns>
        public abstract IEnumerable<SevisValidationResult> DoValidateCreate(TCreate validationEntity);

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
    /// <typeparam name="TCreate"></typeparam>
    /// <typeparam name="TUpdate"></typeparam>
    [ContractClassFor(typeof(SevisValidatorBase<,>))]
    public abstract class SevisValidatorBaseContract<TCreate, TUpdate> : SevisValidatorBase<TCreate, TUpdate>
        where TCreate : class
        where TUpdate : class
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<SevisValidationResult> DoValidateCreate(TCreate validationEntity)
        {
            Contract.Ensures(Contract.Result<IEnumerable<SevisValidationResult>>() != null, "The sevis validator must return a non null value.");
            return new List<SevisValidationResult>();
        }

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
