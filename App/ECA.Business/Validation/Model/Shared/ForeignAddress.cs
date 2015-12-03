using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class ForeignAddress
    {

        [MaxLength(60)]
        [Required(ErrorMessage = "Foreign address is required")]
        public string address1 { get; set; }

        [MaxLength(60)]
        public string address2 { get; set; }

        [MaxLength(60)]
        public string city { get; set; }

        [MaxLength(30)]
        public string province { get; set; }

        [StringLength(2)]
        [Required(ErrorMessage = "Country code is required")]
        public string countryCode { get; set; }

        [StringLength(20)]
        public string postalCode { get; set; }
        

    }
}
