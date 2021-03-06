﻿using ECA.Business.Validation.Sevis.ErrorPaths;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Validation
{
    /// <summary>
    /// A SimpleValidationResult is used to serialize a FluentValidation error result.
    /// </summary>
    public class SimpleValidationResult
    {
        /// <summary>
        /// Creates a new instance with the given validation result.
        /// </summary>
        /// <param name="validationResult">The validation result to initialize this simple validation result with.</param>
        public SimpleValidationResult(ValidationResult validationResult)
        {
            Contract.Requires(validationResult != null, "The validation result must not be null.");
            this.IsValid = validationResult.IsValid;
            var errors = new List<SimpleValidationFailure>();
            foreach(var error in validationResult.Errors)
            {   
                ErrorPath errorPath = null;
                if(error.CustomState != null)
                {
                    Contract.Assert(error.CustomState is ErrorPath, "The custom state object must be an error path.");
                    errorPath = (ErrorPath)error.CustomState;
                }
                errors.Add(new SimpleValidationFailure(errorPath, error.ErrorMessage, error.PropertyName));
            }
            this.Errors = errors.OrderBy(x => x.ErrorMessage);
        }

        /// <summary>
        /// Constructs a new instance of the SimpleValidationResult with the given values.
        /// </summary>
        /// <param name="isValid">True, if the validation result is valid.</param>
        /// <param name="errors">The collection of errors.</param>
        public SimpleValidationResult(bool isValid, IEnumerable<SimpleValidationFailure> errors)
        {
            this.IsValid = isValid;
            this.Errors = errors ?? new List<SimpleValidationFailure>();
            this.Errors = this.Errors.OrderBy(x => x.ErrorMessage);
        }

        /// <summary>
        /// Gets the Is Valid flag.
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Gets the collection of errors.
        /// </summary>
        public IEnumerable<SimpleValidationFailure> Errors { get; private set; }
    }

    /// <summary>
    /// A SuccessfulValidationResult is used as a shortcut to create a validation result that represents successful validation.
    /// </summary>
    public class SuccessfulValidationResult : SimpleValidationResult
    {
        /// <summary>
        /// Creates a new instances.
        /// </summary>
        public SuccessfulValidationResult() : base(true, new List<SimpleValidationFailure>())
        {

        }
    }
}
