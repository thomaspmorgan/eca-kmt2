using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(TerminateStudentValidator))]
    public class TerminateStudent
    {
        public string Reason { get; set; }
        
        public string OtherRemarks { get; set; }
        
        public string Remarks { get; set; }
    }
}
