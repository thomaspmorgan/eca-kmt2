using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(CancelDependentValidator))]
    public class CancelDependent
    {
        public CancelDependent()
        { }

        public string dependentSevisID { get; set; }
        
        public string Reason { get; set; }

        public string OtherRemarks { get; set; }

        public string Remarks { get; set; }
    }
}
