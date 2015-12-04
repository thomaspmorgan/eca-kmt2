using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class SEVISBatchUpdateEV
    {
        // Sevis batch record
        [MaxLength(10)]
        [Required]
        public string userID { get; set; }

        // Sevis batch header
        public BatchHeader batchHeader { get; set; }

        // CreateEV

        // UpdateEV


    }
}
