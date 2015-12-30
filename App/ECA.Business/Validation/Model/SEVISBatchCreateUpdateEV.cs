using ECA.Business.Validation.Model;
using FluentValidation.Attributes;

namespace ECA.Business.Validation
{
    /// <summary>
    /// SEVIS batch create or update exchange visitors
    /// </summary>
    [Validator(typeof(SEVISBatchCreateUpdateEVValidator))]
    public class SEVISBatchCreateUpdateEV
    {
        public SEVISBatchCreateUpdateEV()
        {
            BatchHeader = new BatchHeader();
            CreateEV = new CreateExchVisitor();
        }

        /// <summary>
        /// Sevis batch record
        /// </summary>
        public string userID { get; set; }

        /// <summary>
        /// Sevis batch header
        /// </summary>
        public BatchHeader BatchHeader { get; set; }

        /// <summary>
        /// Create an exchange visitor record
        /// </summary>
        public CreateExchVisitor CreateEV { get; set; }

        /// <summary>
        /// Update an exchange visitor record
        /// </summary>
        public UpdateExchVisitor UpdateEV { get; set; }
    }
}
