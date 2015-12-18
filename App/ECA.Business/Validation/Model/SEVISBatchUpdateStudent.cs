using ECA.Business.Validation.Model;
using FluentValidation.Attributes;

namespace ECA.Business.Validation
{
    [Validator(typeof(SEVISBatchUpdateStudentValidator))]
    public class SEVISBatchUpdateStudent
    {
        public SEVISBatchUpdateStudent()
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
