using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class BatchHeader
    {

        [MaxLength(14)]
        [Required]
        public string BatchID { get; set; }

        [MaxLength(15)]
        [Required]
        public string OrgID { get; set; }






    }
}
