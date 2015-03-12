﻿using ECA.Core.DynamicLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation
{
    /// <summary>
    /// A BusinessValidationResult is class to contain the business validation error message.
    /// </summary>
    public class BusinessValidationResult
    {
        internal BusinessValidationResult()
        {

        }

        /// <summary>
        /// Creates a new BusinessValidationResult with the given error message.
        /// </summary>
        /// <param name="errorMessage">The failed validation error message.</param>
        /// <param name="property">The property that was in error.</param>
        public BusinessValidationResult(string property, string errorMessage)
        {
            Contract.Requires(errorMessage != null, "The error message must not be null.");
            Contract.Requires(property != null, "The property must not be null.");
            this.ErrorMessage = errorMessage;
            this.Property = property;
        }       

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string ErrorMessage { get; protected set; }

        /// <summary>
        /// Gets the property.
        /// </summary>
        public string Property { get; protected set; }
    }

    /// <summary>
    /// The BusinessValidationResult contains an error message and a property name of the business entity
    /// that failed validation.
    /// </summary>
    /// <typeparam name="T">The business entity.</typeparam>
    public class BusinessValidationResult<T> : BusinessValidationResult where T : class
    {
        /// <summary>
        /// Creates a new BusinessValidationResult with the expression to the property that failed validation and the
        /// error message.
        /// </summary>
        /// <param name="propertySelector">The property that failed validation.</param>
        /// <param name="errorMessage">The reason the property failed validation.</param>
        public BusinessValidationResult(Expression<Func<T, object>> propertySelector, string errorMessage)
        {
            Contract.Requires(propertySelector != null, "The field must not be null.");
            this.Property = PropertyHelper.GetPropertyName<T>(propertySelector);
            this.ErrorMessage = errorMessage;
        }
    }
}
