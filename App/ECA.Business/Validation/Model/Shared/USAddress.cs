using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class USAddress
    {
        [MaxLength(64)]
        [Required(ErrorMessage = "US address is required")]
        public string address1 { get; set; }

        [MaxLength(64)]
        public string address2 { get; set; }
        
        [MaxLength(60)]
        public string city { get; set; }

        [StringLength(5)]
        public string PostalCode { get; set; }

        [StringLength(2)]
        public string ExplanationCode { get; set; }

        [MinLength(5)]
        [MaxLength(200)]
        public string Explanation { get; set; }
    }
}
