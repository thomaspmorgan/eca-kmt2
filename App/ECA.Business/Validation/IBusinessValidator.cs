using System;
using System.Linq;
using System.Collections.Generic;
using ECA.Business.Exceptions;
using System.Diagnostics.Contracts;
namespace ECA.Business.Validation
{
    /// <summary>
    /// An IBusinessValidator is used for validating business entities on different operations.
    /// </summary>
    /// <typeparam name="TCreate">The create business entity type.</typeparam>
    /// <typeparam name="TUpdate">The update business entity type.</typeparam>
    [ContractClass(typeof(IBusinessValidatorContract<,>))]
    public interface IBusinessValidator<TCreate, TUpdate>
        where TCreate : class
        where TUpdate : class
    {
        /// <summary>
        /// Validates the given business entity.
        /// </summary>
        /// <param name="validationEntity">The entity to validate.</param>
        /// <returns>The validation results found.</returns>
        IEnumerable<BusinessValidationResult> ValidateCreate(TCreate validationEntity);

        /// <summary>
        /// Validates the given business entity.
        /// </summary>
        /// <param name="validationEntity">The entity to validate.</param>
        /// <returns>The validation results found.</returns>
        IEnumerable<BusinessValidationResult> ValidateUpdate(TUpdate validationEntity);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TCreate"></typeparam>
    /// <typeparam name="TUpdate"></typeparam>
    [ContractClassFor(typeof(IBusinessValidator<,>))]
    public abstract class IBusinessValidatorContract<TCreate, TUpdate> : IBusinessValidator<TCreate, TUpdate>
        where TCreate : class
        where TUpdate : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public IEnumerable<BusinessValidationResult> ValidateCreate(TCreate validationEntity)
        {
            Contract.Requires(validationEntity != null, "The validation entity must not be null.");
            return new List<BusinessValidationResult>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public IEnumerable<BusinessValidationResult> ValidateUpdate(TUpdate validationEntity)
        {
            Contract.Requires(validationEntity != null, "The validation entity must not be null.");
            return new List<BusinessValidationResult>();
        }
    }


}
