using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Sevis.Model.TransLog
{
    public partial class TransactionLogTypeBatchDetailProcessRecord
    {
        /// <summary>
        /// Returns this exchange visitor's request id, or null if the request id is not set.
        /// </summary>
        /// <returns>The request id.</returns>
        public RequestId GetRequestId()
        {
            if (!String.IsNullOrWhiteSpace(this.requestID))
            {
                return new RequestId(this.requestID);
            }
            else
            {
                return null;
            }
        }
    }
}
