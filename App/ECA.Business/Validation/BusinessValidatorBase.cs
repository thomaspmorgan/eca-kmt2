using ECA.Business.Exceptions;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Validation
{
    /// <summary>
    /// The BusinessValidatorBase class is used to validate entities before creation or update.
    /// </summary>
    /// <typeparam name="TCreate">The create business entity type.</typeparam>
    /// <typeparam name="TUpdate">The update business entity type.</typeparam>
    [ContractClass(typeof(BusinessValidatorBaseContract<,>))]
    public abstract class BusinessValidatorBase<TCreate, TUpdate> : IBusinessValidator<TCreate, TUpdate>
        where TCreate : class
        where TUpdate : class
    {
        private bool throwExceptionOnValidation;

        /// <summary>
        /// Creates a new BusinessValidatorBase.  The validator can be configured to throw an exception
        /// if business validation rules are broken, i.e. validation fails.
        /// </summary>
        /// <param name="throwExceptionOnValidation">True, to throw on exception if validation fails.</param>
        public BusinessValidatorBase(bool throwExceptionOnValidation = true)
        {
            this.throwExceptionOnValidation = throwExceptionOnValidation;
        }

        /// <summary>
        /// Validates the create business entity.
        /// </summary>
        /// <param name="validationEntity">The validation entity.</param>
        /// <returns>The collection of validation results.</returns>
        public IEnumerable<BusinessValidationResult> ValidateCreate(TCreate validationEntity)
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
        /// Validates the update business entity.
        /// </summary>
        /// <param name="validationEntity">The validation entity.</param>
        /// <returns>The collection of validation results.</returns>
        public IEnumerable<BusinessValidationResult> ValidateUpdate(TUpdate validationEntity)
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

        private void DoThrowException(List<BusinessValidationResult> validationResults)
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
        public abstract IEnumerable<BusinessValidationResult> DoValidateCreate(TCreate validationEntity);

        /// <summary>
        /// Override to perform the update entity validation.
        /// </summary>
        /// <param name="validationEntity">The entity to validate.</param>
        /// <returns>The collection of validation results.</returns>
        public abstract IEnumerable<BusinessValidationResult> DoValidateUpdate(TUpdate validationEntity);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TCreate"></typeparam>
    /// <typeparam name="TUpdate"></typeparam>
    [ContractClassFor(typeof(BusinessValidatorBase<,>))]
    public abstract class BusinessValidatorBaseContract<TCreate, TUpdate> : BusinessValidatorBase<TCreate, TUpdate>
        where TCreate : class
        where TUpdate : class
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<BusinessValidationResult> DoValidateCreate(TCreate validationEntity)
        {
            Contract.Ensures(Contract.Result<IEnumerable<BusinessValidationResult>>() != null, "The business validator must return a non null value.");
            return new List<BusinessValidationResult>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(TUpdate validationEntity)
        {
            Contract.Ensures(Contract.Result<IEnumerable<BusinessValidationResult>>() != null, "The business validator must return a non null value.");
            return new List<BusinessValidationResult>();
        }
    }
}
