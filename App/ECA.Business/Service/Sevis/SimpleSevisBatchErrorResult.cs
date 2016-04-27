using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Sevis
{
    /// <summary>
    /// A SimpleSevisBatchErrorResult is used to hold information about a sevis batch submission that failed.
    /// </summary>
    public class SimpleSevisBatchErrorResult
    {
        /// <summary>
        /// Gets or sets the Error Code.
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the Error Message.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
