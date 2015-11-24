using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class CompleteProgram
    {
        [MaxLength(500)]
        public string Remarks { get; set; }
        
    }
}
