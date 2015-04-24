using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Exceptions
{
    /// <summary>
    /// General exception for business layer
    /// </summary>
    [Serializable]
    public class EcaBusinessException : System.Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EcaBusinessException()
            : base() {}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">The message</param>
        public EcaBusinessException(string message)
            : base(message) {}
    }
}
