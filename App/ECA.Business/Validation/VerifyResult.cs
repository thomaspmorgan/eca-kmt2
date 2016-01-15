using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace ECA.Business.Validation
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class VerifyValidator<T> : AbstractValidator<T> where T : class
    {   
        /// <summary>
        /// Retrieve participant verification results
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public VerifyResult VerifyParticipant(T context)
        {
            VerifyResult result = new VerifyResult();
            
            return result;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class VerifyResult : ValidationResult
    {
        /// <summary>
        /// Collection of verification errors
        /// </summary>
        public List<VerifyFailure> VerifyErrors { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VerifyFailure : ValidationFailure
    {
        public VerifyFailure(string propertyName, string error) : base(propertyName, error)
        { }

        /// <summary>
        /// Error hyperlink to route to field section on UI
        /// </summary>
        public string FieldRoute { get; }
    }
}
