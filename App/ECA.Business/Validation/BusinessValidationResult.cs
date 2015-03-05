using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation
{
    public class BusinessValidationResult
    {
        internal BusinessValidationResult()
        {

        }

        public BusinessValidationResult(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }       

        public string ErrorMessage { get; set; }
    }

    public class BusinessValidationResult<T> : BusinessValidationResult
    {
        public BusinessValidationResult(Expression<Func<T, object>> propertySelector, string errorMessage)
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
            this.Property = expression.Member.Name;
            this.ErrorMessage = errorMessage;
        }

        public string Property { get; set; }
    }
}
