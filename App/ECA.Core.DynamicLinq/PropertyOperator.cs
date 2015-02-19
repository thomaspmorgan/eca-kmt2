using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq
{
    /// <summary>
    /// A PropertyOperator is a class that represents a operation to a specific property on a class of type T.  For example, filtering or sorting.
    /// </summary>
    /// <typeparam name="T">The type of object whose property will be operated against.</typeparam>
    public class PropertyOperator<T> where T : class
    {
        /// <summary>
        /// Creates a new PropertyOperator with the given name of the property.
        /// </summary>
        /// <param name="property">The name of the property this operator will use.</param>
        public PropertyOperator(string property)
        {
            Contract.Requires(property != null, "The property must not be null.");
            this.PropertyInfo = GetPropertyInfo(property);
            Contract.Assert(this.PropertyInfo != null, "The property info must not be null.");
            this.IsNullable = IsPropertyNullable();
        }

        /// <summary>
        /// Gets whether or not the property operated on is nullable.
        /// </summary>
        public bool IsNullable { get; private set; }

        /// <summary>
        /// Gets the PropertyInfo associated with this property.
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        /// Returns the property info of the property with the given name.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The PropertyInfo associated with the property.</returns>
        protected PropertyInfo GetPropertyInfo(string propertyName)
        {
            Contract.Assert(propertyName != null, "The property name must not be null.");
            var property = typeof(T).GetProperties().Where(x => x.Name.ToLower() == propertyName.ToLower()).FirstOrDefault();
            return property;
        }

        private bool IsPropertyNullable()
        {
            Debug.Assert(this.PropertyInfo != null, "The property info property must be set.");
            return this.PropertyInfo.PropertyType.GetGenericArguments().Length > 0;
        }

        /// <summary>
        /// Returns the underlying type of this properties nullable property.
        /// </summary>
        /// <returns>The property type of this class's underlying nullable property.</returns>
        protected Type GetNullableUnderlyingType()
        {
            return Nullable.GetUnderlyingType(this.PropertyInfo.PropertyType);
        }
    }
}
