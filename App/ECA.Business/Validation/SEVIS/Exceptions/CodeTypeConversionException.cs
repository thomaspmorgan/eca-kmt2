using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis.Exceptions
{
    /// <summary>
    /// A CodeTypeConversionException is thrown when a conversion from an ECA code can not be converted to a sevis code.
    /// </summary>
    [Serializable]
    public class CodeTypeConversionException : System.Exception
    {
        /// <summary>
        /// Creates a new CodeTypeConversionException.
        /// </summary>
        public CodeTypeConversionException()
            : base()
        { }

        /// <summary>
        /// Creates a new CodeTypeConversionException.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public CodeTypeConversionException(string message)
            : base(message)
        { }

        /// <summary>
        /// Creates a new CodeTypeConversionException.
        /// </summary>
        /// <param name="format">The exception message format string.</param>
        /// <param name="args">The format string arguments.</param>
        public CodeTypeConversionException(string format, params object[] args)
            : base(string.Format(format, args))
        { }

        /// <summary>
        /// Creates a new CodeTypeConversionException.
        /// </summary>
        /// <param name="format">The exception message format string.</param>
        /// <param name="args">The format string arguments.</param>
        public CodeTypeConversionException(string enumValue, Type enumType)
            : base(string.Format("The code type [{0}] could not be parsed from the given value [{1}].", enumType, enumValue))
        { }

        /// <summary>
        /// Creates a new CodeTypeConversionException.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CodeTypeConversionException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Creates a new CodeTypeConversionException.
        /// </summary>
        /// <param name="format">The format string for the exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="args">The format string arguments.</param>
        public CodeTypeConversionException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        { }

        /// <summary>
        /// Creates a new ModelNotFoundException.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CodeTypeConversionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }

}
