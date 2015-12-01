using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class StudentReprint
    {
        [Required]
        public bool printForm { get; set; }

        [StringLength(2)]
        public string Reason { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }
    }
}
