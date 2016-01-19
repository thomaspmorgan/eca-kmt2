using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.Shared
{
    /// <summary>
    /// Header identifying SEVIS batch
    /// </summary>
    [Validator(typeof(BatchHeaderValidator))]
    public class BatchHeader
    {
        /// <summary>
        /// Batch document id
        /// </summary>
        public string BatchID { get; set; }

        /// <summary>
        /// Program number (X-1-12345)
        /// </summary>
        public string OrgID { get; set; }
    }
}
