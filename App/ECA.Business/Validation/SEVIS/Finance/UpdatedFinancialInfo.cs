using ECA.Business.Sevis.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis.Finance
{
    public class UpdatedFinancialInfo : FinancialInfo, IFormPrintable
    {
        public bool PrintForm { get; set; }

        /// <summary>
        /// Returns the updated sevis exchange visitor financial info.
        /// </summary>
        /// <returns>The updated sevis exchange visitor financial info.</returns>
        public SEVISEVBatchTypeExchangeVisitorFinancialInfo GetSEVISEVBatchTypeExchangeVisitorFinancialInfo()
        {
            return new SEVISEVBatchTypeExchangeVisitorFinancialInfo
            {

            };
        }
    }
}
