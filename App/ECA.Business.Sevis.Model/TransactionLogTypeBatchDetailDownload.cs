using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Sevis.Model.TransLog
{
    public partial class TransactionLogTypeBatchDetailDownload
    {
        /// <summary>
        /// Gets the resultCode as a DispositionCode.
        /// </summary>
        public DispositionCode DispositionCode
        {
            get
            {
                return DispositionCode.ToDispositionCode(this.resultCode);
            }
        }
    }
}
