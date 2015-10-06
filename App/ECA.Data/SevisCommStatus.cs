using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    /// <summary>
    /// Model to store/retrieve the SevisCommStatus lookup
    /// </summary>
    public partial class SevisCommStatus : IHistorical
    {
        /// <summary>
        /// initialize history for Sevis Comm Status
        /// </summary>
        public SevisCommStatus()
        {
            this.History = new History();
        }
        /// <summary>
        /// the max length of the status name
        /// </summary>
        public const int MAX_STATUS_LENGTH = 50;

        /// <summary>
        /// Gets or sets the ProgramStatusId.
        /// </summary>
        [Key]
        public int SevisCommStatusId { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        [Required]
        [StringLength(MAX_STATUS_LENGTH)]
        public string SevisCommStatusName { get; set; }

        /// <summary>
        /// Gets or sets the History.
        /// </summary>
        public History History { get; set; }

        // relationships

        /// <summary>
        /// navigation property for person participants with this status
        /// </summary>
        public ICollection<ParticipantPersonSevisCommStatus> ParticipantPersonSevisCommStatuses { get; set; }
    }
}
