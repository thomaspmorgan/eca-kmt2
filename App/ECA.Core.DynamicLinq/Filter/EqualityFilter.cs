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
    /// An EqualityFilter is used to compare an object's property equal to another value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EqualityFilter<T> : BinaryFilter<T> where T : class
    {
        /// <summary>
        /// Creates a new EqualityFilter with the given property and value to filter on.
        /// </summary>
        /// <param name="property">The property name to filter on.</param>
        /// <param name="value">The value to filter on.</param>
        public EqualityFilter(string property, object value)
            : base(property, value)
        {
            Contract.Requires(property != null, "The property must not be null.");
            Contract.Requires(value != null, "The property must not be null.");            
        }

        /// <summary>
        /// Returns an expression to filter with in a linq query.
        /// </summary>
        /// <returns>An expression to filter with in a linq query.</returns>
        public override Expression<Func<T, bool>> ToWhereExpression()
        {
            Contract.Assert(this.Value != null, "The value must not be null.");
            var xParameter = Expression.Parameter(typeof(T), "x");
            var xProperty = Expression.Property(xParameter, this.PropertyInfo);

            Expression where;
            if (IsNullable)
            {
                var hasValueProperty = Expression.Property(xProperty, "HasValue");
                var valueProperty = Expression.Property(xProperty, "Value");
                var hasValueExpression = Expression.IsTrue(hasValueProperty);
                var equalityExpression = GetEqualityExpression(valueProperty);
                where = Expression.AndAlso(hasValueExpression, equalityExpression);
            }
            else
            {
                where = GetEqualityExpression(xProperty);
            }
            return Expression.Lambda<Func<T, bool>>(where, xParameter);
        }

        protected abstract Expression GetEqualityExpression(MemberExpression property);
    }
}
