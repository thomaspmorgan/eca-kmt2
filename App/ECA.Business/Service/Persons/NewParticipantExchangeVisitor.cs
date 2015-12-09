using ECA.Core.Exceptions;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An NewParticipantExchangeVisitor is used by a business layer client to create a new record for a person that is an exchange visitor.
    /// </summary>
    public class NewParticipantExchangeVisitor : IAuditable
    {
        /// <summary>
        /// A class to update a Participant Persons Exchange Visitor info
        /// <param name="creator"></param>
        /// <param name="participantId"></param>
        public NewParticipantExchangeVisitor(User creator, int participantId)
        {
            this.Audit = new Create(creator);
            this.ParticipantId = participantId;
        }

        /// <summary>
        /// Gets or sets the participants exchange visitor id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets the update audit.
        /// </summary>
        public Audit Audit { get; private set; }
    }
}
