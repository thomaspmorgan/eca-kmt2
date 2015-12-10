using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(CompleteStudentValidator))]
    public class CompleteStudent
    {
        public string Remarks { get; set; }        
    }
}
