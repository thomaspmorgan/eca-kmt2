using ECA.Business.Validation;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Custom.Filters
{
    /// <summary>
    /// The ValidationErrorResonseContent class contains a standardized object shape for validation errors that occur either
    /// in business validation or db entity validation.
    /// </summary>
    public class ValidationErrorResponseContent
    {
        /// <summary>
        /// The http reason phrase a bad request is being returned.
        /// </summary>
        public const string VALIDATION_FAILED_REASON_FAILED_MESSAGE = "Validation failed.";

        /// <summary>
        /// The error message to show for validation failure.
        /// </summary>
        public const string VALIDATION_FAILED_ERROR_MESSAGE = "A validation error occurred.";

        /// <summary>
        /// Initializes the object with the given message and sets the validation error properties to a new dictionary.
        /// </summary>
        /// <param name="errorMessage">The validation error message.</param>
        internal ValidationErrorResponseContent(string errorMessage)
        {
            this.Message = errorMessage;
            this.ValidationErrors = new Dictionary<string, string>();
        }

        /// <summary>
        /// Creates a new instance given the business validation errors that occurred.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="businessValidationResults">The business validation results.</param>
        public ValidationErrorResponseContent(string errorMessage, IEnumerable<BusinessValidationResult> businessValidationResults) : this(errorMessage)
        {
            businessValidationResults.ToList().ForEach(x => this.ValidationErrors.Add(x.Property, x.ErrorMessage));
        }

        /// <summary>
        /// Creates a new instance given the entity framework db validation errors that occurred.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="dbValidationErrors">The db validation errors.</param>
        public ValidationErrorResponseContent(string errorMessage, IEnumerable<DbValidationError> dbValidationErrors) : this(errorMessage)
        {
            dbValidationErrors.ToList().ForEach(x => this.ValidationErrors.Add(x.PropertyName, x.ErrorMessage));
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the validation errors as key value pairs, where the key is the property and value is the validation message.
        /// </summary>
        public Dictionary<string, string> ValidationErrors { get; private set; }
    }
}