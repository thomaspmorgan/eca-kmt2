using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    /// <summary>
    /// The model class for websites
    /// </summary>
    public class Website : IHistorical
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public Website()
        {
            this.History = new History();
        }

        /// <summary>
        /// Gets or sets the program id
        /// </summary>
        [Key]
        public int WebsiteId { get; set; }
        
        /// <summary>
        /// Gets or sets the program value
        /// </summary>
        [Required]
        [MaxLength(4000)]
        public string WebsiteValue { get; set; }

        /// <summary>
        /// Gets or sets the history
        /// </summary>
        public History History { get; set; }

        /// <summary>
        /// Gets or sets the programs
        /// </summary>
        public virtual ICollection<Program> Programs { get; set; }
    }
}
