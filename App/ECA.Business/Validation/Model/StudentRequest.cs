using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class StudentRequest
    {
        [Required]
        public bool printForm { get; set; }

        [StringLength(2)]
        public string CapGapExtension { get; set; }

        [StringLength(1)]
        public string Status { get; set; }
    }
}
