using ECA.Business.Validation.Model;

namespace ECA.Business.Validation
{
    public class SEVISBatchCreateUpdateStudent
    {
        public SEVISBatchCreateUpdateStudent()
        {
            batchHeader = new BatchHeader();
            createStudent = new CreateStudent();
        }

        // Sevis batch record
        public string userID { get; set; }

        // Sevis batch header
        public BatchHeader batchHeader { get; set; }

        // Sevis student record
        public CreateStudent createStudent { get; set; }        
    }
}
