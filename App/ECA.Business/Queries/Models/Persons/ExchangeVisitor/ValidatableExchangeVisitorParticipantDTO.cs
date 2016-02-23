using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Persons.ExchangeVisitor
{
    /// <summary>
    /// A ValidatableExchangeVisitorParticipantDTO is used to represent participants that can be validated for sevis.
    /// </summary>
    public class ValidatableExchangeVisitorParticipantDTO
    {
        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the Person id.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Gets or sets the Participant Id.
        /// </summary>
        public int ParticipantId { get; set; }


    }
}
