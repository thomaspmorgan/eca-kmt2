using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class School
    {

        [MaxLength(8)]
        public int Amount { get; set; }
        
        [MaxLength(500)]
        public string Remarks { get; set; }
        
    }
}
