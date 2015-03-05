using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
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
        /// <param name="message">The exception message.</param>
        /// <param name="property">The name of the property.</param>
        public ValidationException(string message, string property)
            : base(message) 
        {
            this.Property = property;
        }

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

        /// <summary>
        /// Gets the name of the property that is invalid.
        /// </summary>
        public string Property { get; private set; }

        /// <summary>
        /// Returns the name of the property given the expression of the property.
        /// </summary>
        /// <typeparam name="TSource">The object to get the property of.</typeparam>
        /// <param name="propertySelector">The expression to get the property.</param>
        /// <returns>The name of the property.</returns>
        public static string GetPropertyName<TSource>(Expression<Func<TSource, object>> propertySelector)
        {
            Contract.Requires(propertySelector != null, "The field must not be null.");
            MemberExpression expression = null;
            if (propertySelector.Body is MemberExpression)
            {
                expression = (MemberExpression)propertySelector.Body;
            }
            else if (propertySelector.Body is UnaryExpression)
            {
                expression = (MemberExpression)((UnaryExpression)propertySelector.Body).Operand;
            }
            else
            {
                throw new ArgumentException("The property is not supported.");
            }
            return expression.Member.Name;
        }
    }
}
