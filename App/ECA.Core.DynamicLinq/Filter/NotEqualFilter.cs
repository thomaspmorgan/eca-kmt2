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
    /// A NotEqualFilter is used to test an object's property value not equal to another constant.
    /// </summary>
    /// <typeparam name="T">The type to filter on.</typeparam>
    public class NotEqualFilter<T> : EqualityFilter<T> where T : class
    {
        /// <summary>
        /// Creates a new NotEqualFilter with the given property and value.
        /// </summary>
        /// <param name="property">The property to filter on.</param>
        /// <param name="value">The value to filter with.</param>
        public NotEqualFilter(string property, object value)
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
            Contract.Assume(this.Value != null, "The value must not be null.");
            var valueConstant = Expression.Constant(this.Value);
            return Expression.NotEqual(property, valueConstant);
        }
    }
}
