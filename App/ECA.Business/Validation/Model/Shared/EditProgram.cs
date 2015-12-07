using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class EditProgram
    {
        [Required]
        public bool printForm { get; set; }

        [StringLength(2)]
        public string Level { get; set; }
        
        [MaxLength(7)]
        public string PrimaryMajor { get; set; }

        [MaxLength(7)]
        public string SecondMajor { get; set; }

        [MaxLength(7)]
        public string Minor { get; set; }

        [MaxLength(2)]
        public string LengthOfStudy { get; set; }

        public EngProficiency engProficiency { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }

    }
}
