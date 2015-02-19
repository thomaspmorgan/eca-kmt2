using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq.Filter
{
    /// <summary>
    /// An ExpressionFilter is a convience class for building a filter using Expressions.
    /// </summary>
    /// <typeparam name="TSource">The type of object to filter.</typeparam>
    public class ExpressionFilter<TSource> : SimpleFilter where TSource : class
    {
        /// <summary>
        /// Creates a new filter with the expression to retrieve the property of the object to filter.
        /// </summary>
        /// <param name="propertySelector">The expression to select an object property.</param>
        /// <param name="comparisonType">The comparison type.</param>
        /// <param name="value">The value.</param>
        public ExpressionFilter(Expression<Func<TSource, object>> propertySelector, ComparisonType comparisonType, object value)
        {
            Contract.Requires(propertySelector != null, "The property selector must not be null.");
            Contract.Requires(comparisonType != null, "The comparison type must not be null.");
            this.Comparison = comparisonType.Value;
            this.Value = value;
            this.Property = PropertyOperator<TSource>.GetPropertyName(propertySelector);
        }
    }
}
