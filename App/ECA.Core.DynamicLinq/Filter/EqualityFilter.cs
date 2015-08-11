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
    [ContractClass(typeof(EqualityFilterContract<>))]
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
            Initialize(value);
        }

        /// <summary>
        /// Returns an expression to filter with in a linq query.
        /// </summary>
        /// <returns>An expression to filter with in a linq query.</returns>
        public override Expression<Func<T, bool>> ToWhereExpression()
        {
            var xParameter = Expression.Parameter(typeof(T), "x");
            var xProperty = Expression.Property(xParameter, this.PropertyInfo);

            Expression where;
            if (IsNullable)
            {
                var hasValueProperty = Expression.Property(xProperty, "HasValue");
                var valueProperty = Expression.Property(xProperty, "Value");
                var hasValueExpression = Expression.Equal(Expression.Constant(true), hasValueProperty);
                
                var equalityExpression = GetEqualityExpression(valueProperty);
                Contract.Assert(equalityExpression != null, "The equality expresison must return a value.");
                where = Expression.AndAlso(hasValueExpression, equalityExpression);
            }
            else
            {
                where = GetEqualityExpression(xProperty);
            }
            return Expression.Lambda<Func<T, bool>>(where, xParameter);
        }

        /// <summary>
        /// Initializes the value property of this filter with the given value and tests for compatibility.
        /// </summary>
        /// <param name="value">The value to initialize the filter with.</param>
        private void Initialize(object value)
        {
            if (this.IsNumeric != this.IsTypeNumeric(value.GetType()))
            {
                throw new NotSupportedException("The property to filter on and the value are not the same type.");
            }
            if (this.IsNumeric)
            {
                Type typeToConvertTo = this.PropertyInfo.PropertyType;
                if (this.IsNullable)
                {
                    typeToConvertTo = GetNullableUnderlyingType();
                }
                this.Value = Convert.ChangeType(value, typeToConvertTo);
            }
            else if(value.GetType() == typeof(DateTime)
                && this.PropertyInfo.PropertyType == typeof(DateTimeOffset))
            {
                var dateTimeValue = (DateTime)value;
                this.Value = new DateTimeOffset(dateTimeValue);
            }
            else
            {
                this.Value = value;
            }
        }

        /// <summary>
        /// Override this method to return a property linq expression given the property parameter to filter on.
        /// </summary>
        /// <param name="property">The property that will be filtered on e.g. x.NullableInt.Value, where Value is the property parameter that is passed.</param>
        /// <returns>The equality expression.</returns>
        protected abstract Expression GetEqualityExpression(MemberExpression property);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ContractClassFor(typeof(EqualityFilter<>))]
    public abstract class EqualityFilterContract<T> : EqualityFilter<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public EqualityFilterContract(string property, object value)
            : base(property, value)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected override Expression GetEqualityExpression(MemberExpression property)
        {
            Contract.Requires(property != null, "The property must not be null.");
            Contract.Ensures(Contract.Result<Expression>() != null, "The method must return a non-null expression.");
            return null;
        }
    }
}
