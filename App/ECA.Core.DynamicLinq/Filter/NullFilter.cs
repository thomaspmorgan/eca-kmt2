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
    /// A NullFilter is used to filter objects whose property is or is not null.
    /// </summary>
    /// <typeparam name="T">The type to filter.</typeparam>
    public class NullFilter<T> : PropertyOperator<T> where T : class
    {
        /// <summary>
        /// Creates a new NullFilter with the property to filter and whether or not
        /// to look for objects whose property is null or not null.
        /// </summary>
        /// <param name="property">The name of the property to filter.</param>
        /// <param name="isNotNull">True, if filtering on objects whose property is not null, other false, to filter on objects whose property is null.</param>
        public NullFilter(string property, bool isNotNull = false)
            : base(property)
        {
            Contract.Requires(property != null, "The property must not be null.");
            this.IsNotNull = isNotNull;
        }

        /// <summary>
        /// True, if this filter is filtering on objects whose property is not null, otherwise false.
        /// </summary>
        public bool IsNotNull { get; private set; }

        /// <summary>
        /// Returns an expression to filter with in a linq query.
        /// </summary>
        /// <returns>An expression to filter with in a linq query.</returns>
        public override Expression<Func<T, bool>> ToWhereExpression()
        {
            var xParameter = Expression.Parameter(typeof(T), "x");
            var xProperty = Expression.Property(xParameter, this.PropertyInfo);

            Expression whereExpression = null;
            var nullConstant = Expression.Constant(null);
            if (this.IsNotNull)
            {
                whereExpression = Expression.NotEqual(xProperty, nullConstant);
            }
            else
            {
                whereExpression = Expression.Equal(xProperty, nullConstant);
            }
            return Expression.Lambda<Func<T, bool>>(whereExpression, xParameter);
        }
    }
}
