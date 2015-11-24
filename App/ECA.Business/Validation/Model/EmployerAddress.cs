using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class EmployerAddress
    {
        [MaxLength(60)]
        [Required(ErrorMessage = "Address is required")]
        public string address1 { get; set; }

        [MaxLength(60)]
        public string address2 { get; set; }

        [MaxLength(60)]
        public string city { get; set; }

        [StringLength(2)]
        public string State { get; set; }

        [StringLength(5)]
        public string PostalCode { get; set; }

        [StringLength(4)]
        public string PostalRoundingCode { get; set; }

    }
}
