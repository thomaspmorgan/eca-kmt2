using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq.Filter
{
    /// <summary>
    /// An InFilter is used to filter objects whose property value is contained a collection of items.
    /// </summary>
    /// <typeparam name="T">The type of object to filter.</typeparam>
    public class InFilter<T> : BinaryFilter<T> where T : class
    {
        /// <summary>
        /// Creates a new InFilter and initializes it with the given property to filter on and the value to filter with.
        /// </summary>
        /// <param name="property">The name of the property to filter.</param>
        /// <param name="value">The IEnumerable collection of items to filter.</param>
        public InFilter(string property, object value)
            : base(property, value)
        {
            Contract.Requires(property != null, "The property must not be null.");
            Contract.Requires(value != null, "The value must not be null.");
            var valueType = value.GetType();
            if (!typeof(IEnumerable).IsAssignableFrom(valueType))
            {
                throw new NotSupportedException("The value is not an enumerable.");
            }

            this.CollectionType = valueType.GetGenericArguments()[0];
            var collectionTypeIsNumeric = this.IsTypeNumeric(this.CollectionType);
            if (IsNumeric && !collectionTypeIsNumeric)
            {
                throw new NotSupportedException("The property type to filter is numeric and the collection of values is not.");
            }
            if (!IsNumeric && collectionTypeIsNumeric)
            {
                throw new NotSupportedException("The property type to filter is not numeric and the collection of values is numeric.");
            }
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the type of objects in the collection i.e. the value's collections.
        /// </summary>
        private Type CollectionType { get; set; }

        private IList GetValuesAsList()
        {   
            Type propertyType = this.PropertyInfo.PropertyType;
            if (IsNullable)
            {
                propertyType = GetNullableUnderlyingType();
            }
            var listType = typeof(List<>).MakeGenericType(propertyType);
            var list = (IList)Activator.CreateInstance(listType);
            foreach (var v in (IEnumerable)this.Value)
            {
                list.Add(Convert.ChangeType(v, propertyType));
            }
            return list;
        }

        /// <summary>
        /// Returns a where expression checking the property value will be contained in an IEnumerable of items.
        /// </summary>
        /// <returns>The where expression checking an enumerable object containing the property to filter with.</returns>
        public override Expression<Func<T, bool>> ToWhereExpression()
        {
            Contract.Assert(this.Value != null, "The value must not be null.");
            var xParameter = Expression.Parameter(typeof(T), "x");
            var xProperty = Expression.Property(xParameter, this.PropertyInfo);
            var list = Expression.Constant(GetValuesAsList());

            var propertyType = this.PropertyInfo.PropertyType;
            if (IsNullable)
            {
                propertyType = GetNullableUnderlyingType();
            }
            var containsMethod = typeof(Enumerable).GetMethods()
                               .Where(m => m.Name == "Contains")
                               .Single(m => m.GetParameters().Length == 2)
                               .MakeGenericMethod(propertyType);
            Expression where;
            if (IsNullable)
            {
                var hasValueProperty = Expression.Property(xProperty, "HasValue");
                var valueProperty = Expression.Property(xProperty, "Value");
                var hasValueExpression = Expression.IsTrue(hasValueProperty);
                var callContainsMethod = Expression.Call(containsMethod, list, valueProperty);
                where = Expression.AndAlso(hasValueExpression, callContainsMethod);
            }
            else
            {
                where = Expression.Call(containsMethod, list, xProperty);
            }

            return Expression.Lambda<Func<T, bool>>(where, xParameter);
        }
    }
}
