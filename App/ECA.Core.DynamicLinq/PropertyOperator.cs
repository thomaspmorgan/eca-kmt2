﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq
{
    /// <summary>
    /// A PropertyOperator is a class that represents a operation to a specific property on a class of type T.  For example, filtering or sorting.
    /// </summary>
    /// <typeparam name="TSource">The type of object whose property will be operated against.</typeparam>
    public class PropertyOperator<TSource> where TSource : class
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
            if (this.IsNullable)
            {
                this.IsNumeric = IsTypeNumeric(this.GetNullableUnderlyingType());
            }
            else
            {
                this.IsNumeric = IsTypeNumeric(this.PropertyInfo.PropertyType);
            }
        }

        /// <summary>
        /// Gets whether or not the property operated on is nullable.
        /// </summary>
        public bool IsNullable { get; private set; }

        /// <summary>
        /// Gets whether or not the property operated on is numeric.
        /// </summary>
        public bool IsNumeric { get; private set; }

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
            var property = typeof(TSource).GetProperties().Where(x => x.Name.ToLower() == propertyName.ToLower()).FirstOrDefault();
            return property;
        }

        private bool IsPropertyNullable()
        {
            Debug.Assert(this.PropertyInfo != null, "The property info property must be set.");
            return this.PropertyInfo.PropertyType.GetGenericArguments().Length > 0;
        }

        /// <summary>
        /// Returns true if the property type to filter on is numeric.
        /// </summary>
        /// <returns>True if the property type to filter on is numeric.</returns>
        protected bool IsTypeNumeric(Type t)
        {
            return t == typeof(int)
                || t == typeof(double)
                || t == typeof(float) 
                || t == typeof(long);
        }

        /// <summary>
        /// Returns true if the given type is a collection.
        /// </summary>
        /// <param name="t">The type to test.</param>
        /// <returns>True, if the given type is a collection.</returns>
        protected bool IsTypeCollection(Type t)
        {
            var x = typeof(ICollection<>).IsAssignableFrom(t);
            return x;
        }

        /// <summary>
        /// Returns the underlying type of this properties nullable property.
        /// </summary>
        /// <returns>The property type of this class's underlying nullable property.</returns>
        protected Type GetNullableUnderlyingType()
        {
            return Nullable.GetUnderlyingType(this.PropertyInfo.PropertyType);
        }

        /// <summary>
        /// Returns the name of the property given the expression of the property.
        /// </summary>
        /// <typeparam name="TSource">The object to get the property of.</typeparam>
        /// <param name="propertySelector">The expression to get the property.</param>
        /// <returns>The name of the property.</returns>
        public static string GetPropertyName(Expression<Func<TSource, object>> propertySelector)
        {
            Contract.Requires(propertySelector != null, "The field must not be null.");
            MemberExpression expression = null;
            if (propertySelector.Body is MemberExpression)
            {
                expression = (MemberExpression)propertySelector.Body;
            }
            else if (propertySelector.Body is UnaryExpression)
            {
                expression = (MemberExpression)((UnaryExpression)propertySelector.Body).Operand;
            }
            else
            {
                throw new ArgumentException("The property is not supported.");
            }
            return expression.Member.Name;
        }
    }
}