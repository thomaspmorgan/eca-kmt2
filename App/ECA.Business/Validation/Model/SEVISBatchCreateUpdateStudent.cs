using ECA.Business.Validation.Model;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation
{
    public class SEVISBatchCreateUpdateStudent
    {
        // Sevis batch record
        [MaxLength(10)]
        [Required(ErrorMessage = "User id is required")]
        public string userID { get; set; }

        // Sevis batch header
        [Required(ErrorMessage = "Batch header is required")]
        public BatchHeader batchHeader { get; set; }

        // Sevis student record
        public CreateStudent createStudent { get; set; }        
    }
}
