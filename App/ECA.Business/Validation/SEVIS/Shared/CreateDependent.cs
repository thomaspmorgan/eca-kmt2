using ECA.Business.Validation.Model.CreateEV;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    /// <summary>
    /// Contains new dependent details
    /// </summary>
    [Validator(typeof(CreateDependentValidator))]
    public class CreateDependent
    {
        public CreateDependent()
        {
            Dependent = new AddDependent();
            AddTIPP = new AddTIPP();
        }
        
        /// <summary>
        /// New dependent
        /// </summary>
        public AddDependent Dependent { get; set; }

        /// <summary>
        /// T/IPP information
        /// </summary>
        public AddTIPP AddTIPP { get; set; }        
    }
}
