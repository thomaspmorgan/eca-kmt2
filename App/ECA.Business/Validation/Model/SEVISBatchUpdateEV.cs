using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.CreateEV;

namespace ECA.Business.Validation
{
    public class SEVISBatchUpdateEV
    {
        public SEVISBatchUpdateEV()
        {
            batchHeader = new BatchHeader();
            createVisitor = new CreateEV();
        }

        // Sevis batch record
        public string userID { get; set; }

        // Sevis batch header
        public BatchHeader batchHeader { get; set; }

        // Sevis student record
        public CreateEV createVisitor { get; set; }
    }
}
