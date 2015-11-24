using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class CancelStudent
    {
        [StringLength(2)]
        public string Reason { get; set; }
        
        [MaxLength(500)]
        public string Remarks { get; set; }

    }
}
