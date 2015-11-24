using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class ChangeEducationLevel
    {
        [Required]
        public bool printForm { get; set; }

        public EducationalInfo educationalInfo { get; set; }

        public FinancialInfo financialInfo { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }

    }
}
