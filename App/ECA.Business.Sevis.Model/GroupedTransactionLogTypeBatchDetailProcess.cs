using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Sevis.Model.TransLog
{
    /// <summary>
    /// GroupedTransactionLogTypeBatchDetailProcess objects are used to group sevis transaction process records to a single object, i.e. a participant or dependent.
    /// </summary>
    public class GroupedTransactionLogTypeBatchDetailProcess
    {
        /// <summary>
        /// Gets or sets the Object Id, i.e., the participant id, or the person dependent id.
        /// </summary>
        public int ObjectId { get; set; }

        /// <summary>
        /// Gets or sets whether this object contains participant records.
        /// </summary>
        public bool IsParticipant { get; set; }

        /// <summary>
        /// Gets or sets whether this object contains person dependent records.
        /// </summary>
        public bool IsPersonDependent { get; set; }
        
        /// <summary>
        /// Gets or sets the records related to object with the Object Id.
        /// </summary>
        public IEnumerable<TransactionLogTypeBatchDetailProcessRecord> Records { get; set; }

        /// <summary>
        /// Returns true if all records were succesful otherwise false.
        /// </summary>
        /// <returns>True, if all records were successful otherwise false.</returns>
        public bool AllRecordsSuccessful()
        {
            return this.Records.Where(x => !x.Result.status).Count() == 0;
        }
    }
}
