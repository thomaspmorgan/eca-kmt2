using ECA.Business.Validation.Model;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation
{
    public class SEVISBatchCreateUpdateStudent
    {

        // Sevis batch record
        [MaxLength(10)]
        [Required]
        public string userID { get; set; }
        
        // Sevis batch header
        public BatchHeader batchHeader { get; set; }

        // Student record
        public CreateStudent createStudent { get; set; }
        




    }
}
