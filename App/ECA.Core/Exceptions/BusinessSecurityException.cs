using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Exceptions
{
    /// <summary>
    /// A BusinessSecurityException is thrown when a special security exception has occurred within business
    /// logic that may not have been able to be detected outside of the business layer.
    /// </summary>
    [Serializable]
    public class BusinessSecurityException : System.Exception
    {
        /// <summary>
        /// Creates a new BusinessSecurityException.
        /// </summary>
        public BusinessSecurityException()
            : base() { }

        /// <summary>
        /// Creates a new BusinessSecurityException.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public BusinessSecurityException(string message)
            : base(message) { }

        /// <summary>
        /// Creates a new BusinessSecurityException.
        /// </summary>
        /// <param name="format">The exception message format string.</param>
        /// <param name="args">The format string arguments.</param>
        public BusinessSecurityException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        /// <summary>
        /// Creates a new BusinessSecurityException.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public BusinessSecurityException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Creates a new BusinessSecurityException.
        /// </summary>
        /// <param name="format">The format string for the exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="args">The format string arguments.</param>
        public BusinessSecurityException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        /// <summary>
        /// Creates a new BusinessSecurityException.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected BusinessSecurityException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
