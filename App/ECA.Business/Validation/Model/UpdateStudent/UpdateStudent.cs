using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class UpdateStudent
    {
        [Required(ErrorMessage = "Student update record is required")]
        public StudentUpdate student { get; set; }
        
    }
}
