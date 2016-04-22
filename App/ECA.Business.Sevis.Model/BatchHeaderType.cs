using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Sevis.Model
{
    public partial class BatchHeaderType
    {
        /// <summary>
        /// Returns this header's batch id as a BatchId instance.
        /// </summary>
        /// <returns>The BatchId instance for this BatchHeaderType instance.</returns>
        public BatchId GetBatchId()
        {
            if (this.BatchID == null)
            {
                return null;
            }
            else
            {
                return new BatchId(this.BatchID);
            }
        }

        /// <summary>
        /// Sets this instance's batch id field to the given batch id instance.
        /// </summary>
        /// <param name="id">The id.</param>
        public void SetBatchId(BatchId id)
        {
            if (id == null)
            {
                this.BatchID = null;
            }
            else
            {
                this.BatchID = id.ToString();
            }
        }
    }
}
