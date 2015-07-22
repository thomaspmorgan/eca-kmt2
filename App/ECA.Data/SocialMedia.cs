using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    /// <summary>
    /// The social media entity contains information about social media types and value.
    /// </summary>
    public class SocialMedia : IHistorical
    {
        /// <summary>
        /// The max length of a social media value.
        /// </summary>
        public const int VALUE_MAX_LENGTH = 100;

        /// <summary>
        /// Creates a new social media instance and initializes the history.
        /// </summary>
        public SocialMedia()
        {
            this.History = new History();
        }

        /// <summary>
        /// A social media identity for a participant or organization.
        /// </summary>
        [Key]
        public int SocialMediaId { get; set; }

        /// <summary>
        /// Gets or sets the social media type.
        /// </summary>
        public SocialMediaType SocialMediaType { get; set; }

        /// <summary>
        /// Gets or sets the social media type id.
        /// </summary>
        public int SocialMediaTypeId { get; set; }

        /// <summary>
        /// Gets or sets the social media value.
        /// </summary>
        [Required]
        [MaxLength(VALUE_MAX_LENGTH)]
        public string SocialMediaValue { get; set; }

        /// <summary>
        /// Gets or sets the organization.
        /// </summary>
        public virtual Organization Organization { get; set; }

        /// <summary>
        /// Gets or sets the organization id.
        /// </summary>
        public int? OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the person.
        /// </summary>
        public virtual Person Person { get; set; }

        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        public int? PersonId { get; set; }

        /// <summary>
        /// Gets or set the history.
        /// </summary>
        public History History { get; set; }
    }

}
