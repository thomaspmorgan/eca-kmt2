using ECA.Business.Validation.Sevis.ErrorPaths;

namespace ECA.Business.Validation
{
    /// <summary>
    /// A SimpleValidationFailure is used to serialize to json a FluentValidation ValidationFailure instance.
    /// </summary>
    public class SimpleValidationFailure
    {
        private SimpleValidationFailure(string errorMessage, string propertyName)
        {
            this.ErrorMessage = errorMessage;
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Creates a new SimpleValidationFailuren and initializes the instance properties.
        /// </summary>
        /// <param name="errorPath">The error path.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="propertyName">The property name.</param>
        public SimpleValidationFailure(ErrorPath errorPath, string errorMessage, string propertyName)
            : this(errorMessage, propertyName)
        {
            this.CustomState = errorPath;
        }

        /// <summary>
        /// Gets the custom state.
        /// </summary>
        public object CustomState { get; private set; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Gets the property name.
        /// </summary>
        public string PropertyName { get; private set; }
    }
}
