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
    /// A GreaterThanFilter is a filter used to filter objects whose property value is greater than a given value.
    /// </summary>
    /// <typeparam name="T">The type to filter.</typeparam>
    public class GreaterThanFilter<T> : EqualFilter<T> where T : class
    {
        /// <summary>
        /// Creates a new filter with the property and value to filter with.
        /// </summary>
        /// <param name="property">The property name.</param>
        /// <param name="value">The value to filter on.</param>
        public GreaterThanFilter(string property, object value)
            : base(property, value)
        {
            Contract.Requires(property != null, "The property must not be null.");
            Contract.Requires(value != null, "The value must not be null.");
        }

        /// <summary>
        /// Returns the equality expression given the member expression i.e. the property to filter on.
        /// </summary>
        /// <param name="property">The property to filter with.</param>
        /// <returns>The expression.</returns>
        protected override Expression GetEqualityExpression(MemberExpression property)
        {
            Contract.Assert(property != null, "The property must not be null.");
            Contract.Assert(this.Value != null, "The value must not be null.");
            var valueConstant = Expression.Constant(this.Value);
            return Expression.GreaterThan(property, valueConstant);
        }
    }
}
