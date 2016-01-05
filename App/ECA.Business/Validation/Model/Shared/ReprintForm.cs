using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(ReprintFormValidator))]
    public class ReprintForm
    {
        public ReprintForm()
        { }

        public bool printForm { get; set; }
        
        public string Reason { get; set; }

        public string OtherRemarks { get; set; }

        public string Remarks { get; set; }
    }
}
