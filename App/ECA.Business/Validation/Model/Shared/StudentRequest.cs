using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(StudentRequestValidator))]
    public class StudentRequest
    {
        public bool printForm { get; set; }

        public CapGapExtension capGapExtension { get; set; }
        
        public string Status { get; set; }
    }
}
