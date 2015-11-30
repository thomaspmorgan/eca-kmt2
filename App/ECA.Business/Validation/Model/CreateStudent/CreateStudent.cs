using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class CreateStudent
    {
        [Required(ErrorMessage = "Student is required")]
        public Student student { get; set; }

        public CreateStudent() { }
    }
}
