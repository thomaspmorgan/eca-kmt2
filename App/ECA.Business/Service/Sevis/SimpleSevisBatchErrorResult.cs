using ECA.Business.Sevis.Model.TransLog;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
        /// Creates a new default instance.
        /// </summary>
        public SimpleSevisBatchErrorResult() { }

        /// <summary>
        /// Creates a new instance with the error data in the given result type.
        /// </summary>
        /// <param name="result">The sevis transaction log result type.</param>
        public SimpleSevisBatchErrorResult(ResultType result) 
            : this(result.ErrorCode, result.ErrorMessage)
        {
            Contract.Requires(result != null, "the result must not be null.");
        }

        /// <summary>
        /// Creates a new instance with the error code and message.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="errorMessage">The error message.</param>
        public SimpleSevisBatchErrorResult(string errorCode, string errorMessage)
        {
            Contract.Requires(errorCode != null, "The error code must not be null.");
            Contract.Requires(errorMessage != null, "The error message must not be null.");
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
        }

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
