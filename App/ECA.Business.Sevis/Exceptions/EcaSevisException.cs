using System;

namespace ECA.Business.Sevis.Exceptions
{
    public class EcaSevisException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EcaSevisException() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">The message</param>
        public EcaSevisException(string message) : base(message) { }
    }
}
