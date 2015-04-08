using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    /// <summary>
    /// A Focus is a concentration that a project encompasses.
    /// </summary>
    public class Objective : IHistorical
    {
        /// <summary>
        /// The max length of the focus.
        /// </summary>
        public const int NAME_MAX_LENGTH = 50;

        /// <summary>
        /// Creates a new Focus.
        /// </summary>
        public Objective()
        {
            this.History = new History();
            this.Justification = new Justification();
        }

        /// <summary>
        /// Gets or set the Id.
        /// </summary>
        public int ObjectiveId { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        ///
        [MaxLength(NAME_MAX_LENGTH)]
        public string ObjectiveName { get; set; }

        /// <summary>
        /// Office for the Focus area
        /// </summary>
        public int JustificationId { get; set; }

        /// <summary>
        /// Office of the Focus Area
        /// </summary>
        ///
        public Justification Justification { get; set; }

        public ICollection<Program> Programs { get; set; }

        public ICollection<Project> Projects { get; set; }

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        public History History { get; set; }
    }
}


