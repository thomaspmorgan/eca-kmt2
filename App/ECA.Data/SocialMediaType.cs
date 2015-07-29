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
    /// The SocialMediaType is a lookup type for the varying social media sites on the internet.
    /// </summary>
    public partial class SocialMediaType
    {
        /// <summary>
        /// Creates a new default instance and initializes the history.
        /// </summary>
        public SocialMediaType()
        {
            this.History = new History();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Key]
        public int SocialMediaTypeId { get; set; }

        /// <summary>
        /// Gets or sets the social media type name.
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string SocialMediaTypeName { get; set; }

        /// <summary>
        /// Gets or sets the url of the social media site.
        /// </summary>
        [MaxLength(255)]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        public History History { get; set; }
    }
}
