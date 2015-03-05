using System;
using System.Linq;
using System.Collections.Generic;
using ECA.Business.Exceptions;
namespace ECA.Business.Validation
{
    /// <summary>
    /// An IBusinessValidator is used for validating business entities on different operations.
    /// </summary>
    /// <typeparam name="TCreate">The create business entity type.</typeparam>
    /// <typeparam name="TUpdate">The update business entity type.</typeparam>
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

    
}
