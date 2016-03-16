using ECA.Business.Validation.Sevis.ErrorPaths;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

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
            this.Errors = errors;
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
        }

        public bool IsValid { get; private set; }

        public IEnumerable<SimpleValidationFailure> Errors { get; private set; }
    }
}
