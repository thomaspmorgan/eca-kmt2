using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq.Filter
{
    /// <summary>
    /// A LikeFilter filters an object by a string property using a contains method.
    /// </summary>
    /// <typeparam name="T">The type to filter.</typeparam>
    public class LikeFilter<T> : BinaryFilter<T> where T : class
    {
        /// <summary>
        /// Creates a new LikeFilter with the given property name and object value.
        /// </summary>
        /// <param name="property">The property name to filter on.</param>
        /// <param name="value">The value to filter with.</param>
        public LikeFilter(string property, object value) : base(property, value)
        {
            Contract.Requires(property != null, "The property must not be null.");
            Contract.Requires(value != null, "The value must not be null.");
            if (value.GetType() != typeof(string))
            {
                throw new ArgumentException("The value to filter on must be a string.");
            }
            this.Value = value;
        }

        /// <summary>
        /// Returns an expression to filter with in a linq query.
        /// </summary>
        /// <returns>An expression to filter with in a linq query.</returns>
        public override Expression<Func<T, bool>> ToWhereExpression()
        {
            Contract.Assert(this.Value != null, "The value must not be null.");
            Contract.Assert(this.Value is string, "The value must be a string.");
            var lowerCaseValue = this.Value.ToString().ToLower();
            var lowerCaseValueConstant = Expression.Constant(lowerCaseValue);
            var xParameter = Expression.Parameter(typeof(T), "x");
            var xProperty = Expression.Property(xParameter, this.PropertyInfo);

            var isNotNullExpression = Expression.NotEqual(xProperty, Expression.Constant(null));
            var toLowerMethod = Expression.Call(xProperty, "ToLower", System.Type.EmptyTypes);

            var containsMethod = typeof(string).GetMethod("Contains", new [] {typeof(string)});
            var containsMethodExpression = Expression.Call(toLowerMethod, containsMethod, lowerCaseValueConstant);
            var where = Expression.AndAlso(isNotNullExpression, containsMethodExpression);
            return Expression.Lambda<Func<T, bool>>(where, xParameter);
        }
    }
}
