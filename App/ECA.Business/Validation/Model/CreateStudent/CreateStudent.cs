using ECA.Business.Validation.Model.Create;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    /// <summary>
    /// Top-level object to hold a new create student object
    /// </summary>
    [Validator(typeof(CreateStudentValidator))]
    public class CreateStudent
    {
        public CreateStudent()
        {
            student = new Student();
            student.isNew = true;
        }

        public Student student { get; set; }
    }
}
