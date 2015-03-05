using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation
{
    /// <summary>
    /// A BusinessValidationResult is class to contain the business validation error message.
    /// </summary>
    public class BusinessValidationResult
    {
        internal BusinessValidationResult()
        {

        }

        /// <summary>
        /// Creates a new BusinessValidationResult with the given error message.
        /// </summary>
        /// <param name="errorMessage">The failed validation error message.</param>
        public BusinessValidationResult(string errorMessage)
        {
            Contract.Requires(errorMessage != null, "The error message must not be null.");
            this.ErrorMessage = errorMessage;
        }       

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string ErrorMessage { get; protected set; }
    }

    /// <summary>
    /// The BusinessValidationResult contains an error message and a property name of the business entity
    /// that failed validation.
    /// </summary>
    /// <typeparam name="T">The business entity.</typeparam>
    public class BusinessValidationResult<T> : BusinessValidationResult
    {
        /// <summary>
        /// Creates a new BusinessValidationResult with the expression to the property that failed validation and the
        /// error message.
        /// </summary>
        /// <param name="propertySelector">The property that failed validation.</param>
        /// <param name="errorMessage">The reason the property failed validation.</param>
        public BusinessValidationResult(Expression<Func<T, object>> propertySelector, string errorMessage)
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
            this.Property = expression.Member.Name;
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        public string Property { get; private set; }
    }
}
