using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(EditProgramValidator))]
    public class EditProgram
    {
        public bool printForm { get; set; }
        
        public string Level { get; set; }
        
        public string PrimaryMajor { get; set; }
        
        public string SecondMajor { get; set; }
        
        public string Minor { get; set; }
        
        public string LengthOfStudy { get; set; }

        public EngProficiency engProficiency { get; set; }
        
        public string Remarks { get; set; }
    }
}
