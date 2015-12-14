using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(CancelStudentValidator))]
    public class CancelStudent
    {
        public string Reason { get; set; }
        
        public string Remarks { get; set; }
    }
}
