using System;

namespace ECA.Core.Exceptions
{
    /// <summary>
    /// A ValidationRulesException is thrown when a validation exception has occurred within business
    /// logic that may not have been able to be detected outside of the business layer.
    /// </summary>
    public class ValidationRulesException : Exception
    {
        /// <summary>
        /// Creates a new ValidationRulesException.
        /// </summary>
        public ValidationRulesException() : base()
        {

        }

        /// <summary>
        /// Creates a new ValidationRulesException.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public ValidationRulesException(string message) : base(message)
        {

        }

        /// <summary>
        /// Creates a new ValidationRulesException.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ValidationRulesException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
