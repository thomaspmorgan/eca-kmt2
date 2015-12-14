using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(StudentReprintValidator))]
    public class StudentReprint
    {
        public bool printForm { get; set; }
        
        public string Reason { get; set; }
        
        public string Remarks { get; set; }
    }
}
