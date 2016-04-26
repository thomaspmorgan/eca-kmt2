using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// A ParticipantSevisSubmissionInfo is used to detail the type of submission to be made to sevis.
    /// </summary>
    public class ParticipantSevisSubmissionInfo
    {
        /// <summary>
        /// Gets or sets the participant id.
        /// </summary>
        public int ParticipantId { get; set; }
        
        /// <summary>
        /// Gets or sets if the submission is a queued to submit batch member.
        /// </summary>
        public bool IsQueuedToSubmit { get; set; }

        /// <summary>
        /// Gets or sets if the submission is a queued to validate batch member.
        /// </summary>
        public bool IsQueuedToValidate { get; set; }
    }
}
