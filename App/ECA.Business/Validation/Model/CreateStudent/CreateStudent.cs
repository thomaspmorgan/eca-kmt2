using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class CreateStudent
    {
        [Required(ErrorMessage = "Student information is required")]
        public Student student { get; set; }
        
    }
}
