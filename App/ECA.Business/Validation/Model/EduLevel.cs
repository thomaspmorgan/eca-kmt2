using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class EduLevel
    {
        [StringLength(2)]
        public string Level { get; set; }

        [MaxLength(500)]
        public string OtherRemarks { get; set; }
    }
}
