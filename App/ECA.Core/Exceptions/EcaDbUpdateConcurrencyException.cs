using ECA.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Exceptions
{
    /// <summary>
    /// An EcaDbUpdateConcurrencyException extends DbUpdateConcurrencyException so that a client
    /// able to ascertain the rowversion and object id.
    /// </summary>
    [Serializable]
    public class EcaDbUpdateConcurrencyException : DbUpdateConcurrencyException
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public EcaDbUpdateConcurrencyException()
            : base()
        {

        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public EcaDbUpdateConcurrencyException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public EcaDbUpdateConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        /// <summary>
        /// Gets or sets the concurrent entities.
        /// </summary>
        public IEnumerable<IConcurrentEntity> ConcurrentEntities { get; set; }
    }
}
