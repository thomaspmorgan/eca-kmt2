using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(UpdatedDependentValidator))]
    public class UpdatedDependent
    {
        public string UserDefinedA { get; set; }
        
        public string UserDefinedB { get; set; }
        
        public AddDependent addDependent { get; set; }

        public CancelDependent cancelDependent { get; set; }

        public EditDependent editDependent { get; set; }

        public ReactivateDependent reactivateDependent { get; set; }

        public ReprintForm reprintDependent { get; set; }        
    }
}
