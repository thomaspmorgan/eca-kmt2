using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ECA.Business.Sevis.Validation
{
    /// <summary>
    /// An ISevisValidator is used for validating sevis entities on different operations.
    /// </summary>
    /// <typeparam name="TCreate">The create sevis entity type.</typeparam>
    /// <typeparam name="TUpdate">The update sevis entity type.</typeparam>
    [ContractClass(typeof(ISevisValidatorContract<,>))]
    public interface ISevisValidator<TCreate, TUpdate>
        where TCreate : class
        where TUpdate : class
    {
        /// <summary>
        /// Validates the given sevis entity.
        /// </summary>
        /// <param name="validationEntity">The entity to validate.</param>
        /// <returns>The validation results found.</returns>
        IEnumerable<SevisValidationResult> ValidateCreate(TCreate validationEntity);

        /// <summary>
        /// Validates the given sevis entity.
        /// </summary>
        /// <param name="validationEntity">The entity to validate.</param>
        /// <returns>The validation results found.</returns>
        IEnumerable<SevisValidationResult> ValidateUpdate(TUpdate validationEntity);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TCreate"></typeparam>
    /// <typeparam name="TUpdate"></typeparam>
    [ContractClassFor(typeof(ISevisValidator<,>))]
    public abstract class ISevisValidatorContract<TCreate, TUpdate> : ISevisValidator<TCreate, TUpdate>
        where TCreate : class
        where TUpdate : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public IEnumerable<SevisValidationResult> ValidateCreate(TCreate validationEntity)
        {
            Contract.Requires(validationEntity != null, "The validation entity must not be null.");
            return new List<SevisValidationResult>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public IEnumerable<SevisValidationResult> ValidateUpdate(TUpdate validationEntity)
        {
            Contract.Requires(validationEntity != null, "The validation entity must not be null.");
            return new List<SevisValidationResult>();
        }
    }



}
