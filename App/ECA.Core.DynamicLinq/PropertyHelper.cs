using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq
{
    /// <summary>
    /// PropertyHelper provides some static methods to get names of object properties.
    /// </summary>
    public static class PropertyHelper
    {
        /// <summary>
        /// Returns the name of the property given the expression of the property.
        /// </summary>
        /// <param name="propertySelector">The expression to get the property.</param>
        /// <returns>The name of the property.</returns>
        public static string GetPropertyName<TSource, TEnumerable>(Expression<Func<TSource, IEnumerable<TEnumerable>>> propertySelector) where TSource : class
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

        /// <summary>
        /// Returns the name of the property given the expression of the property.
        /// </summary>
        /// <param name="propertySelector">The expression to get the property.</param>
        /// <returns>The name of the property.</returns>
        public static string GetPropertyName<TSource>(Expression<Func<TSource, object>> propertySelector) where TSource : class
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

        /// <summary>
        /// Returns the name of the property given the expression of the property.
        /// </summary>
        /// <param name="propertySelector">The expression to get the property.</param>
        /// <returns>The name of the property.</returns>
        public static string GetPropertyName<TSource>(Expression<Func<TSource, string>> propertySelector) where TSource : class
        {
            Contract.Requires(propertySelector != null, "The field must not be null.");
            MemberExpression expression = null;
            if (propertySelector.Body is MemberExpression)
            {
                expression = (MemberExpression)propertySelector.Body;
            }
            else
            {
                throw new ArgumentException("The property is not supported.");
            }
            return expression.Member.Name;
        }
    }
}
