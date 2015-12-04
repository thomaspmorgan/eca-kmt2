using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    /// <summary>
    /// Contains new dependent details
    /// </summary>
    [Validator(typeof(CreateDependentValidator))]
    public class CreateDependent
    {
        /// <summary>
        /// New dependent
        /// </summary>
        public AddDependent Dependent { get; set; }
        
        /// <summary>
        /// Dependent record remarks
        /// </summary>
        public string Remarks { get; set; }        
    }
}
