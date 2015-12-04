﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Validation
{
    /// <summary>
    /// An ISevisValidator is used for validating sevis entities on different operations.
    /// </summary>
    [ContractClass(typeof(ISevisValidatorContract))]
    public interface ISevisValidator
    {
        /// <summary>
        /// Validates the given sevis entity.
        /// </summary>
        /// <param name="validationEntity">The entity to validate.</param>
        /// <returns>The validation results found.</returns>
        IEnumerable<ValidationResult> ValidateSevis(SEVISBatchUpdateStudent validationEntity);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(ISevisValidator))]
    public abstract class ISevisValidatorContract : ISevisValidator
    {
        public IEnumerable<ValidationResult> ValidateSevis(SEVISBatchUpdateStudent validationEntity)
        {
            Contract.Requires(validationEntity != null, "The validation entity must not be null.");
            return new List<ValidationResult>().AsQueryable();
        }
        
    }

}
