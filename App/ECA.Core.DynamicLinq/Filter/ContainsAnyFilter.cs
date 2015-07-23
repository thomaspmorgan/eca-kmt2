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
    /// An ContainsAnyFilter is used to filter objects whose collection property contains any of the given values.
    /// </summary>
    /// <typeparam name="T">The type of object to filter.</typeparam>
    public class ContainsAnyFilter<T> : BinaryFilter<T> where T : class
    {
        /// <summary>
        /// Creates a new ContainsAnyFilter that applies a filter to a numeric property checking for the existence of any of the given values
        /// in the Enumerable numeric property.
        /// </summary>
        /// <param name="property">The name of the enumerable numeric property to filter.</param>
        /// <param name="value">The IEnumerable collection of numeric items to filter with.</param>
        public ContainsAnyFilter(string property, object value)
            : base(property, value)
        {
            Contract.Requires(property != null, "The property must not be null.");
            Contract.Requires(value != null, "The value must not be null.");
            var valueType = value.GetType();
            if (!typeof(IEnumerable).IsAssignableFrom(valueType) || valueType == typeof(string))
            {
                throw new NotSupportedException("The value is not an enumerable.");
            }

            var propertyType = this.PropertyInfo.PropertyType;
            if (!typeof(IEnumerable).IsAssignableFrom(propertyType) || propertyType == typeof(string))
            {
                throw new NotSupportedException("The property is not an enumerable.");
            }

            this.ValueCollectionType = valueType.GetGenericArguments()[0];
            this.PropertyCollectionType = propertyType.GetGenericArguments()[0];

            var collectionTypeIsNumeric = this.IsTypeNumeric(this.ValueCollectionType);
            var propertyCollectionTypeIsNumeric = this.IsTypeNumeric(this.PropertyCollectionType);

            if (!collectionTypeIsNumeric)
            {
                throw new NotSupportedException("The collection type is not a numeric collection.");
            }
            if (!propertyCollectionTypeIsNumeric)
            {
                throw new NotSupportedException("The property collection type is not a numeric collection.");
            }

            //if (propertyCollectionTypeIsNumeric && !collectionTypeIsNumeric)
            //{
            //    throw new NotSupportedException("The property type to filter is numeric and the collection of values is not.");
            //}
            //if (!propertyCollectionTypeIsNumeric && collectionTypeIsNumeric)
            //{
            //    throw new NotSupportedException("The property type to filter is not numeric and the collection of values is numeric.");
            //}
            
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the type of objects in the property i.e. the object's property collections collection type.
        /// </summary>
        public Type PropertyCollectionType { get; private set; }

        /// <summary>
        /// Gets or sets the given value's collection type i.e. the generic argument applied to the Enumerable value.
        /// </summary>
        public Type ValueCollectionType { get; private set; }

        /// <summary>
        /// Returns the given value as a list of objects whose type is the type of the class property's collection type.
        /// </summary>
        /// <returns>The typed list of values.</returns>
        private IList GetValuesAsList()
        {   
            var listType = typeof(List<>).MakeGenericType(this.ValueCollectionType);
            var list = (IList)Activator.CreateInstance(listType);
            foreach (var v in (IEnumerable)this.Value)
            {
                //object value = null;
                //if (this.PropertyCollectionType == typeof(string))
                //{
                //    value = ((string)v).ToLower();
                //}
                //else
                //{
                //    value = v;
                //}
                //list.Add(Convert.ChangeType(value, this.ValueCollectionType));
                list.Add(Convert.ChangeType(v, this.ValueCollectionType));
            }
            return list;
        }

        /// <summary>
        /// Returns a where expression checking the property value will be contained in an IEnumerable of items.
        /// </summary>
        /// <returns>The where expression checking an enumerable object containing the property to filter with.</returns>
        public override Expression<Func<T, bool>> ToWhereExpression()
        {
            var xParameter = Expression.Parameter(typeof(T), "x");
            var xProperty = Expression.Property(xParameter, this.PropertyInfo);
            var valueList = GetValuesAsList();
            if (valueList.Count == 0)
            {
                throw new NotSupportedException("There must be at least one value to filter on.");
            }
            var containsMethod = typeof(Enumerable).GetMethods()
                               .Where(m => m.Name == "Contains")
                               .Single(m => m.GetParameters().Length == 2)
                               .MakeGenericMethod(this.ValueCollectionType);

            //if (this.PropertyCollectionType == typeof(string))
            //{
                
            //    var yParameter = Expression.Parameter(this.PropertyCollectionType, "y");
                
            //    var toLowerMethod = Expression.Call(yParameter, "ToLower", System.Type.EmptyTypes);
            //    var fnType = typeof(Func<string, string>);
            //    var selectMethod = typeof(Enumerable).GetMethods()
            //                   .Where(m => m.Name == "Select")
            //                   .Single(m => m.GetParameters().Length == 2)
            //                   .MakeGenericMethod(this.ValueCollectionType);
            //    var selectLower = Expression.Call(toLowerMethod, selectMethod);

            //}

            Expression where = null;
            foreach (var item in valueList)
            {
                var constant = Expression.Constant(item);
                var clause = Expression.Call(containsMethod, xProperty, constant);
                if (where == null)
                {
                    where = clause;
                }
                else
                {
                    where = Expression.Or(where, clause);
                }
            }

            return Expression.Lambda<Func<T, bool>>(where, xParameter);
        }
    }
}
