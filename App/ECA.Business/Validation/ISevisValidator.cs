using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ECA.Business.Validation
{
    /// <summary>
    /// An ISevisValidator is used for validating sevis entities on different operations.
    /// </summary>
    /// <typeparam name="TUpdate">The update sevis entity type.</typeparam>
    [ContractClass(typeof(ISevisValidatorContract<>))]
    public interface ISevisValidator<TUpdate>
        where TUpdate : class
    {
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
    /// <typeparam name="TUpdate"></typeparam>
    [ContractClassFor(typeof(ISevisValidator<>))]
    public abstract class ISevisValidatorContract<TUpdate> : ISevisValidator<TUpdate>
        where TUpdate : class
    {
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
