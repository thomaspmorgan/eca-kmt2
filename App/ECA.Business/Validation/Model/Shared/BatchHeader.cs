
namespace ECA.Business.Validation.Model
{
    /// <summary>
    /// Header identifying SEVIS batch
    /// </summary>
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
