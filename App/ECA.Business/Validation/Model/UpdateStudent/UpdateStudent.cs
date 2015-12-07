using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(UpdateStudentValidator))]
    public class UpdateStudent
    {
        public StudentUpdate student { get; set; }
        
    }
}
