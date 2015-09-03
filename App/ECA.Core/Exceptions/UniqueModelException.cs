using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Exceptions
{
    /// <summary>
    /// A UniqueModelException is thrown when a unique entity alreadry exists in a datastore.
    /// </summary>
    [Serializable]
    public class UniqueModelException : System.Exception
    {
        /// <summary>
        /// Creates a new UniqueModelException.
        /// </summary>
        public UniqueModelException()
            : base() { }

        /// <summary>
        /// Creates a new UniqueModelException.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public UniqueModelException(string message)
            : base(message) { }

        /// <summary>
        /// Creates a new UniqueModelException.
        /// </summary>
        /// <param name="format">The exception message format string.</param>
        /// <param name="args">The format string arguments.</param>
        public UniqueModelException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        /// <summary>
        /// Creates a new UniqueModelException.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public UniqueModelException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Creates a new UniqueModelException.
        /// </summary>
        /// <param name="format">The format string for the exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="args">The format string arguments.</param>
        public UniqueModelException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        /// <summary>
        /// Creates a new UniqueModelException.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected UniqueModelException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
