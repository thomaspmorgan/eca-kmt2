using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Admin
{
    /// <summary>
    /// Class to represent the information in the participant timeline
    /// </summary>
    public class ParticipantTimelineDTO
    {
        /// <summary>
        /// Gets or sets the project id
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the participant id
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets and sets the program id
        /// </summary>
        public int ProgramId { get; set; }

        /// <summary>
        /// Gets and sets the office id
        /// </summary>
        public int OfficeId { get; set; }
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the start date
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date
        /// </summary>
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the office symbol
        /// </summary>
        public string OfficeSymbol { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the status
        /// </summary>
        public string Status { get; set; }
    }
}
