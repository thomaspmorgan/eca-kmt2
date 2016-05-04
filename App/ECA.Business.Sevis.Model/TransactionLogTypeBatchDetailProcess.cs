using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Sevis.Model.TransLog
{
    public partial class TransactionLogTypeBatchDetailProcess
    {
        /// <summary>
        /// Returns a collection of GroupedTransactionLogTypeBatchDetailProcess records.  This is useful when updating sevis records
        /// and multiple requests have been sent for that participant.  All responses for that participant will be goruped into one of the
        /// returned instances.
        /// </summary>
        /// <returns>A collection of GroupedTransactionLogTypeBatchDetailProcess from this TransactionLogTypeBatchDetailProcess.</returns>
        public IEnumerable<GroupedTransactionLogTypeBatchDetailProcess> GetGroupedProcessRecords()
        {
            var query = from record in this.Record
                        let requestId = record.GetRequestId()
                        group record by new { IsParticipant = requestId.IsParticipantId, IsPersonDependent = requestId.IsPersonDependentId, ObjectId = requestId.Id }
                        into g
                        select new GroupedTransactionLogTypeBatchDetailProcess
                        {
                            IsParticipant = g.Key.IsParticipant,
                            IsPersonDependent = g.Key.IsPersonDependent,
                            ObjectId = g.Key.ObjectId,
                            Records = g.Select(x => x).ToList()
                        };
            return query.OrderBy(x => x.ObjectId).ToList();
        }
    }
}
