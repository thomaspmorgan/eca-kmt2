using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class TerminateStudent
    {
        [StringLength(2)]
        public string Reason { get; set; }

        [MaxLength(500)]
        public string OtherRemarks { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }

    }
}
