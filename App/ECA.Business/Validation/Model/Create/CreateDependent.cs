using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    /// <summary>
    /// Contains dependent details
    /// </summary>
    [Validator(typeof(StudentValidator))]
    public class CreateDependent
    {
        /// <summary>
        /// Dependent personal information
        /// </summary>
        public PersonalInfo Dependent { get; set; }
        
        /// <summary>
        /// Dependent record remarks
        /// </summary>
        public string Remarks { get; set; }
        
    }
}
