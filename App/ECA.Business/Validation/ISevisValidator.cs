﻿using ECA.Business.Service.Persons;
using System.Collections.Generic;
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
        IEnumerable<SevisValidationResult> ValidateUpdate(UpdatedParticipantPersonSevisValidationEntity validationEntity);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(ISevisValidator))]
    public abstract class ISevisValidatorContract
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public IEnumerable<SevisValidationResult> ValidateUpdate(UpdatedParticipantPersonSevisValidationEntity validationEntity)
        {
            Contract.Requires(validationEntity != null, "The validation entity must not be null.");
            return new List<SevisValidationResult>().AsQueryable();
        }
    }
    
}
