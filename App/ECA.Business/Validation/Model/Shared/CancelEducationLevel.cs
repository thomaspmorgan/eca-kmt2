using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class CancelEducationLevel
    {
        [MaxLength(500)]
        public string Remarks { get; set; }

    }
}
