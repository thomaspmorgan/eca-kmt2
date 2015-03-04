using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Exceptions
{
    /// <summary>
    /// A ValidationException is thrown when business logic entity validation fails.
    /// </summary>
    [Serializable]
    public class ValidationException : System.Exception
    {
        /// <summary>
        /// Creates a new ValidationException.
        /// </summary>
        public ValidationException()
            : base() { }

        /// <summary>
        /// Creates a new ValidationException.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public ValidationException(string message)
            : base(message) { }

        /// <summary>
        /// Creates a new ValidationException.
        /// </summary>
        /// <param name="format">The exception message format string.</param>
        /// <param name="args">The format string arguments.</param>
        public ValidationException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        /// <summary>
        /// Creates a new ValidationException.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ValidationException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Creates a new ValidationException.
        /// </summary>
        /// <param name="format">The format string for the exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="args">The format string arguments.</param>
        public ValidationException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        /// <summary>
        /// Creates a new ValidationException.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
