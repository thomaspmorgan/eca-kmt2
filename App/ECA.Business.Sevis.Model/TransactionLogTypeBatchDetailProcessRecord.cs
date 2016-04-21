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
        /// Sets the request id on this exchange visitor.
        /// </summary>
        /// <param name="participantId">The participant id of the exchange visitor.</param>
        public void SetRequestId(int participantId)
        {
            this.requestID = new RequestId(participantId, RequestIdType.Participant, RequestActionType.Create).ToString();
        }

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
