using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Exceptions
{
    /// <summary>
    /// A ModelNotFoundException is thrown when an entity can not be located in a repository.
    /// </summary>
    [Serializable]
    public class ModelNotFoundException : System.Exception
    {
        /// <summary>
        /// Creates a new ModelNotFoundException.
        /// </summary>
        public ModelNotFoundException()
            : base() { }

        /// <summary>
        /// Creates a new ModelNotFoundException.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public ModelNotFoundException(string message)
            : base(message) { }

        /// <summary>
        /// Creates a new ModelNotFoundException.
        /// </summary>
        /// <param name="format">The exception message format string.</param>
        /// <param name="args">The format string arguments.</param>
        public ModelNotFoundException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        /// <summary>
        /// Creates a new ModelNotFoundException.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ModelNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Creates a new ModelNotFoundException.
        /// </summary>
        /// <param name="format">The format string for the exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="args">The format string arguments.</param>
        public ModelNotFoundException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        /// <summary>
        /// Creates a new ModelNotFoundException.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ModelNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
