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
    /// An UpdatedParticipantPerson is used by a business layer client to update a person that is a project participant.
    /// </summary>
    public class NewParticipantStudentVisitor : IAuditable
    {
        /// <summary>
        /// A class to update a Participant Persons SEVIS info
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="participantId"></param>
        /// <param name="educationLevelId"></param>
        /// <param name="primaryMajorId"></param>

        public NewParticipantStudentVisitor(User creator, int participantId)
        {
            this.Audit = new Create(creator);
            this.ParticipantId = participantId;
        }

        /// <summary>
        /// Gets or sets the students id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets the update audit.
        /// </summary>
        public Audit Audit { get; private set; }
    }
}
