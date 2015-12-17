using ECA.Business.Validation.Model;
using FluentValidation.Attributes;

namespace ECA.Business.Validation
{
    [Validator(typeof(SEVISBatchUpdateEVValidator))]
    public class SEVISBatchUpdateEV
    {
        public SEVISBatchUpdateEV()
        {
            batchHeader = new BatchHeader();
            createVisitor = new CreateExchVisitor();
        }

        // Sevis batch record
        public string userID { get; set; }

        // Sevis batch header
        public BatchHeader batchHeader { get; set; }

        // Sevis visitor record
        public CreateExchVisitor createVisitor { get; set; }
    }
}
